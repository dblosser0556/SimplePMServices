using System;

namespace SimplePMServices.Models.Entities
{
    public class VendorInvoice
    {
        public int VendorInvoiceId { get; set; }
        
        public DateTime InvoiceDate { get; set; }
        public double Amount { get; set; }
        public string Comments { get; set; }

        public int VendorId { get; set; }
    }
}