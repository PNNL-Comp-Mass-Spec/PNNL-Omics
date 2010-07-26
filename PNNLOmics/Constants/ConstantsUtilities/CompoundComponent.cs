using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class CompoundComponent
    {
        public List<ElementQuantity> Composition { get; set; }

        public CompoundComponent()
        {

        }

        public double MonoIsotopicMass
        {
            get { return getMonoIsotopicMass(); }
        }

        private double getMonoIsotopicMass()
        {
            double monoIsotopicMass = 0;

            foreach (ElementQuantity eq in Composition)
            {
                monoIsotopicMass += eq.Element.MonoIsotopicMass * eq.Quantity;

            }

            return monoIsotopicMass;

        }

        //protected internal void RemoveElement(Elements.Element removedElement,int numberToRemove)
        //{

            //foreach (KeyValuePair<Elements.Element,int> element in Composition)
            //{
            //    if (element.Key.GetType() == removedElement.GetType())
            //    {
            //        Composition[element.Key] -= numberToRemove;
            //        return;
            //    }
             
            //}
   
            //int totalElement = Composition[element];

            //if (numberToRemove <= totalElement)
            //{
            //    Composition[element] -= numberToRemove;
            //}
            //else
            //{
            //    throw new Exception("Error removing " + element.ToString() + " atoms; perhaps you tried removing too many");
            //}

            
        //}



    }
}
