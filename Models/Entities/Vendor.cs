using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Contact { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContractIdentifier { get; set; }
        public string ContractTerms { get; set; }
        public double ContractAmount { get; set; }
        public DateTime ContractEndDate { get; set; }

        public int ProjectId { get; set; }

        public virtual ICollection<VendorPeriod> PeriodEstimates { get; set; }
        public virtual ICollection<VendorInvoice> Invoices { get; set; }
    }
}
