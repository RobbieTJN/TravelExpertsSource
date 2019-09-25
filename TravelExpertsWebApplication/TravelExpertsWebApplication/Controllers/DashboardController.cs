using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelExpertsWebApplication.Models;

/* Author: Robbie Nielsen */

namespace TravelExpertsWebApplication.Controllers
{
    public class DashboardController : Controller
    {
        // create context object for EF
        private TravelExpertsEntities db = new TravelExpertsEntities();

        // Dashboard index
        public ActionResult Index()
        {
            // If a user is logged in, display dasboard. Otherwise, redirect to login
            if (Session["CustomerId"] != null)
            {
                // Get the CustomerId
                int custID = Convert.ToInt32(Session["CustomerId"]);

                // Dashboard - view personal info, a button allows to edit it
                // Construct the viewmodel
                Customer cust = db.Customers.FirstOrDefault(u => u.CustomerId.Equals(custID));
                Customer model = new Customer();
                model.CustFirstName = cust.CustFirstName;
                model.CustLastName = cust.CustLastName;
                model.CustAddress = cust.CustAddress;
                model.CustCity = cust.CustCity;
                model.CustProv = cust.CustProv;
                model.CustPostal = cust.CustPostal;
                model.CustCountry = cust.CustCountry;
                model.CustHomePhone = cust.CustHomePhone;
                model.CustBusPhone = cust.CustBusPhone;
                model.CustEmail = cust.CustEmail;
                model.AgentId = cust.AgentId;

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }
        //--------------------------------------------------Edit Personal Info
        #region Edit_Personal_Info
        // Some code from https://stackoverflow.com/questions/22955872/editing-user-profile-details
        public ActionResult Edit()
        {
            if (Session["CustomerId"] != null)
            {
                // Get the CustomerId
                int custID = Convert.ToInt32(Session["CustomerId"]);
                Customer cust = db.Customers.FirstOrDefault(u => u.CustomerId.Equals(custID));

                // Construct the viewmodel
                Customer model = new Customer();
                model.CustFirstName = cust.CustFirstName;
                model.CustLastName = cust.CustLastName;
                model.CustAddress = cust.CustAddress;
                model.CustCity = cust.CustCity;
                model.CustProv = cust.CustProv;
                model.CustPostal = cust.CustPostal;
                model.CustCountry = cust.CustCountry;
                model.CustHomePhone = cust.CustHomePhone;
                model.CustBusPhone = cust.CustBusPhone;
                model.CustEmail = cust.CustEmail;
                model.AgentId = cust.AgentId;
                // Create lists that can be rendered in the drop down list
                model.Provinces = Lists.getAllProvinces();
                model.Countries = Lists.getAllCountries();

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        [HttpPost]
        public ActionResult Edit(Customer cust)
        {
            // Need to recreate the list for the drop down list again, in case the page is reloaded
            cust.Provinces = Lists.getAllProvinces();
            cust.Countries = Lists.getAllCountries();
            if (ModelState.IsValid)
            {
                int custID = Convert.ToInt32(Session["CustomerId"]);
                // Fetch the userprofile
                Customer model = db.Customers.FirstOrDefault(u => u.CustomerId.Equals(custID));
                // Update columns
                model.CustFirstName = cust.CustFirstName;
                model.CustLastName = cust.CustLastName;
                model.CustAddress = cust.CustAddress;
                model.CustCity = cust.CustCity;
                model.CustProv = cust.CustProv;
                model.CustPostal = cust.CustPostal;
                model.CustCountry = cust.CustCountry;
                model.CustHomePhone = cust.CustHomePhone;
                model.CustBusPhone = cust.CustBusPhone;
                model.CustEmail = cust.CustEmail;
                model.AgentId = cust.AgentId;

                db.Entry(model).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // for debugging
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();

                return View(cust);
            }
        }
        #endregion
        //--------------------------------------------------End Edit Personal Info
        //--------------------------------------------------Package Information
        #region Package_Information

        public ActionResult ViewPackages()
        {
            // If a user is logged in, display dasboard. Otherwise, redirect to login
            if (Session["CustomerId"] != null)
            {
                // Get the CustomerId
                int custID = Convert.ToInt32(Session["CustomerId"]);

                // Dashboard - View purchased travel packages
                string query = "SELECT * FROM Bookings WHERE CustomerId = @p0";
                List<Booking> bookings = db.Bookings.SqlQuery(query, custID).ToList();

                // For displaying total value of every package purchased by this customer
                decimal total = 0;
                List<Booking> prices = db.Bookings.SqlQuery(query, custID).ToList();
                foreach (Booking p in prices)
                {
                    // Database allows nulls here, though it really shouldn't...
                    if (p.BasePrice == null)
                    {
                        total += 0;
                    }
                    else
                    {
                        total += (decimal)p.BasePrice;
                    }
                }
                // Total value of every package purchased by this customer
                ViewBag.TotalPrice = total.ToString("c");
                return View(bookings);
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        public ActionResult Details(int id)
        {
            // If a user is logged in, display details. Otherwise, redirect to login
            if (Session["CustomerId"] != null)
            {
                // SQL query
                string query = "SELECT * FROM BookingDetails WHERE BookingId = @p0";
                Models.BookingDetail detail = db.BookingDetails.SqlQuery(query, id).FirstOrDefault();

                return View(detail);
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        #endregion
        //--------------------------------------------------End Package Information
        //--------------------------------------------------Purchase Package
        #region Purchase

        public ActionResult Purchase(int id)
        {
            /* Get the PackageId. This is outside the if statement, so if the customer needs to enter their credit
             * card info, they can be taken to the confirmation screen after doing so, rather than have to select
             * the package again */
            Session["PkgID"] = id;

            // If a user is logged in, proceed with purchase. Otherwise, prompt for login
            if (Session["CustomerId"] != null)
            {
                // If user has previously entered credit card info, proceed with purchase.
                // Otherwise, take user to another page to enter it
                if (Session["CreditCardID"].ToString() != "-1")
                {

                    // Get the CustomerId
                    int custID = Convert.ToInt32(Session["CustomerId"]);
                    // Check for credit card info
                    CreditCard credit = db.CreditCards.FirstOrDefault(u => u.CustomerId.Equals(custID));
                    CreditCard cred = new CreditCard();

                    cred.CCName = credit.CCName;
                    cred.CCNumber = credit.CCNumber;
                    cred.CCExpiry = credit.CCExpiry;
                    cred.CustomerId = custID;

                    return View(cred);
                }
                else
                {
                    return RedirectToAction("EnterCreditCard", "Dashboard");
                }
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        public ActionResult Confirm()
        {
            // If a user is logged in, confirm purchase. Otherwise, redirect to login
            if (Session["CustomerId"] != null)
            {
                // Get PackageId from TempData
                int pkgID = (int)Session["PkgID"];
                // Select package with given ID
                Models.Package pkg = db.Packages.FirstOrDefault(u => u.PackageId.Equals(pkgID));

                /* Put something in TempData, which is cleared after 1 request, so
                 * success message will only display the first time each package is booked,
                 * rather than every time Dashboard/Index is loaded */
                TempData["PkgMessage"] = "Package successfully booked!";
                // Clear Session variable for later use
                Session["PkgID"] = null;
                return View(pkg);
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        public ActionResult ConfirmPurchase(int id)
        {
            // If a user is logged in, confirm purchase. Otherwise, redirect to login
            if (Session["CustomerId"] != null)
            {
                // Get CustomerId
                int custID = Convert.ToInt32(Session["CustomerId"]);
                // Get PackageId
                int pkgID = id;
                // Select package with given ID to create booking
                Models.Package pkg = db.Packages.FirstOrDefault(u => u.PackageId.Equals(pkgID));
                // Insert selected package into Bookings table
                Booking book = new Booking();
                book.BookingDate = DateTime.Now; // BookingDate is the time the booking is made
                book.BookingNo = RandomStringGenerator.RandomString(6); // creates random string of 6 characters
                book.TravelerCount = 1; // project document specified always 1 traveler, for now
                book.CustomerId = custID;
                book.TripTypeId = "B";
                book.PackageId = pkg.PackageId;
                book.BasePrice = pkg.PkgBasePrice;

                /* "ItineraryNo" from BookingDetails does not auto-increment, AND allows
                 * duplicate values, so this SQL query will grab the highest ItineraryNo
                 * value, which we can add 1 to to make a new ItineraryNo */
                //string query =
                //    "SELECT TOP 1 ItineraryNo " +
                //    "FROM BookingDetails " +
                //    "ORDER BY ItineraryNo DESC";
                double itinerary = BookingDetailDB.GetLastItineraryNo();
                int bookingId = BookingDetailDB.GetLastBookingId();

                /* Switch statement that determines the destination of a package, class, and
                 * RegionId of a package. There's nothing in the Packages table showing this,
                 * so the destinations are hardcoded, as well as the potential package IDs.
                 * Ideally, a destination column would be added to the Packages table */
                string destination;
                string region;
                string classID; // first class, business class, etc...

                switch (pkgID)
                {
                    case 1: // Caribbean
                        destination = "Santo Domingo, Dominican Republic";
                        region = "OTHR"; // Other region
                        classID = "ECN"; // Economy Class
                        break;
                    case 2: // Polynesia
                        destination = "Honolulu, United States";
                        region = "NA"; // North America
                        classID = "ECN"; // Economy Class
                        break;
                    case 3: // Asia
                        destination = "Bangkok, Thailand";
                        region = "ASIA";
                        classID = "DLX"; // Deluxe Class
                        break;
                    case 4: // Europe
                        destination = "Rome, Italy";
                        region = "EU"; // Europe and UK
                        classID = "FST"; // First Class
                        break;
                    default: // New packages without any assignment here, or errors
                        destination = "Atlantis";
                        region = "OTHR"; // Other region
                        classID = "BSN"; // Business Class
                        break;
                }

                // Create new BookingDetail for this booking
                Models.BookingDetail detail = new Models.BookingDetail();
                detail.ItineraryNo = itinerary + 1;
                detail.TripStart = pkg.PkgStartDate;
                detail.TripEnd = pkg.PkgEndDate;
                detail.Description = pkg.PkgDesc;
                detail.Destination = destination;
                detail.BasePrice = pkg.PkgBasePrice;
                detail.AgencyCommission = pkg.PkgAgencyCommission;
                detail.BookingId = bookingId + 1;
                detail.RegionId = region;
                detail.ClassId = classID;
                detail.FeeId = "BK"; // "Booking Charge", hardcoded as no column for this in Packages
                detail.ProductSupplierId = 80; // "80" corresponds to a company that
                                               // provides airfare; hardcoded, once again

                try
                {
                    db.Entry(book).State = EntityState.Added;
                    /* Save changes to Bookings before BookingDetails, because of
                     * foreign key depencency for BookingId */
                    db.SaveChanges();
                    db.Entry(detail).State = EntityState.Added;
                    db.SaveChanges();

                    /* Put something in TempData, which is cleared after 1 request, so
                     * success message will only display the first time each package is booked,
                     * rather than every time Dashboard/Index is loaded */
                    TempData["PkgMessage2"] = "Package successfully booked!";
                    return RedirectToAction("Index", "Dashboard");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
                return RedirectToAction("Login", "Customer");
        }
        //----------------------------------Enter Credit Card
        #region Credit_Card
        public ActionResult EnterCreditCard()
        {
            // If a user is logged in, proceed with entering credit card info.
            // Otherwise, redirect to login
            if (Session["CustomerId"] != null)
            {
                // Create a list that can be rendered in the drop down list
                var model = new CreditCard();
                model.Companies = Lists.getAllCardCompanies();
                return View(model);
            }
            else
                return RedirectToAction("Login", "Customer");
        }
        [HttpPost]
        public ActionResult EnterCreditCard(CreditCard card)
        {
            // Need to recreate list in case page is refreshed
            card.Companies = Lists.getAllCardCompanies();
            // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // TODO: Should allow user to enter credit card info from a dashboard menu option. Currently, they can only do so after selecting a package

            // If information is valid, proceed
            if (ModelState.IsValid)
            {
                // Clear ViewBag.Message, in case a user was registered this session
                ViewBag.Message = null;
                // Get the CustomerId
                int custID = Convert.ToInt32(Session["CustomerId"]);
                // Create new entry in database for credit card info
                CreditCard model = new CreditCard();
                model.CCName = card.CCName;
                model.CCNumber = card.CCNumber;
                model.CCExpiry = card.CCExpiry;
                model.CustomerId = custID;




                // Check if credit card number is valid
                bool cardNum = CardValidator.isValidCreditCard(model.CCNumber);
                bool cardLength = CardValidator.isValidLength(model);
                bool cardType = CardValidator.isValidType(model);
                bool cardExpiry = CardValidator.isValidExpiry(model.CCExpiry);

                if (cardNum && cardLength && cardType && cardExpiry) // if valid, save to database
                {
                    try
                    {
                        // Save
                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();
                        // Set Session CreditCardID variable
                        Session["CreditCardID"] =
                            db.CreditCards.Where(m => m.CreditCardId.Equals(model.CreditCardId)).FirstOrDefault();

                        // TempData used to display a div on index indicating successful registration
                        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // TODO: Add the div that this is connected to, to the package confirmation page as well
                        TempData["Message"] = "Credit card information successfully registered!";

                        /* If the user came to the Enter Credit Card page after selecting a package, take them to 
                         * the purchase page */
                        if (Session["PkgID"] != null)
                        {
                            return RedirectToAction("Purchase", "Dashboard", new { id = Session["PkgID"] });
                        }
                        else // if not, redirect to the index
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else // if invalid
                {
                    // Use TempData to send user an error message
                    TempData["InvalidCard"] = "Invalid card number";
                    return View(card);
                }

            }
            // If information is invalid, reload page with previously entered info
            else
            {
                return View(card);
            }
        }

        //----------------------------------End Enter Credit Card
        #endregion

        #endregion
        //--------------------------------------------------End Purchase Package

    }
}