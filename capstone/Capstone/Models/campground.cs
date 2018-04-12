using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        const int space00 = 32;
        const int space01 = 20;
        const int space02 = 18;

        private static string infoHeader = $"{"Name",-space00}{"Open",-space01}{"Close",-space01}{"Daily Fee",-space02}";
        public bool selected = false;

        public int campgroundId { get; set; }
        public int parkId { get; set; }
        public string name { get; set; }
        public int openMonth { get; set; }
        public int closeMonth { get; set; }
        public decimal  dailyFee { get; set; }

        public void DrawInfo()
        {
            Console.WriteLine($"\t{name,-space00}{GetMonth(openMonth),-space01}{GetMonth(closeMonth),-space01}{dailyFee,-space02:c}"); 
        }

        public static void DrawInfoHead()
        {
            Console.WriteLine("\t"+infoHeader);
        }

        private string GetMonth(int monthNum)
        {
            string output = "";
            switch (monthNum)
            {
                case 1:
                    output = "January";
                    break;
                case 2:
                    output = "February";
                    break;
                case 3:
                    output = "March";
                    break;
                case 4:
                    output = "April";
                    break;
                case 5:
                    output = "May";
                    break;
                case 6:
                    output = "June";
                    break;
                case 7:
                    output = "July";
                    break;
                case 8:
                    output = "August";
                    break;
                case 9:
                    output = "September";
                    break;
                case 10:
                    output = "October";
                    break;
                case 11:
                    output = "November";
                    break;
                case 12:
                    output = "December";
                    break;
                default:
                    output = "N/A";
                    break;
            }
            return output;
        }
    }
}
