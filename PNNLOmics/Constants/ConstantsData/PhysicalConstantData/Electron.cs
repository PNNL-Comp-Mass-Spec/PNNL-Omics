using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class Electron : PhysicalConstant
    {
        public Electron()
        {
            this.Name = "Electron";
            this.Symbol = "e-";
            this.ExactMass = 0.00054857990943;//Da glycolyzer
        }
    }
}
