using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    //Define the months for project.  Each project will have one or many months
    public class Month
    {
        public int MonthId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int MonthNo { get; set; }
        public int PhaseId { get; set; }
        public double TotalPlannedExpense { get; set; }
        public double TotalPlannedCapital { get; set; }
        public double TotalActualExpense { get; set; }
        public double TotalActualCapital { get; set; }

    }

}
