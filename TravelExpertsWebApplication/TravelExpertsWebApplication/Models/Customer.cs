//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

/* Edited by Robbie */

namespace TravelExpertsWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Bookings = new HashSet<Booking>();
            this.CreditCards = new HashSet<CreditCard>();
            this.Customers_Rewards = new HashSet<Customers_Rewards>();
        }

        public int CustomerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your first name")]
        [MaxLength(25, ErrorMessage = "Maximum length is 25 characters")]
        [DisplayName("First Name")]
        public string CustFirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your last name")]
        [MaxLength(25, ErrorMessage = "Maximum length is 25 characters")]
        [DisplayName("Last Name")]
        public string CustLastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your street address")]
        [MaxLength(75, ErrorMessage = "Maximum length is 75 characters")]
        [DisplayName("Street Address")]
        public string CustAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your city of residence")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters")]
        [DisplayName("City")]
        public string CustCity { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select your province/territory")]
        [MaxLength(2)]
        [DisplayName("Province/Territory")]
        public string CustProv { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your postal code")]
        [RegularExpression(
            "^[ABCEGHJ-NPRSTVXYabceghj-nprstvxy][0-9][ABCEGHJ-NPRSTV-Zabceghj-nprstv-z][ ]?[0-9][ABCEGHJ-NPRSTV-Zabceghj-nprstv-z][0-9]$",
            ErrorMessage = "Invalid postal code (ex: H0H 0H0, space optional)")]
        [DisplayName("Postal Code")]
        public string CustPostal { get; set; }

        [Required(ErrorMessage = "Please enter your country")]
        [MaxLength(25)]
        [DisplayName("Country")]
        public string CustCountry { get; set; }

        [Required(ErrorMessage = "Please enter your home phone number")]
        [MinLength(10, ErrorMessage = "Invalid phone number")]
        [MaxLength(13, ErrorMessage = "Invalid phone number")]
        [DataType(DataType.PhoneNumber)] // This won't actually validate for proper phone number
        /* Excellent answer by Francis Gagnon on using regex for phone numbers here:
         * https://stackoverflow.com/questions/16699007/regular-expression-to-match-standard-10-digit-phone-number
         * This regex allows a 10-digit phone number with optional (brackets) for Area Code and optional
         * space or dash (-) between Exchange Code and Subscriber Number
         * The @ will allow \d escape charater, which denotes any numeric digit */
        [RegularExpression(@"^[(]?(\d{3})[ )]?(\d{3})[ -]?(\d{4})$", ErrorMessage = "Invalid phone number")]
        [DisplayName("Home Phone Number")]
        public string CustHomePhone { get; set; }

        [MinLength(10, ErrorMessage = "Invalid phone number")]
        [MaxLength(20, ErrorMessage = "Invalid phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[(]?(\d{3})[ )]?(\d{3})[ -]?(\d{4})$", ErrorMessage = "Invalid phone number")]
        [DisplayName("Business Phone Number")]
        public string CustBusPhone { get; set; }

        [MaxLength(50, ErrorMessage = "Email too long")]
        [EmailAddress] // This will validate the email address without the need of a regex
        [DisplayName("Email")]
        public string CustEmail { get; set; }
        public Nullable<int> AgentId { get; set; }

        // Lists to allow user to select their province and country (only Canada for now)
        public IEnumerable<SelectListItem> Provinces { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public virtual Agent Agent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditCard> CreditCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customers_Rewards> Customers_Rewards { get; set; }
    }
}
