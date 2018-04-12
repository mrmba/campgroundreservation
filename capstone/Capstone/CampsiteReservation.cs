using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Capstone;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    public class CampsiteReservation
    {
        private string dbConnectionString;
        private bool exiting = false;

        const ConsoleKey command_exit = ConsoleKey.Escape;
        const ConsoleKey command_parkDirectory = ConsoleKey.D1;     //  1
        const ConsoleKey command_siteAvailability = ConsoleKey.D2;  //  2
        const ConsoleKey command_bookReservation = ConsoleKey.D3;   //  3

        public CampsiteReservation()
        {
            dbConnectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        }
        public CampsiteReservation(string dbString)    
        {
            dbConnectionString = dbString;
        }

        public void Start()
        {
            
            while(exiting == false)
            {
                Console.Clear();

                DrawHeader();
                DrawMenu();

                ConsoleKey input = Console.ReadKey(true).Key;

                Console.Clear();
                switch (input)
                {
                    case command_exit:
                        exiting = true;
                        break;
                    case command_parkDirectory:
                        DrawParks();
                        break;
                    case command_siteAvailability:
                        SiteSeach();
                        break;
                    //case command_bookReservation:
                        //BookSite();
                        //break;
                    default:
                        break;
                }
            }
        }

        void DrawParks()
        {
            CampSqlDAL sqlParks = new CampSqlDAL(dbConnectionString);
            List<Park> parks = sqlParks.GetParks();
            foreach (Park park in parks)
            {
                park.DrawInformation();
            }

            MenuHelper.EnterToRelease();

        }

        void SiteSeach()
        {
            CampSqlDAL sqlParks = new CampSqlDAL(dbConnectionString);

            ReservationAndSite reservation = new ReservationAndSite();

            Park park = MenuHelper.SelectFrom(sqlParks.GetParks());
            Campground camp = MenuHelper.SelectFrom(sqlParks.GetCampgrounds(park));

            bool correct = false;
            while (!correct)
            {

                Console.Clear();
                Campground.DrawInfoHead();
                camp.DrawInfo();
                Console.WriteLine();
                Console.Write("Arrival date: ");
                reservation.GetDate(true);


                Console.Clear();
                Campground.DrawInfoHead();
                camp.DrawInfo();
                Console.WriteLine();
                Console.Write("Departure date: ");
                reservation.GetDate(false);

                Console.Clear();
                Campground.DrawInfoHead();
                camp.DrawInfo();
                Console.WriteLine();
                Console.WriteLine($"Arrival date: " + reservation.startDate.ToShortDateString() + "\t Departure date: " + reservation.endDate.ToShortDateString());
                correct = MenuHelper.GetConfirmation();
            }

            ReservationAndSite selectedSite = MenuHelper.SelectFrom(sqlParks.GetSites(reservation.startDate,reservation.endDate,camp));
            correct = false;
            while (!correct)
            {
                while (reservation.reservationName == null)
                {
                    Console.Clear();
                    Campground.DrawInfoHead();
                    camp.DrawInfo();
                    Console.WriteLine();
                    Console.WriteLine($"Arrival date: " + reservation.startDate.ToShortDateString() + "\t Departure date: " + reservation.endDate.ToShortDateString());
                    Console.Write("Reservation Name: ");
                    reservation.GetName();
                }
                Console.Clear();
                Campground.DrawInfoHead();
                camp.DrawInfo();
                Console.WriteLine();
                Console.WriteLine($"Arrival date: " + reservation.startDate.ToShortDateString() + "\t Departure date: " + reservation.endDate.ToShortDateString());
                Console.Write("Reservation Name: " + reservation.reservationName);
                Console.WriteLine();
                correct = MenuHelper.GetConfirmation();
            }
            reservation.siteId = selectedSite.siteId;

            int resID = sqlParks.MakeReservation(reservation);
            Console.WriteLine("Your confirmation number is: "+ resID);
            MenuHelper.EnterToRelease();
        }

        void DrawHeader()
        {
            Console.WriteLine("Campground Reservation System");
            Console.WriteLine();
        }

        void DrawMenu()
        {
            Console.WriteLine($"[{command_parkDirectory.ToString().Substring(1)}]\tPark Directory");
            Console.WriteLine($"[{command_siteAvailability.ToString().Substring(1)}]\tCreate Reservation");
            //Console.WriteLine($"[{command_bookReservation.ToString().Substring(1)}]\tBrowse Available Sites");

            Console.WriteLine($"[{command_exit.ToString().Substring(0,3)}]\tExit");

        }

       
       
    }
}
