using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class FixedPriceMonth
    {

        public int FixedPriceMonthId { get; set; }

        [Required]
        public int MonthNo { get; set; }
        public double PlannedCost { get; set; }
        public double PlannedCostCapPercent { get; set; }
        public int? PlannedCostStyle { get; set; }
        public double ActualCost { get; set; }
        public double ActualCostCapPercent { get; set; }
        public int? ActualCostStyle { get; set; }

        [Required]
        public int FixedPriceId { get; set; }

    }

}
