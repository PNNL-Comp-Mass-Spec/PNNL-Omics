using System;
using System.Collections.Generic;

using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.SpectralComparisons;
using PNNLOmics.Data.Constants.Libraries;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Aligns multiple datasets based on MS/MS clustering methods.
    /// </summary>
    public class MSMSClusterer: IProgressNotifer
    {
        /// <summary>
        /// Fired when progress is made.
        /// </summary>
        public event EventHandler<ProgressNotifierArgs> Progress;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MSMSClusterer()
        {
            AdductMass          = SubAtomicParticleLibrary.MASS_PROTON;
            ScanRange           = 800;
            MinimumClusterSize  = 2;
            MzTolerance         = .5;
            MassTolerance       = 6;
            
        }

        #region Properties
        /// <summary>
        /// Gets or sets the scan range.
        /// </summary>
        public int ScanRange
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the similarity tolerance to use.
        /// </summary>
        public double SimilarityTolerance
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the object used to compare two spectra for alignment matching.
        /// </summary>
        public ISpectralComparer SpectralComparer
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the minimum cluster size.
        /// </summary>
        public int MinimumClusterSize
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the m/z tolerance for precursor matches.
        /// </summary>
        public double MzTolerance
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the mass tolerance.
        /// </summary>
        public double MassTolerance
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the adduct mass e.g. Proton H+
        /// </summary>
        public double AdductMass
        {
            get;
            set;
        }
        #endregion


        private void UpdateStatus(string message)
        {
            if (this.Progress != null)
            {
                Progress(this, new ProgressNotifierArgs(message));
            }
        }
        /// <summary>
        /// Clusters spectra together based on similarity.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="features"></param>
        private List<MSMSCluster> Cluster(int start, 
                                         int stop, 
                                         List<MSFeatureLight>   features,
                                         ISpectraProvider       provider,
                                         double                 similarityTolerance)
        {
            double massTolerance = MassTolerance;

            // Maps the feature to a cluster ID.
            Dictionary<MSFeatureLight, int> featureMap  = new Dictionary<MSFeatureLight, int>();

            // Maps the cluster ID to a cluster.
            Dictionary<int, MSMSCluster> clusterMap     = new Dictionary<int, MSMSCluster>();
            List<MSMSCluster> clusters                  = new List<MSMSCluster>();

            // Create singleton clusters.
            int id = 0;
            for (int i = start; i < stop; i++)
            {
                MSFeatureLight feature  = features[i];
                MSMSCluster cluster     = new MSMSCluster();
                cluster.ID              = id++;
                cluster.MeanScore       = 0; 
                cluster.Features.Add(feature);
                
                featureMap.Add(feature,    cluster.ID);
                clusterMap.Add(cluster.ID, cluster);
            }
            double protonMass = AdductMass;

            // Then iterate and cluster.
            for (int i = start; i < stop; i++)
            {
                MSFeatureLight featureI = features[i];
                MSMSCluster clusterI    = clusterMap[featureMap[featureI]];

                for (int j = i + 1; j < stop; j++)
                {
                    
                    MSFeatureLight featureJ = features[j];
                    MSMSCluster clusterJ    = clusterMap[featureMap[featureJ]];

                    // Don't cluster the same thing
                    if (clusterI.ID == clusterJ.ID)   continue;

                    // Don't cluster from the same dataset.  Let the linkage algorithm decide if they 
                    // belong in the same cluster, and later, go back and determine if the cluster is valid or not.
                    if (featureI.GroupID == featureJ.GroupID) continue;
                    // Check the scan difference.  If it fits then we are within range.
                    int scanDiff = Math.Abs(featureI.Scan - featureJ.Scan);
                    if (scanDiff <= ScanRange)
                    {

                        // Use the most abundant mass because it had a higher chance of being fragmented.
                        double mzI = (featureI.MassMonoisotopicMostAbundant / featureI.ChargeState) + protonMass;
                        double mzJ = (featureJ.MassMonoisotopicMostAbundant / featureJ.ChargeState) + protonMass;

                        double mzDiff = Math.Abs(mzI - mzJ);
                        if (mzDiff <= MzTolerance)
                        {                            
                            if (featureI.MSnSpectra[0].Peaks.Count <= 0)
                            {
                                featureI.MSnSpectra[0].Peaks = provider.GetRawSpectra(featureI.MSnSpectra[0].Scan, featureI.GroupID);
                                featureI.MSnSpectra[0].Peaks = XYData.Bin(featureI.MSnSpectra[0].Peaks,
                                                                            0,
                                                                            2000,
                                                                            MzTolerance);
                            }
                            if (featureJ.MSnSpectra[0].Peaks.Count <= 0)
                            {
                                featureJ.MSnSpectra[0].Peaks = provider.GetRawSpectra(featureJ.MSnSpectra[0].Scan, featureJ.GroupID);
                                featureJ.MSnSpectra[0].Peaks = XYData.Bin(featureJ.MSnSpectra[0].Peaks, 
                                                                            0,
                                                                            2000,
                                                                            MzTolerance);
                            }


                            // Compute similarity 
                            double score = SpectralComparer.CompareSpectra(featureI.MSnSpectra[0], featureJ.MSnSpectra[0]);
                    
                            if (score >= similarityTolerance)
                            {
                                clusterJ.MeanScore += score;
                                foreach (MSFeatureLight xFeature in clusterI.Features)
                                {
                                    clusterJ.Features.Add(xFeature);
                                    featureMap[xFeature] = clusterJ.ID;
                                    clusterMap.Remove(clusterI.ID);
                                }
                            }
                        }
                    }
                }
            }

            clusters.AddRange(clusterMap.Values);

            for (int i = start; i < stop; i++)
            {
                features[i].MSnSpectra[0].Peaks.Clear();
            }
            foreach (MSMSCluster cluster in clusters)
            {
                cluster.MeanScore /= (cluster.Features.Count - 1);
            }
            return clusters;
        }
        /// <summary>
        /// Aligns features based on MSMS spectral similarity.
        /// </summary>
        /// <param name="featureMap"></param>
        /// <param name="msms"></param>
        public List<MSMSCluster> Cluster(List<UMCLight> features, ISpectraProvider provider)
        {
           
            UpdateStatus("Mapping UMC's to MS/MS spectra using intensity profile.");
            // Step 1: Cluster the spectra 
            // Create the collection of samples.
            List<MSFeatureLight> msFeatures = new List<MSFeatureLight>();

            // Sort through the features
            foreach(UMCLight feature in features)
            {
                // Sort out charge states...?
                Dictionary<int, MSFeatureLight> chargeMap = new Dictionary<int,MSFeatureLight>();

                long   abundance            = long.MinValue;
                MSFeatureLight maxFeature   = null;

                // Find the max abundance spectrum.  This the number of features we have to search.
                foreach(MSFeatureLight msFeature in feature.MSFeatures)
                {
                    if (msFeature.Abundance > abundance && msFeature.MSnSpectra.Count > 0)
                    {
                        abundance   = msFeature.Abundance;
                        maxFeature  = msFeature; 
                    }
                }

                if (maxFeature != null)
                {
                    msFeatures.Add(maxFeature);
                }
            }

            UpdateStatus(string.Format("Found {0} total spectra for clustering.", msFeatures.Count));

            UpdateStatus("Sorting spectra.");
            // Sort based on mass using the max abundance of the feature.
            msFeatures.Sort(delegate(MSFeatureLight x, MSFeatureLight y) 
                { return x.MassMonoisotopicMostAbundant.CompareTo(y.MassMonoisotopicMostAbundant); });

            // Then cluster the spectra.
            int j = 1;
            int h = 0;
            int N = msFeatures.Count;

            List<MSMSCluster> clusters  = new List<MSMSCluster>();
            double tol                  = MassTolerance;
            int lastTotal               = 0;
            UpdateStatus("Clustering spectra.");
            while(j < N)
            {
                int i = j - 1;
                MSFeatureLight featureJ = msFeatures[j];
                MSFeatureLight featureI = msFeatures[i];
                double diff             = Feature.ComputeMassPPMDifference(featureJ.MassMonoisotopicMostAbundant, featureI.MassMonoisotopicMostAbundant);

                if (Math.Abs(diff) > tol)
                {
                    // We only care to create clusters of size greater than one.
                    if ((j - h) > 1)
                    {
                        List<MSMSCluster> data = Cluster(   h,
                                                            j,
                                                            msFeatures,
                                                            provider,
                                                            SimilarityTolerance);
                        clusters.AddRange(data);
                    }

                    // Reset the count, we're done looking at those clusters.
                    h = j;
                }
                if (j - lastTotal > 500)
                {
                    lastTotal = j;
                    UpdateStatus(string.Format("Processed {0} / {1} total spectra.", lastTotal, N));
                }
                j++;
            }            
            UpdateStatus("Finishing last cluster data.");
            
            // Cluster the rest 
            if ((j - h) > 1)
            {
                List<MSMSCluster> data = Cluster(   h,
                                                    j,
                                                    msFeatures,
                                                    provider,
                                                    SimilarityTolerance);
                clusters.AddRange(data);
            }
            UpdateStatus("Finished clustering.");            
            List<MSMSCluster> passingClusters = clusters.FindAll(delegate (MSMSCluster cluster)
                                                                    {
                                                                        return (cluster.Features.Count >= MinimumClusterSize);
                                                                    });
            return passingClusters;
        }
    }   
}
