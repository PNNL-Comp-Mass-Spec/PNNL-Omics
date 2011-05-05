using System;
using System.Collections.Generic;

using NUnit.Framework;

using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Algorithms.FeatureClustering;

/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    LCMSWarp Alignment Dataset Tests
 * File:    AlignmentDatasetTests.cs
 * Author:  Brian LaMarche 
 * Purpose: Tests the data structures that hold the alignment data.
 * Date:    12-1-2010
 * Revisions:
 *          12-01-2010 - BLL - Created test class.
 *          
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using PNNLOmics.Algorithms.Alignment;
namespace PNNLOmics.UnitTests.AlgorithmTests.Alignment
{
	//[TestFixture]
	//public class AlignmentDatasetTest
	//{
	//    /// <summary>
	//    /// Constructor test.
	//    /// </summary>
	//    /// <param name="numberOfFeaturesToAdd"></param>
	//    /// <param name="sections"></param>
	//    [Test(Description="Constructor test with invalid data.")]
	//    [TestCase(-1, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(0, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(1, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(100, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(1, -1, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(1, 1, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(100, 100, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    public void ConstructorInvalid(int numberOfFeaturesToAdd,
	//                            int sections)
	//    {
	//        List<UMCLight> features = null;
	//        if (numberOfFeaturesToAdd >= 0)
	//        {
	//            features = new List<UMCLight>();
	//            for(int i = 0; i < numberOfFeaturesToAdd; i++)
	//            {
	//                features.Add(new UMCLight());
	//            }
	//        }
	//        AlignmentDataset<UMCLight> dataset = new AlignmentDataset<UMCLight>(features, sections);
	//    }
	//}
}
