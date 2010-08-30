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
        public static void LoadXML(string constantsFileXMLName, out List<string> elementSymbolList, out List<Element> elementList)
        {
            XmlReader Xreader = XmlReader.Create(constantsFileXMLName);

            int numberOfIsotopes = 0;
            int atomicity = 0;
            int isotopeNumber = 0;
            double isotopeMass = 0;
            double isotopeProbability = 0;
            double monoIsotopicMass = 0;
            elementSymbolList = new List<string>();
            elementList = new List<Element>();

            while (Xreader.Read())
            {
                if (Xreader.NodeType == XmlNodeType.Element)
                {
                    if (Xreader.Name == "NumElements")
                    {
                        int totalNumberOfElements = Xreader.ReadElementContentAsInt();// Parse(Xreader.GetAttribute("Symbol"));
                    }

                    if (Xreader.Name == "Element")
                    {
                        Element newElement = new Element();
                        Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();

                        Xreader.ReadToFollowing("Symbol");
                        newElement.Symbol = Xreader.ReadElementContentAsString();
                        
                        Xreader.ReadToFollowing("Name");
                        newElement.Name = Xreader.ReadElementContentAsString();

                        Xreader.ReadToFollowing("NumIsotopes");
                        numberOfIsotopes = Xreader.ReadElementContentAsInt();

                        Xreader.ReadToFollowing("Atomicity");
                        atomicity = Xreader.ReadElementContentAsInt();

                        //for each isotope
                        for (int i = 0; i < numberOfIsotopes; i++)
                        {
                            Xreader.ReadToFollowing("Isotope");
                            
                            Xreader.ReadToFollowing("IsotopeNumber");
                            
                            isotopeNumber = Xreader.ReadElementContentAsInt();

                            Xreader.ReadToFollowing("Mass");

                            isotopeMass = Xreader.ReadElementContentAsDouble();

                            if(i==0)
                            {
                                monoIsotopicMass=isotopeMass;
                            }

                            Xreader.ReadToFollowing("Probability");
                            isotopeProbability = Xreader.ReadElementContentAsDouble();

                            Isotope NewIsotope = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);

                            newIsotopeDictionary.Add(newElement.Symbol+isotopeNumber.ToString(), NewIsotope);
                            //newIsotopeDictionary.Add(newElement.Symbol + i.ToString(), NewIsotope);//used for interating through
                        }
                        newElement.IsotopeDictionary = newIsotopeDictionary;
                        newElement.MonoIsotopicMass = monoIsotopicMass;
                        newElement.MassAverage = 0;//not used yet//1.007947;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                        elementList.Add(newElement);
                        elementSymbolList.Add(newElement.Symbol);

                        Xreader.Skip();//skip white space
                    }       
                }
            }
        }
    }
}
