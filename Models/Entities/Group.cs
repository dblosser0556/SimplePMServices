using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplePMServices.Models.Entities
{
   
    // A program will can have a program budget and a set of projects.
    public class Group
    {

        public int GroupId { get; set; }
        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }
        public string GroupDesc { get; set; }
        public string GroupManager { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }

    }
}
