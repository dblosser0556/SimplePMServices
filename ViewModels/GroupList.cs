using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.ViewModels
{
    public class GroupList
    {
       
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int? ParentId { get; set; }
        public int ParentName { get; set; }
        public string GroupDesc { get; set; }
        public string GroupManager { get; set; }
        public string GroupManagerName { get; set; }
        public double TotalPlannedExpense { get; set; }
        public double TotalActualExpense { get; set; }
        public double TotalPlannedCapital { get; set; }
        public double TotalActualCapital { get; set; }
        public double TotalExpenseBudget { get; set; }
        public double TotalCapitalBudget { get; set; }
    }
}
