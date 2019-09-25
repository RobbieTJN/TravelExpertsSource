using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelExpertsWebApplication.Models;

/* Author: Robbie Nielsen */

namespace TravelExpertsWebApplication.Controllers
{
    public class CustomerController : Controller
    {
        // create context object for EF
        private TravelExpertsEntities db = new TravelExpertsEntities();

        //--------------------------------------------------Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            // Checks if user has already logged in
            if (Session["CustomerId"] == null)
            {
                // Create lists that can be rendered in the drop down list
                var model = new CustomerRegistrationData();
                model.Provinces = Lists.getAllProvinces();
                model.Countries = Lists.getAllCountries();
                // If not logged in, proceed to registration
                return View(model);
            }
            else
            {
                // If logged in, redirect to dashboard
                return RedirectToAction("Index", "Dashboard");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(CustomerRegistrationData cust)
        {
            // Need to recreate the list for the drop down list again, in case the page is reloaded
            cust.Provinces = Lists.getAllProvinces();
            cust.Countries = Lists.getAllCountries();
            if (ModelState.IsValid)
            {
                // Check if username is already in use
                var usernameExists = UsernameTaken(cust.Username);
                if (usernameExists) // username is taken
                {
                    ModelState.AddModelError("UsernameExist", "That username is taken");
                    return View(cust);
                }

                // Check if email is already in use
                var emailExists = EmailTaken(cust.CustEmail);
                if (emailExists) // email is taken and not null
                {
                    ModelState.AddModelError("EmailExist", "That email is already registered");
                    return View(cust);
                }

                // Password hashing - salt isn't working
                //cust.Salt = Crypto.CreateSalt(10);
                //cust.UserPassword = Crypto.Hash(cust.UserPassword, cust.Salt);
                //cust.ConfirmPassword = Crypto.Hash(cust.ConfirmPassword, cust.Salt);
                cust.UserPassword = Crypto.HashNoSalt(cust.UserPassword);
                cust.ConfirmPassword = Crypto.HashNoSalt(cust.ConfirmPassword);

                // Convert enumerator values for Province and Country to string values for DB entry

                // Save customer login info into RegisteredUsers table in the database
                using (db)
                {
                    // Save all customer info in CustomerRegistrationData table in the database
                    db.CustomerRegistrationDatas.Add(cust);

                    // Save customer personal info in Customers table in the database
                    Customer cst = new Customer();
                    cst.CustFirstName = cust.CustFirstName;
                    cst.CustLastName = cust.CustLastName;
                    cst.CustAddress = cust.CustAddress;
                    cst.CustCity = cust.CustCity;
                    cst.CustProv = cust.CustProv;
                    cst.CustPostal = cust.CustPostal;
                    cst.CustCountry = cust.CustCountry;
                    cst.CustHomePhone = cust.CustHomePhone;
                    cst.CustBusPhone = cust.CustBusPhone;
                    cst.CustEmail = cust.CustEmail;
                    db.Customers.Add(cst);

                    // Save customer login info in RegisteredUsers table in the database
                    RegisteredUser user = new RegisteredUser();
                    user.Username = cust.Username;
                    user.Salt = cust.Salt;
                    user.UserPassword = cust.UserPassword;
                    db.RegisteredUsers.Add(user);

                    // Save all database changes
                    try
                    {
                        db.SaveChanges();
                        // Clear ModelState for future registration
                        ModelState.Clear();
                        cust = null;
                        user = null;
                        TempData["AcctMessage"] = "Account registration was successful!";
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                    ve.PropertyName,
                                    eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                    ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return RedirectToAction("Login", "Customer");
            }
            else // invalid data, make user re-enter info
            {
                return View(cust);
            }
        }
        //--------------------------------------------------End Register

        //--------------------------------------------------Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            // Checks if user has already logged in
            if (Session["CustomerId"] == null)
            {
                // If not logged in, proceed to login
                return View();
            }
            else
            {
                // If logged in, log out
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }

        // From https://www.c-sharpcorner.com/article/simple-login-application-using-Asp-Net-mvc/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(RegisteredUser user)
        {
            if (ModelState.IsValid)
            {
                // Password hashing - salt isn't working
                //cust.Salt = Crypto.CreateSalt(10);
                //cust.UserPassword = Crypto.Hash(cust.UserPassword, cust.Salt);
                //cust.ConfirmPassword = Crypto.Hash(cust.ConfirmPassword, cust.Salt);
                user.UserPassword = Crypto.HashNoSalt(user.UserPassword);

                using (db)
                {
                    var obj = db.RegisteredUsers.Where(model => model.Username.Equals(user.Username) &&
                        model.UserPassword.Equals(user.UserPassword)).FirstOrDefault();

                    if (obj != null) // if user was found
                    {
                        // Check CreditCards table in DB if the customer's ID is there, this means they registered their info before
                        var creditObj =
                            db.CreditCards.Where(model => model.CustomerId.Equals(obj.CustomerId)).FirstOrDefault();

                        // Check if the customer has registered credit card info, and if not, make the value an empty string
                        string creditCard = creditObj?.CreditCardId.ToString() ?? "";

                        // Store customer ID and username in Session variables
                        Session["CustomerId"] = obj.CustomerId.ToString();
                        Session["Username"] = obj.Username.ToString();

                        if (creditCard == "") // Customer has not registered their credit card info
                        {
                            Session["CreditCardID"] = -1;
                        }
                        else
                        {
                            Session["CreditCardID"] = creditObj.CreditCardId.ToString();
                        }

                        // Check if user came to login page after selecting a package to purchase
                        if (Session["PkgID"] != null) // they came here from selecting a package to purchase on the Index
                        {
                            return RedirectToAction("Purchase", "Dashboard", new { id = Session["PkgID"] });
                        }
                        else // they logged in the normal way
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }

                    }
                    else // if user was not found
                    {
                        // Using TempData, so if user refreshes the login page, the warning will not appear again
                        TempData["InvalidUser"] = "Incorrect username or password, please try again.";
                        return RedirectToAction("Login", "Customer");
                    }
                }
            }
            else // ModelState is invalid; for debugging. Place breakpoint here-ish
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
            }
            return View(user);
        }
        //--------------------------------------------------End Login

        // Code for checking if username/email are already in use
        #region NonActions
        // Checks if the entered email is already used by a registered customer. Returns true if so
        // From http://www.dotnetawesome.com/2017/04/complete-login-registration-system-asp-mvc.html
        [NonAction]
        public bool EmailTaken(string emailID)
        {
            using (TravelExpertsEntities ent = new TravelExpertsEntities())
            {
                // Looks for the first or default instance of entered string
                var v = ent.CustomerRegistrationDatas.Where(a => a.CustEmail == emailID).FirstOrDefault();
                // If the entered email was null, allow it by returning false, "email is not taken"
                if (emailID == null)
                    return false;
                // If the email exists (value is not null), return true
                return v != null;
            }
        }

        // Checks if the entered username is taken. Returns true if so
        // From http://www.dotnetawesome.com/2017/04/complete-login-registration-system-asp-mvc.html
        [NonAction]
        public bool UsernameTaken(string username)
        {
            using (TravelExpertsEntities ent = new TravelExpertsEntities())
            {
                // Looks for the first or default instance of entered string
                var v = ent.CustomerRegistrationDatas.Where(a => a.Username == username).FirstOrDefault();
                // If the username exists (value is not null), return true
                return v != null;
            }
        }
        #endregion
    }
}