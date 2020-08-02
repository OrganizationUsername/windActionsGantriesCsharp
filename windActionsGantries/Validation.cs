using System;
using System.Collections.Generic;
using System.Text;

namespace windActionsGantries
{
    class Validation
    {
        public static double inputNumber(string question)
        {
            double number = 0;
            Console.WriteLine(question);
            string input = Console.ReadLine();
            while (!double.TryParse(input, out number) || number <= 0)
            {
                Console.WriteLine("This is not a positive non-zero number! Try Again");
                input = Console.ReadLine();
            }
            return number;
        }

        /// <summary>
        /// Enter Message and ask for yes or no input, validate so no other option
        /// </summary>
        /// <param name="question">Message to ask for</param>
        /// <returns>True or False</returns>
        public static bool askBool(string question)
        {
            while (true)
            {
                Console.Write(question);
                var input = Console.ReadLine().Trim().ToLowerInvariant();
                //Check for input value and return delta_s - damping factor
                switch (input)
                {
                    case "y":
                    case "yes": return true;
                    case "n":
                    case "no": return false;
                }
                Console.WriteLine("You did not enter y = [YES] or n = [NO] : ");
            }
        }

        /// <summary>
        /// Input y [yes] or n [no] and if yes, print out inputted string
        /// </summary>
        /// <param name="message">Input message provided in console</param>
        /// <param name="output">Ask for what WriteLine string for printing results should be</param>
        /// <returns>Output WriteLine string for printing results</returns>
        public static string inputPrintYesNo(string question, string output)
        {
            bool truefalse = askBool(question);
            if (truefalse)
            {
                return output;
            }
            else
            {
                return "No output returned.";
            }
        }

    }
}
