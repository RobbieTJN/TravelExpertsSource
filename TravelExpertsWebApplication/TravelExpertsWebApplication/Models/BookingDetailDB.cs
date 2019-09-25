using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Author: Robbie Nielsen */

namespace TravelExpertsWebApplication.Models
{
    public static class BookingDetailDB
    {
        // Gets the highest value from the ItineraryNo column
        public static double GetLastItineraryNo()
        {
            // Declare variables
            double itineraryNo = 0; // itinerary number to return
            // Create connection to database
            using (SqlConnection conn = TravelExpertsDB.GetConnection())
            {
                string query =
                    "SELECT TOP 1 ItineraryNo " +
                    "FROM BookingDetails " +
                    "ORDER BY ItineraryNo DESC";
                // Run the SELECT query and recycle object when finished 
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    // Automatically closes database connection when using statement ends
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read()) // if there is data
                    {
                        BookingDetail detail = new BookingDetail();
                        detail.ItineraryNo = Convert.ToDouble(reader["ItineraryNo"]);

                        itineraryNo = (double)detail.ItineraryNo;
                    }
                }
            }
            return itineraryNo;
        }

        // Gets the highest value from the BookingId column
        public static int GetLastBookingId()
        {
            // Declare variables
            int bookingId = 0; // itinerary number to return
            // Create connection to database
            using (SqlConnection conn = TravelExpertsDB.GetConnection())
            {
                string query =
                    "SELECT TOP 1 BookingId " +
                    "FROM BookingDetails " +
                    "ORDER BY BookingId DESC";
                // Run the SELECT query and recycle object when finished
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    // Automatically closes database connection when using statement ends
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read()) // if there is data
                    {
                        BookingDetail detail = new BookingDetail();
                        detail.BookingId = Convert.ToInt32(reader["BookingId"]);

                        bookingId = (int)detail.BookingId;
                    }
                }
            }
            return bookingId;
        }
    }
}
