using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Element
    {
        public List<Isotope> IsotopeList { get; set; }

        public string Name { get; set; }
        public string Symbol { get; set; }
       
        public double AverageMass
        {
            get { return GetAverageMass(); }
        }

        public double MonoIsotopicMass
        {
            get { return GetMostAbundantIsotope().Mass; }
        }

        private double GetAverageMass()
        {
            double averageMass = 0;
            foreach (Isotope isotope in this.IsotopeList)
            {
                averageMass += isotope.Mass * isotope.NaturalAbundance;
            }
            return averageMass;
        }

        private Isotope GetMostAbundantIsotope()
        {
            double abundance = 0;
            Isotope mostAbundantIsotope = new Isotope(0, 0, 0);

            foreach (Isotope isotope in this.IsotopeList)
            {
                if (isotope.NaturalAbundance > abundance)
                {
                    mostAbundantIsotope = isotope;
                    abundance = isotope.NaturalAbundance;
                }
            }
            return mostAbundantIsotope;
        }
    }
}
