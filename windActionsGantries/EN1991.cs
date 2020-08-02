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
        public double ro { get; set; }
        public int c0 { get; set; }
        public double kr { get; set; }
        public double cr { get; set; }
        public double vm { get; set; }
        public Common g { get; set; }

        /// <summary>
        /// Constructor to initialise EN1991 calculations with common property entries
        /// </summary>
        /// <param name="z0zmin">Dictionary containing z0 and zmin values, as defined in Lookups.inputTerrain() function</param>
        /// <param name="aDelta_s">Structural damping factor, as defined in Lookups.inputConnecType() function</param>
        /// <param name="aMass">Mass of structure per unit metre</param>
        public EN1991(Dictionary<string, double> z0zmin, double aDelta_s, double aMass, Common aG)
        {
            z0 = z0zmin["z0"];
            zmin = z0zmin["zmin"];
            delta_s = aDelta_s;
            mass = aMass;
            ro = 1.25; //kg/m3
            double z0ii = 0.05; //Sec 4.3.2
            c0 = 1; //Sec 4.3.3 assumed
            kr = 0.19 * Math.Pow(z0 / z0ii, 0.07);
            g = aG;
            cr = kr * Math.Log(Math.Max(zmin, g.z) / z0);
            vm = cr * c0 * g.vb; //Eq 4.3
        }
        /// <summary>
        /// Get mean speed, this function may be combined into the full cd_cs calculation later.
        /// </summary>
        /// <param name="G">Geometric and other parameters defined in a 'Common' class</param>
        /// <returns>Wind Mean Speed</returns>
        public void cs_cd()
        {
            double kl = 1.0;
            double Iv = kl / (c0 * Math.Log(g.z / z0));

            // Sec B.1 (1) Wind Turbulence
            double zt = 200.0; //(m) Reference Height
            double Lt = 300.0; //(m) Reference Length
            double alpha = 0.67 + 0.05 * Math.Log(z0);
            double L = Lt * Math.Pow(Math.Max(zmin, g.z) / zt,alpha);

            // Sec B.1 (2) Wind Distribution over frequencies - Power spectral function
            double fL = g.n * L / vm;
            double SL = 6.8 * fL / Math.Pow((1.0 + 10.2 * fL),5.0/3.0);

            // F.5 Logarithmic decrement of damping
            double delta_d = 0.0; //Assumed no special damping devices
            double dens_air = 1.25; //(kg/m3)
            double delta_a = g.cf * dens_air * vm / (2 * g.n * mass / g.h);
            double delta = delta_s + delta_a + delta_d;

            // B.2 Structural Factors
            double B2 = 1.0 / (1.0 + 0.9 * Math.Pow((g.b + g.h) / L,0.63)); //Eq B.3 Background Factor allow lack full pressure correlation
            double nh = 4.6 * g.h * fL / L;
            double nb = 4.6 * g.b * fL / L;
            double Rh = 1.0 / nh - 1 / (2.0 * nh*nh) * (1 - Math.Exp(-2.0 * nh)); //Eq B.7 Aerodynamic admittance function (h)
            double Rb = 1.0 / nb - 1 / (2.0 * nb*nb) * (1 - Math.Exp(-2.0 * nb)); //Eq B.8 Aerodynamic admittance function (b)
            double R2 = Math.PI*Math.PI * SL * Rh * Rb / (2.0 * delta); //Eq B.6 Resonance response Factor
            double v = g.n * Math.Sqrt(R2 / (B2 + R2)); //(Hz) Eq B.5 Up-crossing Frequency
            double T = 600.0; //(s) Eq B.4 Averaging time for mean wind velocity
            double kp = Math.Max(Math.Sqrt(2 * Math.Log(v * T)) + 0.6 / Math.Sqrt(2 * Math.Log(v * T)), 3);
            double cs = (1 + 7 * Iv * Math.Sqrt(B2)) / (1 + 7 * Iv); //size factor
            double cd = (1 + 2 * kp * Iv * Math.Sqrt(B2 + R2)) / (1 + 7 * Iv * Math.Sqrt(B2)); //dynamic factor
            double cs_cd = (1 + 2 * kp * Iv * Math.Sqrt(B2 + R2)) / (1 + 7 * Iv); //combined size and dynamic factor
            Console.WriteLine("cs_cd = " + cs_cd);
        }
    }
}
