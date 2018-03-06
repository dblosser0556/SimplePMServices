using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class Role
    {
        
        public int RoleId { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
    }
}
