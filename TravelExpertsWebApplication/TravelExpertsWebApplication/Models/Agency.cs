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

    public partial class Agency
    {
        private List<Agency> list;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agency()
        {
            this.Agents = new HashSet<Agent>();
        }

        public Agency(List<Agency> list)
        {
            this.list = list;
        }

        public int AgencyId { get; set; }
        public string AgncyAddress { get; set; }
        public string AgncyCity { get; set; }
        public string AgncyProv { get; set; }
        public string AgncyPostal { get; set; }
        public string AgncyCountry { get; set; }
        public string AgncyPhone { get; set; }
        public string AgncyFax { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent> Agents { get; set; }
    }
}
