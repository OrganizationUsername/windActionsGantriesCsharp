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
            Console.WriteLine(Validation.inputPrintYesNo("Do you want to see the intermediate values ? y = [YES] n = [NO]: ",
                            @$"TURBULENCE, SPECTRAL FUNC & DAMPING
                            kr={kr,10:F4}
                            cr={cr,10:F2}
                            vm={vm,10:F2}
                            Iv={Iv,10:F2}
                            alpha={alpha,7:F2}
                            L={L,11:F2}
                            fL={fL,10:F2}
                            SL={SL,10:F2}
                            delta_s={delta_s,5:F2}
                            delta_a={delta_a,5:F2}
                            STRUCTURAL FACTORS INPUTS:
                            B2={B2,10:F2}
                            nh={nh,10:F2}
                            nb={nb,10:F2}
                            Rh={Rh,10:F2}
                            Rb={Rb,10:F2}
                            R2={R2,10:F2}
                            v={v,11:F2}
                            T={T,11:F2}
                            kp={kp,10:F2}"));
        }

        public void VortexShedding(double d)
        {
            double b = g.h; //height of beam variable definition
            double l = g.b; //Length of beam variable redefinition
            //Read Graph of Strouhal Number Table E.1 EN1991.1.4
            double St = Lookups.inputStrouhal(d / b);

            //Critical Wind Velocity vcrit,i
            double vcrit = b * g.n / St;

            //Scruton Number [Ratio structural mass to fluid mass]
            //The ability of the structure to absorb and dissipate the energy
            //from vortex shedding depends on the structural damping
            double Sc = 2 * delta_s * mass / (ro * b*b);

            //Reynolds Number
            double v = 15 * Math.Pow(10,-6); //m2/s kinematic velocity of air
            double Re = b * vcrit / v;

            //Vortex Shedding Action
            //APPROACH 1
            double K = 0.1; //Table E.5 for simply supported structure
            //Correlation Length E 1.5.2.3
            //clat Calculations Table E.2 & E.3
            double clat0 = 1.1;
            double clat = 0;
            if (vcrit / vm <= 0.83)
            {
                clat = clat0;
            }
            else if (vcrit / vm <= 1.25)
            {
                clat = (3 - 2.4 * vcrit / vm) * clat0;
            }
            else
            {
                clat0 = 0;
            }

            //TODO - Check whether manually collating mode shape factors is needed in certain structures
            //Refer Table E.5 for n and m mode shape factors
            //Refer Table F.1 for phi.iy.s

            //Calculate correlation lenght factor on assumption of Lj length.
            double Lj_div_b = 6.0; //TODO CHECK assumption. Based on Sigmund spreadsheets example 30-G
            double lamda = l / b;
            double Kw = Math.Min(Math.Cos(Math.PI / 2 * (1 - (Lj_div_b) / lamda)), 0.6);
            //Max displacement over time of the point with phi_iy = 1
            double Yfmax = b * (1 / (St*St)) * (1 / Sc) * K * Kw * clat; //E.7

            //Inertia force per unit length
            double phi_iys = 1; //at midspan normalised
            double Fw = mass * Math.Pow(2 * Math.PI * g.n, 2) * phi_iys * Yfmax;
            Console.WriteLine(Fw);

            Console.WriteLine(@$"The maximum displacement over time of a point with phi_iy = 1 is:
            Yfmax={Yfmax,7:F3} m
            The inertia force per unit length at distance s along beam [taken as midspan] is:
            Fw={Fw,10:F0} N/m");

            //Ask user whether intermediate results are required:
            Console.WriteLine(Validation.inputPrintYesNo("Do you want to see the intermediate values? y = [YES] n = [NO]: ",
                            @$"delta_s={delta_s:5:F2}
                            kr={kr,10:F2}
                            cr={cr,10:F2}
                            vm={vm,10:F2}
                            St={St,10:F2}
                            vcrit={vcrit,7:F2}
                            Sc={Sc,10:F2}
                            Re={Re,10:N0}
                            K={K,11:F2}
                            clat0={clat0,7:F2}
                            clat={clat,8:F2}
                            lamda={lamda,7:F2}
                            Kw={Kw,10:F2}"));
        }

    }
}
