using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplePMServices.Models.Entities
{
   
    //Flexilbe organizational hierarchy.  this uses the visitaion pattern where the left and right encaplilate the 
    public class Group
    {

        
        public int GroupId { get; set; }
        public int? ParentId { get; set; }

        //indicator of the organization level withe level 1 being the top level.
        public int Level { get; set; }
        public string LevelDesc { get; set; }

        // this indicates the position in the hierarchy.  
        // these are calculated fields.
        public int? LevelId { get; set; }
        public int? Left { get; set; }
        public int? Right { get; set; }
        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }
        public string GroupDesc { get; set; }
        public string GroupManager { get; set; }

        
        public virtual ICollection<Budget> Budgets { get; set; }

    }
}
