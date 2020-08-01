using System;
using System.Collections.Generic;
using System.Text;

namespace windActionsGantries
{
    class Validation
    {
        public static double inputNumber()
        {
            double number = 0;
            Console.WriteLine("Enter a positive non-zero number ");
            string input = Console.ReadLine();
            while (!double.TryParse(input, out number) || number <= 0)
            {
                Console.WriteLine("This is not a positive non-zero number! Try Again");
                Console.WriteLine("Enter the page number : ");
                input = Console.ReadLine();
            }
            return number;
        }



    }
}
