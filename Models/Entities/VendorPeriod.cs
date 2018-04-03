using System;

namespace SimplePMServices.Models.Entities
{
    public class VendorPeriod
    {
        public int VendorPeriodId { get; set; }
        public int VendorId { get; set; }
        public int PeriodNo { get; set; }
        public double PeriodEstimate { get; set; }
        
    }
}