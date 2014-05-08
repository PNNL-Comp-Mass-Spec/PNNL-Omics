using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using PNNLOmics.Utilities;

/// <example>
/// dictionary implementation                        
/// Dictionary<string,ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
/// double elementC12Mass = ElementDictionary["C"].IsotopeDictionary["C12"].Mass;
/// double elementC13Mass = ElementDictionary["C"].IsotopeDictionary["C13"].Mass;
/// double elementC12Abund = ElementDictionary["C"].IsotopeDictionary["C12"].NaturalAbundance;
/// double elementC13Abund = ElementDictionary["C"].IsotopeDictionary["C13"].NaturalAbundance;
/// double elemetMonoMass = ElementDictionary["C"].MonoIsotopicMass;
/// string elementName = ElementDictionary["C"].Name;
/// string elementSymbol = ElementDictionary["C"].Symbol;                     
///
/// One line implementation
/// double elementMonoMass = ElementConstantsStaticLibrary.GetMonoisotopicMass("C");
/// string elementName = ElementConstantsStaticLibrary.GetName("C");
/// string elementSymbol = ElementConstantsStaticLibrary.GetSymbol("C");
///
/// double elementMass3 = ElementStaticLibrary.GetMonoisotopicMass(SelectElement.Hydrogen);
/// </example>
namespace PNNLOmics.Data.Constants.Libraries
{
    /// <summary>
    /// Sets up the element library by loading the information from the disk
    /// </summary>
    public class ElementLibrary : MatterLibrary<Element, ElementName>
    {
		protected const string OMICS_ELEMENT_DATA_FILE = "PNNLOmicsElementData.xml";

        #region Loading Data
        /// <summary>
        /// This is a Class designed to load periodic table of the elements data from a XML file PNNLOmicsElementData.xml
        /// IUPAC 2000 Atomic Weights of the Elements (published 2003) was used.
        /// Differences from the old version:  Elements H, B, C, N, O, Na, P, S, Cl, K, Ca were updated.  Table 5 in the paper has the probabilities (best measurement column was used).
        /// </summary>
        private void LoadXML(string constantsFileName, out List<string> elementSymbolList, out List<Element> elementList)
        {
            var readerXML = XmlReader.Create(constantsFileName);

            var numberOfIsotopes = 0;
            var atomicity = 0;
            var isotopeNumber = 0;
            double massAverage = 0;
            double massAverageUncertainty = 0;
            double isotopeMass = 0;
            double isotopeProbability = 0;
            double monoIsotopicMass = 0;
            elementSymbolList = new List<string>();
            elementList = new List<Element>();

            while (readerXML.Read())
            {
                if (readerXML.NodeType == XmlNodeType.Element)
                {
                    if (readerXML.Name == "NumElements")
                    {
                        var numElements = readerXML.ReadElementContentAsInt();// Parse(Xreader.GetAttribute("Symbol"));
                    }

                    if (readerXML.Name == "Element")
                    {
                        var newElement = new Element();
                        var newIsotopeDictionary = new Dictionary<string, Isotope>();

                        readerXML.ReadToFollowing("Symbol");
                        newElement.Symbol = readerXML.ReadElementContentAsString();

                        readerXML.ReadToFollowing("Name");
                        newElement.Name = readerXML.ReadElementContentAsString();

                        readerXML.ReadToFollowing("NumIsotopes");
                        numberOfIsotopes = readerXML.ReadElementContentAsInt();

                        readerXML.ReadToFollowing("Atomicity");
                        atomicity = readerXML.ReadElementContentAsInt();

                        readerXML.ReadToFollowing("MassAverage");
                        massAverage = readerXML.ReadElementContentAsDouble();

                        readerXML.ReadToFollowing("MassAverageUncertainty");
                        massAverageUncertainty = readerXML.ReadElementContentAsDouble();

                        //for each isotope
                        for (var i = 0; i < numberOfIsotopes; i++)
                        {
                            readerXML.ReadToFollowing("Isotope");

                            readerXML.ReadToFollowing("IsotopeNumber");

                            isotopeNumber = readerXML.ReadElementContentAsInt();

                            readerXML.ReadToFollowing("Mass");

                            isotopeMass = readerXML.ReadElementContentAsDouble();

                            if (i == 0)
                            {
                                monoIsotopicMass = isotopeMass;
                            }

                            readerXML.ReadToFollowing("Probability");
                            isotopeProbability = readerXML.ReadElementContentAsDouble();

                            var NewIsotope = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);

                            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber, NewIsotope);
                            //newIsotopeDictionary.Add(newElement.Symbol + i.ToString(), NewIsotope);//used for interating through
                        }

                        newElement.IsotopeDictionary = newIsotopeDictionary;
                        newElement.MassMonoIsotopic = monoIsotopicMass;
                        newElement.MassAverage = massAverage;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
                        newElement.MassAverageUncertainty = massAverageUncertainty;

                        elementList.Add(newElement);
                        elementSymbolList.Add(newElement.Symbol);

                        readerXML.Skip();//skip white space
                    }
                }
            }
        }

        /// <summary>
        /// This is a Class designed to convert raw values into element objects (including masses and isotope abundances)
        /// and create an element dictionary searchable by key string such as "C" for carbon.
        /// </summary>
        public override void LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, Element>();
            m_enumToSymbolMap = new Dictionary<ElementName, string>();

            var uncPathCheck = new ResolveUNCPath.MappedDriveResolver();
            var asemblyDirectoryOrUNCDirectory = uncPathCheck.ResolveToUNC(PathUtililities.AssemblyDirectory);

            var constantsFileInfo = new FileInfo(System.IO.Path.Combine(asemblyDirectoryOrUNCDirectory, OMICS_ELEMENT_DATA_FILE));
			//FileInfo constantsFileInfo = new FileInfo(System.IO.Path.Combine(PathUtil.AssemblyDirectory, OMICS_ELEMENT_DATA_FILE));

			if (!constantsFileInfo.Exists)
            {
				throw new FileNotFoundException("The " + OMICS_ELEMENT_DATA_FILE + " file cannot be found at " + constantsFileInfo.FullName);
            }

            var elementSymbolList = new List<string>();
            var elementList = new List<Element>();

			LoadXML(constantsFileInfo.FullName, out elementSymbolList, out elementList);

            for (var i = 0; i < elementSymbolList.Count; i++)
            {
                m_symbolToCompoundMap.Add(elementSymbolList[i], elementList[i]);
            }

            var counter = 0;
            foreach (ElementName enumElement in Enum.GetValues(typeof(ElementName)))
            {
                m_enumToSymbolMap.Add(enumElement, elementList[counter].Symbol);
                counter++;
            }
        }

        #endregion
    }

}
