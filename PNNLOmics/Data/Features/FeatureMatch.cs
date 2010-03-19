using System.Collections.Generic;
using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Data
{
	public class FeatureMatch: BaseData
	{
        private Feature m_matchedFeature;

        public Feature MatchedFeature
        {
            get { return m_matchedFeature; }
            set { m_matchedFeature = value; }
        }
        private MassTag m_matchedMassTag;

        public MassTag MatchedMassTag
        {
            get { return m_matchedMassTag; }
            set { m_matchedMassTag = value; }
        }
        private double m_smartScore;

        public double SMARTScore
        {
            get { return m_smartScore; }
            set { m_smartScore = value; }
        }
        private double m_smartSpecificity;

        public double SMARTSpecificity
        {
            get { return m_smartSpecificity; }
            set { m_smartSpecificity = value; }
        }
        private double m_slicScore;

        public double SLiCScore
        {
            get { return m_slicScore; }
            set { m_slicScore = value; }
        }
        private double m_delSLiC;

        public double DelSLiC
        {
            get { return m_delSLiC; }
            set { m_delSLiC = value; }
        }
        private Matrix m_differenceVector;
        
        public Matrix DifferenceVector
        {
            get { return m_differenceVector; }
        }
        private Matrix m_validMeans;

        public Matrix ValidMeans
        {
            get { return m_validMeans; }
        }

        private UMCCluster m_matchedUMCCluster;
        public UMCCluster  MatchedUMCCluster
        {
            get
            {
                return m_matchedUMCCluster;
            }
            set
            {
                m_matchedUMCCluster = value;
            }
        }
        #region BaseData Members
        public int ID { get; set; }
        public override void Clear()
        {
            m_matchedFeature = new UMC();
            m_matchedUMCCluster = new UMCCluster();
            m_matchedMassTag = new MassTag();
            m_delSLiC = 0;
            m_slicScore = 0;
            m_smartScore = 0;
            m_smartSpecificity = 0;
            m_differenceVector = new Matrix(4, 1);
            m_validMeans = new Matrix(4, 1.0);
        }
        #endregion

        public static Comparison<FeatureMatch> UMCComparison = delegate(FeatureMatch featureMatch1, FeatureMatch featureMatch2)
        {
            return featureMatch1.m_matchedFeature.ID.CompareTo(featureMatch2.MatchedFeature.ID);
        };

        public static Comparison<FeatureMatch> UMCClusterComparison = delegate(FeatureMatch featureMatch1, FeatureMatch featureMatch2)
        {
            return featureMatch1.m_matchedUMCCluster.ID.CompareTo(featureMatch2.MatchedUMCCluster.ID);
        };

        public static Comparison<FeatureMatch> SMARTComparison = delegate(FeatureMatch featureMatch1, FeatureMatch featureMatch2)
        {
            return featureMatch1.SMARTScore.CompareTo(featureMatch2.SMARTScore);
        };

        public static Comparison<FeatureMatch> MassDifferenceComparison = delegate(FeatureMatch featureMatch1, FeatureMatch featureMatch2)
        {
            return featureMatch1.m_differenceVector[1,1].CompareTo(featureMatch2.DifferenceVector[1,1]);
        };

        public FeatureMatch(UMC umc, MassTag massTag)
        {
            Clear();
            m_matchedFeature = umc;
            m_matchedMassTag = massTag;
            SetDifferenceVector();
        }

        public FeatureMatch(UMCCluster umcCluster, MassTag massTag)
        {
            Clear();
            m_matchedUMCCluster = umcCluster;
            m_matchedMassTag = massTag;
            SetDifferenceVector();
        }

        private void SetDifferenceVector()
        {
            if (m_matchedFeature.MassMonoisotopic == 0)
            {
                m_differenceVector[1, 1] = CalculateMassDifferencePPM(m_matchedMassTag.MassMonoisotopicAligned,m_matchedUMCCluster.MassMonoisotopicAligned);
                m_differenceVector[2, 1] = m_matchedMassTag.NETAligned - m_matchedUMCCluster.NETAligned;
                if (m_matchedMassTag.DriftTime > 0)
                {
                    m_differenceVector[3, 1] = m_matchedMassTag.DriftTime - m_matchedUMCCluster.DriftTime;
                    m_differenceVector[4, 1] = 0;
                    m_validMeans[4, 4] = 0;
                }
                else if (m_matchedMassTag.DriftTimePredicted>0)
                {
                    m_differenceVector[3, 1] = 0;
                    m_validMeans[3, 3] = 0;
                    m_differenceVector[4, 1] = m_matchedMassTag.DriftTimePredicted - m_matchedUMCCluster.DriftTime;
                }
                else
                {
                    m_differenceVector[3, 1] = 0;
                    m_differenceVector[4, 1] = 0;
                    m_validMeans[3, 3] = 0;
                    m_validMeans[4, 4] = 0;
                }
            }
            else
            {
                m_differenceVector[1, 1] = CalculateMassDifferencePPM(m_matchedMassTag.MassMonoisotopicAligned,m_matchedFeature.MassMonoisotopicAligned);
                m_differenceVector[2, 1] = m_matchedMassTag.NETAligned - m_matchedFeature.NETAligned;
                if (m_matchedMassTag.DriftTime > 0)
                {
                    m_differenceVector[3, 1] = m_matchedMassTag.DriftTime - m_matchedFeature.DriftTime;
                    m_differenceVector[4, 1] = 0;
                    m_validMeans[4, 4] = 0;
                }
                else if(m_matchedMassTag.DriftTimePredicted>0)
                {
                    m_differenceVector[3, 1] = 0;
                    m_validMeans[3, 3] = 0;
                    m_differenceVector[4, 1] = m_matchedMassTag.DriftTimePredicted - m_matchedFeature.DriftTime;
                }
                else
                {
                    m_differenceVector[3, 1] = 0;
                    m_differenceVector[4, 1] = 0;
                    m_validMeans[3, 3] = 0;
                    m_validMeans[4, 4] = 0;
                }
            }
        }

        public double CalculateMassDifferencePPM(double mass1, double mass2)
        {
            return ((mass2 - mass1) / mass1 * 1000000);
        }

        public Matrix LCMSDifferenceVector()
        {
            return(m_differenceVector.GetMatrix(1,2,1,1));
        }
	}
}
