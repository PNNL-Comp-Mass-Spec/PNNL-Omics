using System;
using PNNLOmics.Data.MassTags;


namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSAlignment
{
    public class LCMSMassTimeFeature: IComparable<LCMSMassTimeFeature>
    {
        double m_mono_mass;
        double m_mono_mass_calibrated;
        double m_mono_mass_original;
        double m_mz;
        double m_net;
        double m_abundance;
        double m_aligned_net;
        double m_driftTime;
        int m_id;
        int m_conformerID;

        public MassTagLight Thingy(LCMSMassTimeFeature thing)
        {
            MassTagLight data = new MassTagLight();

            data.MassMonoisotopic = thing.MonoMass;
            data.NET = thing.NET;
            data.NETAligned = thing.AlignedNet;
            data.Abundance = Convert.ToInt64(thing.Abundance);
            data.ChargeState = (int)(thing.MonoMass/thing.MZ);


            return data;
        }

        public double AlignedNet
        {
            get { return m_aligned_net; }
            set { m_aligned_net = value; }
        }

        public double MonoMass
        {
            get { return m_mono_mass; }
            set { m_mono_mass = value; }
        }
        public double MonoMassCalibrated
        {
            get { return m_mono_mass_calibrated; }
            set { m_mono_mass_calibrated = value; }
        }
        public double MonoMassOriginal
        {
            get { return m_mono_mass_original; }
            set { m_mono_mass_original = value; }
        }
        public double MZ
        {
            get { return m_mz; }
            set { m_mz = value; }
        }
        public double NET
        {
            get { return m_net; }
            set { m_net = value; }
        }
        public double Abundance
        {
            get { return m_abundance; }
            set { m_abundance = value; }
        }
        public double DriftTime
        {
            get { return m_driftTime; }
            set { m_driftTime = value; }
        }
        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }
        public int ConformerID
        {
            get { return m_conformerID; }
            set { m_conformerID = value; }
        }

        public LCMSMassTimeFeature()
        {
            
        }

        public LCMSMassTimeFeature(LCMSMassTimeFeature copy)
        {
            m_mono_mass = copy.m_mono_mass;
            m_net = copy.m_net;
            m_id = copy.m_id;
            m_abundance = copy.m_abundance;
            m_aligned_net = copy.m_aligned_net;
            m_mono_mass_calibrated = copy.m_mono_mass_calibrated;
            m_mono_mass_original = copy.m_mono_mass_original;
            m_mz = copy.m_mz;
            m_driftTime = copy.m_driftTime;
            m_conformerID = copy.m_conformerID;
        }

        public override int GetHashCode()
        {
            return m_id;
        }

        public bool Equals(LCMSMassTimeFeature obj)
        {
            if (obj == null) return false;
            return (this.ID.Equals(obj.ID));
        }

        public int CompareTo(LCMSMassTimeFeature compareFeature)
        {
            if (compareFeature == null)
            {
                return 1;
            }
            return MonoMass.CompareTo(compareFeature.MonoMass);
        }
    }
}
