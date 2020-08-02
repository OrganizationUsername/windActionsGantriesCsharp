using System;
using System.Collections.Generic;
using System.Text;

namespace windActionsGantries
{
    /// <summary>
    /// Class to store common properties, to be used by EN1991 and AS1170 calcs
    /// </summary>
    class Common
    {
        /// <summary>
        /// Common properties such as geometry and natural frequency of the structure
        /// </summary>
        public double z { get; set; }
        public double b { get; set; }
        public double h { get; set; }
        public double n { get; set; }
        public double vb { get; set; }
        public double cf { get; set; }

        /// <summary>
        /// Test constructor method, to be filled out with code
        /// </summary>
        /// <param name="aTitle"></param>
        /// <param name="aAuthor"></param>
        /// <param name="aPages"></param>
        public Common(double aZ, double aB, double aH, double aN, double aVB, double aCF, double aPages)
        {
            z = aZ;
            b = aB;
            h = aH;
            n = aN;
            vb = aVB;
            cf = aCF;
        }
    }
}
