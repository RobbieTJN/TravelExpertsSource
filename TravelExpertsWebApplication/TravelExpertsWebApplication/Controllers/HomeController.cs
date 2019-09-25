using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelExpertsWebApplication.Models;

namespace TravelExpertsWebApplication.Controllers
{
    public class HomeController : Controller
    {
        // create context object for EF
        private TravelExpertsEntities db = new TravelExpertsEntities();

        [AllowAnonymous]
        public ActionResult Index()
        {
            //ViewBag.CreditMessage = "Credit card information successfully registered!";
            try
            {
                List<Package> packages = db.Packages.ToList();
                return View(packages);
            }
            catch (Exception e)
            {
                Session["Debug"] = e;
                return View();
            }

        }

        [AllowAnonymous]
        public ActionResult About()
        {
            /* Author: Ryan Bonnell */
            //List<Agent> agents = new List<Agent>();
            //string constr = ConfigurationManager.ConnectionStrings["TravelExpertsWebAppContext"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{ 
            //    String sql = "SELECT AgentId, AgtFirstName, AgtMiddleInitial, AgtLastName, AgtBusPhone, AgtEmail, AgtPosition, AgencyId FROM Agents";
            //    using (SqlCommand cmd = new SqlCommand(sql))
            //    {
            //        cmd.Connection = con;
            //        con.Open();
            //        using (SqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {
            //                agents.Add(new Agent
            //                {
            //                    AgentId = (int)sdr["AgentId"],
            //                    AgtFirstName = sdr["AgtFirstName"].ToString(),
            //                    AgtMiddleInitial = sdr["AgtMiddleInitial"].ToString(),
            //                    AgtLastName = sdr["AgtLastName"].ToString(),
            //                    AgtBusPhone = sdr["AgtBusPhone"].ToString(),
            //                    AgtEmail = sdr["AgtEmail"].ToString(),
            //                    AgtPosition = sdr["AgtPosition"].ToString(),
            //                    AgencyId = (int?)sdr["AgencyId"]
            //                });
            //            }
            //        }
            //        con.Close();
            //    }
            //}
            //ViewBag.Agents = agents;

            // Normally, we can only use one model with a View. However, we can use a "holder" class to store the main classes
            //      In this case, that's data from the Agents and Agencies tables
            // Having problems getting that to work, so using the ViewBag for now
            List<Agency> agencies = db.Agencies.ToList();
            List<Agent> agents = db.Agents.ToList();

            ViewBag.Agencies = agencies;

            return View(agents);
        }
    }
}