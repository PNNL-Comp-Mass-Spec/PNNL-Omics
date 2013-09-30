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
            NSigma = 5;
            DumpLinearRelationship = false;
            ShouldTestClustersWithinTolerance = true;
        }
        public UMCPrimsClustering(double sigmaCutoff)
        {
            NSigma = sigmaCutoff;
            DumpLinearRelationship = false;
            ShouldTestClustersWithinTolerance = true;
        }

        public bool DumpLinearRelationship { get; set; }

        /// <summary>
        /// The number of sigma to allow before cutting the linear relationship.
        /// </summary>
        public double NSigma
        {
            get;
            set;
        }

        protected override bool AreClustersWithinTolerance(T clusterX, T clusterY)
        {
            return true;
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
            List<U> newClusters                 = new List<U>();
            List<PairwiseDistance<T>> distances = new List<PairwiseDistance<T>>();

            // There is an edge case with this setup that a singleton outside of the range 
            // of other features made it into the batch of edges, but there is no corresponding edge 
            // to the rest of the graph(s).  So here we hash all features
            // then we ask for within the range, pare down that hash to a set of features that 
            // have no corresponding edge.  These guys would ultimately be singletons we want 
            // to capture...
            HashSet<T> clusterMap = new HashSet<T>();
            foreach (U cluster in clusters.Values)
            {
                foreach (T feature in cluster.Features)
                {
                    if (!clusterMap.Contains(feature))
                    {
                        clusterMap.Add(feature);
                    }
                }
            }


            foreach (PairwiseDistance<T> distance in potentialDistances)
            {
                if (AreClustersWithinTolerance(distance.FeatureX, distance.FeatureY))
                {
                    //distances.Add(distance);
                    if (clusterMap.Contains(distance.FeatureX))
                    {
                        clusterMap.Remove(distance.FeatureX);
                    }
                    if (clusterMap.Contains(distance.FeatureY))
                    {
                        clusterMap.Remove(distance.FeatureY);
                    }
                }
            }

            // Once we have removed any cluster
            foreach (T feature in clusterMap)
            {
                U cluster = new U();
                feature.SetParentFeature(cluster);
                cluster.AddChildFeature(feature);
                newClusters.Add(cluster);
            }

            List<PairwiseDistance<T>> newDistances = (from element in potentialDistances
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


            // Now we start at the MST building
            if (DumpLinearRelationship)
            {
                Console.WriteLine("GraphEdgeLength");

            }
            while(queue.Count > 0)
            {
                Edge<T> startEdge               = queue.Dequeue();

                // If we have already seen the edge, ignore it...
                if (seenEdge.Contains(startEdge.ID))
                    continue;

                MinimumSpanningTree<T> mstGroup = ConstructSubTree(graph,
                                                                   seenEdge,
                                                                   startEdge);

                MstLrTree<Edge<T>> clusterTree = new MstLrTree<Edge<T>>();

                // Get the mst value .
                double sum  = 0;
                double mean = 0;
                foreach (Edge<T> dist in mstGroup.LinearRelationship)
                {
                    seenEdge.Add(dist.ID);
                    sum += dist.Length;

                    clusterTree.Insert(dist);

                    double ppmDist = Feature.ComputeMassPPMDifference(dist.VertexB.MassMonoisotopicAligned,
                                                                      dist.VertexA.MassMonoisotopicAligned);

                    if (DumpLinearRelationship)
                    {
                        Console.WriteLine("{0}", dist.Length); /*,,{1},{2},{3},{4},{5},{6},{7},{8}", dist.Length,
                                                                          dist.VertexA.RetentionTime,
                                                                          dist.VertexA.MassMonoisotopicAligned,
                                                                          dist.VertexA.DriftTime,
                                                                          dist.VertexB.RetentionTime,
                                                                          dist.VertexB.MassMonoisotopicAligned,
                                                                          dist.VertexB.DriftTime,
                                                                          ppmDist,
                                                                          Math.Abs(dist.VertexA.RetentionTime - dist.VertexB.RetentionTime));
                                                         */
                    }
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
                double cutoff = NSigma; // *stdev; // stdev* NSigma;

                List<U> mstClusters = CreateClusters(mstGroup, cutoff);
                newClusters.AddRange(mstClusters);                
            }   
            //List<MinimumSpanningTree<T>> trees = new List<MinimumSpanningTree<T>>();

            
            //    // Get the mst value .
            //double sum  = 0;
            //double mean = 0;
            //double N    = 0;

            //while (queue.Count > 0)
            //{
            //    Edge<T> startEdge = queue.Dequeue();

            //    // If we have already seen the edge, ignore it...
            //    if (seenEdge.Contains(startEdge.ID))
            //        continue;

            //    MinimumSpanningTree<T> mstGroup = ConstructSubTree(graph,
            //                                                       seenEdge,
            //                                                       startEdge);

            //    MstLrTree<Edge<T>> clusterTree = new MstLrTree<Edge<T>>();

            //    foreach (Edge<T> dist in mstGroup.LinearRelationship)
            //    {
            //        seenEdge.Add(dist.ID);
            //        sum += dist.Length;
            //        N++;
            //    }
            //    trees.Add(mstGroup);
            //}
            
            //// Calculate the standard deviation.
            //mean = sum / N;
            //sum = 0;

            //foreach (MinimumSpanningTree<T> mstGroup in trees)
            //{
            //    foreach (Edge<T> dist in mstGroup.LinearRelationship)
            //    {
            //        double diff = dist.Length - mean;
            //        sum += (diff * diff);

            //        if (DumpLinearRelationship)
            //        {
            //            double ppmDist = Feature.ComputeMassPPMDifference(dist.VertexB.MassMonoisotopicAligned,
            //                                                            dist.VertexA.MassMonoisotopicAligned);
            //            Console.WriteLine("{0},,{1},{2},{3},{4},{5},{6},{7},{8}", dist,
            //                                                                dist.VertexA.RetentionTime,
            //                                                                dist.VertexA.MassMonoisotopicAligned,
            //                                                                dist.VertexA.DriftTime,
            //                                                                dist.VertexB.RetentionTime,
            //                                                                dist.VertexB.MassMonoisotopicAligned,
            //                                                                dist.VertexB.DriftTime,
            //                                                                ppmDist,
            //                                                                Math.Abs(dist.VertexA.RetentionTime - dist.VertexB.RetentionTime));
            //        }
            //    }
            //}

            //double stdev = Math.Sqrt(sum / N);
            //double cutoff = stdev * NSigma;

            //foreach(MinimumSpanningTree<T> mstGroup in trees)
            //{                                               
            //    List<U> mstClusters = CreateClusters(mstGroup, cutoff);
            //    newClusters.AddRange(mstClusters);
            //}           
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
            if (mst.LinearRelationship.Count < 1)
                return clusters;
            
            Edge<T> previous          = mst.LinearRelationship[0];
            U currentCluster          = new U();
            HashSet<T> hashedFeatures = new HashSet<T>(); // Tracks the current feature

            // These are the features that dont ever get included into a cluster...
            // This can only happen if the MST building picked a bunch of low-life features that 
            // dont ever construct a graph...
            List<T> lowLifeFeatures = new List<T>();

            for (int i = 0; i < mst.LinearRelationship.Count; i++)
            {
                // note this isnt O(n^2), this is just the search for a sub cluster
                //                 
                Edge<T> currentEdge = mst.LinearRelationship[i];
                T vertexA = currentEdge.VertexA;
                T vertexB = currentEdge.VertexB;
                
                bool seenA = hashedFeatures.Contains(vertexA);
                bool seenB = hashedFeatures.Contains(vertexB);

                if (currentEdge.Length < cutoff)
                {
                    if (!seenA)                        
                    {
                        hashedFeatures.Add(vertexA);                            
                        currentCluster.AddChildFeature(vertexA);
                    }
                        
                    if (!seenB)              
                    {
                        hashedFeatures.Add(vertexB);                                    
                        currentCluster.AddChildFeature(vertexB);
                    }                        
                }
                else
                {
                    if (currentCluster.Features.Count > 0)
                    {
                        clusters.Add(currentCluster);
                    }

                    currentCluster = new U();
                    if (!seenA && !seenB)
                    {
                        // I DONT KNOW WHAT TO DO WITH THESE ASSHOLES!
                        lowLifeFeatures.Add(vertexA);
                        lowLifeFeatures.Add(vertexB);

                        // We dont hash these guys yet, because later we'll see if they hit the market
                        // with their fake DVD's 
                    }
                    else if (!seenA)
                    {
                        currentCluster.AddChildFeature(vertexA);
                        hashedFeatures.Add(vertexA);
                    }
                    else
                    {
                        hashedFeatures.Add(vertexB);
                        currentCluster.AddChildFeature(vertexB);
                    }
                }                                                                
            }

            // Make sure we add the current cluster if it's not full yet...
            if (currentCluster.Features.Count > 0)
            {
                clusters.Add(currentCluster);
            }

            foreach (T lowLife in lowLifeFeatures)
            {
                if (!hashedFeatures.Contains(lowLife))
                {
                    U cluster = new U();
                    cluster.AddChildFeature(lowLife);
                    clusters.Add(cluster);
                }
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
