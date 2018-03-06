using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class ResourceMonth
    {

        public int ResourceMonthId { get; set; }
        [Required]
        public int MonthNo { get; set; }
        public double PlannedEffort { get; set; }
        public double PlannedEffortCapPercent { get; set; }
        public int? PlannedEffortStyle { get; set; }
        public double ActualEffort { get; set; }
        public double ActualEffortCapPercent { get; set; }
        public int? ActualEffortStyle { get; set; }
       
        public int ResourceId { get; set; }


    }
}
