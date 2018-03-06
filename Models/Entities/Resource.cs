using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class Resource
    {

        public int ResourceId { get; set; }
        [Required]
        public string ResourceName { get; set; }
        [Required]
        public double Rate { get; set; }
        public string Vendor { get; set; }
        public int RoleId { get; set; }
        public double TotalPlannedEffort { get; set; }
        public double TotalActualEffort { get; set; }



        public int ResourceTypeId { get; set; }


        public int ProjectId { get; set; }
        public virtual ICollection<ResourceMonth> ResourceMonths { get; set; }
    }
}
