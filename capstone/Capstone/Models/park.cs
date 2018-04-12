using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        const int rightSpace = 64;
        const int leftSpace = 40;

        public bool selected = false;

        public int parkId { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public DateTime establishDate { get; set; }
        public int area { get; set; }
        public int visitors { get; set; }
        public string description { get; set; }


        public void DrawInformation()
        {

            Queue<string> words = new Queue<string>();
            foreach (string word in description.Split(' '))
            {
                words.Enqueue(word);
            }

            int round = 0;
            while (words.Count > 0 || round < 5)
            {
                string lineDesc = "";
                while (lineDesc.Length < rightSpace && words.Count > 0)
                {
                    lineDesc += words.Dequeue() + " ";
                }
                switch(round)
                {
                    case 0:
                        Console.WriteLine($"{name + ", " + location,-leftSpace}{lineDesc,-rightSpace}");
                        break;
                    case 2:
                        Console.WriteLine($"{"Est. " + establishDate.ToShortDateString(),-leftSpace}{lineDesc,-rightSpace}");
                        break;
                    case 3:
                        Console.WriteLine($"{"Kilometers square: " + area,-leftSpace}{lineDesc,-rightSpace}");
                        break;
                    case 4:
                        Console.WriteLine($"{"Annual visitors: " + visitors,-leftSpace}{lineDesc,-rightSpace}");
                        break;
                    default:
                        Console.WriteLine($"{"",leftSpace}{lineDesc,-rightSpace}");
                        break;
                }
                round++;
            }
            Console.WriteLine();           
        }
        
    }
}