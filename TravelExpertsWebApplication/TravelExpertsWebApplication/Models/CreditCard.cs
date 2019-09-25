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

    public partial class CreditCard
    {
        public int CreditCardId { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Please select your credit card company name")]
        [MaxLength(10)]
        [DisplayName("Card Company")]
        public string CCName { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Please enter your credit card number")]
        [MinLength(15, ErrorMessage = "Card number is too short")]
        [MaxLength(16, ErrorMessage = "Card number is too long")]
        [DisplayName("Card Number")]
        public string CCNumber { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Please enter your credit card expiry date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Card Expiry Date")]
        /* Field needs to be nullable in order to prevent an ugly default value of
         * year 0001 being displayed, even if expiry is required */
        public DateTime? CCExpiry { get; set; }
        public int CustomerId { get; set; }

        // List of credit card companies to select from
        public IEnumerable<SelectListItem> Companies { get; set; }

        public virtual Customer Customer { get; set; }
    }
}