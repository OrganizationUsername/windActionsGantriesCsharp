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
            Common a = new Common(10, 63, 1.3, 0.5, 30, 2.1, 3);
            Console.WriteLine(a.b);
            EN1991 b = new EN1991(Lookups.InputTerrain(), Lookups.InputConnecType(), 1000);
            Console.WriteLine(b.printMeanSpeed(a));
            //Console.WriteLine(Lookups.inputDampingAS());
            //Console.WriteLine(Validation.inputPrintYesNo("Input Print Yes NO : ","This is a string"));
            //Console.WriteLine(Lookups.InputConnecType());
            /////Some instance creation of the validation and lookups for testing purposes. To be removed.
            //Console.WriteLine(Lookups.InputTerrainIh(15));
            //Dictionary<string, double> z0zmin = Lookups.InputTerrain();
            //Console.WriteLine(z0zmin["zmin"]);
            //double number = Validation.inputNumber();
        }

    }
}
