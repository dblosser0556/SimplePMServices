using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
  
    
   

    // Define the project structure.  Each project will have project months and 
    // resources which have resource monthes and fix or other costs which have
    // months as well.
    public class Project
    {
        
        public int ProjectId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string ProjectManager { get; set; }
        [Required]
        public DateTime PlannedStartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }

        public int GroupId { get; set; }
       
        public int StatusId { get; set; }
       

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Month> Months { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<FixedPrice> FixedPriceCosts { get; set; }


    }

  
}
