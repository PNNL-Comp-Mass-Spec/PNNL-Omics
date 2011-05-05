using System;
using System.Collections.Generic;

using NUnit.Framework;

using PNNLOmics.IO;
using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Algorithms.Alignment;
using PNNLOmics.Algorithms.FeatureClustering;

/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    LCMSWarp Alignment Tests
 * File:    LCMSWarpTests.cs
 * Author:  Brian LaMarche 
 * Purpose: Tests the ported LCMSWarp algorithm.
 * Date:    12-1-2010
 * Revisions:
 *          12-01-2010 - BLL - Created test class.
 *          
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

namespace PNNLOmics.UnitTests.AlgorithmTests.Alignment
{
	//[TestFixture]
	//public class  LCMSWarpTests        
	//{
	//    /// <summary>
	//    /// Runs Test Case AB aligning features file A to features file B.
	//    /// </summary>
	//    private const int TEST_CASE_AB = 0;

	//    /// <summary>
	//    /// Tests the constructor passing valid and invalid data types.
	//    /// </summary>
	//    /// <param name="aligneeSectionCount"></param>
	//    /// <param name="referenceSectionCount"></param>
	//    /// <param name="expansionFactor"></param>
	//    /// <param name="discontinuousNETSections"></param>
	//    /// <param name="discontinuousMassSections"></param>
	//    [Test(Description = "Tests the constructor of the alignment processor.")]
	//    [TestCase(0, 0, 0, 0, 0,  ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(-1, 0, 0, 0, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(0, -1, 0, 0, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(0, 0, 0, 0, 0,  ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(0, 0, -1, 0, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(0, 0, 0, -1, 0, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(0, 0, 0, 0, -1, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    [TestCase(100, 100, 100, 100, 100)]
	//    public void Constructor(int aligneeSectionCount, 
	//                            int referenceSectionCount, 
	//                            int expansionFactor,
	//                            int discontinuousNETSections, 
	//                            int discontinuousMassSections)
	//    {
	//        // Properties are tested inherently because data checks are performed
	//        // there.
	//        LCMSWarp<UMCLight, UMCLight> warper = new LCMSWarp<UMCLight, UMCLight>(aligneeSectionCount, 
	//                                       referenceSectionCount, 
	//                                       expansionFactor,
	//                                       discontinuousNETSections, 
	//                                       discontinuousMassSections);           
	//    }
	//    /// <summary>
	//    /// Tests Null feature lists.
	//    /// </summary>
	//    /// <param name="aligneeSectionCount"></param>
	//    /// <param name="referenceSectionCount"></param>
	//    /// <param name="expansionFactor"></param>
	//    /// <param name="discontinuousNETSections"></param>
	//    /// <param name="discontinuousMassSections"></param>
	//    [Test(Description = "Aligns null feature list")]
	//    [TestCase(100, 100, 3, 5, 5, ExpectedException = typeof(NullReferenceException))]
	//    public void AlignNullFeatures(int aligneeSectionCount,
	//                            int referenceSectionCount,
	//                            int expansionFactor,
	//                            int discontinuousNETSections,
	//                            int discontinuousMassSections)
	//    {
	//        LCMSWarp<UMCLight, UMCLight> warper = new LCMSWarp<UMCLight, UMCLight>(aligneeSectionCount,
	//                                       referenceSectionCount,
	//                                       expansionFactor,
	//                                       discontinuousNETSections,
	//                                       discontinuousMassSections);

	//        warper.Align(null, null);
	//    }
	//    /// <summary>
	//    /// Tests empty feature lists.
	//    /// </summary>
	//    /// <param name="aligneeSectionCount"></param>
	//    /// <param name="referenceSectionCount"></param>
	//    /// <param name="expansionFactor"></param>
	//    /// <param name="discontinuousNETSections"></param>
	//    /// <param name="discontinuousMassSections"></param>
	//    [Test(Description = "Aligns empty feature list")]
	//    [TestCase(100, 100, 3, 5, 5, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    public void AlignEmptyFeatures(int aligneeSectionCount,
	//                            int referenceSectionCount,
	//                            int expansionFactor,
	//                            int discontinuousNETSections,
	//                            int discontinuousMassSections)
	//    {
	//        LCMSWarp<UMCLight, UMCLight> warper = new LCMSWarp<UMCLight, UMCLight>(aligneeSectionCount,
	//                                       referenceSectionCount,
	//                                       expansionFactor,
	//                                       discontinuousNETSections,
	//                                       discontinuousMassSections);

	//        warper.Align(new List<UMCLight>(), new List<UMCLight>());
	//    }
	//    /// <summary>
	//    /// Tests empty feature lists.
	//    /// </summary>
	//    /// <param name="aligneeSectionCount"></param>
	//    /// <param name="referenceSectionCount"></param>
	//    /// <param name="expansionFactor"></param>
	//    /// <param name="discontinuousNETSections"></param>
	//    /// <param name="discontinuousMassSections"></param>
	//    [Test(Description = "Aligns empty feature list")]
	//    [TestCase(TEST_CASE_AB, ExpectedException = typeof(InvalidAlignmentParameterException))]
	//    public void AlignFeaturesWithSameNETS(int caseToUse)
	//    {
	//        string fileNameAlignee = null;
	//        string fileNameReference = null;

	//        switch (caseToUse)
	//        {
	//            case TEST_CASE_AB:
	//                fileNameAlignee   = DataResources.featuresAFilePath;
	//                fileNameReference = DataResources.featuresBFilePath;
	//                break;
	//        }

	//        Assert.NotNull(fileNameAlignee);
	//        Assert.NotNull(fileNameReference);

	//        LCMSWarp<UMCLight, UMCLight> warper = new LCMSWarp<UMCLight, UMCLight>( 100,
	//                                                            100,
	//                                                            3,
	//                                                            5,
	//                                                            5);

	//        UMCLightTextReader reader      = new UMCLightTextReader();
	//        List<UMCLight> alignee     = reader.ReadFile(fileNameAlignee) as List<UMCLight>;
	//        List<UMCLight> reference   = reader.ReadFile(fileNameAlignee) as List<UMCLight>;

	//        LCMSWarp<UMCLight, UMCLight>.NormalizeElutionTimes(alignee);
	//        LCMSWarp<UMCLight, UMCLight>.NormalizeElutionTimes(reference);

	//        List<UMCLight> newFeatures = warper.Align(alignee, reference);
	//    }
        
    
	//    /// <summary>
	//    /// Tests empty feature lists.
	//    /// </summary>
	//    /// <param name="aligneeSectionCount"></param>
	//    /// <param name="referenceSectionCount"></param>
	//    /// <param name="expansionFactor"></param>
	//    /// <param name="discontinuousNETSections"></param>
	//    /// <param name="discontinuousMassSections"></param>
	//    [Test(Description = "Aligns two datasets together from two files.")]
	//    [TestCase(TEST_CASE_AB)]        
	//    public void AlignFeatureFiles(int caseToUse)
	//    {
	//        string fileNameAlignee = null;
	//        string fileNameReference = null;

	//        switch (caseToUse)
	//        {
	//            case TEST_CASE_AB:
	//                fileNameAlignee = DataResources.featuresAFilePath;
	//                fileNameReference = DataResources.featuresBFilePath;
	//                break;
	//        }

	//        Assert.NotNull(fileNameAlignee);
	//        Assert.NotNull(fileNameReference);

	//        LCMSWarp<UMCLight, UMCLight> warper = new LCMSWarp<UMCLight, UMCLight>( 100,
	//                                                            100,
	//                                                            3,
	//                                                            5, 
	//                                                            5);

	//        UMCLightTextReader reader  = new UMCLightTextReader();
	//        List<UMCLight> alignee     = reader.ReadFile(fileNameAlignee) as List<UMCLight>;
	//        List<UMCLight> reference   = reader.ReadFile(fileNameReference) as List<UMCLight>;

	//        LCMSWarp<UMCLight, UMCLight>.NormalizeElutionTimes(alignee);
	//        LCMSWarp<UMCLight, UMCLight>.NormalizeElutionTimes(reference);           
	//        List<UMCLight> newFeatures = warper.Align(alignee, reference);

	//        UMCLightTextWriter writer = new UMCLightTextWriter();
	//        writer.WriteFile("AlignedData.txt", newFeatures);
	//    }        
	//}
}