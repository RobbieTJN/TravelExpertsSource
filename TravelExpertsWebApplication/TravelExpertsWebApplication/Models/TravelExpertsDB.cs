using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsWebApplication.Models
{
    public static class TravelExpertsDB
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = "Server=tcp:{DATABASE};Initial Catalog={DATABASE_NAME};Persist Security Info=False;User ID={USERNAME};password={PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            return new SqlConnection(connectionString);
        }
    }
}
