using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    // Define fixed cost or costs not based on effort.  there may or maynot be fixed costs rows 
    // however

    public class FixedPriceType
    {

        public int FixedPriceTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string FixedPriceTypeName { get; set; }
        public string FixedPriceTypeDesc { get; set; }


    }

}
