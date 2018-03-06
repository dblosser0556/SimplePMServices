using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplePMServices.Models.Entities
{
    public class Phase
    {
        
        public int PhaseId { get; set; }
        [Required]
        [StringLength(50)]
        public string PhaseName { get; set; }
        public string PhaseDesc { get; set; }

        //public ICollection<ProjectMonth> ProjectMonths { get; set; }
    }
}
