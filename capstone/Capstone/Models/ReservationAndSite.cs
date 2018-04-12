using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class ReservationAndSite
    {
        const int space01 = 10;
        const int space02 = 14;
        const int space03 = 16;

        private static string infoHeader = $"{"Site No.",-space01}{"Max Occup.",-space02}{"Accessible?",-space02}{"Max RV Length",-space03}{"Utility",-space01}{"Cost",-space02}";
        public bool selected = false;

        public int reservationId { get; set; }
        public DateTime createDate { get; set; }
        public string campgroundName { get; set; }
        public int siteId { get; set; }
        public int siteNum { get; set; }
        public string reservationName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal totalPrice { get; set; }  
        public int maxOccupancy { get; set; }
        public bool accessible { get; set; }
        public decimal maxLenRV { get; set; }
        public bool utilities { get; set; }

        public void DrawInfo()
        {
            Console.WriteLine($"{siteNum,-space01}{GetMaxOcc(),-space02}{GetAccess(),-space02}{GetMaxRVL(),-space03}{GetHasUti(),-space01}{totalPrice,-space02:c}");
        }

        public static void DrawInfoHead()
        {
            Console.WriteLine(infoHeader);
        }

        public bool GetDate(bool isStartDate)
        {
            bool success = false;

            try
            {
                if (isStartDate)
                {
                    startDate = Convert.ToDateTime(Console.ReadLine());
                }
                else
                {
                    endDate = Convert.ToDateTime(Console.ReadLine());
                }
                success = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input, try MM-DD-YYYY or MM/DD/YYYY");
            }
            
            return success;
        }

        public bool GetName()
        {
            bool output = false;
            try
            {
                reservationName = Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input");
            }
            return output;
        }
        public string GetMaxOcc()
        {
            string max = maxOccupancy.ToString();
            if (maxOccupancy < 1)
            {
                max = "N/A";
            }
            return max;
        }
        public string GetMaxRVL()
        {
            string max = maxLenRV.ToString();
            if (maxLenRV < 1)
            {
                max = "N/A";
            }
            return max;
        }
        public string GetHasUti()
        {
            string output = "No";
            if (utilities)
            {
                output = "Yes";
            }
            return output;
        }
        public string GetAccess()
        {
            string output = "No";
            if (accessible)
            {
                output = "Yes";
            }
            return output;
        }


    }
}
