using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{

    public class FixedPrice
    {

        public int FixedPriceId { get; set; }


        public string FixedPriceName { get; set; }
        public string Vendor { get; set; }
        public int? FixedPriceTypeId { get; set; }
        public int? ResourceTypeId { get; set; }

        public double TotalPlannedCost { get; set; }
        public double TotalActualCost { get; set; }


        public int ProjectId { get; set; }
        public virtual ICollection<FixedPriceMonth> FixedPriceMonths { get; set; }
    }
}
