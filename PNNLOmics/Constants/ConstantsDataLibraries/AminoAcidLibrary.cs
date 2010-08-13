using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//dictionary implementation
//Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
//double AAmass = AminoAcidsDictionary['A'].MonoIsotopicMass;
//string AAName = AminoAcidsDictionary['A'].Name;
//string AAFormula = AminoAcidsDictionary['A'].ChemicalFormula;


namespace Constants
{
    public class AminoAcidLibrary
    {
        public static Dictionary<char, AminoAcidObject> LoadAminoAcidData()
        {
            Dictionary<char, AminoAcidObject> AminoAcidsDictionary = new Dictionary<char, AminoAcidObject>();

            //Alanine.NewElements(C H N O S P)

            AminoAcidObject Alanine = new AminoAcidObject();
            Alanine.NewElements(3, 5, 1, 1, 0, 0);
            Alanine.Name = "Alanine";
            Alanine.SingleLetterCode = 'A';
            Alanine.ChemicalFormula = "C3H5NO";
            Alanine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Alanine);

            AminoAcidObject Arginine = new AminoAcidObject();
            Arginine.NewElements(6, 12, 4, 1, 0, 0);
            Arginine.Name = "Arginine";
            Arginine.SingleLetterCode = 'R';
            Arginine.ChemicalFormula = "C6H12N4O";
            Arginine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Arginine);

            AminoAcidObject Asparagine = new AminoAcidObject();
            Asparagine.NewElements(4, 6, 2, 2, 0, 0);
            Asparagine.Name = "Asparagine";
            Asparagine.SingleLetterCode = 'N';
            Asparagine.ChemicalFormula = "C4H6N2O2";
            Asparagine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Asparagine);

            AminoAcidObject AsparticAcid = new AminoAcidObject();
            AsparticAcid.NewElements(4, 5, 1, 3, 0, 0);
            AsparticAcid.Name = "AsparticAcid";
            AsparticAcid.SingleLetterCode = 'D';
            AsparticAcid.ChemicalFormula = "C5H5NO3";
            AsparticAcid.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(AsparticAcid);

            AminoAcidObject Cysteine = new AminoAcidObject();
            Cysteine.NewElements(3, 5, 1, 1, 1, 0);
            Cysteine.Name = "Cysteine";
            Cysteine.SingleLetterCode = 'C';
            Cysteine.ChemicalFormula = "C3H5NOS";
            Cysteine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Cysteine);

            AminoAcidObject GlutamicAcid = new AminoAcidObject();
            GlutamicAcid.NewElements(5, 7, 1, 3, 0, 0);
            GlutamicAcid.Name = "GlutamicAcid";
            GlutamicAcid.SingleLetterCode = 'E';
            GlutamicAcid.ChemicalFormula = "C5H7NO3";
            GlutamicAcid.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(GlutamicAcid);

            AminoAcidObject Glutamine = new AminoAcidObject();
            Glutamine.NewElements(5, 8, 2, 2, 0, 0);
            Glutamine.Name = "Glutamine";
            Glutamine.SingleLetterCode = 'Q';
            Glutamine.ChemicalFormula = "C5H8N2O2";
            Glutamine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Glutamine);

            AminoAcidObject Glycine = new AminoAcidObject();
            Glycine.NewElements(2, 3, 1, 1, 0, 0);
            Glycine.Name = "Glycine";
            Glycine.SingleLetterCode = 'G';
            Glycine.ChemicalFormula = "C2H3NO";
            Glycine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Glycine);

            AminoAcidObject Histidine = new AminoAcidObject();
            Histidine.NewElements(6, 7, 3, 1, 0, 0);
            Histidine.Name = "Histidine";
            Histidine.SingleLetterCode = 'H';
            Histidine.ChemicalFormula = "C6H7N3O";
            Histidine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Histidine);

            AminoAcidObject Isoleucine = new AminoAcidObject();
            Isoleucine.NewElements(6, 11, 1, 1, 0, 0);
            Isoleucine.Name = "Isoleucine";
            Isoleucine.SingleLetterCode = 'I';
            Isoleucine.ChemicalFormula = "C6H11NO";
            Isoleucine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Isoleucine);

            AminoAcidObject Leucine = new AminoAcidObject();
            Leucine.NewElements(6, 11, 1, 1, 0, 0);
            Leucine.Name = "Leucine";
            Leucine.SingleLetterCode = 'L';
            Leucine.ChemicalFormula = "C6H11NO";
            Leucine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Leucine);

            AminoAcidObject Lysine = new AminoAcidObject();
            Lysine.NewElements(6, 12, 2, 1, 0, 0);
            Lysine.Name = "Lysine";
            Lysine.SingleLetterCode = 'K';
            Lysine.ChemicalFormula = "C6H12N2O";
            Lysine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Lysine);

            AminoAcidObject Methionine = new AminoAcidObject();
            Methionine.NewElements(5, 9, 1, 1, 1, 0);
            Methionine.Name = "Methionine";
            Methionine.SingleLetterCode = 'M';
            Methionine.ChemicalFormula = "C5H9NOS";
            Methionine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Methionine);

            AminoAcidObject Phenylalanine = new AminoAcidObject();
            Phenylalanine.NewElements(9, 9, 1, 1, 0, 0);
            Phenylalanine.Name = "Phenylalanine";
            Phenylalanine.SingleLetterCode = 'F';
            Phenylalanine.ChemicalFormula = "C9H9NO";
            Phenylalanine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Phenylalanine);

            AminoAcidObject Proline = new AminoAcidObject();
            Proline.NewElements(5, 7, 1, 1, 0, 0);
            Proline.Name = "Proline";
            Proline.SingleLetterCode = 'P';
            Proline.ChemicalFormula = "C5H7NO";
            Proline.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Proline);

            AminoAcidObject Serine = new AminoAcidObject();
            Serine.NewElements(11, 10, 2, 1, 0, 0);
            Serine.Name = "Serine";
            Serine.SingleLetterCode = 'S';
            Serine.ChemicalFormula = "C3H5NO2";
            Serine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Serine);

            AminoAcidObject Threonine = new AminoAcidObject();
            Threonine.NewElements(4, 7, 1, 2, 0, 0);
            Threonine.Name = "Threonine";
            Threonine.SingleLetterCode = 'T';
            Threonine.ChemicalFormula = "C4H7NO";
            Threonine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Threonine);

            AminoAcidObject Tryptophan = new AminoAcidObject();
            Tryptophan.NewElements(11, 10, 2, 1, 0, 0);
            Tryptophan.Name = "Tryptophan";
            Tryptophan.SingleLetterCode = 'W';
            Tryptophan.ChemicalFormula = "C11H10N2O";
            Tryptophan.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Tryptophan);

            AminoAcidObject Tyrosine = new AminoAcidObject();
            Tyrosine.NewElements(9, 9, 1, 2, 0, 0);
            Tyrosine.Name = "Tyrosine";
            Tyrosine.SingleLetterCode = 'Y';
            Tyrosine.ChemicalFormula = "C9H9NO2";
            Tyrosine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Tyrosine);

            AminoAcidObject Valine = new AminoAcidObject();
            Valine.NewElements(5, 9, 1, 1, 0, 0);
            Valine.Name = "Valine";
            Valine.SingleLetterCode = 'V';
            Valine.ChemicalFormula = "C5H9NO";
            Valine.MonoIsotopicMass = AminoAcidObject.GetMonoisotopicMass(Valine);

            AminoAcidsDictionary.Add(Alanine.SingleLetterCode, Alanine);
            AminoAcidsDictionary.Add(Arginine.SingleLetterCode, Arginine);
            AminoAcidsDictionary.Add(Asparagine.SingleLetterCode, Asparagine);
            AminoAcidsDictionary.Add(AsparticAcid.SingleLetterCode, AsparticAcid);
            AminoAcidsDictionary.Add(Cysteine.SingleLetterCode, Cysteine);
            AminoAcidsDictionary.Add(GlutamicAcid.SingleLetterCode, GlutamicAcid);
            AminoAcidsDictionary.Add(Glutamine.SingleLetterCode, Glutamine);
            AminoAcidsDictionary.Add(Glycine.SingleLetterCode, Glycine);
            AminoAcidsDictionary.Add(Histidine.SingleLetterCode, Histidine);
            AminoAcidsDictionary.Add(Isoleucine.SingleLetterCode, Isoleucine);
            AminoAcidsDictionary.Add(Leucine.SingleLetterCode, Leucine);
            AminoAcidsDictionary.Add(Lysine.SingleLetterCode, Lysine);
            AminoAcidsDictionary.Add(Methionine.SingleLetterCode, Methionine);
            AminoAcidsDictionary.Add(Phenylalanine.SingleLetterCode, Phenylalanine);
            AminoAcidsDictionary.Add(Proline.SingleLetterCode, Proline);
            AminoAcidsDictionary.Add(Serine.SingleLetterCode, Serine);
            AminoAcidsDictionary.Add(Threonine.SingleLetterCode, Threonine);
            AminoAcidsDictionary.Add(Tryptophan.SingleLetterCode, Tryptophan);
            AminoAcidsDictionary.Add(Tyrosine.SingleLetterCode, Tyrosine);
            AminoAcidsDictionary.Add(Valine.SingleLetterCode, Valine);
           
            return AminoAcidsDictionary;
        }

    }
}
