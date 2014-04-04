﻿using System;


namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSAlignment
{
    public class LCMSFeatureMatch: IComparable<LCMSFeatureMatch>
    {
        /// <summary>
        /// Constructor, initializes the testing values to -1 to ensure ability to see
        /// when there hasn't been a match made
        /// </summary>
        public LCMSFeatureMatch()
        {
            FeatureIndex = -1;
            FeatureIndex2 = -1;
            Net = -1;
            Net2 = -1;

        }

        /// <summary>
        /// PPM Mass Error 
        /// Equal to the difference between the feature Mass and the baseline Mass
        /// auto property
        /// </summary>
        public double PpmMassError { get; set; }

        /// <summary>
        /// Normalized Elution Time error
        /// Equal to the difference between the feature NET and the baseline NET
        /// auto property
        /// </summary>
        public double NetError { get; set; }

        /// <summary>
        /// Drift Time Error
        /// Equal to the difference between the feature Drift time and the baseline Drift time
        /// auto property
        /// </summary>
        public double DriftError { get; set; }

        /// <summary>
        /// Normalized elution time of the feature auto property
        /// </summary>
        public double Net { get; set; }

        /// <summary>
        /// Normalized elution time of the baseline feature that
        /// this feature matches to.
        /// Auto property
        /// </summary>
        public double Net2 { get; set; }

        /// <summary>
        /// Index of the feature that this match corresponds to
        /// Auto property
        /// </summary>
        public int FeatureIndex { get; set; }

        /// <summary>
        /// Index of the baseline feature that this match corresponds to
        /// Auto property
        /// </summary>
        public int FeatureIndex2 { get; set; }

        public int CompareTo(LCMSFeatureMatch compareFeature)
        {
            if (compareFeature == null)
            {
                return 1;
            }
            return Net.CompareTo(compareFeature.Net);    
        }
    }
}
