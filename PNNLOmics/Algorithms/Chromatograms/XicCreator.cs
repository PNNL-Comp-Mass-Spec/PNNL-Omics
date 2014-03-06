using PNNLOmics.Algorithms.FeatureClustering;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Extensions;

namespace PNNLOmics.Algorithms.Chromatograms
{
    public class XicFeature : Chromatogram, IComparable<XicFeature>
    {
        public double LowMz { get; set; }
        public double HighMz { get; set; }                
        public int Id { get; set; }
        public UMCLight Feature { get; set; }


        /// <summary>
        /// Compares this xic feature to another based on m/z
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(XicFeature other)
        {
            return Mz.CompareTo(other.Mz);
        }
    }

    public class XicCreator
    {
        public XicCreator()
        {
            ScanWindowSize = 100;
            FragmentationSizeWindow = .5;
        }

        public void CreateXic(IList<UMCLight> features, 
                              double            massError,
                              ISpectraProvider  provider)
        {
            // this algorithm works as follows
            // 
            //  PART A - Build the XIC target list 
            //  For each UMC Light , find the XIC representation
            //      for each charge in a feature          
            //          from start scan to end scan
            //              1. Compute a lower / upper m/z bound
            //              2. build an XIC chomatogram object
            //              3. reference the original UMC Feature -- this allows us to easily add 
            //                  chromatograms to the corresponding feature
            //              4. store the chomatogram (with unique ID across all features)
            //
            //  PART B - Read Data From File
            //  Sort the list of XIC's by scan
            //  for each scan s = start scan to end scan 
            //      1. find all xic's that start before and end after s - 
            //          a. cache these xics in a dictionary based on unique id
            //          b. NOTE: this is why we sort so we can do an O(N) search for 
            //             all XIC's that need data from this scan s
            //      2.  Then for each XIC that needs data
            //          a. Pull intensity data from lower / upper m/z bound 
            //          b. create an MS Feature
            //          c. store in original UMC Feature
            //          d. Test to see if the XIC is done building (Intensity < 1 or s > scan end)
            //      3. Remove features that are done building from cache
            // 
            //  CONCLUSIONS
            //  Building UMC's then takes linear time  (well O(N Lg N) time if you consider sort)
            //      and theoretically is only bounded by the time it takes to read an entire raw file
            // 
            if (features.Count <= 0) return;

            var minScan = Math.Max(0, features.Min(x => x.Scan - ScanWindowSize));
            var maxScan = features.Max(x => x.Scan + ScanWindowSize);


            // PART A 
            // Map the feature ID to the xic based features
            var xicFeatures = new SortedSet<XicFeature>();
            var allFeatures = CreateXicTargets(features, massError);

            // PART B 
            // sort the features...
            var m           = allFeatures.Count;
            allFeatures     = allFeatures.OrderBy(x => x.StartScan).ToList();

            // This map tracks all possible features to keep
            var featureMap  = new Dictionary<int, XicFeature>();
            int j           = 0;  // this is our feature index
            int msFeatureId = 0;
            
            // This list stores a temporary amount of parent MS features
            // so that we can link MS/MS spectra to MS Features
            var parentMsList = new List<MSFeatureLight>();

            // Creates a comparison function for building a BST from a spectrum.
            Func<XYData, XYData, int> bstComparisonFunc = (data, xyData) => data.X.CompareTo(xyData.X);
            var msmsFeatureId = 0;

            // Iterate over all the scans...
            for (int s = minScan; s < maxScan; s++)
            {
                // Find any features that need data from this scan, s 
                while (j < m)
                {
                    var xicFeature = allFeatures[j];
                    // This means that no new features were eluting with this scan....
                    if (xicFeature.StartScan > s)
                        break;

                    // This means that there is a new feature...
                    if (s < xicFeature.EndScan)
                    {
                       // if (!xicFeatures.ContainsKey(xicFeature.Id))
                       //     xicFeatures.Add(xicFeature.Id, xicFeature);
                        xicFeatures.Add(xicFeature);
                    }
                    j++;
                }

                // Skip pulling the data from the file if there is nothing to pull from.
                if (xicFeatures.Count < 1)
                    continue;

                // Here We link the MSMS Spectra to the UMC Features
                var summary           = new ScanSummary();
                List<XYData> spectrum = null;
                try
                {
                    spectrum = provider.GetRawSpectra(s, 0, 1, out summary);                
                }
                catch (Exception)
                {
                    // Since we dont control the last scan,
                    // here if there is an error we are betting that it's the last 
                    // scan...hopefully...
                    break;
                }
                
                if (summary.MsLevel > 1)
                {
                    // If it is an MS 2 spectra... then let's link it to the parent MS
                    // Feature
                    var matching = parentMsList.FindAll(
                        x => Math.Abs(x.Mz - summary.PrecursorMZ) <= FragmentationSizeWindow
                        );

                    var spectraData         = new MSSpectra
                    {
                        ID                  = msmsFeatureId++,
                        ScanMetaData        = summary,
                        Scan                = s,
                        Peaks               = spectrum,
                        MSLevel             = summary.MsLevel,
                        PrecursorMZ         = summary.PrecursorMZ,
                        TotalIonCurrent     = summary.TotalIonCurrent
                    };

                    foreach (var match in matching)
                    {
                        match.MSnSpectra.Add(spectraData);
                    }

                    continue;
                }



                var sortedList = spectrum.OrderBy(x => x.X).ToList();
                

                // Tracks which spectra need to be removed from the cache
                var toRemove = new List<XicFeature>();
                // Tracks which features we need to link to MSMS spectra with
                parentMsList.Clear();
                
                // now we iterate through all features that need data from this scan
                //foreach (var xic in xicFeatures.Values)
                int k = 0;
                foreach (var xic in xicFeatures)
                {                    
                    var lower  = xic.LowMz;
                    var higher = xic.HighMz;


                    while (sortedList[k].X < lower)
                    {
                        k++;
                    }

                    double summedIntensity = 0;
                    while (sortedList[k].X <= higher)
                    {
                        summedIntensity += sortedList[k++].Y;
                    }                    
                    // See if we need to remove this feature
                    // We only do so if the intensity has dropped off and we are past the end of the feature.
                    if (summedIntensity < 1 && xic.EndScan < s)
                    {
                        toRemove.Add(xic);
                        continue;
                    }

                    var umc = xic.Feature;

                    // otherwise create a new feature here...
                    var msFeature               = new MSFeatureLight
                    {
                        ChargeState      = xic.ChargeState,   
                        Mz               = xic.Mz,
                        MassMonoisotopic = umc.MassMonoisotopic,
                        Scan             = s,
                        Abundance        = Convert.ToInt64(summedIntensity),
                        ID               = msFeatureId++,
                        DriftTime        = umc.DriftTime,
                        RetentionTime    = s,
                        GroupID          = umc.GroupID
                    };
                    parentMsList.Add(msFeature);
                    xic.Feature.AddChildFeature(msFeature);
                }

                toRemove.ForEach(x => xicFeatures.Remove(x));
            }
        }
        /// <summary>
        /// Creates XIC Targets from a list of UMC Features
        /// </summary>
        /// <param name="features"></param>
        /// <param name="massError"></param>
        /// <returns></returns>
        private  List<XicFeature> CreateXicTargets(IEnumerable<UMCLight> features, double massError)
        {
            
            var allFeatures = new List<XicFeature>();

            // Create XIC Features
            var id = 0;
            // Then for each feature turn it into a new feature
            foreach (var feature in features)
            {
                // Build XIC features from each
                var x = feature.CreateChargeMap();
                foreach (var charge in x.Keys)
                {
                    long maxIntensity = 0;
                    double mz = 0;
                    var min = double.MaxValue;
                    var max = double.MinValue;

                    int scanStart = int.MaxValue;
                    int scanEnd = 0;

                    foreach (var chargeFeature in x[charge])
                    {
                        min         = Math.Min(min, chargeFeature.Mz);
                        max         = Math.Max(max, chargeFeature.Mz);
                        scanStart   = Math.Min(scanStart, chargeFeature.Scan);
                        scanEnd     = Math.Min(scanStart, chargeFeature.Scan);

                        if (chargeFeature.Abundance > maxIntensity)
                        {
                            maxIntensity = chargeFeature.Abundance;
                            mz = chargeFeature.Mz;
                        }
                    }

                    // Clear the ms feature list...because later we will populate it
                    feature.MSFeatures.Clear();

                    var xicFeature = new XicFeature()
                    {
                        Area        = 0,
                        HighMz      = Feature.ComputeDaDifferenceFromPPM(mz, -massError),
                        LowMz       = Feature.ComputeDaDifferenceFromPPM(mz, massError),
                        Mz          = mz,
                        Feature     = feature,
                        Id          = id++,
                        EndScan     = scanEnd + ScanWindowSize,
                        StartScan   = scanStart - ScanWindowSize,
                        ChargeState = charge
                    };

                    allFeatures.Add(xicFeature);
                }
            }

            return allFeatures;
        }

        /// <summary>
        /// Creates an XIC from the given set of target features.
        /// </summary>
        /// <param name="massError">Mass error to use when pulling peaks</param>
        /// <param name="msFeatures">Seed features that provide the targets</param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IEnumerable<MSFeatureLight> CreateXic(   IList<MSFeatureLight> msFeatures, 
                                                        double massError,                  
                                                        ISpectraProvider provider)
        {
            var newFeatures = new List<MSFeatureLight>();

            if (msFeatures.Count <= 0) return newFeatures;

            var minScan  = msFeatures[0].Scan;
            var maxScan  = msFeatures[msFeatures.Count - 1].Scan;
            minScan     -= 100;
            maxScan     += 100;
            minScan      = Math.Max(0, minScan); 

            var  min             = double.MaxValue;
            var  max             = double.MinValue;
            long maxIntensity    = 0;
            var  featureMap      = new Dictionary<int, MSFeatureLight>();
            double mz            = 0;
            foreach (var chargeFeature in msFeatures)
            {
                min = Math.Min(min, chargeFeature.Mz);
                max = Math.Max(max, chargeFeature.Mz);                    

                if (chargeFeature.Abundance > maxIntensity)
                {
                    maxIntensity = chargeFeature.Abundance;
                    mz           = chargeFeature.Mz;
                }

                // Map the feature...
                if (!featureMap.ContainsKey(chargeFeature.Scan))
                {
                    featureMap.Add(chargeFeature.Scan, chargeFeature);
                }
            }

            var features = CreateXic(mz, massError, minScan, maxScan, provider);
            foreach (var msFeature in features)
            {
                var scan = msFeature.Scan;
                if (featureMap.ContainsKey(msFeature.Scan))                
                    featureMap[scan].Abundance = msFeature.Abundance;                                
                newFeatures.Add(msFeature);                
            }
            return newFeatures;
        }
        /// <summary>
        /// Creates an XIC from the m/z values provided.
        /// </summary>
        /// <param name="mz"></param>
        /// <param name="massError"></param>
        /// <param name="minScan"></param>
        /// <param name="maxScan"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IEnumerable<MSFeatureLight> CreateXic(double mz,
                                                     double massError,
                                                     int minScan,
                                                     int maxScan,
                                                     ISpectraProvider provider)
        {

            var newFeatures = new List<MSFeatureLight>();
            var lower  = Feature.ComputeDaDifferenceFromPPM(mz, massError);
            var higher = Feature.ComputeDaDifferenceFromPPM(mz, -massError);

                        

            for (var i = minScan; i < maxScan; i++)
            {
                List<XYData> spectrum = null;

                try
                {
                    var summary = new ScanSummary();
                    spectrum = provider.GetRawSpectra(i, 0, 1, out summary);
                }catch
                {
                    
                }

                if (spectrum == null)
                    continue;

                var data = (from x in spectrum
                            where x.X > lower && x.X < higher
                            select x).ToList();

                var summedIntensity = data.Sum(x => x.Y);

               
                var newFeature = new MSFeatureLight
                {
                    Scan = i,
                    RetentionTime = i,
                    Abundance = Convert.ToInt64(summedIntensity)
                };
                newFeatures.Add(newFeature);                
            }
            return newFeatures;   
        }

        public IDictionary<int, IList<MSFeatureLight>> CreateXic(UMCLight feature, double massError, ISpectraProvider provider)
        {
            var features        = new Dictionary<int, IList<MSFeatureLight>>();
            var chargeFeatures  = feature.CreateChargeMap();

            // For each UMC...
            foreach (var charge in chargeFeatures.Keys)
            {
                // Find the mininmum and maximum features                             
                var msFeatures = CreateXic(chargeFeatures[charge], 
                                            massError,
                                            provider);

                features.Add(charge, new List<MSFeatureLight>());

                foreach (var newFeature in msFeatures)
                {
                    // Here we ask if this is a new MS Feature or old...
                    if (!chargeFeatures.ContainsKey(newFeature.Scan))
                    {
                        // Otherwise add the new feature
                        newFeature.MassMonoisotopic = feature.MassMonoisotopic;
                        newFeature.DriftTime        = feature.DriftTime;
                        newFeature.GroupID          = feature.GroupID;
                    }
                    features[charge].Add(newFeature);
                }
            }
            return features;
        }
        /// <summary>
        /// Gets or sets how many scans to add before and after an initial XIC target
        /// </summary>
        public int ScanWindowSize { get; set; }
        /// <summary>
        /// Gets or sets the size of the m/z window to use when linking MS Features to MS/MS spectra
        /// </summary>
        public double FragmentationSizeWindow { get; set; }

    }

}
