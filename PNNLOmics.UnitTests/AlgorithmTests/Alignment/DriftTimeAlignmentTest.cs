using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.Alignment;
using PNNLOmics.Data;

namespace PNNLOmics.UnitTests.AlgorithmTests.Alignment
{
	[TestFixture]
	public class DriftTimeAlignmentTest
	{
		[Test]
		public void TestDriftTimeAlignment()
		{
			List<UMC> observedUMCList = null;
			List<UMC> targetUMCList = null;

			CreateObservedAndTargetUMCLists(ref observedUMCList, ref targetUMCList);

			DriftTimeAlignment<UMC, UMC>.AlignObservedEnumerable(observedUMCList, observedUMCList, targetUMCList, 20, 0.03);

			Assert.AreEqual(Math.Round(observedUMCList[0].DriftTimeAligned, 2), 17.10);
			Assert.AreEqual(Math.Round(observedUMCList[1].DriftTimeAligned, 2), 28.67);
			Assert.AreEqual(Math.Round(observedUMCList[2].DriftTimeAligned, 2), 19.31);
			Assert.AreEqual(Math.Round(observedUMCList[3].DriftTimeAligned, 2), 18.80);
			Assert.AreEqual(Math.Round(observedUMCList[4].DriftTimeAligned, 2), 20.43);
			Assert.AreEqual(Math.Round(observedUMCList[5].DriftTimeAligned, 2), 21.56);
			Assert.AreEqual(Math.Round(observedUMCList[6].DriftTimeAligned, 2), 22.67);
			Assert.AreEqual(Math.Round(observedUMCList[7].DriftTimeAligned, 2), 23.41);
			Assert.AreEqual(Math.Round(observedUMCList[8].DriftTimeAligned, 2), 18.10);
			Assert.AreEqual(Math.Round(observedUMCList[9].DriftTimeAligned, 2), 24.86);
		}

		[Test]
		public void TestLinearEquationCalculation()
		{
			List<UMC> observedUMCList = null;
			List<UMC> targetUMCList = null;

			CreateObservedAndTargetUMCLists(ref observedUMCList, ref targetUMCList);

			List<XYData> xyDataList = new List<XYData>();

			for (int i = 0; i < observedUMCList.Count; i++)
			{
				XYData xyData = new XYData(observedUMCList[i].DriftTime, targetUMCList[i].DriftTime);
				xyDataList.Add(xyData);
			}

			LinearEquation linearEquation = LinearEquationCalculator.CalculateLinearEquation(xyDataList);
			Assert.AreEqual(Math.Round(linearEquation.Slope, 4), 0.7142);
			Assert.AreEqual(Math.Round(linearEquation.Intercept, 4), 1.1324);
		}

		private void CreateObservedAndTargetUMCLists(ref List<UMC> observedUMCList, ref List<UMC> targetUMCList)
		{
			observedUMCList = new List<UMC>();
			targetUMCList = new List<UMC>();

			UMC observedFeature1 = new UMC();
			observedFeature1.MassMonoisotopic = 771.47578;
			observedFeature1.NET = 0.5;
			observedFeature1.ChargeState = 2;
			observedFeature1.DriftTime = 22.35554f;
			observedUMCList.Add(observedFeature1);

			UMC targetFeature1 = new UMC();
			targetFeature1.MassMonoisotopic = 771.47313;
			targetFeature1.NET = 0.5;
			targetFeature1.ChargeState = 2;
			targetFeature1.DriftTime = 16.99548f;
			targetUMCList.Add(targetFeature1);

			UMC observedFeature2 = new UMC();
			observedFeature2.MassMonoisotopic = 783.40238;
			observedFeature2.NET = 0.5;
			observedFeature2.ChargeState = 1;
			observedFeature2.DriftTime = 38.56024f;
			observedUMCList.Add(observedFeature2);

			UMC targetFeature2 = new UMC();
			targetFeature2.MassMonoisotopic = 783.40651;
			targetFeature2.NET = 0.5;
			targetFeature2.ChargeState = 1;
			targetFeature2.DriftTime = 28.64959f;
			targetUMCList.Add(targetFeature2);

			UMC observedFeature3 = new UMC();
			observedFeature3.MassMonoisotopic = 1045.5403;
			observedFeature3.NET = 0.5;
			observedFeature3.ChargeState = 2;
			observedFeature3.DriftTime = 25.4562f;
			observedUMCList.Add(observedFeature3);

			UMC targetFeature3 = new UMC();
			targetFeature3.MassMonoisotopic = 1045.53546;
			targetFeature3.NET = 0.5;
			targetFeature3.ChargeState = 2;
			targetFeature3.DriftTime = 19.34219f;
			targetUMCList.Add(targetFeature3);

			UMC observedFeature4 = new UMC();
			observedFeature4.MassMonoisotopic = 1059.56535;
			observedFeature4.NET = 0.5;
			observedFeature4.ChargeState = 2;
			observedFeature4.DriftTime = 24.7409f;
			observedUMCList.Add(observedFeature4);

			UMC targetFeature4 = new UMC();
			targetFeature4.MassMonoisotopic = 1059.56132;
			targetFeature4.NET = 0.5;
			targetFeature4.ChargeState = 2;
			targetFeature4.DriftTime = 18.79057f;
			targetUMCList.Add(targetFeature4);

			UMC observedFeature5 = new UMC();
			observedFeature5.MassMonoisotopic = 1227.72843;
			observedFeature5.NET = 0.5;
			observedFeature5.ChargeState = 2;
			observedFeature5.DriftTime = 27.01977f;
			observedUMCList.Add(observedFeature5);

			UMC targetFeature5 = new UMC();
			targetFeature5.MassMonoisotopic = 1227.72107;
			targetFeature5.NET = 0.5;
			targetFeature5.ChargeState = 2;
			targetFeature5.DriftTime = 20.46228f;
			targetUMCList.Add(targetFeature5);

			UMC observedFeature6 = new UMC();
			observedFeature6.MassMonoisotopic = 1346.72985;
			observedFeature6.NET = 0.5;
			observedFeature6.ChargeState = 2;
			observedFeature6.DriftTime = 28.60684f;
			observedUMCList.Add(observedFeature6);

			UMC targetFeature6 = new UMC();
			targetFeature6.MassMonoisotopic = 1346.72875;
			targetFeature6.NET = 0.5;
			targetFeature6.ChargeState = 2;
			targetFeature6.DriftTime = 21.55198f;
			targetUMCList.Add(targetFeature6);

			UMC observedFeature7 = new UMC();
			observedFeature7.MassMonoisotopic = 1453.89352;
			observedFeature7.NET = 0.5;
			observedFeature7.ChargeState = 2;
			observedFeature7.DriftTime = 30.15363f;
			observedUMCList.Add(observedFeature7);

			UMC targetFeature7 = new UMC();
			targetFeature7.MassMonoisotopic = 1453.89305;
			targetFeature7.NET = 0.5;
			targetFeature7.ChargeState = 2;
			targetFeature7.DriftTime = 22.72478f;
			targetUMCList.Add(targetFeature7);

			UMC observedFeature8 = new UMC();
			observedFeature8.MassMonoisotopic = 1524.94014;
			observedFeature8.NET = 0.5;
			observedFeature8.ChargeState = 2;
			observedFeature8.DriftTime = 31.19778f;
			observedUMCList.Add(observedFeature8);

			UMC targetFeature8 = new UMC();
			targetFeature8.MassMonoisotopic = 1524.92889;
			targetFeature8.NET = 0.5;
			targetFeature8.ChargeState = 2;
			targetFeature8.DriftTime = 23.4855f;
			targetUMCList.Add(targetFeature8);

			UMC observedFeature9 = new UMC();
			observedFeature9.MassMonoisotopic = 1621.98666;
			observedFeature9.NET = 0.5;
			observedFeature9.ChargeState = 3;
			observedFeature9.DriftTime = 23.75201f;
			observedUMCList.Add(observedFeature9);

			UMC targetFeature9 = new UMC();
			targetFeature9.MassMonoisotopic = 1621.98151;
			targetFeature9.NET = 0.5;
			targetFeature9.ChargeState = 3;
			targetFeature9.DriftTime = 18.13624f;
			targetUMCList.Add(targetFeature9);

			UMC observedFeature10 = new UMC();
			observedFeature10.MassMonoisotopic = 1757.92318;
			observedFeature10.NET = 0.5;
			observedFeature10.ChargeState = 2;
			observedFeature10.DriftTime = 33.22705f;
			observedUMCList.Add(observedFeature10);

			UMC targetFeature10 = new UMC();
			targetFeature10.MassMonoisotopic = 1757.91498;
			targetFeature10.NET = 0.5;
			targetFeature10.ChargeState = 2;
			targetFeature10.DriftTime = 24.77506f;
			targetUMCList.Add(targetFeature10);
		}
	}
}
