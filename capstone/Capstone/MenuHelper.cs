using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone
{
    public class MenuHelper
    {
        const ConsoleKey command_Yes = ConsoleKey.Y;
        const ConsoleKey comman_No = ConsoleKey.N;

        public static ReservationAndSite SelectFrom(List<ReservationAndSite> someSites)
        {
            ReservationAndSite output = new ReservationAndSite();
            Console.Clear();
            Console.Write("\t");
            ReservationAndSite.DrawInfoHead();

            foreach (ReservationAndSite item in someSites)
            {
                int position = someSites.IndexOf(item) + 1;
                Console.Write($"[{position}]\t");
                item.DrawInfo();
            }
            while (output.selected == false)
            {
                string input = Console.ReadKey(true).KeyChar.ToString();
                int.TryParse(input, out int x);
                if (0 < x && x <= someSites.Count)
                {
                    output = someSites[x - 1];
                    output.selected = true;
                }
            }
            return output;
        }

        public static Campground SelectFrom(List<Campground> someCamps)
        {
            Campground output = new Campground();
            Console.Clear();
            Campground.DrawInfoHead();

            foreach (Campground item in someCamps)
            {
                int position = someCamps.IndexOf(item) + 1;
                Console.Write($"[{position}]");
                item.DrawInfo();
            }
            while (output.selected == false)
            {
                string input = Console.ReadKey(true).KeyChar.ToString();
                int.TryParse(input, out int x);
                if (0 < x && x <= someCamps.Count)
                {
                    output = someCamps[x-1];
                    output.selected = true;
                }
            }
            return output;
        }

        public static Park SelectFrom(List<Park> someParks)
        {
            Park output = new Park();
            Console.Clear();
            Console.WriteLine("\tPark Name");
            foreach (Park item in someParks)
            {
                int position = someParks.IndexOf(item) + 1;
                Console.Write($"[{position}]\t");
                Console.WriteLine(item.name);
            }
            while (output.selected == false)
            {
                string input = Console.ReadKey(true).KeyChar.ToString();
                int.TryParse(input, out int x);
                if (0 < x && x <= someParks.Count)
                {
                    output = someParks[x-1];
                    output.selected = true;
                }
            }
                return output;
        }

        public static bool GetConfirmation()
        {
            bool output = false;
            Console.WriteLine("Is this correct?");
            Console.WriteLine("[Y]es");
            Console.WriteLine("[N]o");
            bool done = false;
            while (!done)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case command_Yes:
                        output = true;
                        done = true;
                        break;
                    case comman_No:
                        output = false;
                        done = true;
                        break;
                    default:
                        break;
                }
            }
            return output;
        }

        public static void EnterToRelease()
        {
            Console.Write("\n[Enter>]\tReturn to menu");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }
    }
}
