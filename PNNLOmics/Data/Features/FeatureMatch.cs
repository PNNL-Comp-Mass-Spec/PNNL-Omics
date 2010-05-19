﻿using System.Collections.Generic;
using System;
using PNNLOmics.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Algorithms.FeatureMatcher.Utilities;
using PNNLOmics.Algorithms.FeatureMatcher.Data;

namespace PNNLOmics.Data.Features
{
    public class FeatureMatch<T, U>: BaseData where T: Feature, new() where U: Feature, new()
    {
        #region Members
        private double m_stacScore;
        private double m_stacSpecificity;
        private double m_slicScore;
        private double m_delSLiC;

        private T m_observedFeature;
        private U m_targetFeature;

        private Matrix m_differenceVector;
        private Matrix m_reducedDifferenceVector;

        private bool m_useDriftTimePredicted;
        private bool m_withinRefinedRegion;
        private bool m_shiftedMatch;
        #endregion

        #region Properties
        public double STACScore
        {
            get { return m_stacScore; }
            set { m_stacScore = value; }
        }
        public double STACSpecificity
        {
            get { return m_stacSpecificity; }
            set { m_stacSpecificity = value; }
        }
        public double SLiCScore
        {
            get { return m_slicScore; }
            set { m_slicScore = value; }
        }
        public double DelSLiC
        {
            get { return m_slicScore; }
            set { m_slicScore = value; }
        }

        public T ObservedFeature
        {
            get { return m_observedFeature; }
            set { m_observedFeature = value; }
        }
        public U TargetFeature
        {
            get { return m_targetFeature; }
            set { m_targetFeature = value; }
        }

        public Matrix DifferenceVector
        {
            get { return m_differenceVector; }
        }
        public Matrix ReducedDifferenceVector
        {
            get { return m_reducedDifferenceVector; }
        }

        public bool UseDriftTimePredicted
        {
            get { return m_useDriftTimePredicted; }
        }
        public bool WithinRefinedRegion
        {
            get { return m_withinRefinedRegion; }
            set { m_withinRefinedRegion = value; }
        }
        public bool ShiftedMatch
        {
            get { return m_shiftedMatch; }
            set { m_shiftedMatch = value; }
        }
        #endregion

        #region Constructors
        public FeatureMatch()
        {
            Clear();
        }
        public FeatureMatch(T observedFeature, U targetFeature, bool useDriftTime, bool shiftedMatch)
        {
            Clear();
            m_observedFeature = observedFeature;
            m_targetFeature = targetFeature;
            m_reducedDifferenceVector = MatrixUtilities.Differences<T, U>(observedFeature, targetFeature, useDriftTime);
            m_differenceVector = MatrixUtilities.Differences<T, U>(observedFeature, targetFeature, useDriftTime, true);
            SetFlags(useDriftTime);
            m_shiftedMatch = shiftedMatch;
        }
        #endregion

        #region Comparisons
        public static Comparison<FeatureMatch<T, U>> FeatureComparison = delegate(FeatureMatch<T, U> featureMatch1, FeatureMatch<T, U> featureMatch2)
        {
            return featureMatch1.m_observedFeature.ID.CompareTo(featureMatch2.ObservedFeature.ID);
        };
        public static Comparison<FeatureMatch<T,U>> STACComparison = delegate(FeatureMatch<T,U> featureMatch1, FeatureMatch<T,U> featureMatch2)
        {
            return featureMatch1.m_stacScore.CompareTo(featureMatch2.STACScore);
        };
        #endregion

        #region Private functions
        public override void Clear()
        {
            m_observedFeature = new T();
            m_targetFeature = new U();
            m_delSLiC = 0;
            m_slicScore = 0;
            m_stacScore = 0;
            m_stacSpecificity = 0;
            m_differenceVector = new Matrix(2, 1, 0.0);
            m_useDriftTimePredicted = false;
            m_withinRefinedRegion = false;
        }
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
        public void AddFeatures(T observedFeature, U targetFeature, bool useDriftTime, bool shiftedMatch)
        {
            Clear();
            m_observedFeature = observedFeature;
            m_targetFeature = targetFeature;
            m_reducedDifferenceVector = MatrixUtilities.Differences<T, U>(observedFeature, targetFeature, useDriftTime);
            m_differenceVector = MatrixUtilities.Differences<T, U>(observedFeature, targetFeature, useDriftTime, true);
            SetFlags(useDriftTime);
            m_shiftedMatch = shiftedMatch;
        }

        public bool InRegion(FeatureMatcherTolerances tolerances, bool useElllipsoid)
        {
            if (m_targetFeature == new U())
            {
                throw new InvalidOperationException("Match must be populated before using functions involving the match.");
            }
            int dimensions = m_reducedDifferenceVector.RowCount;
            Matrix toleranceMatrix = tolerances.AsVector(true);
            if (useElllipsoid)
            {
                double distance = 0;
                for (int i = 0; i < dimensions; i++)
                {
                    distance += m_reducedDifferenceVector[i,0]*m_reducedDifferenceVector[i,0]/toleranceMatrix[i,0]/toleranceMatrix[i,0];
                }
                m_withinRefinedRegion = (distance <= 1);
            }
            else
            {
                bool truthValue = true;
                for( int i =0; i<dimensions; i++)
                {
                    truthValue = (truthValue && Math.Abs(m_reducedDifferenceVector[i, 0]) <= toleranceMatrix[i, 0]);
                }
                m_withinRefinedRegion = truthValue;
            }
            return m_withinRefinedRegion;
        }
        #endregion
    }
}
