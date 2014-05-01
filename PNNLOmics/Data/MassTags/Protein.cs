namespace PNNLOmics.Data
{
    /// <summary>
    /// Class that holds information about a protein.
    /// </summary>
    public class Protein: Molecule
    {

        public Protein()
        {
            Clear();
        }

        /// <summary>
        /// Gets or sets the protein sequence of amino acids.
        /// </summary>
        public string Sequence
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the reference ID
        /// </summary>
        public int RefID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the protein ID
        /// </summary>
        public int ProteinID
        {
            get;
            set;
        }
        public string ProteinDescription
        {
            get;
            set;
        }
        /// <summary>
        /// Clears the 
        /// </summary>
        public override void Clear()
        {
            RefID    = -1;
            Sequence = "";
        }
    }

    /// <summary>
    /// Class that holds information about a protein.
    /// </summary>
    public class Metabolite : Molecule
    {

        /// <summary>
        /// Clears the 
        /// </summary>
        public override void Clear()
        {
        }
    }
}
