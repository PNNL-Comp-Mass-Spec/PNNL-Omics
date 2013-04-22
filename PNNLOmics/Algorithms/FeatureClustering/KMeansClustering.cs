//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using PNNLOmics.Data.Features;

//namespace PNNLOmics.Algorithms.FeatureClustering
//{
//    public class KMeansClustering : IClusterer<UMCLight, UMCClusterLight>
//    {

//        #region IClusterer<UMCLight,UMCCluster> Members

//        public FeatureClusterParameters<UMCLight> Parameters
//        {
//            get;
//            set;
//        }

//        public int K
//        {
//            get;
//            set;
//        }

//        public List<UMCClusterLight> Cluster(List<UMCLight> data, List<UMCClusterLight> clusters)
//        {            
//            if (K < 1)
//                throw new Exception("The number of clusters is incorrect.  Should be greather than 0.");

//            if (clusters.Count > K)
//            {
//                // Randomly assigns the clusters based on the data provided.                
//                Random randomAssignment             = new Random();
//                for (int i = K; i < clusters.Count; i++)
//                {
//                    foreach(UMCLight feature in clusters[i].UMCList)
//                    {
//                        int clusterId = randomAssignment.Next(0, K - 1);                        
//                        clusters[clusterId].AddChildFeature(feature);                        
//                    }
//                }                
//            }

//            int     iterations = 0;
//            int     movements  = 0;

//            List<double> meanVariance = new List<double>();


//            while (iterations > 100)
//            {
//                // First find what cluster I best belong to.
//                foreach (UMCClusterLight cluster in clusters)
//                {
//                    foreach (UMCLight feature in cluster.UMCList)
//                    {
                        
//                    }
//                }
//            }
           
//            return clusters;
//        }

//        public List<UMCClusterLight> Cluster(List<UMCLight> data)
//        {.
//            // Randomly assigns the clusters based on the data provided.
//            List<UMCClusterLight> newClusters = new List<UMCClusterLight>();
//            for (int i = 0; i < K; i++)
//            {
//                newClusters.Add(new UMCClusterLight());
//            }

//            // Randomly assign 
//            Random randomAssignment = new Random();
//            foreach (UMCLight feature in data)
//            {
//                int clusterId = randomAssignment.Next(0, K-1);
//                newClusters[clusterId].AddChildFeature(feature);
//            }

//            return Cluster(data, newClusters);
//        }

//        #endregion
//    }

//    public class KMeansClusteringIterator
//    {

//    }
//}
