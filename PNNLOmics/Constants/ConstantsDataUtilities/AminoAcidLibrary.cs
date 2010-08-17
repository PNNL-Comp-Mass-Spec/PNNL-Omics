using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsDataLayer;

//dictionary implementation
//Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
//double AAmass = AminoAcidsDictionary['A'].MonoIsotopicMass;
//string AAName = AminoAcidsDictionary['A'].Name;
//string AAFormula = AminoAcidsDictionary['A'].ChemicalFormula;


namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class AminoAcidLibrary
    {
        //TODO: SRK Method name should start with a capital letter.
        public static Dictionary<char, AminoAcid> loadAminoAcidData()
        {
            Dictionary<char, AminoAcid> aminoAcidsDictionary = new Dictionary<char, AminoAcid>();

            //Alanine.NewElements(C H N O S P)

            AminoAcid alanine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            alanine.NewElements(3, 5, 1, 1, 0, 0);
            alanine.Name = "Alanine";
            alanine.SingleLetterCode = 'A';
            alanine.ChemicalFormula = "C3H5NO";
            alanine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(alanine);

            AminoAcid arginine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            arginine.NewElements(6, 12, 4, 1, 0, 0);
            arginine.Name = "Arginine";
            arginine.SingleLetterCode = 'R';
            arginine.ChemicalFormula = "C6H12N4O";
            arginine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(arginine);

            AminoAcid asparagine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            asparagine.NewElements(4, 6, 2, 2, 0, 0);
            asparagine.Name = "Asparagine";
            asparagine.SingleLetterCode = 'N';
            asparagine.ChemicalFormula = "C4H6N2O2";
            asparagine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(asparagine);

            AminoAcid asparticAcid = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            asparticAcid.NewElements(4, 5, 1, 3, 0, 0);
            asparticAcid.Name = "AsparticAcid";
            asparticAcid.SingleLetterCode = 'D';
            asparticAcid.ChemicalFormula = "C5H5NO3";
            asparticAcid.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(asparticAcid);

            AminoAcid cysteine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            cysteine.NewElements(3, 5, 1, 1, 1, 0);
            cysteine.Name = "Cysteine";
            cysteine.SingleLetterCode = 'C';
            cysteine.ChemicalFormula = "C3H5NOS";
            cysteine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(cysteine);

            AminoAcid glutamicAcid = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            glutamicAcid.NewElements(5, 7, 1, 3, 0, 0);
            glutamicAcid.Name = "GlutamicAcid";
            glutamicAcid.SingleLetterCode = 'E';
            glutamicAcid.ChemicalFormula = "C5H7NO3";
            glutamicAcid.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(glutamicAcid);

            AminoAcid glutamine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            glutamine.NewElements(5, 8, 2, 2, 0, 0);
            glutamine.Name = "Glutamine";
            glutamine.SingleLetterCode = 'Q';
            glutamine.ChemicalFormula = "C5H8N2O2";
            glutamine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(glutamine);

            AminoAcid glycine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            glycine.NewElements(2, 3, 1, 1, 0, 0);
            glycine.Name = "Glycine";
            glycine.SingleLetterCode = 'G';
            glycine.ChemicalFormula = "C2H3NO";
            glycine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(glycine);

            AminoAcid histidine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            histidine.NewElements(6, 7, 3, 1, 0, 0);
            histidine.Name = "Histidine";
            histidine.SingleLetterCode = 'H';
            histidine.ChemicalFormula = "C6H7N3O";
            histidine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(histidine);

            AminoAcid isoleucine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            isoleucine.NewElements(6, 11, 1, 1, 0, 0);
            isoleucine.Name = "Isoleucine";
            isoleucine.SingleLetterCode = 'I';
            isoleucine.ChemicalFormula = "C6H11NO";
            isoleucine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(isoleucine);

            AminoAcid leucine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            leucine.NewElements(6, 11, 1, 1, 0, 0);
            leucine.Name = "Leucine";
            leucine.SingleLetterCode = 'L';
            leucine.ChemicalFormula = "C6H11NO";
            leucine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(leucine);

            AminoAcid lysine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            lysine.NewElements(6, 12, 2, 1, 0, 0);
            lysine.Name = "Lysine";
            lysine.SingleLetterCode = 'K';
            lysine.ChemicalFormula = "C6H12N2O";
            lysine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(lysine);

            AminoAcid methionine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            methionine.NewElements(5, 9, 1, 1, 1, 0);
            methionine.Name = "Methionine";
            methionine.SingleLetterCode = 'M';
            methionine.ChemicalFormula = "C5H9NOS";
            methionine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(methionine);

            AminoAcid phenylalanine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            phenylalanine.NewElements(9, 9, 1, 1, 0, 0);
            phenylalanine.Name = "Phenylalanine";
            phenylalanine.SingleLetterCode = 'F';
            phenylalanine.ChemicalFormula = "C9H9NO";
            phenylalanine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(phenylalanine);

            AminoAcid proline = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            proline.NewElements(5, 7, 1, 1, 0, 0);
            proline.Name = "Proline";
            proline.SingleLetterCode = 'P';
            proline.ChemicalFormula = "C5H7NO";
            proline.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(proline);

            AminoAcid serine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            serine.NewElements(11, 10, 2, 1, 0, 0);
            serine.Name = "Serine";
            serine.SingleLetterCode = 'S';
            serine.ChemicalFormula = "C3H5NO2";
            serine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(serine);

            AminoAcid threonine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            threonine.NewElements(4, 7, 1, 2, 0, 0);
            threonine.Name = "Threonine";
            threonine.SingleLetterCode = 'T';
            threonine.ChemicalFormula = "C4H7NO";
            threonine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(threonine);

            AminoAcid tryptophan = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            tryptophan.NewElements(11, 10, 2, 1, 0, 0);
            tryptophan.Name = "Tryptophan";
            tryptophan.SingleLetterCode = 'W';
            tryptophan.ChemicalFormula = "C11H10N2O";
            tryptophan.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(tryptophan);

            AminoAcid tyrosine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            tyrosine.NewElements(9, 9, 1, 2, 0, 0);
            tyrosine.Name = "Tyrosine";
            tyrosine.SingleLetterCode = 'Y';
            tyrosine.ChemicalFormula = "C9H9NO2";
            tyrosine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(tyrosine);

            AminoAcid valine = new AminoAcid();
            //TODO: SRK What are these constants for?  These are "magic" numbers.   Add a comment or constants
            valine.NewElements(5, 9, 1, 1, 0, 0);
            valine.Name = "Valine";
            valine.SingleLetterCode = 'V';
            valine.ChemicalFormula = "C5H9NO";
            valine.MonoIsotopicMass = AminoAcid.GetMonoisotopicMass(valine);

            aminoAcidsDictionary.Add(alanine.SingleLetterCode, alanine);
            aminoAcidsDictionary.Add(arginine.SingleLetterCode, arginine);
            aminoAcidsDictionary.Add(asparagine.SingleLetterCode, asparagine);
            aminoAcidsDictionary.Add(asparticAcid.SingleLetterCode, asparticAcid);
            aminoAcidsDictionary.Add(cysteine.SingleLetterCode, cysteine);
            aminoAcidsDictionary.Add(glutamicAcid.SingleLetterCode, glutamicAcid);
            aminoAcidsDictionary.Add(glutamine.SingleLetterCode, glutamine);
            aminoAcidsDictionary.Add(glycine.SingleLetterCode, glycine);
            aminoAcidsDictionary.Add(histidine.SingleLetterCode, histidine);
            aminoAcidsDictionary.Add(isoleucine.SingleLetterCode, isoleucine);
            aminoAcidsDictionary.Add(leucine.SingleLetterCode, leucine);
            aminoAcidsDictionary.Add(lysine.SingleLetterCode, lysine);
            aminoAcidsDictionary.Add(methionine.SingleLetterCode, methionine);
            aminoAcidsDictionary.Add(phenylalanine.SingleLetterCode, phenylalanine);
            aminoAcidsDictionary.Add(proline.SingleLetterCode, proline);
            aminoAcidsDictionary.Add(serine.SingleLetterCode, serine);
            aminoAcidsDictionary.Add(threonine.SingleLetterCode, threonine);
            aminoAcidsDictionary.Add(tryptophan.SingleLetterCode, tryptophan);
            aminoAcidsDictionary.Add(tyrosine.SingleLetterCode, tyrosine);
            aminoAcidsDictionary.Add(valine.SingleLetterCode, valine);
           
            return aminoAcidsDictionary;
        }

    }
}
