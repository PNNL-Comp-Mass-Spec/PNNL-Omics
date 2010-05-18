using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Utilities;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
	public class FeatureMatcherParameters
    {
        #region Members
        private Tolerances m_userTolerances;

        private bool m_useEllipsoid;
        private bool m_useTrapezoid;
        private bool m_calculateShiftFDR;
        private bool m_calculateSTAC;
        private bool m_calculateHistogramFDR;
        private bool m_calculateSLiC;
        private bool m_useDriftTime;
        private bool m_usePriors;

        private List<double> m_chargeStateList;

        private double m_shiftAmount;
        private double m_histogramBinWidth;
        #endregion

        #region Properties
        public Tolerances UserTolerances
        {
            get { return m_userTolerances; }
            set { m_userTolerances = value; }
        }
        public void SetTolerances(double massTolerance, double netTolerance, float driftTimeTolerance)
        {
            m_userTolerances.MassTolerancePPM = massTolerance;
            m_userTolerances.NETTolerance = netTolerance;
            m_userTolerances.DriftTimeTolerance = driftTimeTolerance;
        }

        public bool UseEllipsoid
        {
            get { return m_useEllipsoid; }
            set { m_useEllipsoid = value; }
        }
        public bool UseTrapezoid
        {
            get { return m_useTrapezoid; }
            set { m_useTrapezoid = value; }
        }
        public bool CalculateShiftFDR
        {
            get { return m_calculateShiftFDR; }
            set { m_calculateShiftFDR = value; }
        }
        public bool CalculateSTAC
        {
            get { return m_calculateSTAC; }
            set { m_calculateSTAC = value; }
        }
        public bool CalculateHistogramFDR
        {
            get { return m_calculateHistogramFDR; }
            set { m_calculateHistogramFDR = value; }
        }
        public bool CalculateSLiC
        {
            get { return m_calculateSLiC; }
            set { m_calculateSLiC = value; }
        }
        public bool UseDriftTime
        {
            get { return m_useDriftTime; }
            set { m_useDriftTime = value; }
        }
        public bool UsePriors
        {
            get { return m_usePriors; }
            set { m_usePriors = value; }
        }

        public List<double> ChargeStateList
        {
            get { return m_chargeStateList; }
            set { m_chargeStateList = value; }
        }

        public double ShiftAmount
        {
            get{ return m_shiftAmount; }
            set{ m_shiftAmount = value; }
        }
        public double HistogramBinWidth
        {
            get { return m_histogramBinWidth; }
            set { m_histogramBinWidth = value; }
        }
        #endregion

        #region Constructors
        public FeatureMatcherParameters()
		{
			Clear();
		}
        #endregion

        #region Private functions
        private void Clear()
		{
            m_userTolerances = new Tolerances();
            m_useEllipsoid = true;
            m_useTrapezoid = false;
            m_calculateShiftFDR = true;
            m_calculateSTAC = true;
            m_calculateHistogramFDR = false;
            m_calculateSLiC = true;
            m_useDriftTime = false;
            m_usePriors = true;
            m_chargeStateList = new List<double>();
            m_shiftAmount = 11.0;
            m_histogramBinWidth = 0.02;
        }
        #endregion   
    }
}
