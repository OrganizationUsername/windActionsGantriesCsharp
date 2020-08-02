using System;
using System.Collections.Generic;
using System.Text;

namespace windActionsGantries
{
    /// <summary>
    /// Perform EN1991 calculations using geometric and other properties from Common class
    /// </summary>
    class EN1991
    {
        public double z0;
        public double zmin;
        public double delta_s { get; set; }
        public double mass { get; set; }
        /// <summary>
        /// Constructor to initialise EN1991 calculations with common property entries
        /// </summary>
        /// <param name="z0zmin">Dictionary containing z0 and zmin values, as defined in Lookups.inputTerrain() function</param>
        /// <param name="aDelta_s">Structural damping factor, as defined in Lookups.inputConnecType() function</param>
        /// <param name="aMass">Mass of structure per unit metre</param>
        public EN1991(Dictionary<string, double> z0zmin, double aDelta_s, double aMass)
        {
            z0 = z0zmin["z0"];
            zmin = z0zmin["zmin"];
            delta_s = aDelta_s;
            mass = aMass;
        }
        /// <summary>
        /// Get mean speed, this function may be combined into the full cd_cs calculation later.
        /// </summary>
        /// <param name="G">Geometric and other parameters defined in a 'Common' class</param>
        /// <returns>Wind Mean Speed</returns>
        public double printMeanSpeed(Common G)
        {
            double z0ii = 0.05; //Sec 4.3.2
            int c0 = 1; //Sec 4.3.3 assumed
            double kr = 0.19 * Math.Pow(z0 / z0ii,0.07);
            double cr = kr * Math.Log(Math.Max(zmin, G.z) / z0);
            double vm = cr * c0 * G.vb; //Eq 4.3
            return vm;
        }
    }
}
