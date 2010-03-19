﻿using System;

namespace PNNLOmics.Data
{
    public class ScanIMS: Scan
    {
        private float m_driftTime;

        public float DriftTime
        {
            get { return m_driftTime; }
            set { m_driftTime = value; }
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}
