using System;
using System.Collections.Generic;
using System.Text;

namespace windActionsGantries
{
    /// <summary>
    /// THIS IS UNRELATED CODE TO BE REMOVED SHORTLY
    /// </summary>
    class EN1991
    {
        /// <summary>
        /// Test input values
        /// </summary>
        public string title;
        public string author;
        private double pages;

        /// <summary>
        /// Test constructor method, to be filled out with code
        /// </summary>
        /// <param name="aTitle"></param>
        /// <param name="aAuthor"></param>
        /// <param name="aPages"></param>
        public EN1991(string aTitle, string aAuthor, double aPages)
        {
            title = aTitle;
            author = aAuthor;
            Pages = aPages;
        }

        /// <summary>
        /// Test Getter and Setter method with some validation. Potentially some validation and lookups can go in here
        /// </summary>
        public double Pages
        {
            get { return pages; }
            set {
                if (value <= 100)
                { 
                    pages = value;
                }
                else
                {
                    pages = 0;
                }
            }
        }
    }
}
