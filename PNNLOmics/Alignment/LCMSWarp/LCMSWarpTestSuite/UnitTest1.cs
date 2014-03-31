using System;
using System.Collections.Generic;
using Aligner;
using MTDBFramework.IO;
using MTDBFramework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PNNLOmics.Data.Features;

namespace LCMSWarpTestSuite
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AlignToSelf()
        {
            Console.Write("I'm testing!\n");
            List<UMCLight> baseline = new List<UMCLight>();
            List<UMCLight> features = new List<UMCLight>();

            FeatureMatching aligner = new FeatureMatching();
            Options options = new Options();
            var msgfReader = new MSGFPlusPHRPReader(options);
            string path1 = @"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_14-02-01\QC_Shew_13_02_2a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt";
            string path2 = @"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_14_02_02\QC_Shew_13_02_2b_03Mar14_Leopard_14-02-02_msgfdb_syn.txt";
            var dataset = msgfReader.Read(path1);
            foreach (Evidence evidence in dataset.Evidences)
            {
                var umcLightEvidence = new UMCLight();
                umcLightEvidence.NET = evidence.ObservedNet;
                umcLightEvidence.ChargeState = evidence.Charge;
                umcLightEvidence.Mz = evidence.Mz;
                umcLightEvidence.Scan = evidence.Scan;
                umcLightEvidence.MassMonoisotopic = evidence.ObservedMonoisotopicMass;
                umcLightEvidence.MassMonoisotopicAligned = evidence.MonoisotopicMass;
                umcLightEvidence.ID = evidence.AnalysisId;
                baseline.Add(umcLightEvidence);
            }

            dataset = msgfReader.Read(path2);
            foreach (var evidence in dataset.Evidences)
            {
                var umcLightEvidence = new UMCLight();
                umcLightEvidence.NET = evidence.ObservedNet;
                umcLightEvidence.ChargeState = evidence.Charge;
                umcLightEvidence.Mz = evidence.Mz;
                umcLightEvidence.Scan = evidence.Scan;
                umcLightEvidence.MassMonoisotopic = evidence.ObservedMonoisotopicMass;
                umcLightEvidence.MassMonoisotopicAligned = evidence.MonoisotopicMass;
                umcLightEvidence.ID = evidence.AnalysisId;
                features.Add(umcLightEvidence);
            }

            AlignmentData data = aligner.Align(baseline, features);

        }
    }
}
