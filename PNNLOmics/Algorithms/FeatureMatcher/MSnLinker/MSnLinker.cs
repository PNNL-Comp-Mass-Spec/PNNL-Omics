using System.Collections.Generic;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using System;
using System.Linq;
using PNNLOmics.Data.Constants.Libraries;

namespace PNNLOmics.Algorithms.FeatureMatcher.MSnLinker
{
    /// <summary>
    /// Maps MS/MS data to MS features.
    /// </summary>
    public class BoxMSnLinker: IMSnLinker
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public BoxMSnLinker()
        {
            Tolerances      = new FeatureTolerances();
            Tolerances.Mass = .5;
            AdductMass      = SubAtomicParticleLibrary.MASS_PROTON;
        }
        /// <summary>
        /// Gets or sets the feature tolerances to use.
        /// </summary>
        public FeatureTolerances Tolerances
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the adduct mass (e.g. proton, H+)
        /// </summary>
        public double AdductMass
        {
            get;
            set;
        }

        public Dictionary<int, int> LinkMSFeaturesToMSn(List<MSFeatureLight> features,
            List<MSSpectra> fragmentSpectra,
            ISpectraProvider provider)
        {
            return LinkMSFeaturesToMSn(features, fragmentSpectra);
        }
        /// <summary>
        /// Links MS features to MSMS (or MSn) data.
        /// </summary>
        /// <param name="features">Features to link.</param>
        /// <param name="spectra">Spectra to link to.</param>
        /// <param name="rawSpectraProvider">Provides access to raw scans if more data is required.</param>
        /// <returns>A map of a ms spectra id to the number of times it was mapped.</returns>
        public Dictionary<int, int>  LinkMSFeaturesToMSn(List<MSFeatureLight> features, 
            List<MSSpectra> fragmentSpectra)
        {
            // First  - Sort the MSn features based on scan
            // Second - map all the features to a scan number for MS Features
            //          and a parent scan number of MSn features.  
            //          The MSSpectra list should have missing scan numbers so when sorted
            //          will be monotonically increasing with 1, 2, 3, 5, 6, 7, 9, 10, 11, 13
            //          This indicates the parent scan these features were fragmented from.
            // Third  - Once these spectra have been mapped, then we can do a quicker search
            List<MSSpectra> spectra = new List<MSSpectra>();
            spectra.AddRange(fragmentSpectra);           
            spectra.OrderBy(x => x.Scan);
                         
            // Map the scans
            Dictionary<int, List<MSFeatureLight>> featureMap    = new Dictionary<int, List<MSFeatureLight>>();
            Dictionary<int, List<MSSpectra>> spectraMap         = new Dictionary<int, List<MSSpectra>>();
            Dictionary<int, int> mappedMSSpectra                = new Dictionary<int,int>();             
            foreach(MSFeatureLight feature in features)
            {
                int scan                = feature.Scan;
                bool containsFeature    = featureMap.ContainsKey(scan);
                if (!containsFeature)
                {
                    featureMap.Add(scan, new List<MSFeatureLight>());
                }
                featureMap[scan].Add(feature);
            }
            
            int totalSpectra        = spectra.Count;
            List<MSSpectra> scans   = new List<MSSpectra>();
            int parentScan          = spectra[0].Scan - 1;
            for(int i = 1; i < totalSpectra; i++)
            {
                int prevScan    = spectra[i - 1].Scan;
                int currentScan = spectra[i].Scan;
                if (currentScan - prevScan > 1)
                {
                    // Copy current data to the map.
                    List<MSSpectra> tempSpectra = new List<MSSpectra>();
                    tempSpectra.AddRange(scans);
                    spectraMap.Add(parentScan, tempSpectra);

                    // Get ready for next MSMS scan data.
                    parentScan = currentScan - 1;
                    scans.Clear();
                }
                scans.Add(spectra[i]);
            }

            // then we search for links          
            double ppmRange     = Tolerances.Mass;
            double protonMass   = AdductMass;

            // Go through each scan, and see if there is a corresponding 
            // MSMS scan range of spectra associated, some may not have it.
            foreach(int scan in  featureMap.Keys)
            {
                bool containsMSMSFragments = spectraMap.ContainsKey(scan);
                if (containsMSMSFragments)
                {
                    // If MSMS exists there, then search all features in that mass spectra window for 
                    // close spectra.
                    List<MSSpectra> suspectSpectra = spectraMap[scan];
                    foreach (MSFeatureLight feature in featureMap[scan])
                    {
                        // Use the most abundant mass because it had a higher chance of being fragmented.
                        double mass = feature.Mz;

                        List<MSSpectra> matching = suspectSpectra.FindAll(
                                    delegate(MSSpectra x)
                                    {
                                        return Math.Abs(x.PrecursorMZ - mass) <= ppmRange;                            
                                    }
                                    );                        

                        // Finally link!
                        foreach (MSSpectra spectrum in matching)
                        {
                            int spectrumID      = spectrum.ID;
                            bool hasBeenMapped  = mappedMSSpectra.ContainsKey(spectrumID);
                            if (!hasBeenMapped)
                            {
                                mappedMSSpectra[spectrumID] = 0;
                            }
                            mappedMSSpectra[spectrumID]++;
                            feature.MSnSpectra.Add(spectrum);
                            feature.MsMsCount++;
                            spectrum.ParentFeature = feature;
                        }
                    }
                }                
            }
            return mappedMSSpectra;
        }
    }
}
