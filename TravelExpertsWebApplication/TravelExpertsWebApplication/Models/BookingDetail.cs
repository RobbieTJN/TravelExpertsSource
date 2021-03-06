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

    public partial class BookingDetail
    {
        public int BookingDetailId { get; set; }

        [DisplayName("Itenerary Number")]
        public Nullable<double> ItineraryNo { get; set; }

        [DisplayName("Trip Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> TripStart { get; set; }

        [DisplayName("Trip End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> TripEnd { get; set; }

        public string Description { get; set; }

        public string Destination { get; set; }

        [DisplayName("Price")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> BasePrice { get; set; }

        [DataType(DataType.Currency)]
        public Nullable<decimal> AgencyCommission { get; set; }

        [DisplayName("Booking ID")]
        public Nullable<int> BookingId { get; set; }
        public string RegionId { get; set; }
        public string ClassId { get; set; }
        public string FeeId { get; set; }
        public Nullable<int> ProductSupplierId { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Class Class { get; set; }
        public virtual Fee Fee { get; set; }
        public virtual Products_Suppliers Products_Suppliers { get; set; }
        public virtual Region Region { get; set; }
    }
}
