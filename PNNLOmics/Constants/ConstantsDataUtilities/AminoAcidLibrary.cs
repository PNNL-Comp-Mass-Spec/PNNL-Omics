using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//dictionary implementation
//Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
//double AAmass = AminoAcidsDictionary['A'].MonoIsotopicMass;
//string AAName = AminoAcidsDictionary['A'].Name;
//string AAFormula = AminoAcidsDictionary['A'].ChemicalFormula;


namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class AminoAcidLibrary
    {
        public static Dictionary<char, AminoAcidObject> loadAminoAcidData()
        {
            Dictionary<char, AminoAcidObject> aminoAcidsDictionary = new Dictionary<char, AminoAcidObject>();

            //Alanine.NewElements(C H N O S P)

            AminoAcidObject alanine = new AminoAcidObject();
            alanine.NewElements(3, 5, 1, 1, 0, 0);
            alanine.Name = "Alanine";
            alanine.SingleLetterCode = 'A';
            alanine.ChemicalFormula = "C3H5NO";
            alanine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(alanine);

            AminoAcidObject arginine = new AminoAcidObject();
            arginine.NewElements(6, 12, 4, 1, 0, 0);
            arginine.Name = "Arginine";
            arginine.SingleLetterCode = 'R';
            arginine.ChemicalFormula = "C6H12N4O";
            arginine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(arginine);

            AminoAcidObject asparagine = new AminoAcidObject();
            asparagine.NewElements(4, 6, 2, 2, 0, 0);
            asparagine.Name = "Asparagine";
            asparagine.SingleLetterCode = 'N';
            asparagine.ChemicalFormula = "C4H6N2O2";
            asparagine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(asparagine);

            AminoAcidObject asparticAcid = new AminoAcidObject();
            asparticAcid.NewElements(4, 5, 1, 3, 0, 0);
            asparticAcid.Name = "AsparticAcid";
            asparticAcid.SingleLetterCode = 'D';
            asparticAcid.ChemicalFormula = "C5H5NO3";
            asparticAcid.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(asparticAcid);

            AminoAcidObject cysteine = new AminoAcidObject();
            cysteine.NewElements(3, 5, 1, 1, 1, 0);
            cysteine.Name = "Cysteine";
            cysteine.SingleLetterCode = 'C';
            cysteine.ChemicalFormula = "C3H5NOS";
            cysteine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(cysteine);

            AminoAcidObject glutamicAcid = new AminoAcidObject();
            glutamicAcid.NewElements(5, 7, 1, 3, 0, 0);
            glutamicAcid.Name = "GlutamicAcid";
            glutamicAcid.SingleLetterCode = 'E';
            glutamicAcid.ChemicalFormula = "C5H7NO3";
            glutamicAcid.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(glutamicAcid);

            AminoAcidObject glutamine = new AminoAcidObject();
            glutamine.NewElements(5, 8, 2, 2, 0, 0);
            glutamine.Name = "Glutamine";
            glutamine.SingleLetterCode = 'Q';
            glutamine.ChemicalFormula = "C5H8N2O2";
            glutamine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(glutamine);

            AminoAcidObject glycine = new AminoAcidObject();
            glycine.NewElements(2, 3, 1, 1, 0, 0);
            glycine.Name = "Glycine";
            glycine.SingleLetterCode = 'G';
            glycine.ChemicalFormula = "C2H3NO";
            glycine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(glycine);

            AminoAcidObject histidine = new AminoAcidObject();
            histidine.NewElements(6, 7, 3, 1, 0, 0);
            histidine.Name = "Histidine";
            histidine.SingleLetterCode = 'H';
            histidine.ChemicalFormula = "C6H7N3O";
            histidine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(histidine);

            AminoAcidObject isoleucine = new AminoAcidObject();
            isoleucine.NewElements(6, 11, 1, 1, 0, 0);
            isoleucine.Name = "Isoleucine";
            isoleucine.SingleLetterCode = 'I';
            isoleucine.ChemicalFormula = "C6H11NO";
            isoleucine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(isoleucine);

            AminoAcidObject leucine = new AminoAcidObject();
            leucine.NewElements(6, 11, 1, 1, 0, 0);
            leucine.Name = "Leucine";
            leucine.SingleLetterCode = 'L';
            leucine.ChemicalFormula = "C6H11NO";
            leucine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(leucine);

            AminoAcidObject lysine = new AminoAcidObject();
            lysine.NewElements(6, 12, 2, 1, 0, 0);
            lysine.Name = "Lysine";
            lysine.SingleLetterCode = 'K';
            lysine.ChemicalFormula = "C6H12N2O";
            lysine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(lysine);

            AminoAcidObject methionine = new AminoAcidObject();
            methionine.NewElements(5, 9, 1, 1, 1, 0);
            methionine.Name = "Methionine";
            methionine.SingleLetterCode = 'M';
            methionine.ChemicalFormula = "C5H9NOS";
            methionine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(methionine);

            AminoAcidObject phenylalanine = new AminoAcidObject();
            phenylalanine.NewElements(9, 9, 1, 1, 0, 0);
            phenylalanine.Name = "Phenylalanine";
            phenylalanine.SingleLetterCode = 'F';
            phenylalanine.ChemicalFormula = "C9H9NO";
            phenylalanine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(phenylalanine);

            AminoAcidObject proline = new AminoAcidObject();
            proline.NewElements(5, 7, 1, 1, 0, 0);
            proline.Name = "Proline";
            proline.SingleLetterCode = 'P';
            proline.ChemicalFormula = "C5H7NO";
            proline.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(proline);

            AminoAcidObject serine = new AminoAcidObject();
            serine.NewElements(11, 10, 2, 1, 0, 0);
            serine.Name = "Serine";
            serine.SingleLetterCode = 'S';
            serine.ChemicalFormula = "C3H5NO2";
            serine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(serine);

            AminoAcidObject threonine = new AminoAcidObject();
            threonine.NewElements(4, 7, 1, 2, 0, 0);
            threonine.Name = "Threonine";
            threonine.SingleLetterCode = 'T';
            threonine.ChemicalFormula = "C4H7NO";
            threonine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(threonine);

            AminoAcidObject tryptophan = new AminoAcidObject();
            tryptophan.NewElements(11, 10, 2, 1, 0, 0);
            tryptophan.Name = "Tryptophan";
            tryptophan.SingleLetterCode = 'W';
            tryptophan.ChemicalFormula = "C11H10N2O";
            tryptophan.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(tryptophan);

            AminoAcidObject tyrosine = new AminoAcidObject();
            tyrosine.NewElements(9, 9, 1, 2, 0, 0);
            tyrosine.Name = "Tyrosine";
            tyrosine.SingleLetterCode = 'Y';
            tyrosine.ChemicalFormula = "C9H9NO2";
            tyrosine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(tyrosine);

            AminoAcidObject valine = new AminoAcidObject();
            valine.NewElements(5, 9, 1, 1, 0, 0);
            valine.Name = "Valine";
            valine.SingleLetterCode = 'V';
            valine.ChemicalFormula = "C5H9NO";
            valine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(valine);

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
