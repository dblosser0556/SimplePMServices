using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class Status
    {


        public int StatusId { get; set; }
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }
        public string StatusDesc { get; set; }
        public bool Dashboard { get; set; }
        public int Order { get; set; }



    }
}
