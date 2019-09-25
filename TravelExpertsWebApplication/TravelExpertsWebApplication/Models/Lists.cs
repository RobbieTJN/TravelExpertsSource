using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelExpertsWebApplication.Models
{
    public class Lists
    {
        /* Lists of every province in Canada and every country available (only Canada so far)
         * Tutorial on using drop down lists in MVC:
         * https://nimblegecko.com/using-simple-drop-down-lists-in-ASP-NET-MVC/ 
         * We don't have a database table with provinces and countries, and I desired that
         * the display text would show the full province name, while the database requires
         * a 2-letter province code, so that info is hardcoded here */
        public static IEnumerable<SelectListItem> getAllProvinces()
        {
            var provList = new List<SelectListItem>();
            provList.Add(new SelectListItem
            {
                Text = "British Columbia",
                Value = "BC"
            });
            provList.Add(new SelectListItem
            {
                Text = "Alberta",
                Value = "AB"
            });
            provList.Add(new SelectListItem
            {
                Text = "Saskatchewan",
                Value = "SK"
            });
            provList.Add(new SelectListItem
            {
                Text = "Manitoba",
                Value = "MB"
            });
            provList.Add(new SelectListItem
            {
                Text = "Ontario",
                Value = "ON"
            });
            provList.Add(new SelectListItem
            {
                Text = "Quebec",
                Value = "QC"
            });
            provList.Add(new SelectListItem
            {
                Text = "New Brunswick",
                Value = "NB"
            });
            provList.Add(new SelectListItem
            {
                Text = "Nova Scotia",
                Value = "NS"
            });
            provList.Add(new SelectListItem
            {
                Text = "Prince Edward Island",
                Value = "PE"
            });
            provList.Add(new SelectListItem
            {
                Text = "Newfoundland and Labrador",
                Value = "NL"
            });
            provList.Add(new SelectListItem
            {
                Text = "Yukon",
                Value = "YT"
            });
            provList.Add(new SelectListItem
            {
                Text = "Northwest Territories",
                Value = "NT"
            });
            provList.Add(new SelectListItem
            {
                Text = "Nunavut",
                Value = "NU"
            });

            return provList;
        }

        public static IEnumerable<SelectListItem> getAllCountries()
        {
            var countryList = new List<SelectListItem>();
            countryList.Add(new SelectListItem
            {
                Text = "Canada",
                Value = "Canada"
            });

            return countryList;
        }

        /* List of available credit card companies
         * Database sample data only had Visa, MC, Diners and Amex, so only allowing those */
        public static IEnumerable<SelectListItem> getAllCardCompanies()
        {
            // For each value, trying to match value of existing entries in database
            var companyList = new List<SelectListItem>();
            companyList.Add(new SelectListItem
            {
                Text = "Master Card",
                Value = "MC"
            });
            companyList.Add(new SelectListItem
            {
                Text = "Visa",
                Value = "VISA"
            });
            companyList.Add(new SelectListItem
            {
                Text = "American Express",
                Value = "AMEX"
            });
            companyList.Add(new SelectListItem
            {
                Text = "Diners Club/Carte Blanche",
                Value = "Diners"
            });

            return companyList;
        }
    }
}