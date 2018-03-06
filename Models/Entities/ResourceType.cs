using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    // Define resource rows and thier efforts
    public class ResourceType
    {

        public int ResourceTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string ResourceTypeName { get; set; }
        public string ResourceTypeDesc { get; set; }

    }
}
