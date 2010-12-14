using System.Collections.Generic;
using System;
using PNNLOmics.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Algorithms.FeatureMatcher.Utilities;
using PNNLOmics.Algorithms.FeatureMatcher.Data;

namespace PNNLOmics.Data.Features
{
    public class FeatureMatch<T, U> : BaseData
        where T : Feature, new()
        where U : Feature, new()
    {
        #region Members
        private bool m_useDriftTime;
        private bool m_useDriftTimePredicted;
        private bool m_withinRefinedRegion;
        private bool m_shiftedMatch;

        private double m_stacScore;
        private double m_stacSpecificity;
        private double m_slicScore;
        private double m_delSLiC;

        private Matrix m_differenceVector;
        private Matrix m_reducedDifferenceVector;

        private T m_observedFeature;
        private U m_targetFeature;
        #endregion

        #region Properties
        /// <summary>
        /// Gets whether the drift time provided was predicted.
        /// </summary>
        public bool UseDriftTimePredicted
        {
            get { return m_useDriftTimePredicted; }
        }
        /// <summary>
        /// Gets or sets whether the match was within the refined region.
        /// </summary>
        public bool WithinRefinedRegion
        {
            get { return m_withinRefinedRegion; }
            set { m_withinRefinedRegion = value; }
        }
        /// <summary>
        /// Gets or sets whether the match is a shifted match.
        /// </summary>
        public bool ShiftedMatch
        {
            get { return m_shiftedMatch; }
            set { m_shiftedMatch = value; }
        }

        /// <summary>
        /// Gets or sets the STAC score for the match.
        /// </summary>
        public double STACScore
        {
            get { return m_stacScore; }
            set { m_stacScore = value; }
        }
        /// <summary>
        /// Gets or sets the STAC Specificity of the match.
        /// </summary>
        public double STACSpecificity
        {
            get { return m_stacSpecificity; }
            set { m_stacSpecificity = value; }
        }
        /// <summary>
        /// Gets or sets the SLiC score for the match.
        /// </summary>
        public double SLiCScore
        {
            get { return m_slicScore; }
            set { m_slicScore = value; }
        }
        /// <summary>
        /// Gets or sets the delSLiC for the match.
        /// </summary>
        public double DelSLiC
        {
            get { return m_delSLiC; }
            set { m_delSLiC = value; }
        }

        /// <summary>
        /// Gets the difference vector between the matched features.  This includes both observed and predicted drift times where appropriate.
        /// </summary>
        public Matrix DifferenceVector
        {
            get { return m_differenceVector; }
        }
        /// <summary>
        /// Gets the distance matrix with only applicable dimensions.
        /// </summary>
        public Matrix ReducedDifferenceVector
        {
            get { return m_reducedDifferenceVector; }
        }

        /// <summary>
        /// Gets the observed feature, i.e. the feature seen in the analysis.
        /// </summary>
        public T ObservedFeature
        {
            get { return m_observedFeature; }
        }
        /// <summary>
        /// Gets the target feature, i.e. the feature that was matched to.
        /// </summary>
        public U TargetFeature
        {
            get { return m_targetFeature; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless constructor.  Must use AddFeatures function before attempting to use match.
        /// </summary>
        public FeatureMatch()
        {
            Clear();
        }
        /// <summary>
        /// Constructor that takes in all necessary information.
        /// </summary>
        /// <param name="observedFeature">Feature observed in experiment.  Typically a UMC or UMCCluster.</param>
        /// <param name="targetFeature">Feature to match to.  Typically an AMTTag.</param>
        /// <param name="useDriftTime">Whether to use the drift time in distance vectors.</param>
        /// <param name="shiftedMatch">Whether the match is the result of a fixed shift.</param>
        public FeatureMatch(T observedFeature, U targetFeature, bool useDriftTime, bool shiftedMatch)
        {
            AddFeatures(observedFeature, targetFeature, useDriftTime, shiftedMatch);
        }
        #endregion

        #region Comparisons
        /// <summary>
        /// Comparison function for sorting by feature ID.
        /// </summary>
        public static Comparison<FeatureMatch<T, U>> FeatureComparison = delegate(FeatureMatch<T, U> featureMatch1, FeatureMatch<T, U> featureMatch2)
        {
            return featureMatch1.m_observedFeature.ID.CompareTo(featureMatch2.ObservedFeature.ID);
        };
        /// <summary>
        /// Comparison function for sorting by STAC score.
        /// </summary>
        public static Comparison<FeatureMatch<T, U>> STACComparison = delegate(FeatureMatch<T, U> featureMatch1, FeatureMatch<T, U> featureMatch2)
        {
            return featureMatch1.m_stacScore.CompareTo(featureMatch2.STACScore);
        };
        #endregion

        #region Private functions
        /// <summary>
        /// Sets internal flag as to whether drift time or predicted drift time is used.
        /// </summary>
        /// <param name="useDriftTime"></param>
        private void SetFlags(bool useDriftTime)
        {
            if (useDriftTime)
            {
                if (m_targetFeature.DriftTime == 0)
                {
                    m_useDriftTimePredicted = true;
                }
            }
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Resets all member variables to default values.  Must use AddFeatures to add features after use.
        /// </summary>
        public override void Clear()
        {
            m_observedFeature = new T();
            m_targetFeature = new U();
            m_delSLiC = 0;
            m_slicScore = 0;
            m_stacScore = 0;
            m_stacSpecificity = 0;
            m_differenceVector = new Matrix(2, 1, 0.0);
            m_reducedDifferenceVector = m_differenceVector;
            m_useDriftTimePredicted = false;
            m_withinRefinedRegion = false;
            m_shiftedMatch = false;
        }
        /// <summary>
        /// Add (or replace) features in a match.
        /// </summary>
        /// <param name="observedFeature">Feature observed in experiment.  Typically a UMC or UMCCluster.</param>
        /// <param name="targetFeature">Feature to match to.  Typically an AMTTag.</param>
        /// <param name="useDriftTime">Whether to use the drift time in distance vectors.</param>
        /// <param name="shiftedMatch">Whether the match is the result of a fixed shift.</param>
        public void AddFeatures(T observedFeature, U targetFeature, bool useDriftTime, bool shiftedMatch)
        {
            Clear();
            m_observedFeature = observedFeature;
            m_targetFeature = targetFeature;
            m_useDriftTime = useDriftTime;
            m_shiftedMatch = shiftedMatch;
        }
        /// <summary>
        /// Sets the internal flag as to whether the match is within the given tolerances.
        /// </summary>
        /// <param name="tolerances">Tolerances to use for matching.</param>
        /// <param name="useElllipsoid">Whether to use ellipsoidal region for matching.</param>
        /// <returns></returns>
        public bool InRegion(FeatureMatcherTolerances tolerances, bool useElllipsoid)
        {
            if (m_targetFeature == new U())
            {
                throw new InvalidOperationException("Match must be populated before using functions involving the match.");
            }
            Matrix toleranceMatrix = tolerances.AsVector(true);
            if (m_reducedDifferenceVector != new Matrix(2, 1, 0.0))
            {
                int dimensions = m_reducedDifferenceVector.RowCount;

                if (useElllipsoid)
                {
                    double distance = 0;
                    for (int i = 0; i < dimensions; i++)
                    {
                        distance += m_reducedDifferenceVector[i, 0] * m_reducedDifferenceVector[i, 0] / toleranceMatrix[i, 0] / toleranceMatrix[i, 0];
                    }
                    m_withinRefinedRegion = (distance <= 1);
                }
                else
                {
                    bool truthValue = true;
                    for (int i = 0; i < dimensions; i++)
                    {
                        truthValue = (truthValue && Math.Abs(m_reducedDifferenceVector[i, 0]) <= toleranceMatrix[i, 0]);
                    }
                    m_withinRefinedRegion = truthValue;
                }
            }
            else
            {
                if (useElllipsoid)
                {
                    double distance = 0;
                    double massDiff = m_observedFeature.MassMonoisotopicAligned - m_targetFeature.MassMonoisotopicAligned;
                    double netDiff = m_observedFeature.NETAligned - m_targetFeature.NETAligned;
                    distance += massDiff * massDiff / toleranceMatrix[1, 0] / toleranceMatrix[1, 0];
                    distance += netDiff * netDiff / toleranceMatrix[2, 0] / toleranceMatrix[2, 0];
                    // Add drift time difference.
                    m_withinRefinedRegion = (distance <= 1);
                }
                else
                {
                    //bool truthValue = true;
                    //truthValue = (truthValue && Math.Abs(m_reducedDifferenceVector[i, 0]) <= toleranceMatrix[i, 0]);
                    //m_withinRefinedRegion = truthValue;
                }
            }
            return m_withinRefinedRegion;
        }

        public void SetDifferenceMatrices()
        {
            m_reducedDifferenceVector = MatrixUtilities.Differences<T, U>(m_observedFeature, m_targetFeature, m_useDriftTime);
            m_differenceVector = MatrixUtilities.Differences<T, U>(m_observedFeature, m_targetFeature, m_useDriftTime, true);
            SetFlags(m_useDriftTime);
        }
        #endregion
    }
}
