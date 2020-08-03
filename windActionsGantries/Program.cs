using System;
using System.Collections.Generic;
using System.Linq;

namespace windActionsGantries
{
    class Program
    {
        /// <summary>
        /// Class <c>Main</c>
        /// launches the program </summary>
        /// <param name="args">string array that contains the command line arguments used to invoke the program</param>
        static void Main(string[] args)
        {
            Common structure = new Common(Validation.inputNumber("Enter the height above ground 'z' in metres : "),
                Validation.inputNumber("Length of Beam perpendicular to the wind 'b' in metres : "),
                Validation.inputNumber("Height of beam 'h' in metres : "),
                Validation.inputNumber("Natural Frequency 'n' in Hz : "),
                Validation.inputNumber("Mean Wind speed 10 min ave [refer Durst Curve for conversion from 3s] 'vb' in m/s: "),
                Validation.inputNumber("Aerodynamic shape factor 'cf' : "));
            if (Validation.askBool("Conduct EN1991.1.4 calculations y = [YES] n = [NO] : "))
            {
                EN1991 EN1991_calcs = new EN1991(
            Lookups.InputTerrain(),
            Lookups.InputConnecType(),
            Validation.inputNumber("Enter the mass per unit metre of beam at the mid - span 'mass' in kg / m : "),
                structure);

                if (Validation.askBool(@"Conduct cs_cd calculation AnnB EN1991.1.4 [applies to cantilevers / beams with constant sign
y = [YES] n = [NO] : "))
                {
                    EN1991_calcs.cs_cd();
                }

                if (Validation.askBool(@"Conduct Vortex Shedding Calculation as per EN1991.1.4 [simply supported beams only]
y = [YES] n = [NO] : "))
                {
                    EN1991_calcs.VortexShedding(Validation.inputNumber("Horizontal width of section 'd' in metres : "));
                }
            }
        }
    }
}
