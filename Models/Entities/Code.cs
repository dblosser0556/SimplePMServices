using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SimplePMServices.Models.Entities
{
    public class Code
    {
        public int ID { get; set; }
        [Required]
        public int CodeTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string CodeName { get; set; }
        public string CodeDesc { get; set; }
    }
}
