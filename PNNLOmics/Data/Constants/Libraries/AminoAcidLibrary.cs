using System;
using System.Collections.Generic;

/// <example>
/// dictionary implementation
/// Dictionary<string, Compound> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
/// double AAmass = AminoAcidsDictionary["A"].MonoIsotopicMass;
/// string AAName = AminoAcidsDictionary["A"].Name;
/// string AAFormula = AminoAcidsDictionary["A"].ChemicalFormula;
///
/// one line implementation
/// double AAmass2 = AminoAcidConstantsStaticLibrary.GetMonoisotopicMass("A");
/// string AAName2 = AminoAcidConstantsStaticLibrary.GetName("A");
/// string AAFormula2 = AminoAcidConstantsStaticLibrary.GetFormula("A");
///
/// double mass3 = AminoAcidStaticLibrary.GetMonoisotopicMass(SelectAminoAcid.GlutamicAcid);
///
/// how to calculate the mass of a peptide
/// double massPeptide=0;
/// string peptideSequence = "NRTL";
/// {
///     massPeptide += AminoAcidStaticLibrary.GetMonoisotopicMass(peptideSequence[y].ToString());
/// }
/// massPeptide = 484.27578094385393
/// </example>

namespace PNNLOmics.Data.Constants.Libraries
{
    //TODO: SCOTT - CR - add XML comments
    public class AminoAcidLibrary : MatterLibrary<Compound, AminoAcidName>
    {
       //TODO:  SCOTT this is not a class
        /// <summary>
        /// This is a Class designed to create amino acids objects from the elements.
        /// The amino acids are added to a Dictionary searchable by char keys such as 'A' for Alanine.
        /// This is the only library with char keys.
        /// </summary>		
        public override void LoadLibrary()
        {
            //TODO: CONSTANTS Load data from XML - put this data into XML

            m_symbolToCompoundMap = new Dictionary<string, Compound>();
            m_enumToSymbolMap = new Dictionary<AminoAcidName, string>();

            //each integer stands for the number of atoms in the compound -->X.NewElements(C H N O S P)
            //alanine.NewElements(C H N O S P)

            Compound alanine = new Compound();
            alanine.NewElements(3, 5, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            alanine.Name = "Alanine";
            alanine.Symbol = "A";
            alanine.ChemicalFormula = "C3H5NO";
            alanine.MassMonoIsotopic = Compound.GetMonoisotopicMass(alanine);

            Compound arginine = new Compound();

            arginine.NewElements(6, 12, 4, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            arginine.Name = "Arginine";
            arginine.Symbol = "R";
            arginine.ChemicalFormula = "C6H12N4O";
            arginine.MassMonoIsotopic = Compound.GetMonoisotopicMass(arginine);

            Compound asparagine = new Compound();
            asparagine.NewElements(4, 6, 2, 2, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            asparagine.Name = "Asparagine";
            asparagine.Symbol = "N";
            asparagine.ChemicalFormula = "C4H6N2O2";
            asparagine.MassMonoIsotopic = Compound.GetMonoisotopicMass(asparagine);

            Compound asparticAcid = new Compound();
            asparticAcid.NewElements(4, 5, 1, 3, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            asparticAcid.Name = "AsparticAcid";
            asparticAcid.Symbol = "D";
            asparticAcid.ChemicalFormula = "C4H5NO3";
            asparticAcid.MassMonoIsotopic = Compound.GetMonoisotopicMass(asparticAcid);

            Compound cysteine = new Compound();
            cysteine.NewElements(3, 5, 1, 1, 1, 0);//-->X.NewElements(C H N O S P) number of atoms
            cysteine.Name = "Cysteine";
            cysteine.Symbol = "C";
            cysteine.ChemicalFormula = "C3H5NOS";
            cysteine.MassMonoIsotopic = Compound.GetMonoisotopicMass(cysteine);

            Compound glutamicAcid = new Compound();
            glutamicAcid.NewElements(5, 7, 1, 3, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            glutamicAcid.Name = "GlutamicAcid";
            glutamicAcid.Symbol = "E";
            glutamicAcid.ChemicalFormula = "C5H7NO3";
            glutamicAcid.MassMonoIsotopic = Compound.GetMonoisotopicMass(glutamicAcid);

            Compound glutamine = new Compound();
            glutamine.NewElements(5, 8, 2, 2, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            glutamine.Name = "Glutamine";
            glutamine.Symbol = "Q";
            glutamine.ChemicalFormula = "C5H8N2O2";
            glutamine.MassMonoIsotopic = Compound.GetMonoisotopicMass(glutamine);

            Compound glycine = new Compound();
            glycine.NewElements(2, 3, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            glycine.Name = "Glycine";
            glycine.Symbol = "G";
            glycine.ChemicalFormula = "C2H3NO";
            glycine.MassMonoIsotopic = Compound.GetMonoisotopicMass(glycine);

            Compound histidine = new Compound();
            histidine.NewElements(6, 7, 3, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            histidine.Name = "Histidine";
            histidine.Symbol = "H";
            histidine.ChemicalFormula = "C6H7N3O";
            histidine.MassMonoIsotopic = Compound.GetMonoisotopicMass(histidine);

            Compound isoleucine = new Compound();
            isoleucine.NewElements(6, 11, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            isoleucine.Name = "Isoleucine";
            isoleucine.Symbol = "I";
            isoleucine.ChemicalFormula = "C6H11NO";
            isoleucine.MassMonoIsotopic = Compound.GetMonoisotopicMass(isoleucine);

            Compound leucine = new Compound();
            leucine.NewElements(6, 11, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            leucine.Name = "Leucine";
            leucine.Symbol = "L";
            leucine.ChemicalFormula = "C6H11NO";
            leucine.MassMonoIsotopic = Compound.GetMonoisotopicMass(leucine);

            Compound lysine = new Compound();
            lysine.NewElements(6, 12, 2, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            lysine.Name = "Lysine";
            lysine.Symbol = "K";
            lysine.ChemicalFormula = "C6H12N2O";
            lysine.MassMonoIsotopic = Compound.GetMonoisotopicMass(lysine);

            Compound methionine = new Compound();
            methionine.NewElements(5, 9, 1, 1, 1, 0);//-->X.NewElements(C H N O S P) number of atoms
            methionine.Name = "Methionine";
            methionine.Symbol = "M";
            methionine.ChemicalFormula = "C5H9NOS";
            methionine.MassMonoIsotopic = Compound.GetMonoisotopicMass(methionine);

            Compound phenylalanine = new Compound();
            phenylalanine.NewElements(9, 9, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            phenylalanine.Name = "Phenylalanine";
            phenylalanine.Symbol = "F";
            phenylalanine.ChemicalFormula = "C9H9NO";
            phenylalanine.MassMonoIsotopic = Compound.GetMonoisotopicMass(phenylalanine);

            Compound proline = new Compound();
            proline.NewElements(5, 7, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            proline.Name = "Proline";
            proline.Symbol = "P";
            proline.ChemicalFormula = "C5H7NO";
            proline.MassMonoIsotopic = Compound.GetMonoisotopicMass(proline);

            Compound serine = new Compound();
            serine.NewElements(11, 10, 2, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            serine.Name = "Serine";
            serine.Symbol = "S";
            serine.ChemicalFormula = "C3H5NO2";
            serine.MassMonoIsotopic = Compound.GetMonoisotopicMass(serine);

            Compound threonine = new Compound();
            threonine.NewElements(4, 7, 1, 2, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            threonine.Name = "Threonine";
            threonine.Symbol = "T";
            threonine.ChemicalFormula = "C4H7NO2";
            threonine.MassMonoIsotopic = Compound.GetMonoisotopicMass(threonine);

            Compound tryptophan = new Compound();
            tryptophan.NewElements(11, 10, 2, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            tryptophan.Name = "Tryptophan";
            tryptophan.Symbol = "W";
            tryptophan.ChemicalFormula = "C11H10N2O";
            tryptophan.MassMonoIsotopic = Compound.GetMonoisotopicMass(tryptophan);

            Compound tyrosine = new Compound();
            tyrosine.NewElements(9, 9, 1, 2, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            tyrosine.Name = "Tyrosine";
            tyrosine.Symbol = "Y";
            tyrosine.ChemicalFormula = "C9H9NO2";
            tyrosine.MassMonoIsotopic = Compound.GetMonoisotopicMass(tyrosine);

            Compound valine = new Compound();
            valine.NewElements(5, 9, 1, 1, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            valine.Name = "Valine";
            valine.Symbol = "V";
            valine.ChemicalFormula = "C5H9NO";
            valine.MassMonoIsotopic = Compound.GetMonoisotopicMass(valine);

            m_symbolToCompoundMap.Add(alanine.Symbol, alanine);
            m_symbolToCompoundMap.Add(arginine.Symbol, arginine);
            m_symbolToCompoundMap.Add(asparagine.Symbol, asparagine);
            m_symbolToCompoundMap.Add(asparticAcid.Symbol, asparticAcid);
            m_symbolToCompoundMap.Add(cysteine.Symbol, cysteine);
            m_symbolToCompoundMap.Add(glutamicAcid.Symbol, glutamicAcid);
            m_symbolToCompoundMap.Add(glutamine.Symbol, glutamine);
            m_symbolToCompoundMap.Add(glycine.Symbol, glycine);
            m_symbolToCompoundMap.Add(histidine.Symbol, histidine);
            m_symbolToCompoundMap.Add(isoleucine.Symbol, isoleucine);
            m_symbolToCompoundMap.Add(leucine.Symbol, leucine);
            m_symbolToCompoundMap.Add(lysine.Symbol, lysine);
            m_symbolToCompoundMap.Add(methionine.Symbol, methionine);
            m_symbolToCompoundMap.Add(phenylalanine.Symbol, phenylalanine);
            m_symbolToCompoundMap.Add(proline.Symbol, proline);
            m_symbolToCompoundMap.Add(serine.Symbol, serine);
            m_symbolToCompoundMap.Add(threonine.Symbol, threonine);
            m_symbolToCompoundMap.Add(tryptophan.Symbol, tryptophan);
            m_symbolToCompoundMap.Add(tyrosine.Symbol, tyrosine);
            m_symbolToCompoundMap.Add(valine.Symbol, valine);

            m_enumToSymbolMap.Add(AminoAcidName.Alanine, alanine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Arginine, arginine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Asparagine, asparagine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.AsparticAcid, asparticAcid.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Cysteine, cysteine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.GlutamicAcid, glutamicAcid.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Glutamine, glutamine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Glycine, glycine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Histidine, histidine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Isoleucine, isoleucine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Leucine, leucine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Lysine, lysine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Methionine, methionine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Phenylalanine, phenylalanine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Proline, proline.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Serine, serine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Threonine, threonine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Tryptophan, tryptophan.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Tyrosine, tyrosine.Symbol);
            m_enumToSymbolMap.Add(AminoAcidName.Valine, valine.Symbol);
        }
    }

}
