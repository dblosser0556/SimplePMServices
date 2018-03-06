using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{

    public enum BudgetType : int
    {
        Capital,
        Expense
    }

    // Define the budget structure.
    public class Budget
    {
        public int BudgetId { get; set; }
        [Required]
        public BudgetType BudgetType { get; set; }
        [Required]
        public DateTime ApprovedDateTime { get; set; }
        public double Amount { get; set; }

        public int ProjectId { get; set; }

    }

}
