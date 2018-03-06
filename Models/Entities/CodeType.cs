using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SimplePMServices.Models.Entities
{
    public class CodeType
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }
        public string TypeDesc { get; set; }
    }
}
