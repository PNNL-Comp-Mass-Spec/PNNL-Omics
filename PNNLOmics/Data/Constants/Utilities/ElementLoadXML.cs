using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class ElementLoadXML
    {
        /// <summary>
        /// This is a Class designed to load periodic table of the elements data from a XML file PNNLOmicsElementData.xml
        /// IUPAC 2000 Atomic Weights of the Elements (published 2003) was used.
        /// Differences from the old version:  Elements H, B, C, N, O, Na, P, S, Cl, K, Ca were updated.  Table 5 in the paper has the probabilities (best measurement column was used).
        /// </summary>
        public static void LoadXML(string constantsFileName, out List<string> elementSymbolList, out List<Element> elementList)
        {
            XmlReader readerXML = XmlReader.Create(constantsFileName);

            int numberOfIsotopes = 0;
            int atomicity = 0;
            int isotopeNumber = 0;
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
                        int numElements = readerXML.ReadElementContentAsInt();// Parse(Xreader.GetAttribute("Symbol"));
                    }

                    if (readerXML.Name == "Element")
                    {
                        Element newElement = new Element();
                        Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();

                        readerXML.ReadToFollowing("Symbol");
                        newElement.Symbol = readerXML.ReadElementContentAsString();
                        
                        readerXML.ReadToFollowing("Name");
                        newElement.Name = readerXML.ReadElementContentAsString();

                        readerXML.ReadToFollowing("NumIsotopes");
                        numberOfIsotopes = readerXML.ReadElementContentAsInt();

                        readerXML.ReadToFollowing("Atomicity");
                        atomicity = readerXML.ReadElementContentAsInt();

                        //for each isotope
                        for (int i = 0; i < numberOfIsotopes; i++)
                        {
                            readerXML.ReadToFollowing("Isotope");
                            
                            readerXML.ReadToFollowing("IsotopeNumber");
                            
                            isotopeNumber = readerXML.ReadElementContentAsInt();

                            readerXML.ReadToFollowing("Mass");

                            isotopeMass = readerXML.ReadElementContentAsDouble();

                            if(i==0)
                            {
                                monoIsotopicMass=isotopeMass;
                            }

                            readerXML.ReadToFollowing("Probability");
                            isotopeProbability = readerXML.ReadElementContentAsDouble();

                            Isotope NewIsotope = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);

                            newIsotopeDictionary.Add(newElement.Symbol+isotopeNumber.ToString(), NewIsotope);
                            //newIsotopeDictionary.Add(newElement.Symbol + i.ToString(), NewIsotope);//used for interating through
                        }
                        newElement.IsotopeDictionary = newIsotopeDictionary;
                        newElement.MonoIsotopicMass = monoIsotopicMass;
                        newElement.MassAverage = 0;//not used yet//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                        elementList.Add(newElement);
                        elementSymbolList.Add(newElement.Symbol);

                        readerXML.Skip();//skip white space
                    }       
                }
            }
        }
    }
}
