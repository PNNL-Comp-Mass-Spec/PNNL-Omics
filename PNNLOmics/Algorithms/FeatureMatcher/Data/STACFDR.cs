﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
    public class STACFDR
    {
        #region Members
        private double m_cutoff;
        private double m_fdr;

        private int m_matches;
        private int m_falseMatches;

        private String m_label;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the FDR for the given cutoff.
        /// </summary>
        public double FDR
        {
            get { return m_fdr; }
            set { m_fdr = value; }
        }
        /// <summary>
        /// Gets or sets the cutoff.
        /// </summary>
        public double Cutoff
        {
            get { return m_cutoff; }
            set { m_cutoff = value; }
        }

        /// <summary>
        /// Gets or sets the number of matches for the cutoff level.
        /// </summary>
        public int Matches
        {
            get { return m_matches; }
            set { m_matches = value; }
        }
        /// <summary>
        /// Gets or sets the number of false matches at the cutoff.
        /// </summary>
        public int FalseMatches
        {
            get { return m_falseMatches; }
            set { m_falseMatches = value; }
        }

        /// <summary>
        /// Gets or sets the display for the cutoff.
        /// </summary>
        public String Label
        {
            get { return m_label; }
            set { m_label = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Generic constructor.  Sets cutoff to 0.
        /// </summary>
        public STACFDR()
        {
            Clear();
        }
        /// <summary>
        /// Constructor to set cutoff to passed value.
        /// </summary>
        /// <param name="cutoff">Value at which to set cutoff.</param>
        public STACFDR(double cutoff)
        {
            Clear();
            SetLabel(cutoff);
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Sets the label for the given cutoff.
        /// </summary>
        /// <param name="cutoff">Cutoff to set a label for.</param>
        public void SetLabel(double cutoff)
        {
            m_cutoff = cutoff;
            if (cutoff > 0)
            {
                m_label = String.Concat(">=", cutoff);
            }
            else
            {
                m_label = String.Concat(">", cutoff);
            }
        }
        /// <summary>
        /// Set the cutoff to 0 with a 100% FDR.
        /// </summary>
        public void Clear()
        {
            m_fdr = 1;
            SetLabel(0);
            m_matches = 0;
            m_falseMatches = 0;
        }
        /// <summary>
        /// Fills in the required data for a cutoff level.
        /// </summary>
        /// <param name="fdr">The false discovery rate (FDR) at the cutoff.</param>
        /// <param name="matches">The number of matches found at the cutoff.</param>
        /// <param name="falseMatches">The estimated number of false matches found at the cutoff.  Rounded to an integer value.</param>
        public void FillLine(double fdr, int matches, int falseMatches)
        {
            m_fdr = fdr;
            m_falseMatches = falseMatches;
            m_matches = matches;
        }
        #endregion
    }
}