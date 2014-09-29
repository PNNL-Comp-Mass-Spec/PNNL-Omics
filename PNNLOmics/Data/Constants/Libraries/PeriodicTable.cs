using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.Libraries
{
    /// <summary>
    /// hard coded periodic table of the elements
    /// </summary>
    public static class PeriodicTable
    {
        //PeriodicTable.SetElement_H(ref elementSymbolList, ref elementList);
        public static void SetElement_H(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "H";
            newElement.Name = "Hydrogen";
            // atomicity = 1;
            massAverage = 1.00794;
            massAverageUncertainty = 0.00007;


            isotopeNumber = 1;
            isotopeMass = 1.00782503196;
            isotopeProbability = 0.999844265;
            Isotope NewIsotope1 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope1);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 2;
            isotopeMass = 2.01410177796;
            isotopeProbability = 0.000155745;
            Isotope NewIsotope2 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope2);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_He(ref elementSymbolList, ref elementList);
        public static void SetElement_He(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "He";
            newElement.Name = "Helium";
            // atomicity = 2;
            massAverage = 4.002602;
            massAverageUncertainty = 0.000002;


            isotopeNumber = 3;
            isotopeMass = 3.01603;
            isotopeProbability = 0.00000134313;
            Isotope NewIsotope3 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope3);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 4;
            isotopeMass = 4.0026;
            isotopeProbability = 0.99999865713;
            Isotope NewIsotope4 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope4);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Li(ref elementSymbolList, ref elementList);
        public static void SetElement_Li(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Li";
            newElement.Name = "Lithium";
            // atomicity = 3;
            massAverage = 6.941;
            massAverageUncertainty = 0.002;


            isotopeNumber = 6;
            isotopeMass = 6.015121;
            isotopeProbability = 0.075;
            Isotope NewIsotope6 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope6);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 7;
            isotopeMass = 7.016003;
            isotopeProbability = 0.925;
            Isotope NewIsotope7 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope7);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Be(ref elementSymbolList, ref elementList);
        public static void SetElement_Be(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Be";
            newElement.Name = "Beryllium";
            // atomicity = 4;
            massAverage = 9.012182;
            massAverageUncertainty = 0.000003;


            isotopeNumber = 9;
            isotopeMass = 9.012182;
            isotopeProbability = 1;
            Isotope NewIsotope9 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope9);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_B(ref elementSymbolList, ref elementList);
        public static void SetElement_B(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "B";
            newElement.Name = "Boron";
            // atomicity = 5;
            massAverage = 10.811;
            massAverageUncertainty = 0.007;


            isotopeNumber = 10;
            isotopeMass = 10.01293713;
            isotopeProbability = 0.19822;
            Isotope NewIsotope10 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope10);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 11;
            isotopeMass = 11.00930554;
            isotopeProbability = 0.80182;
            Isotope NewIsotope11 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope11);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_C(ref elementSymbolList, ref elementList);
        public static void SetElement_C(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "C";
            newElement.Name = "Carbon";
            // atomicity = 6;
            massAverage = 12.0107;
            massAverageUncertainty = 0.0008;


            isotopeNumber = 12;
            isotopeMass = 12;
            isotopeProbability = 0.98892228;
            Isotope NewIsotope12 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope12);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 13;
            isotopeMass = 13.0033548385;
            isotopeProbability = 0.01107828;
            Isotope NewIsotope13 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope13);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_N(ref elementSymbolList, ref elementList);
        public static void SetElement_N(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "N";
            newElement.Name = "Nitrogen";
            // atomicity = 7;
            massAverage = 14.0067;
            massAverageUncertainty = 0.0002;


            isotopeNumber = 14;
            isotopeMass = 14.003074007418;
            isotopeProbability = 0.9963374;
            Isotope NewIsotope14 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope14);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 15;
            isotopeMass = 15.00010897312;
            isotopeProbability = 0.0036634;
            Isotope NewIsotope15 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope15);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_O(ref elementSymbolList, ref elementList);
        public static void SetElement_O(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "O";
            newElement.Name = "Oxygen";
            // atomicity = 8;
            massAverage = 15.9994;
            massAverageUncertainty = 0.0003;


            isotopeNumber = 16;
            isotopeMass = 15.994914622325;
            isotopeProbability = 0.99762065;
            Isotope NewIsotope16 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope16);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 17;
            isotopeMass = 16.9991315022;
            isotopeProbability = 0.00037909;
            Isotope NewIsotope17 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope17);

            isotopeNumber = 18;
            isotopeMass = 17.99916049;
            isotopeProbability = 0.00200045;
            Isotope NewIsotope18 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope18);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_F(ref elementSymbolList, ref elementList);
        public static void SetElement_F(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "F";
            newElement.Name = "Fluorine";
            // atomicity = 9;
            massAverage = 18.9984032;
            massAverageUncertainty = 0.0000005;


            isotopeNumber = 19;
            isotopeMass = 18.9984;
            isotopeProbability = 1;
            Isotope NewIsotope19 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope19);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ne(ref elementSymbolList, ref elementList);
        public static void SetElement_Ne(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ne";
            newElement.Name = "Neon";
            // atomicity = 10;
            massAverage = 20.1797;
            massAverageUncertainty = 0.0006;


            isotopeNumber = 20;
            isotopeMass = 19.99244;
            isotopeProbability = 0.9048;
            Isotope NewIsotope20 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope20);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 21;
            isotopeMass = 20.99384;
            isotopeProbability = 0.0027;
            Isotope NewIsotope21 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope21);

            isotopeNumber = 22;
            isotopeMass = 21.99138;
            isotopeProbability = 0.0925;
            Isotope NewIsotope22 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope22);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Na(ref elementSymbolList, ref elementList);
        public static void SetElement_Na(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Na";
            newElement.Name = "Sodium";
            // atomicity = 11;
            massAverage = 22.98976928;
            massAverageUncertainty = 0.00000002;


            isotopeNumber = 23;
            isotopeMass = 22.9897696626;
            isotopeProbability = 1;
            Isotope NewIsotope23 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope23);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Mg(ref elementSymbolList, ref elementList);
        public static void SetElement_Mg(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Mg";
            newElement.Name = "Magnesium";
            // atomicity = 12;
            massAverage = 24.305;
            massAverageUncertainty = 0.006;


            isotopeNumber = 24;
            isotopeMass = 23.98504;
            isotopeProbability = 0.7899;
            Isotope NewIsotope24 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope24);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 25;
            isotopeMass = 24.98584;
            isotopeProbability = 0.1;
            Isotope NewIsotope25 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope25);

            isotopeNumber = 26;
            isotopeMass = 25.98259;
            isotopeProbability = 0.1101;
            Isotope NewIsotope26 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope26);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Al(ref elementSymbolList, ref elementList);
        public static void SetElement_Al(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Al";
            newElement.Name = "Alumunium";
            // atomicity = 13;
            massAverage = 26.9815386;
            massAverageUncertainty = 0.0000008;


            isotopeNumber = 27;
            isotopeMass = 26.98154;
            isotopeProbability = 1;
            Isotope NewIsotope27 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope27);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Si(ref elementSymbolList, ref elementList);
        public static void SetElement_Si(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Si";
            newElement.Name = "Silicon";
            // atomicity = 14;
            massAverage = 28.0855;
            massAverageUncertainty = 0.0003;


            isotopeNumber = 28;
            isotopeMass = 27.97693;
            isotopeProbability = 0.9223;
            Isotope NewIsotope28 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope28);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 29;
            isotopeMass = 28.9765;
            isotopeProbability = 0.0467;
            Isotope NewIsotope29 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope29);

            isotopeNumber = 30;
            isotopeMass = 29.97377;
            isotopeProbability = 0.031;
            Isotope NewIsotope30 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope30);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_P(ref elementSymbolList, ref elementList);
        public static void SetElement_P(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "P";
            newElement.Name = "Phosphorous";
            // atomicity = 15;
            massAverage = 30.973762;
            massAverageUncertainty = 0.000002;


            isotopeNumber = 31;
            isotopeMass = 30.9737614927;
            isotopeProbability = 1;
            Isotope NewIsotope31 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope31);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_S(ref elementSymbolList, ref elementList);
        public static void SetElement_S(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "S";
            newElement.Name = "Sulfur";
            // atomicity = 16;
            massAverage = 32.065;
            massAverageUncertainty = 0.005;


            isotopeNumber = 32;
            isotopeMass = 31.9720707315;
            isotopeProbability = 0.950407488;
            Isotope NewIsotope32 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope32);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 33;
            isotopeMass = 32.9714585415;
            isotopeProbability = 0.00748696;
            Isotope NewIsotope33 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope33);

            isotopeNumber = 34;
            isotopeMass = 33.9678668714;
            isotopeProbability = 0.041959966;
            Isotope NewIsotope34 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope34);

            isotopeNumber = 36;
            isotopeMass = 35.9670808825;
            isotopeProbability = 0.0001457989;
            Isotope NewIsotope36 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope36);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Cl(ref elementSymbolList, ref elementList);
        public static void SetElement_Cl(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cl";
            newElement.Name = "Chlorine";
            // atomicity = 17;
            massAverage = 35.453;
            massAverageUncertainty = 0.002;


            isotopeNumber = 35;
            isotopeMass = 34.968852714;
            isotopeProbability = 0.7577145;
            Isotope NewIsotope35 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope35);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 37;
            isotopeMass = 36.965902605;
            isotopeProbability = 0.2422945;
            Isotope NewIsotope37 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope37);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ar(ref elementSymbolList, ref elementList);
        public static void SetElement_Ar(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ar";
            newElement.Name = "Argon";
            // atomicity = 18;
            massAverage = 39.948;
            massAverageUncertainty = 0.001;


            isotopeNumber = 36;
            isotopeMass = 35.96754;
            isotopeProbability = 0.00337;
            Isotope NewIsotope36 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope36);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 38;
            isotopeMass = 37.96273;
            isotopeProbability = 0.00063;
            Isotope NewIsotope38 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope38);

            isotopeNumber = 40;
            isotopeMass = 39.96238;
            isotopeProbability = 0.996;
            Isotope NewIsotope40 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope40);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_K(ref elementSymbolList, ref elementList);
        public static void SetElement_K(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "K";
            newElement.Name = "Potassium";
            // atomicity = 19;
            massAverage = 39.0983;
            massAverageUncertainty = 0.0001;


            isotopeNumber = 39;
            isotopeMass = 38.96370693;
            isotopeProbability = 0.9325811292;
            Isotope NewIsotope39 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope39);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 40;
            isotopeMass = 39.9639986729;
            isotopeProbability = 0.0001167241;
            Isotope NewIsotope40 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope40);

            isotopeNumber = 41;
            isotopeMass = 40.9618259728;
            isotopeProbability = 0.0673022292;
            Isotope NewIsotope41 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope41);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ca(ref elementSymbolList, ref elementList);
        public static void SetElement_Ca(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ca";
            newElement.Name = "Calcium";
            // atomicity = 20;
            massAverage = 40.078;
            massAverageUncertainty = 0.004;


            isotopeNumber = 40;
            isotopeMass = 39.96259123;
            isotopeProbability = 0.969416;
            Isotope NewIsotope40 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope40);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 42;
            isotopeMass = 41.95861834;
            isotopeProbability = 0.006473;
            Isotope NewIsotope42 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope42);

            isotopeNumber = 43;
            isotopeMass = 42.95876685;
            isotopeProbability = 0.001352;
            Isotope NewIsotope43 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope43);

            isotopeNumber = 44;
            isotopeMass = 43.95548119;
            isotopeProbability = 0.020864;
            Isotope NewIsotope44 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope44);

            isotopeNumber = 46;
            isotopeMass = 45.953692725;
            isotopeProbability = 0.000041;
            Isotope NewIsotope46 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope46);

            isotopeNumber = 48;
            isotopeMass = 47.9525334;
            isotopeProbability = 0.001871;
            Isotope NewIsotope48 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope48);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Sc(ref elementSymbolList, ref elementList);
        public static void SetElement_Sc(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Sc";
            newElement.Name = "Scandium";
            // atomicity = 21;
            massAverage = 44.955912;
            massAverageUncertainty = 0.000006;


            isotopeNumber = 45;
            isotopeMass = 44.95591;
            isotopeProbability = 1;
            Isotope NewIsotope45 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope45);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }


        //PeriodicTable.SetElement_Ti(ref elementSymbolList, ref elementList);
        public static void SetElement_Ti(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ti";
            newElement.Name = "Titanium";
            // atomicity = 22;
            massAverage = 47.867;
            massAverageUncertainty = 0.001;


            isotopeNumber = 46;
            isotopeMass = 45.95263;
            isotopeProbability = 0.08;
            Isotope NewIsotope46 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope46);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 47;
            isotopeMass = 46.95176;
            isotopeProbability = 0.073;
            Isotope NewIsotope47 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope47);

            isotopeNumber = 48;
            isotopeMass = 47.94795;
            isotopeProbability = 0.738;
            Isotope NewIsotope48 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope48);

            isotopeNumber = 49;
            isotopeMass = 48.94787;
            isotopeProbability = 0.055;
            Isotope NewIsotope49 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope49);

            isotopeNumber = 50;
            isotopeMass = 49.94479;
            isotopeProbability = 0.054;
            Isotope NewIsotope50 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope50);
            
            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_V(ref elementSymbolList, ref elementList);
        public static void SetElement_V(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "V";
            newElement.Name = "Vanadium";
            // atomicity = 23;
            massAverage = 50.9415;
            massAverageUncertainty = 0.0001;


            isotopeNumber = 50;
            isotopeMass = 49.94716;
            isotopeProbability = 0.0025;
            Isotope NewIsotope50 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope50);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 51;
            isotopeMass = 50.94396;
            isotopeProbability = 0.9975;
            Isotope NewIsotope51 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope51);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Cr(ref elementSymbolList, ref elementList);
        public static void SetElement_Cr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cr";
            newElement.Name = "Chromium";
            // atomicity = 24;
            massAverage = 51.9961;
            massAverageUncertainty = 0.0006;


            isotopeNumber = 50;
            isotopeMass = 49.94604;
            isotopeProbability = 0.04345;
            Isotope NewIsotope50 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope50);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 52;
            isotopeMass = 51.94051;
            isotopeProbability = 0.8379;
            Isotope NewIsotope52 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope52);

            isotopeNumber = 53;
            isotopeMass = 52.94065;
            isotopeProbability = 0.095;
            Isotope NewIsotope53 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope53);

            isotopeNumber = 54;
            isotopeMass = 53.93888;
            isotopeProbability = 0.02365;
            Isotope NewIsotope54 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope54);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Mn(ref elementSymbolList, ref elementList);
        public static void SetElement_Mn(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Mn";
            newElement.Name = "Manganese";
            // atomicity = 25;
            massAverage = 54.938045;
            massAverageUncertainty = 0.000005;


            isotopeNumber = 55;
            isotopeMass = 54.93805;
            isotopeProbability = 1;
            Isotope NewIsotope55 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope55);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_Fe(ref elementSymbolList, ref elementList);
        public static void SetElement_Fe(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Fe";
            newElement.Name = "Iron";
            // atomicity = 26;
            massAverage = 55.845;
            massAverageUncertainty = 0.002;


            isotopeNumber = 54;
            isotopeMass = 53.93961;
            isotopeProbability = 0.059;
            Isotope NewIsotope54 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope54);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 56;
            isotopeMass = 55.93494;
            isotopeProbability = 0.9172;
            Isotope NewIsotope56 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope56);

            isotopeNumber = 57;
            isotopeMass = 56.93539;
            isotopeProbability = 0.021;
            Isotope NewIsotope57 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope57);

            isotopeNumber = 58;
            isotopeMass = 57.93328;
            isotopeProbability = 0.0028;
            Isotope NewIsotope58 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope58);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Co(ref elementSymbolList, ref elementList);
        public static void SetElement_Co(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Co";
            newElement.Name = "Cobalt";
            // atomicity = 27;
            massAverage = 58.933195;
            massAverageUncertainty = 0.000005;


            isotopeNumber = 59;
            isotopeMass = 58.9332;
            isotopeProbability = 1;
            Isotope NewIsotope59 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope59);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

           
            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ni(ref elementSymbolList, ref elementList);
        public static void SetElement_Ni(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ni";
            newElement.Name = "Nickel";
            // atomicity = 28;
            massAverage = 58.6934;
            massAverageUncertainty = 0.0004;


            isotopeNumber = 58;
            isotopeMass = 57.93534;
            isotopeProbability = 0.6827;
            Isotope NewIsotope58 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope58);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 60;
            isotopeMass = 59.93079;
            isotopeProbability = 0.261;
            Isotope NewIsotope60 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope60);

            isotopeNumber = 61;
            isotopeMass = 60.93106;
            isotopeProbability = 0.0113;
            Isotope NewIsotope61 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope61);

            isotopeNumber = 62;
            isotopeMass = 61.92834;
            isotopeProbability = 0.0359;
            Isotope NewIsotope62 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope62);

            isotopeNumber = 64;
            isotopeMass = 63.92797;
            isotopeProbability = 0.0091;
            Isotope NewIsotope64 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope64);


            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Cu(ref elementSymbolList, ref elementList);
        public static void SetElement_Cu(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cu";
            newElement.Name = "Copper";
            // atomicity = 29;
            massAverage = 63.546;
            massAverageUncertainty = 0.003;


            isotopeNumber = 63;
            isotopeMass = 62.9396;
            isotopeProbability = 0.6917;
            Isotope NewIsotope63 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope63);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 65;
            isotopeMass = 64.9278;
            isotopeProbability = 0.3083;
            Isotope NewIsotope65 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope65);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Zn(ref elementSymbolList, ref elementList);
        public static void SetElement_Zn(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Zn";
            newElement.Name = "Zinc";
            // atomicity = 30;
            massAverage = 65.38;
            massAverageUncertainty = 0.02;


            isotopeNumber = 64;
            isotopeMass = 63.92915;
            isotopeProbability = 0.486;
            Isotope NewIsotope64 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope64);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 66;
            isotopeMass = 65.92603;
            isotopeProbability = 0.279;
            Isotope NewIsotope66 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope66);

            isotopeNumber = 67;
            isotopeMass = 66.92713;
            isotopeProbability = 0.041;
            Isotope NewIsotope67 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope67);

            isotopeNumber = 68;
            isotopeMass = 67.92484;
            isotopeProbability = 0.188;
            Isotope NewIsotope68 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope68);

            isotopeNumber = 70;
            isotopeMass = 69.92532;
            isotopeProbability = 0.006;
            Isotope NewIsotope70 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope70);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ga(ref elementSymbolList, ref elementList);
        public static void SetElement_Ga(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ga";
            newElement.Name = "Gallium";
            // atomicity = 31;
            massAverage = 69.723;
            massAverageUncertainty = 0.001;


            isotopeNumber = 69;
            isotopeMass = 68.92558;
            isotopeProbability = 0.60108;
            Isotope NewIsotope69 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope69);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 71;
            isotopeMass = 70.9247;
            isotopeProbability = 0.39892;
            Isotope NewIsotope71 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope71);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Ge(ref elementSymbolList, ref elementList);
        public static void SetElement_Ge(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ge";
            newElement.Name = "Germanium";
            // atomicity = 32;
            massAverage = 72.64;
            massAverageUncertainty = 0.01;


            isotopeNumber = 70;
            isotopeMass = 69.92425;
            isotopeProbability = 0.205;
            Isotope NewIsotope70 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope70);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 72;
            isotopeMass = 71.92208;
            isotopeProbability = 0.274;
            Isotope NewIsotope72 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope72);

            isotopeNumber = 73;
            isotopeMass = 72.92346;
            isotopeProbability = 0.078;
            Isotope NewIsotope73 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope73);

            isotopeNumber = 74;
            isotopeMass = 73.92118;
            isotopeProbability = 0.365;
            Isotope NewIsotope74 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope74);

            isotopeNumber = 76;
            isotopeMass = 75.9214;
            isotopeProbability = 0.078;
            Isotope NewIsotope76 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope76);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_As(ref elementSymbolList, ref elementList);
        public static void SetElement_As(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "As";
            newElement.Name = "Arsenic";
            // atomicity = 33;
            massAverage = 74.9216;
            massAverageUncertainty = 0.0002;


            isotopeNumber = 75;
            isotopeMass = 74.92159;
            isotopeProbability = 1;
            Isotope NewIsotope75 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope75);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Se(ref elementSymbolList, ref elementList);
        public static void SetElement_Se(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Se";
            newElement.Name = "Selenium";
            // atomicity = 34;
            massAverage = 78.96;
            massAverageUncertainty = 0.03;


            isotopeNumber = 74;
            isotopeMass = 73.92248;
            isotopeProbability = 0.009;
            Isotope NewIsotope74 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope74);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 76;
            isotopeMass = 75.91921;
            isotopeProbability = 0.091;
            Isotope NewIsotope76 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope76);

            isotopeNumber = 77;
            isotopeMass = 76.91991;
            isotopeProbability = 0.076;
            Isotope NewIsotope77 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope77);

            isotopeNumber = 78;
            isotopeMass = 77.919;
            isotopeProbability = 0.236;
            Isotope NewIsotope78 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope78);

            isotopeNumber = 80;
            isotopeMass = 79.91652;
            isotopeProbability = 0.499;
            Isotope NewIsotope80 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope80);

            isotopeNumber = 82;
            isotopeMass = 81.91669;
            isotopeProbability = 0.089;
            Isotope NewIsotope82 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope82);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            


        }


        //PeriodicTable.SetElement_Br(ref elementSymbolList, ref elementList);
        public static void SetElement_Br(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Br";
            newElement.Name = "Bromine";
            // atomicity = 35;
            massAverage = 79.904;
            massAverageUncertainty = 0.001;


            isotopeNumber = 79;
            isotopeMass = 78.91833;
            isotopeProbability = 0.5069;
            Isotope NewIsotope79 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope79);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 81;
            isotopeMass = 80.91629;
            isotopeProbability = 0.4931;
            Isotope NewIsotope81 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope81);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Kr(ref elementSymbolList, ref elementList);
        public static void SetElement_Kr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Kr";
            newElement.Name = "Krypton";
            // atomicity = 36;
            massAverage = 83.798;
            massAverageUncertainty = 0.002;


            isotopeNumber = 78;
            isotopeMass = 77.914;
            isotopeProbability = 0.0035;
            Isotope NewIsotope78 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope78);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 80;
            isotopeMass = 79.91638;
            isotopeProbability = 0.0225;
            Isotope NewIsotope80 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope80);

            isotopeNumber = 82;
            isotopeMass = 81.91348;
            isotopeProbability = 0.116;
            Isotope NewIsotope82 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope82);

            isotopeNumber = 83;
            isotopeMass = 82.91414;
            isotopeProbability = 0.115;
            Isotope NewIsotope83 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope83);

            isotopeNumber = 84;
            isotopeMass = 83.91151;
            isotopeProbability = 0.57;
            Isotope NewIsotope84 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope84);

            isotopeNumber = 86;
            isotopeMass = 85.91061;
            isotopeProbability = 0.173;
            Isotope NewIsotope86 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope86);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Rb(ref elementSymbolList, ref elementList);
        public static void SetElement_Rb(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Rb";
            newElement.Name = "Rubidium";
            // atomicity = 37;
            massAverage = 85.4678;
            massAverageUncertainty = 0.0003;


            isotopeNumber = 85;
            isotopeMass = 84.9118;
            isotopeProbability = 0.7217;
            Isotope NewIsotope85 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope85);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 87;
            isotopeMass = 86.90919;
            isotopeProbability = 0.2783;
            Isotope NewIsotope87 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope87);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Sr(ref elementSymbolList, ref elementList);
        public static void SetElement_Sr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Sr";
            newElement.Name = "Strontium";
            // atomicity = 38;
            massAverage = 87.62;
            massAverageUncertainty = 0.01;


            isotopeNumber = 84;
            isotopeMass = 83.91343;
            isotopeProbability = 0.0056;
            Isotope NewIsotope84 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope84);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 86;
            isotopeMass = 85.90926;
            isotopeProbability = 0.0986;
            Isotope NewIsotope86 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope86);

            isotopeNumber = 87;
            isotopeMass = 86.90888;
            isotopeProbability = 0.07;
            Isotope NewIsotope87 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope87);

            isotopeNumber = 88;
            isotopeMass = 87.90562;
            isotopeProbability = 0.8258;
            Isotope NewIsotope88 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope88);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Y(ref elementSymbolList, ref elementList);
        public static void SetElement_Y(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Y";
            newElement.Name = "Yttrium";
            // atomicity = 39;
            massAverage = 88.90585;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 89;
            isotopeMass = 88.90585;
            isotopeProbability = 1;
            Isotope NewIsotope89 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope89);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Zr(ref elementSymbolList, ref elementList);
        public static void SetElement_Zr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Zr";
            newElement.Name = "Zirconium";
            // atomicity = 40;
            massAverage = 91.224;
            massAverageUncertainty = 0.002;


            isotopeNumber = 90;
            isotopeMass = 89.9047;
            isotopeProbability = 0.5145;
            Isotope NewIsotope90 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope90);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 91;
            isotopeMass = 90.90565;
            isotopeProbability = 0.1122;
            Isotope NewIsotope91 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope91);

            isotopeNumber = 92;
            isotopeMass = 91.90504;
            isotopeProbability = 0.1715;
            Isotope NewIsotope92 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope92);

            isotopeNumber = 94;
            isotopeMass = 93.90631;
            isotopeProbability = 0.1738;
            Isotope NewIsotope94 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope94);

            isotopeNumber = 96;
            isotopeMass = 95.90827;
            isotopeProbability = 0.028;
            Isotope NewIsotope96 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope96);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Nb(ref elementSymbolList, ref elementList);
        public static void SetElement_Nb(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Nb";
            newElement.Name = "Niobium";
            // atomicity = 41;
            massAverage = 92.90638;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 93;
            isotopeMass = 92.90638;
            isotopeProbability = 1;
            Isotope NewIsotope93 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope93);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Mo(ref elementSymbolList, ref elementList);
        public static void SetElement_Mo(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Mo";
            newElement.Name = "Molybdenum";
            // atomicity = 42;
            massAverage = 95.96;
            massAverageUncertainty = 0.02;


            isotopeNumber = 92;
            isotopeMass = 91.90681;
            isotopeProbability = 0.1484;
            Isotope NewIsotope92 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope92);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 94;
            isotopeMass = 93.90508;
            isotopeProbability = 0.0925;
            Isotope NewIsotope94 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope94);

            isotopeNumber = 95;
            isotopeMass = 94.90584;
            isotopeProbability = 0.1592;
            Isotope NewIsotope95 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope95);

            isotopeNumber = 96;
            isotopeMass = 95.90468;
            isotopeProbability = 0.1668;
            Isotope NewIsotope96 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope96);

            isotopeNumber = 97;
            isotopeMass = 96.90602;
            isotopeProbability = 0.0955;
            Isotope NewIsotope97 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope97);

            isotopeNumber = 98;
            isotopeMass = 97.9054;
            isotopeProbability = 0.2413;
            Isotope NewIsotope98 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope98);

            isotopeNumber = 100;
            isotopeMass = 99.90748;
            isotopeProbability = 0.0963;
            Isotope NewIsotope100 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope100);


            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Tc(ref elementSymbolList, ref elementList);
        public static void SetElement_Tc(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Tc";
            newElement.Name = "Technetium";
            // atomicity = 43;
            massAverage = 98;
            massAverageUncertainty = 0;


            isotopeNumber = 98;
            isotopeMass = 98;
            isotopeProbability = 1;
            Isotope NewIsotope98 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope98);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ru(ref elementSymbolList, ref elementList);
        public static void SetElement_Ru(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ru";
            newElement.Name = "Ruthenium";
            // atomicity = 44;
            massAverage = 101.07;
            massAverageUncertainty = 0.02;


            isotopeNumber = 96;
            isotopeMass = 95.9076;
            isotopeProbability = 0.0554;
            Isotope NewIsotope96 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope96);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 98;
            isotopeMass = 97.90529;
            isotopeProbability = 0.0186;
            Isotope NewIsotope98 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope98);

            isotopeNumber = 99;
            isotopeMass = 98.90594;
            isotopeProbability = 0.127;
            Isotope NewIsotope99 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope99);

            isotopeNumber = 100;
            isotopeMass = 99.90422;
            isotopeProbability = 0.126;
            Isotope NewIsotope100 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope100);

            isotopeNumber = 101;
            isotopeMass = 100.9056;
            isotopeProbability = 0.171;
            Isotope NewIsotope101 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope101);

            isotopeNumber = 102;
            isotopeMass = 101.9044;
            isotopeProbability = 0.316;
            Isotope NewIsotope102 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope102);

            isotopeNumber = 104;
            isotopeMass = 103.9054;
            isotopeProbability = 0.186;
            Isotope NewIsotope104 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope104);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Rh(ref elementSymbolList, ref elementList);
        public static void SetElement_Rh(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Rh";
            newElement.Name = "Rhodium";
            // atomicity = 45;
            massAverage = 102.9055;
            massAverageUncertainty = 0.0002;


            isotopeNumber = 103;
            isotopeMass = 102.9055;
            isotopeProbability = 1;
            Isotope NewIsotope103 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope103);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;


            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Pd(ref elementSymbolList, ref elementList);
        public static void SetElement_Pd(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pd";
            newElement.Name = " Palladium";
            // atomicity = 46;
            massAverage = 106.42;
            massAverageUncertainty = 0.01;


            isotopeNumber = 102;
            isotopeMass = 101.9056;
            isotopeProbability = 0.0102;
            Isotope NewIsotope102 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope102);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 104;
            isotopeMass = 103.904;
            isotopeProbability = 0.1114;
            Isotope NewIsotope104 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope104);

            isotopeNumber = 105;
            isotopeMass = 104.9051;
            isotopeProbability = 0.2233;
            Isotope NewIsotope105 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope105);

            isotopeNumber = 106;
            isotopeMass = 105.9035;
            isotopeProbability = 0.2733;
            Isotope NewIsotope106 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope106);

            isotopeNumber = 108;
            isotopeMass = 107.9039;
            isotopeProbability = 0.2646;
            Isotope NewIsotope108 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope108);

            isotopeNumber = 110;
            isotopeMass = 109.9052;
            isotopeProbability = 0.1172;
            Isotope NewIsotope110 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope110);


            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ag(ref elementSymbolList, ref elementList);
        public static void SetElement_Ag(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ag";
            newElement.Name = "Silver";
            // atomicity = 47;
            massAverage = 107.8682;
            massAverageUncertainty = 0.0002;


            isotopeNumber = 107;
            isotopeMass = 106.9051;
            isotopeProbability = 0.51839;
            Isotope NewIsotope107 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope107);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 109;
            isotopeMass = 108.9048;
            isotopeProbability = 0.48161;
            Isotope NewIsotope109 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope109);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Cd(ref elementSymbolList, ref elementList);
        public static void SetElement_Cd(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cd";
            newElement.Name = "Cadmium";
            // atomicity = 48;
            massAverage = 112.411;
            massAverageUncertainty = 0.008;


            isotopeNumber = 106;
            isotopeMass = 105.9065;
            isotopeProbability = 0.0125;
            Isotope NewIsotope106 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope106);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 108;
            isotopeMass = 107.9042;
            isotopeProbability = 0.0089;
            Isotope NewIsotope108 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope108);

            isotopeNumber = 110;
            isotopeMass = 109.903;
            isotopeProbability = 0.1249;
            Isotope NewIsotope110 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope110);

            isotopeNumber = 111;
            isotopeMass = 110.9042;
            isotopeProbability = 0.128;
            Isotope NewIsotope111 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope111);

            isotopeNumber = 112;
            isotopeMass = 111.9028;
            isotopeProbability = 0.2413;
            Isotope NewIsotope112 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope112);
            
            isotopeNumber = 113;
            isotopeMass = 112.9044;
            isotopeProbability = 0.1222;
            Isotope NewIsotope113 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope113);

            isotopeNumber = 114;
            isotopeMass = 113.9034;
            isotopeProbability = 0.2873;
            Isotope NewIsotope114 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope114);

            isotopeNumber = 116;
            isotopeMass = 115.9048;
            isotopeProbability = 0.0749;
            Isotope NewIsotope116 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope116);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            
        }

        //PeriodicTable.SetElement_In(ref elementSymbolList, ref elementList);
        public static void SetElement_In(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "In";
            newElement.Name = "Indium";
            // atomicity = 49;
            massAverage = 114.818;
            massAverageUncertainty = 0.003;


            isotopeNumber = 113;
            isotopeMass = 112.9041;
            isotopeProbability = 0.043;
            Isotope NewIsotope113 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope113);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 115;
            isotopeMass = 114.9039;
            isotopeProbability = 0.957;
            Isotope NewIsotope115 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope115);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Sn(ref elementSymbolList, ref elementList);
        public static void SetElement_Sn(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Sn";
            newElement.Name = "Tin";
            // atomicity = 50;
            massAverage = 118.71;
            massAverageUncertainty = 0.07;


            isotopeNumber = 112;
            isotopeMass = 111.9048;
            isotopeProbability = 0.0097;
            Isotope NewIsotope112 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope112);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 114;
            isotopeMass = 113.9028;
            isotopeProbability = 0.0065;
            Isotope NewIsotope114 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope114);

            isotopeNumber = 115;
            isotopeMass = 114.9034;
            isotopeProbability = 0.0036;
            Isotope NewIsotope115 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope115);

            isotopeNumber = 116;
            isotopeMass = 115.9017;
            isotopeProbability = 0.1453;
            Isotope NewIsotope116 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope116);

            isotopeNumber = 117;
            isotopeMass = 116.903;
            isotopeProbability = 0.0768;
            Isotope NewIsotope117 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope117);

            isotopeNumber = 118;
            isotopeMass = 117.9016;
            isotopeProbability = 0.2422;
            Isotope NewIsotope118 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope118);

            isotopeNumber = 119;
            isotopeMass = 118.9033;
            isotopeProbability = 0.0858;
            Isotope NewIsotope119 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope119);

            isotopeNumber = 120;
            isotopeMass = 119.9022;
            isotopeProbability = 0.3259;
            Isotope NewIsotope120 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope120);

            isotopeNumber = 122;
            isotopeMass = 121.9034;
            isotopeProbability = 0.0463;
            Isotope NewIsotope122 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope122);

            isotopeNumber = 124;
            isotopeMass = 123.9053;
            isotopeProbability = 0.0579;
            Isotope NewIsotope124 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope124);


            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Sb(ref elementSymbolList, ref elementList);
        public static void SetElement_Sb(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Sb";
            newElement.Name = "Antimony";
            // atomicity = 51;
            massAverage = 121.76;
            massAverageUncertainty = 0.01;


            isotopeNumber = 121;
            isotopeMass = 120.9038;
            isotopeProbability = 0.574;
            Isotope NewIsotope121 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope121);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 123;
            isotopeMass = 122.9042;
            isotopeProbability = 0.426;
            Isotope NewIsotope123 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope123);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Te(ref elementSymbolList, ref elementList);
        public static void SetElement_Te(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Te";
            newElement.Name = "Tellurium";
            // atomicity = 52;
            massAverage = 127.6;
            massAverageUncertainty = 0.3;


            isotopeNumber = 120;
            isotopeMass = 119.904;
            isotopeProbability = 0.00095;
            Isotope NewIsotope120 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope120);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 122;
            isotopeMass = 121.9031;
            isotopeProbability = 0.0259;
            Isotope NewIsotope122 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope122);

            isotopeNumber = 123;
            isotopeMass = 122.9043;
            isotopeProbability = 0.00905;
            Isotope NewIsotope123 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope123);

            isotopeNumber = 124;
            isotopeMass = 123.9028;
            isotopeProbability = 0.0479;
            Isotope NewIsotope124 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope124);

            isotopeNumber = 125;
            isotopeMass = 124.9044;
            isotopeProbability = 0.0712;
            Isotope NewIsotope125 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope125);

            isotopeNumber = 126;
            isotopeMass = 125.9033;
            isotopeProbability = 0.1893;
            Isotope NewIsotope126 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope126);

            isotopeNumber = 128;
            isotopeMass = 127.9045;
            isotopeProbability = 0.317;
            Isotope NewIsotope128 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope128);

            isotopeNumber = 130;
            isotopeMass = 129.9062;
            isotopeProbability = 0.3387;
            Isotope NewIsotope130 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope130);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_I(ref elementSymbolList, ref elementList);
        public static void SetElement_I(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "I";
            newElement.Name = "Iodine";
            // atomicity = 53;
            massAverage = 126.90447;
            massAverageUncertainty = 0.00003;


            isotopeNumber = 127;
            isotopeMass = 126.9045;
            isotopeProbability = 1;
            Isotope NewIsotope127 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope127);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Xe(ref elementSymbolList, ref elementList);
        public static void SetElement_Xe(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Xe";
            newElement.Name = "Xenon";
            // atomicity = 54;
            massAverage = 131.293;
            massAverageUncertainty = 0.006;


            isotopeNumber = 124;
            isotopeMass = 123.9059;
            isotopeProbability = 0.001;
            Isotope NewIsotope124 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope124);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 126;
            isotopeMass = 125.9043;
            isotopeProbability = 0.0009;
            Isotope NewIsotope126 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope126);

            isotopeNumber = 128;
            isotopeMass = 127.9035;
            isotopeProbability = 0.0191;
            Isotope NewIsotope128 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope128);

            isotopeNumber = 129;
            isotopeMass = 128.9048;
            isotopeProbability = 0.264;
            Isotope NewIsotope129 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope129);

            isotopeNumber = 130;
            isotopeMass = 129.9035;
            isotopeProbability = 0.041;
            Isotope NewIsotope130 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope130);

            isotopeNumber = 131;
            isotopeMass = 130.9051;
            isotopeProbability = 0.212;
            Isotope NewIsotope131 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope131);

            isotopeNumber = 132;
            isotopeMass = 131.9041;
            isotopeProbability = 0.269;
            Isotope NewIsotope132 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope132);

            isotopeNumber = 134;
            isotopeMass = 133.9054;
            isotopeProbability = 0.104;
            Isotope NewIsotope134 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope134);

            isotopeNumber = 136;
            isotopeMass = 135.9072;
            isotopeProbability = 0.089;
            Isotope NewIsotope136 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope136);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Cs(ref elementSymbolList, ref elementList);
        public static void SetElement_Cs(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cs";
            newElement.Name = "Cesium";
            // atomicity = 55;
            massAverage = 132.9054519;
            massAverageUncertainty = 0.0000002;


            isotopeNumber = 133;
            isotopeMass = 132.9054;
            isotopeProbability = 1;
            Isotope NewIsotope133 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope133);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ba(ref elementSymbolList, ref elementList);
        public static void SetElement_Ba(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ba";
            newElement.Name = "Barium";
            // atomicity = 56;
            massAverage = 137.327;
            massAverageUncertainty = 0.007;


            isotopeNumber = 130;
            isotopeMass = 129.9063;
            isotopeProbability = 0.00106;
            Isotope NewIsotope130 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope130);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 132;
            isotopeMass = 131.905;
            isotopeProbability = 0.00101;
            Isotope NewIsotope132 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope132);

            isotopeNumber = 134;
            isotopeMass = 133.9045;
            isotopeProbability = 0.0242;
            Isotope NewIsotope134 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope134);

            isotopeNumber = 135;
            isotopeMass = 134.9057;
            isotopeProbability = 0.06593;
            Isotope NewIsotope135 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope135);

            isotopeNumber = 136;
            isotopeMass = 135.9046;
            isotopeProbability = 0.0785;
            Isotope NewIsotope136 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope136);

            isotopeNumber = 137;
            isotopeMass = 136.9058;
            isotopeProbability = 0.1123;
            Isotope NewIsotope137 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope137);

            isotopeNumber = 138;
            isotopeMass = 137.9052;
            isotopeProbability = 0.717;
            Isotope NewIsotope138 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope138);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_La(ref elementSymbolList, ref elementList);
        public static void SetElement_La(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "La";
            newElement.Name = "Lanthanum";
            // atomicity = 57;
            massAverage = 138.90547;
            massAverageUncertainty = 0.00007;


            isotopeNumber = 138;
            isotopeMass = 137.9071;
            isotopeProbability = 0.0009;
            Isotope NewIsotope138 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope138);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 139;
            isotopeMass = 138.9063;
            isotopeProbability = 0.9991;
            Isotope NewIsotope139 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope139);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ce(ref elementSymbolList, ref elementList);
        public static void SetElement_Ce(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ce";
            newElement.Name = "Cerium";
            // atomicity = 58;
            massAverage = 140.116;
            massAverageUncertainty = 0.001;


            isotopeNumber = 136;
            isotopeMass = 135.9071;
            isotopeProbability = 0.0019;
            Isotope NewIsotope136 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope136);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 138;
            isotopeMass = 137.906;
            isotopeProbability = 0.0025;
            Isotope NewIsotope138 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope138);

            isotopeNumber = 140;
            isotopeMass = 139.9054;
            isotopeProbability = 0.8843;
            Isotope NewIsotope140 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope140);

            isotopeNumber = 142;
            isotopeMass = 141.9092;
            isotopeProbability = 0.1113;
            Isotope NewIsotope142 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope142);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Pr(ref elementSymbolList, ref elementList);
        public static void SetElement_Pr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pr";
            newElement.Name = "Praseodymium";
            // atomicity = 59;
            massAverage = 140.90765;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 141;
            isotopeMass = 140.9077;
            isotopeProbability = 1;
            Isotope NewIsotope141 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope141);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Nd(ref elementSymbolList, ref elementList);
        public static void SetElement_Nd(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Nd";
            newElement.Name = "Neodymium";
            // atomicity = 60;
            massAverage = 144.242;
            massAverageUncertainty = 0.003;


            isotopeNumber = 142;
            isotopeMass = 141.9077;
            isotopeProbability = 0.2713;
            Isotope NewIsotope142 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope142);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 143;
            isotopeMass = 142.9098;
            isotopeProbability = 0.1218;
            Isotope NewIsotope143 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope143);

            isotopeNumber = 144;
            isotopeMass = 143.9101;
            isotopeProbability = 0.238;
            Isotope NewIsotope144 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope144);

            isotopeNumber = 145;
            isotopeMass = 144.9126;
            isotopeProbability = 0.083;
            Isotope NewIsotope145 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope145);

            isotopeNumber = 146;
            isotopeMass = 145.9131;
            isotopeProbability = 0.1719;
            Isotope NewIsotope146 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope146);

            isotopeNumber = 148;
            isotopeMass = 147.9169;
            isotopeProbability = 0.0576;
            Isotope NewIsotope148 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope148);

            isotopeNumber = 150;
            isotopeMass = 149.9209;
            isotopeProbability = 0.0564;
            Isotope NewIsotope150 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope150);


            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Pm(ref elementSymbolList, ref elementList);
        public static void SetElement_Pm(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pm";
            newElement.Name = "Promethium";
            // atomicity = 61;
            massAverage = 145;
            massAverageUncertainty = 0;


            isotopeNumber = 145;
            isotopeMass = 145;
            isotopeProbability = 1;
            Isotope NewIsotope145 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope145);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Sm(ref elementSymbolList, ref elementList);
        public static void SetElement_Sm(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Sm";
            newElement.Name = "Samarium";
            // atomicity = 62;
            massAverage = 150.36;
            massAverageUncertainty = 0.02;


            isotopeNumber = 144;
            isotopeMass = 143.912;
            isotopeProbability = 0.031;
            Isotope NewIsotope144 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope144);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 147;
            isotopeMass = 146.9149;
            isotopeProbability = 0.15;
            Isotope NewIsotope147 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope147);

            isotopeNumber = 148;
            isotopeMass = 147.9148;
            isotopeProbability = 0.113;
            Isotope NewIsotope148 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope148);

            isotopeNumber = 149;
            isotopeMass = 148.9172;
            isotopeProbability = 0.138;
            Isotope NewIsotope149 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope149);

            isotopeNumber = 150;
            isotopeMass = 149.9173;
            isotopeProbability = 0.074;
            Isotope NewIsotope150 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope150);

            isotopeNumber = 152;
            isotopeMass = 151.9197;
            isotopeProbability = 0.267;
            Isotope NewIsotope152 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope152);

            isotopeNumber = 154;
            isotopeMass = 153.9222;
            isotopeProbability = 0.227;
            Isotope NewIsotope154 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope154);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Eu(ref elementSymbolList, ref elementList);
        public static void SetElement_Eu(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Eu";
            newElement.Name = "Europium";
            // atomicity = 63;
            massAverage = 151.964;
            massAverageUncertainty = 0.001;


            isotopeNumber = 151;
            isotopeMass = 150.9198;
            isotopeProbability = 0.478;
            Isotope NewIsotope151 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope151);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 153;
            isotopeMass = 152.9212;
            isotopeProbability = 0.522;
            Isotope NewIsotope153 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope153);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Gd(ref elementSymbolList, ref elementList);
        public static void SetElement_Gd(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Gd";
            newElement.Name = "Gadolinium";
            // atomicity = 64;
            massAverage = 157.25;
            massAverageUncertainty = 0.03;


            isotopeNumber = 152;
            isotopeMass = 151.9198;
            isotopeProbability = 0.002;
            Isotope NewIsotope152 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope152);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 154;
            isotopeMass = 153.9209;
            isotopeProbability = 0.0218;
            Isotope NewIsotope154 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope154);

            isotopeNumber = 155;
            isotopeMass = 154.9226;
            isotopeProbability = 0.148;
            Isotope NewIsotope155 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope155);

            isotopeNumber = 156;
            isotopeMass = 155.9221;
            isotopeProbability = 0.2047;
            Isotope NewIsotope156 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope156);

            isotopeNumber = 157;
            isotopeMass = 156.924;
            isotopeProbability = 0.1565;
            Isotope NewIsotope157 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope157);

            isotopeNumber = 158;
            isotopeMass = 157.9241;
            isotopeProbability = 0.2484;
            Isotope NewIsotope158 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope158);

            isotopeNumber = 160;
            isotopeMass = 159.927;
            isotopeProbability = 0.2186;
            Isotope NewIsotope160 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope160);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Tb(ref elementSymbolList, ref elementList);
        public static void SetElement_Tb(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Tb";
            newElement.Name = "Terbium";
            // atomicity = 65;
            massAverage = 158.92535;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 159;
            isotopeMass = 158.9253;
            isotopeProbability = 1;
            Isotope NewIsotope159 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope159);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Dy(ref elementSymbolList, ref elementList);
        public static void SetElement_Dy(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Dy";
            newElement.Name = "Dysprosium";
            // atomicity = 66;
            massAverage = 162.5;
            massAverageUncertainty = 0.1;


            isotopeNumber = 156;
            isotopeMass = 155.9253;
            isotopeProbability = 0.0006;
            Isotope NewIsotope156 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope156);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 158;
            isotopeMass = 157.9244;
            isotopeProbability = 0.001;
            Isotope NewIsotope158 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope158);

            isotopeNumber = 160;
            isotopeMass = 159.9252;
            isotopeProbability = 0.0234;
            Isotope NewIsotope160 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope160);

            isotopeNumber = 161;
            isotopeMass = 160.9269;
            isotopeProbability = 0.189;
            Isotope NewIsotope161 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope161);

            isotopeNumber = 162;
            isotopeMass = 161.9268;
            isotopeProbability = 0.255;
            Isotope NewIsotope162 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope162);

            isotopeNumber = 163;
            isotopeMass = 162.9287;
            isotopeProbability = 0.249;
            Isotope NewIsotope163 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope163);

            isotopeNumber = 164;
            isotopeMass = 163.9292;
            isotopeProbability = 0.282;
            Isotope NewIsotope164 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope164);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Ho(ref elementSymbolList, ref elementList);
        public static void SetElement_Ho(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ho";
            newElement.Name = "Holmium";
            // atomicity = 67;
            massAverage = 164.93032;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 165;
            isotopeMass = 164.9303;
            isotopeProbability = 1;
            Isotope NewIsotope165 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope165);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Er(ref elementSymbolList, ref elementList);
        public static void SetElement_Er(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Er";
            newElement.Name = "Erbium";
            // atomicity = 68;
            massAverage = 167.259;
            massAverageUncertainty = 0.003;


            isotopeNumber = 162;
            isotopeMass = 161.9288;
            isotopeProbability = 0.0014;
            Isotope NewIsotope162 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope162);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 164;
            isotopeMass = 163.9292;
            isotopeProbability = 0.0161;
            Isotope NewIsotope164 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope164);

            isotopeNumber = 166;
            isotopeMass = 165.9303;
            isotopeProbability = 0.336;
            Isotope NewIsotope166 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope166);

            isotopeNumber = 167;
            isotopeMass = 166.9321;
            isotopeProbability = 0.2295;
            Isotope NewIsotope167 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope167);

            isotopeNumber = 168;
            isotopeMass = 167.9324;
            isotopeProbability = 0.268;
            Isotope NewIsotope168 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope168);

            isotopeNumber = 170;
            isotopeMass = 169.9355;
            isotopeProbability = 0.149;
            Isotope NewIsotope170 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope170);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Tm(ref elementSymbolList, ref elementList);
        public static void SetElement_Tm(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Tm";
            newElement.Name = "Thulium";
            // atomicity = 69;
            massAverage = 168.93421;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 169;
            isotopeMass = 168.9342;
            isotopeProbability = 1;
            Isotope NewIsotope169 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope169);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Yb(ref elementSymbolList, ref elementList);
        public static void SetElement_Yb(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Yb";
            newElement.Name = "Ytterbium";
            // atomicity = 70;
            massAverage = 173.054;
            massAverageUncertainty = 0.005;


            isotopeNumber = 168;
            isotopeMass = 167.9339;
            isotopeProbability = 0.0013;
            Isotope NewIsotope168 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope168);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 170;
            isotopeMass = 169.9348;
            isotopeProbability = 0.0305;
            Isotope NewIsotope170 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope170);

            isotopeNumber = 171;
            isotopeMass = 170.9363;
            isotopeProbability = 0.143;
            Isotope NewIsotope171 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope171);

            isotopeNumber = 172;
            isotopeMass = 171.9364;
            isotopeProbability = 0.219;
            Isotope NewIsotope172 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope172);

            isotopeNumber = 173;
            isotopeMass = 172.9382;
            isotopeProbability = 0.1612;
            Isotope NewIsotope173 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope173);

            isotopeNumber = 174;
            isotopeMass = 173.9389;
            isotopeProbability = 0.318;
            Isotope NewIsotope174 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope174);

            isotopeNumber = 176;
            isotopeMass = 175.9426;
            isotopeProbability = 0.127;
            Isotope NewIsotope176 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope176);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Lu(ref elementSymbolList, ref elementList);
        public static void SetElement_Lu(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Lu";
            newElement.Name = "Lutetium";
            // atomicity = 71;
            massAverage = 174.9668;
            massAverageUncertainty = 0.0001;


            isotopeNumber = 175;
            isotopeMass = 174.9408;
            isotopeProbability = 0.9741;
            Isotope NewIsotope175 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope175);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 176;
            isotopeMass = 175.9427;
            isotopeProbability = 0.0259;
            Isotope NewIsotope176 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope176);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Hf(ref elementSymbolList, ref elementList);
        public static void SetElement_Hf(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Hf";
            newElement.Name = "Hafnium";
            // atomicity = 72;
            massAverage = 178.49;
            massAverageUncertainty = 0.02;


            isotopeNumber = 174;
            isotopeMass = 173.94;
            isotopeProbability = 0.00162;
            Isotope NewIsotope174 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope174);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 176;
            isotopeMass = 175.9414;
            isotopeProbability = 0.05206;
            Isotope NewIsotope176 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope176);

            isotopeNumber = 177;
            isotopeMass = 176.9432;
            isotopeProbability = 0.18606;
            Isotope NewIsotope177 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope177);

            isotopeNumber = 178;
            isotopeMass = 177.9437;
            isotopeProbability = 0.27297;
            Isotope NewIsotope178 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope178);

            isotopeNumber = 179;
            isotopeMass = 178.9458;
            isotopeProbability = 0.13629;
            Isotope NewIsotope179 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope179);

            isotopeNumber = 180;
            isotopeMass = 179.9465;
            isotopeProbability = 0.351;
            Isotope NewIsotope180 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope180);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ta(ref elementSymbolList, ref elementList);
        public static void SetElement_Ta(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ta";
            newElement.Name = "Tantalum";
            // atomicity = 73;
            massAverage = 180.94788;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 180;
            isotopeMass = 179.9475;
            isotopeProbability = 0.00012;
            Isotope NewIsotope180 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope180);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 181;
            isotopeMass = 180.948;
            isotopeProbability = 0.99988;
            Isotope NewIsotope181 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope181);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }
        //PeriodicTable.SetElement_W(ref elementSymbolList, ref elementList);
        public static void SetElement_W(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "W";
            newElement.Name = "Tungsten";
            // atomicity = 74;
            massAverage = 183.84;
            massAverageUncertainty = 0.01;


            isotopeNumber = 180;
            isotopeMass = 179.9467;
            isotopeProbability = 0.0012;
            Isotope NewIsotope180 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope180);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 182;
            isotopeMass = 181.9482;
            isotopeProbability = 0.263;
            Isotope NewIsotope182 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope182);

            isotopeNumber = 183;
            isotopeMass = 182.9502;
            isotopeProbability = 0.1428;
            Isotope NewIsotope183 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope183);

            isotopeNumber = 184;
            isotopeMass = 183.9509;
            isotopeProbability = 0.307;
            Isotope NewIsotope184 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope184);

            isotopeNumber = 186;
            isotopeMass = 185.9544;
            isotopeProbability = 0.286;
            Isotope NewIsotope186 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope186);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Re(ref elementSymbolList, ref elementList);
        public static void SetElement_Re(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Re";
            newElement.Name = "Rhenium";
            // atomicity = 75;
            massAverage = 186.207;
            massAverageUncertainty = 0.001;


            isotopeNumber = 185;
            isotopeMass = 184.953;
            isotopeProbability = 0.374;
            Isotope NewIsotope185 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope185);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 187;
            isotopeMass = 186.9557;
            isotopeProbability = 0.626;
            Isotope NewIsotope187 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope187);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Os(ref elementSymbolList, ref elementList);
        public static void SetElement_Os(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Os";
            newElement.Name = "Osmium";
            // atomicity = 76;
            massAverage = 190.23;
            massAverageUncertainty = 0.03;


            isotopeNumber = 184;
            isotopeMass = 183.9525;
            isotopeProbability = 0.0002;
            Isotope NewIsotope184 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope184);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 186;
            isotopeMass = 185.9538;
            isotopeProbability = 0.0158;
            Isotope NewIsotope186 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope186);

            isotopeNumber = 187;
            isotopeMass = 186.9557;
            isotopeProbability = 0.016;
            Isotope NewIsotope187 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope187);

            isotopeNumber = 188;
            isotopeMass = 187.9559;
            isotopeProbability = 0.133;
            Isotope NewIsotope188 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope188);

            isotopeNumber = 189;
            isotopeMass = 188.9581;
            isotopeProbability = 0.161;
            Isotope NewIsotope189 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope189);

            isotopeNumber = 190;
            isotopeMass = 189.9584;
            isotopeProbability = 0.264;
            Isotope NewIsotope190 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope190);

            isotopeNumber = 192;
            isotopeMass = 191.9615;
            isotopeProbability = 0.41;
            Isotope NewIsotope192 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope192);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

//PeriodicTable.SetElement_Ir(ref elementSymbolList, ref elementList);
        public static void SetElement_Ir(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ir";
            newElement.Name = "Iridium";
            // atomicity = 77;
            massAverage = 192.217;
            massAverageUncertainty = 0.003;


            isotopeNumber = 191;
            isotopeMass = 190.9606;
            isotopeProbability = 0.373;
            Isotope NewIsotope191 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope191);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 193;
            isotopeMass = 192.9629;
            isotopeProbability = 0.627;
            Isotope NewIsotope193 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope193);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Pt(ref elementSymbolList, ref elementList);
        public static void SetElement_Pt(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pt";
            newElement.Name = "Platinum";
            // atomicity = 78;
            massAverage = 195.084;
            massAverageUncertainty = 0.009;


            isotopeNumber = 190;
            isotopeMass = 189.9599;
            isotopeProbability = 0.0001;
            Isotope NewIsotope190 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope190);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 192;
            isotopeMass = 191.961;
            isotopeProbability = 0.0079;
            Isotope NewIsotope192 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope192);

            isotopeNumber = 194;
            isotopeMass = 193.9627;
            isotopeProbability = 0.329;
            Isotope NewIsotope194 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope194);

            isotopeNumber = 195;
            isotopeMass = 194.9648;
            isotopeProbability = 0.338;
            Isotope NewIsotope195 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope195);

            isotopeNumber = 196;
            isotopeMass = 195.9649;
            isotopeProbability = 0.253;
            Isotope NewIsotope196 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope196);

            isotopeNumber = 198;
            isotopeMass = 197.9679;
            isotopeProbability = 0.072;
            Isotope NewIsotope198 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope198);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Au(ref elementSymbolList, ref elementList);
        public static void SetElement_Au(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Au";
            newElement.Name = "Gold";
            // atomicity = 79;
            massAverage = 196.966569;
            massAverageUncertainty = 0.000004;


            isotopeNumber = 197;
            isotopeMass = 196.9665;
            isotopeProbability = 1;
            Isotope NewIsotope197 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope197);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Hg(ref elementSymbolList, ref elementList);
        public static void SetElement_Hg(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Hg";
            newElement.Name = "Mercury";
            // atomicity = 80;
            massAverage = 200.59;
            massAverageUncertainty = 0.02;


            isotopeNumber = 196;
            isotopeMass = 195.9658;
            isotopeProbability = 0.0015;
            Isotope NewIsotope196 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope196);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 198;
            isotopeMass = 197.9667;
            isotopeProbability = 0.1;
            Isotope NewIsotope198 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope198);

            isotopeNumber = 199;
            isotopeMass = 198.9682;
            isotopeProbability = 0.169;
            Isotope NewIsotope199 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope199);

            isotopeNumber = 200;
            isotopeMass = 199.9683;
            isotopeProbability = 0.231;
            Isotope NewIsotope200 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope200);

            isotopeNumber = 201;
            isotopeMass = 200.9703;
            isotopeProbability = 0.132;
            Isotope NewIsotope201 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope201);

            isotopeNumber = 202;
            isotopeMass = 201.9706;
            isotopeProbability = 0.298;
            Isotope NewIsotope202 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope202);

            isotopeNumber = 204;
            isotopeMass = 203.9735;
            isotopeProbability = 0.0685;
            Isotope NewIsotope204 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability); 
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope204);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Tl(ref elementSymbolList, ref elementList);
        public static void SetElement_Tl(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Tl";
            newElement.Name = "Thallium";
            // atomicity = 81;
            massAverage = 204.3833;
            massAverageUncertainty = 0.0002;


            isotopeNumber = 203;
            isotopeMass = 202.9723;
            isotopeProbability = 0.29524;
            Isotope NewIsotope203 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope203);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 205;
            isotopeMass = 204.9744;
            isotopeProbability = 0.70476;
            Isotope NewIsotope205 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope205);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Pb(ref elementSymbolList, ref elementList);
        public static void SetElement_Pb(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pb";
            newElement.Name = "Lead";
            // atomicity = 82;
            massAverage = 207.2;
            massAverageUncertainty = 0.1;


            isotopeNumber = 204;
            isotopeMass = 203.973;
            isotopeProbability = 0.014;
            Isotope NewIsotope204 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope204);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 206;
            isotopeMass = 205.9744;
            isotopeProbability = 0.241;
            Isotope NewIsotope206 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope206);

            isotopeNumber = 207;
            isotopeMass = 206.9759;
            isotopeProbability = 0.221;
            Isotope NewIsotope207 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope207);

            isotopeNumber = 208;
            isotopeMass = 207.9766;
            isotopeProbability = 0.524;
            Isotope NewIsotope208 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope208);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Bi(ref elementSymbolList, ref elementList);
        public static void SetElement_Bi(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Bi";
            newElement.Name = "Bismuth";
            // atomicity = 83;
            massAverage = 208.9804;
            massAverageUncertainty = 0.0001;


            isotopeNumber = 209;
            isotopeMass = 208.9804;
            isotopeProbability = 1;
            Isotope NewIsotope209 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope209);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Po(ref elementSymbolList, ref elementList);
        public static void SetElement_Po(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Po";
            newElement.Name = "Polonium";
            // atomicity = 84;
            massAverage = 209;
            massAverageUncertainty = 0;


            isotopeNumber = 209;
            isotopeMass = 209;
            isotopeProbability = 1;
            Isotope NewIsotope209 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope209);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_At(ref elementSymbolList, ref elementList);
        public static void SetElement_At(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "At";
            newElement.Name = "Astatine";
            // atomicity = 85;
            massAverage = 210;
            massAverageUncertainty = 0;


            isotopeNumber = 210;
            isotopeMass = 210;
            isotopeProbability = 1;
            Isotope NewIsotope210 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope210);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Rn(ref elementSymbolList, ref elementList);
        public static void SetElement_Rn(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Rn";
            newElement.Name = "Radon";
            // atomicity = 86;
            massAverage = 222;
            massAverageUncertainty = 0;


            isotopeNumber = 222;
            isotopeMass = 222;
            isotopeProbability = 1;
            Isotope NewIsotope222 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope222);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Fr(ref elementSymbolList, ref elementList);
        public static void SetElement_Fr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Fr";
            newElement.Name = "Francium";
            // atomicity = 87;
            massAverage = 223;
            massAverageUncertainty = 0;


            isotopeNumber = 223;
            isotopeMass = 223;
            isotopeProbability = 1;
            Isotope NewIsotope223 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope223);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ra(ref elementSymbolList, ref elementList);
        public static void SetElement_Ra(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ra";
            newElement.Name = "Radium";
            // atomicity = 88;
            massAverage = 226.25;
            massAverageUncertainty = 0;


            isotopeNumber = 226;
            isotopeMass = 226.025;
            isotopeProbability = 1;
            Isotope NewIsotope226 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope226);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Ac(ref elementSymbolList, ref elementList);
        public static void SetElement_Ac(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Ac";
            newElement.Name = "Actinium";
            // atomicity = 89;
            massAverage = 227.028;
            massAverageUncertainty = 0;


            isotopeNumber = 227;
            isotopeMass = 227.028;
            isotopeProbability = 1;
            Isotope NewIsotope227 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope227);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Th(ref elementSymbolList, ref elementList);
        public static void SetElement_Th(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Th";
            newElement.Name = "Thorium";
            // atomicity = 90;
            massAverage = 232.03806;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 232;
            isotopeMass = 232.0381;
            isotopeProbability = 1;
            Isotope NewIsotope232 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope232);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Pa(ref elementSymbolList, ref elementList);
        public static void SetElement_Pa(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pa";
            newElement.Name = "Protactinium";
            // atomicity = 91;
            massAverage = 231.03588;
            massAverageUncertainty = 0.00002;


            isotopeNumber = 231;
            isotopeMass = 231.0359;
            isotopeProbability = 1;
            Isotope NewIsotope231 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope231);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

       //PeriodicTable.SetElement_U(ref elementSymbolList, ref elementList);
        public static void SetElement_U(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "U";
            newElement.Name = "Uranium";
            // atomicity = 92;
            massAverage = 238.02891;
            massAverageUncertainty = 0.00003;


            isotopeNumber = 234;
            isotopeMass = 234.0409;
            isotopeProbability = 0.000055;
            Isotope NewIsotope234 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope234);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            isotopeNumber = 235;
            isotopeMass = 235.0439;
            isotopeProbability = 0.0072;
            Isotope NewIsotope235 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope235);

            isotopeNumber = 238;
            isotopeMass = 238.0508;
            isotopeProbability = 0.992745;
            Isotope NewIsotope238 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope238);

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Np(ref elementSymbolList, ref elementList);
        public static void SetElement_Np(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Np";
            newElement.Name = "Neptunium";
            // atomicity = 93;
            massAverage = 237.048;
            massAverageUncertainty = 0;


            isotopeNumber = 237;
            isotopeMass = 237.048;
            isotopeProbability = 1;
            Isotope NewIsotope237 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope237);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }


        //PeriodicTable.SetElement_Pu(ref elementSymbolList, ref elementList);
        public static void SetElement_Pu(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Pu";
            newElement.Name = "Plutonium";
            // atomicity = 94;
            massAverage = 244;
            massAverageUncertainty = 0;


            isotopeNumber = 244;
            isotopeMass = 244;
            isotopeProbability = 1;
            Isotope NewIsotope244 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope244);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Am(ref elementSymbolList, ref elementList);
        public static void SetElement_Am(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Am";
            newElement.Name = "Americium";
            // atomicity = 95;
            massAverage = 243;
            massAverageUncertainty = 0;


            isotopeNumber = 243;
            isotopeMass = 243;
            isotopeProbability = 1;
            Isotope NewIsotope243 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope243);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Cm(ref elementSymbolList, ref elementList);
        public static void SetElement_Cm(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cm";
            newElement.Name = "Curium";
            // atomicity = 96;
            massAverage = 247;
            massAverageUncertainty = 0;


            isotopeNumber = 247;
            isotopeMass = 247;
            isotopeProbability = 1;
            Isotope NewIsotope247 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope247);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Bk(ref elementSymbolList, ref elementList);
        public static void SetElement_Bk(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Bk";
            newElement.Name = "Berkelium";
            // atomicity = 97;
            massAverage = 247;
            massAverageUncertainty = 0;


            isotopeNumber = 247;
            isotopeMass = 247;
            isotopeProbability = 1;
            Isotope NewIsotope247 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope247);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Cf(ref elementSymbolList, ref elementList);
        public static void SetElement_Cf(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Cf";
            newElement.Name = "Californium";
            // atomicity = 98;
            massAverage = 251;
            massAverageUncertainty = 0;


            isotopeNumber = 251;
            isotopeMass = 251;
            isotopeProbability = 1;
            Isotope NewIsotope251 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope251);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Es(ref elementSymbolList, ref elementList);
        public static void SetElement_Es(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Es";
            newElement.Name = "Einsteinium";
            // atomicity = 99;
            massAverage = 252;
            massAverageUncertainty = 0;


            isotopeNumber = 252;
            isotopeMass = 252;
            isotopeProbability = 1;
            Isotope NewIsotope252 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope252);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Fm(ref elementSymbolList, ref elementList);
        public static void SetElement_Fm(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Fm";
            newElement.Name = "Fermium";
            // atomicity = 100;
            massAverage = 257;
            massAverageUncertainty = 0;


            isotopeNumber = 257;
            isotopeMass = 257;
            isotopeProbability = 1;
            Isotope NewIsotope257 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope257);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Md(ref elementSymbolList, ref elementList);
        public static void SetElement_Md(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Md";
            newElement.Name = "Medelevium";
            // atomicity = 101;
            massAverage = 258;
            massAverageUncertainty = 0;


            isotopeNumber = 258;
            isotopeMass = 258;
            isotopeProbability = 1;
            Isotope NewIsotope258 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope258);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_No(ref elementSymbolList, ref elementList);
        public static void SetElement_No(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "No";
            newElement.Name = "Nobelium";
            // atomicity = 102;
            massAverage = 259;
            massAverageUncertainty = 0;


            isotopeNumber = 259;
            isotopeMass = 259;
            isotopeProbability = 1;
            Isotope NewIsotope259 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope259);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_Lr(ref elementSymbolList, ref elementList);
        public static void SetElement_Lr(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "Lr";
            newElement.Name = "Lawrencium";
            // atomicity = 103;
            massAverage = 262;
            massAverageUncertainty = 0;


            isotopeNumber = 262;
            isotopeMass = 260;
            isotopeProbability = 1;
            Isotope NewIsotope262 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope262);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }

        //PeriodicTable.SetElement_X(ref elementSymbolList, ref elementList);
        public static void SetElement_X(ref List<string> elementSymbolList, ref List<Element> elementList)
        {
            Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();
            Element newElement = new Element();
            double monoIsotopicMass = 0;
            double isotopeProbability;
            double massAverage;
            double massAverageUncertainty;
            double isotopeMass;
            int isotopeNumber;
            newElement.Symbol = "X";
            newElement.Name = "Generic";
            // atomicity = 104;
            massAverage = 999;
            massAverageUncertainty = 0;


            isotopeNumber = 999;
            isotopeMass = 999;
            isotopeProbability = 0.5;
            Isotope NewIsotope999 = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);
            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope999);
            monoIsotopicMass = newIsotopeDictionary[(newElement.Symbol + isotopeNumber).ToString()].Mass;

            newElement.IsotopeDictionary = newIsotopeDictionary;
            newElement.MassMonoIsotopic = monoIsotopicMass;
            newElement.MassAverage = massAverage; //IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            newElement.MassAverageUncertainty = massAverageUncertainty;

            elementList.Add(newElement);
            elementSymbolList.Add(newElement.Symbol);
            

        }
    }
}
