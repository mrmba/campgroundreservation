using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampSqlDAL
    {
        //private string SQL_GetParkId = "select park_id from park where name = @name;";
        private string SQL_GetCampgrounds = "select * from campground where park_id = @park;";
        private string SQL_SelectParks = "Select park_id, name, location, establish_date, area, visitors, description From Park Order by name asc;";
        private string SQL_AvailableSites = "Select Top 5 s.[site_id], c.[name] 'Campground', s.[site_number] 'Site_Number', (DateDiff(day, @startdate, @enddate) * c.[daily_fee]) 'Total_Cost' From [site] as s inner join [campground] as c on (s.campground_id = c.campground_id) Where site_id not in (select site_id from reservation as res where res.from_date > @startdate and res.to_date < @enddate) AND c.campground_id = @camp Order By (DateDiff(day, @startdate, @enddate) * c.[daily_fee]);";
        private string SQL_CreateReservation = "Insert into Reservation (site_id, name, from_date, to_date) Values (@siteid, @name, @startdate, @enddate);";
        private string SQL_GetResId = "select reservation_id from reservation where create_date in (select max(create_date) from reservation);";
        private string connectionString;

        // Single Parameter Constructor
        public CampSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //method parks
        public List<Park> GetParks()
        {
            List<Park> output = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Park thisPark = GetParkFromReader(reader);
                        output.Add(thisPark);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }

        //covert park from database
        private Park GetParkFromReader(SqlDataReader reader)
        {
            Park convertPark = new Park();
            convertPark.parkId = Convert.ToInt32(reader["park_id"]);
            convertPark.name = Convert.ToString(reader["name"]);
            convertPark.location = Convert.ToString(reader["location"]);
            convertPark.establishDate = Convert.ToDateTime(reader["establish_date"]);
            convertPark.area = Convert.ToInt32(reader["area"]);
            convertPark.visitors = Convert.ToInt32(reader["visitors"]);
            convertPark.description = Convert.ToString(reader["description"]);

            return convertPark;
        }

        //method sites
        public List<ReservationAndSite> GetSites(DateTime startDate, DateTime endDate, Campground campground)
        {
            List<ReservationAndSite> output = new List<ReservationAndSite>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AvailableSites, conn);
                    cmd.Parameters.AddWithValue("@startdate", startDate);
                    cmd.Parameters.AddWithValue("@enddate", endDate);
                    cmd.Parameters.AddWithValue("@camp", campground.campgroundId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ReservationAndSite thisSite = GetSiteFromReader(reader);
                        output.Add(thisSite);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }

        //covert site from database
        private ReservationAndSite GetSiteFromReader(SqlDataReader reader)
        {
            ReservationAndSite convertSite = new ReservationAndSite();
            convertSite.siteId = Convert.ToInt32(reader["site_id"]);
            convertSite.campgroundName = Convert.ToString(reader["Campground"]);
            convertSite.siteNum = Convert.ToInt32(reader["Site_Number"]);
            convertSite.totalPrice = Convert.ToDecimal(reader["Total_Cost"]);

            return convertSite;
        }

        //method create reservation
        public int MakeReservation(ReservationAndSite newReservation)
        {
            int reservationConfirmation = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateReservation, conn);
                    cmd.Parameters.AddWithValue("@siteid", newReservation.siteId);
                    cmd.Parameters.AddWithValue("@name", newReservation.reservationName);
                    cmd.Parameters.AddWithValue("@startdate", newReservation.startDate);
                    cmd.Parameters.AddWithValue("@enddate", newReservation.endDate);
                    //cmd.Parameters.AddWithValue("@getdate", newReservation.createDate);


                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.RecordsAffected > 0)
                    {
                        conn.Close();
                        conn.Open();
                        //Select reservation_id from reservation where name = @name and creationdate =  
                        SqlCommand cmd2 = new SqlCommand(SQL_GetResId, conn);
                        reservationConfirmation = (int)cmd2.ExecuteScalar();
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return reservationConfirmation;
        }

        //method campgrounds
        public List<Campground> GetCampgrounds(Park park)
        {

            List<Campground> output = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    SqlCommand cmd = new SqlCommand(SQL_GetCampgrounds, conn);
                    cmd.Parameters.AddWithValue("@park", park.parkId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground camp = CampgroundFromSql(reader);
                        output.Add(camp);
                    }
                }
            }

            catch (SqlException)
            {
                throw;
            }
            return output;

        }

        //convert campground from database
        public Campground CampgroundFromSql(SqlDataReader reader)
        {
            Campground cg = new Campground();

            cg.campgroundId = Convert.ToInt32(reader["campground_id"]);
            cg.parkId = Convert.ToInt32(reader["park_id"]);
            cg.name = Convert.ToString(reader["name"]);
            cg.openMonth = Convert.ToInt32(reader["open_from_mm"]);
            cg.closeMonth = Convert.ToInt32(reader["open_to_mm"]);
            cg.dailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return cg;
        }
    }
}