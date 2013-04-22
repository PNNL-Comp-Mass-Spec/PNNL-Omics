using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;
using System.Collections;
using PNNLOmics.Algorithms.Distance;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    public class UMCPrimsClustering<T, U> : LinkageClustererBase<T, U>
        where T : FeatureLight, IChildFeature<U>, new()
        where U : FeatureLight, IFeatureCluster<T>, new()
    {
        public UMCPrimsClustering()
        {
            
        }

        public override List<U> Cluster(List<T> data, List<U> clusters)
        {

            return base.Cluster(data, clusters);
        }

        /// <summary>
        /// Clusters features based on their pairwise distances by finding the minimal spanning tree (MST) via Prim's algorithm.
        /// </summary>
        /// <param name="distances">Pairwise distances between all features in question.</param>
        /// <param name="clusters">Singleton clusters from each feature.</param>
        /// <returns>List of features clustered together.</returns>
        public override List<U> LinkFeatures(List<PairwiseDistance<T>> potentialDistances, Dictionary<int, U> clusters)
        {
            List<PairwiseDistance<T>> distances = new List<PairwiseDistance<T>>();
            foreach (PairwiseDistance<T> distance in potentialDistances)
            {
                if (AreClustersWithinTolerance(distance.FeatureX, distance.FeatureY))
                {
                    distances.Add(distance);
                }
            }

            List<PairwiseDistance<T>> newDistances = (from element in distances
                                                        orderby element.Distance
                                                        select element).ToList();
            
            Queue<Edge<T>> queue  = new Queue<Edge<T>>();
            FeatureGraph<T> graph = new FeatureGraph<T>();

            // Sort out the distances so we dont have to recalculate distances.
            int id = 0;
            List<Edge<T>> edges = new List<Edge<T>>();            
            newDistances.ForEach(x => edges.Add(new Edge<T>(id++, 
                                                            x.Distance,
                                                            x.FeatureX,
                                                            x.FeatureY)));
            graph.CreateGraph(edges);
            edges.ForEach(x => queue.Enqueue(x));

            // This makes sure we have 
            HashSet<int> seenEdge   = new HashSet<int>();            
            List<U> newClusters     = new List<U>();
            

            // Now we start at the MST building
            while(queue.Count > 0)
            {
                Edge<T> startEdge               = queue.Dequeue();

                // If we have already seen the edge, ignore it...
                if (seenEdge.Contains(startEdge.ID))
                    continue;

                MinimumSpanningTree<T> mstGroup = ConstructSubTree(graph,
                                                                   seenEdge,
                                                                   startEdge);
                // Get the mst value .
                double sum  = 0;
                double mean = 0;
                foreach (Edge<T> dist in mstGroup.LinearRelationship)
                {
                    seenEdge.Add(dist.ID);
                    sum += dist.Length;

                    double ppmDist = Feature.ComputeMassPPMDifference(dist.VertexB.MassMonoisotopicAligned,
                                                                      dist.VertexA.MassMonoisotopicAligned);
                    Console.WriteLine("{0},,{1},{2},{3},{4},{5},{6}", dist, 
                                                                      dist.VertexA.MassMonoisotopicAligned,
                                                                      dist.VertexA.RetentionTime,
                                                                      dist.VertexB.MassMonoisotopicAligned,
                                                                      dist.VertexB.RetentionTime,
                                                                      ppmDist,
                                                                      Math.Abs(dist.VertexA.RetentionTime - dist.VertexB.RetentionTime));                            
                }

                double N = Convert.ToDouble(mstGroup.LinearRelationship.Count);

                // Calculate the standard deviation.
                mean = sum / N;
                sum  = 0;
                foreach (Edge<T> dist in mstGroup.LinearRelationship)
                {
                    double diff = dist.Length - mean;
                    sum += (diff * diff);
                }

                double stdev  = Math.Sqrt(sum / N);
                double cutoff = stdev * 3;

                List<U> mstClusters = CreateClusters(mstGroup, cutoff);
                newClusters.AddRange(mstClusters);                
            }                      
            return newClusters;
        }
        /// <summary>
        /// Creates clusters based on the MST's linear relationship made via construction.  Cutoff is the score (length) per edge
        /// that is allowed.
        /// </summary>
        /// <param name="mst">Minimum Spanning Tree</param>
        /// <param name="cutoff">Cutoff score</param>
        /// <returns>List of clusters</returns>
        private List<U> CreateClusters(MinimumSpanningTree<T> mst, double cutoff)
        {
            List<U> clusters        = new List<U>();           
            U currentCluster = null;

            // Maps features that are not part of a cluster
            // because their edge is too long (above the cutoff)
            Dictionary<T, Edge<T>> notMapped = new Dictionary<T,Edge<T>>();
            HashSet<T> mapped = new HashSet<T>();

            for (int i = 0; i < mst.LinearRelationship.Count; i++)
            {
                Edge<T> edge = mst.LinearRelationship[i];
                
                // Add to cluster if above cutoff.
                if (edge.Length < cutoff)
                {
                    if (currentCluster == null)
                    {
                        currentCluster = new U();
                    }

                    T a = edge.VertexA;
                    T b = edge.VertexB;



                    // Add the vertices
                    if (!mapped.Contains(a))
                        currentCluster.AddChildFeature(a);
                    if (!mapped.Contains(b))
                        currentCluster.AddChildFeature(b);

                    // Here we see if the edge was skipped previously, 
                    // because it was part of an edge that was above the threshold.                    
                    // But it was part of another edge that allowed it to be clustered
                    //
                    //     A-----------------B--C--D-------------E--H--F
                    //
                    //  A-B would be skipped, but B would later be added with C,D
                    //  The same would happen for E,H,F.  We want to make sure that the
                    //  B is removed from previously being tagged as not mapped.
                    //
                    //  Alternatively
                    //  D-E would be flagged as not mapped...so we need to respect that edge case
                    //  and make sure we flag those that are mapped
                    //  that's why we track which ones were mapped.
                    // 
                    if (notMapped.ContainsKey(a))                    
                        notMapped.Remove(a);                    
                    if (notMapped.ContainsKey(b))                    
                        notMapped.Remove(b);
                }
                else
                {
                    if (currentCluster != null)
                    {
                        clusters.Add(currentCluster);
                        currentCluster = null;
                    }

                    T a = edge.VertexA;
                    T b = edge.VertexB;

                    // Only map if we have not mapped it before...
                    // see the above comment for reasons why.
                    if (!mapped.Contains(a))
                    {                        
                        if (!notMapped.ContainsKey(a))
                            notMapped.Add(a, edge);
                    }
                    if (!mapped.Contains(b))
                    {
                        if (!notMapped.ContainsKey(b))
                            notMapped.Add(b, edge);
                    }
                }                
            }

            // Make sure we add the current cluster if it was not cut off
            if (currentCluster != null)
            {
                clusters.Add(currentCluster);
            }

            // Then map all the singletons (any feature that did not make it into a cluster)
            // these would be part of edges that are not part of a linear relationship            
            foreach (T feature in notMapped.Keys)
            {
                // Only continue if we have not mapped it yet....
                if (mapped.Contains(feature))                
                    continue;

                // Since we havent, create a cluster (singleton)!
                U cluster = new U();
                cluster.AddChildFeature(feature);
                clusters.Add(cluster);
            }

            return clusters;
        }

        /// <summary>
        /// constructs a sub-tree 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="distanceMap"></param>
        /// <param name="startEdge"></param>
        /// <returns></returns>
        private MinimumSpanningTree<T> ConstructSubTree(FeatureGraph<T> graph,  
                                                        HashSet<int>    visitedEdges,                                         
                                                        Edge<T>         startEdge)
        {
            // Manages the tree being constructed.
            MinimumSpanningTree<T> tree = new MinimumSpanningTree<T>();    
                    
            // Manages the list of candidate edges
            UniqueEdgeList<T> tempEdges = new UniqueEdgeList<T>();
             
            // Seed of the breadth first search (BFS)
            tempEdges.AddEdge(startEdge);            

            // Start BFS
            while (tempEdges.Count > 0)
            {
                // Sort the edges based on distace.
                tempEdges.Sort();
                               
                Edge<T> shortestEdge = null;
                List<Edge<T>> edgesToRemove = new List<Edge<T>>();

                // Find the shortest edge...
                foreach (Edge<T> edge in tempEdges.Edges)
                {
                    bool edgeSeen   = tree.HasEdgeBeenSeen(edge);
                    bool vertexSeen = tree.HasEdgeVerticesBeenSeen(edge);

                    // Make sure that we havent seen this edge.
                    if (edgeSeen)
                        continue;

                    if (vertexSeen)
                    {
                        visitedEdges.Add(edge.ID);
                        edgesToRemove.Add(edge);
                        continue;
                    }

                    shortestEdge = edge;
                    tree.AddEdge(shortestEdge);
                    break;
                }

                // Remove any edges that have been used up..
                edgesToRemove.ForEach(x => tempEdges.RemoveEdge(x));
                edgesToRemove.ForEach(x => graph.RemoveEdge(x));

                // We didnt find an edge, so we have nothing else to connect...
                if (shortestEdge == null)
                {
                    // Make sure that we assert that there are no edges left...should be the case here!
                    System.Diagnostics.Debug.Assert(tempEdges.Count == 0);
                    break;
                }

                visitedEdges.Add(shortestEdge.ID);

                // Removes the shortest edge from the graph...                
                graph.RemoveEdge(shortestEdge);

                UniqueEdgeList<T> adjacentEdges = graph.GetAdjacentEdgesFromEdgeVertices(shortestEdge);
                //adjacentEdges.Sort();

                tempEdges.AddEdges(adjacentEdges.Edges);                              

                // Remove the shortest edge from the list of available edges left...                
                tempEdges.RemoveEdge(shortestEdge);       
            }

            return tree;
        }
    }

}
