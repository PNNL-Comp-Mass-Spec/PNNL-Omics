using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class ElementQuantity
    {
        public ElementQuantity(Element element, int quantity)
        {
            this.Element = element;
            this.Quantity = quantity;

        }
        
        public Element Element { get; set; }
        public string ElementName { get { return this.Element.Name; } }

        public int Quantity { get; set; }


    }
}
