using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.ViewModels
{
    public class ProjectList
    {
        public int ProjectId { get; set; }
        public bool IsTemplate { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectManagerName { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedStartYear { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualStartYear { get; set; }
        public string FilterYear { get; set; }
        public int GroupId { get; set; }
        public int StatusId { get; set; }
        public string GroupName { get; set; }
        public string GroupManager { get; set; }
        public string StatusName { get; set; }
        public double TotalPlannedExpense { get; set; }
        public double TotalActualExpense { get; set; }
        public double TotalPlannedCapital { get; set; }
        public double TotalActualCapital { get; set; }
        public double TotalExpenseBudget { get; set; }
        public double TotalCapitalBudget { get; set; }
        
    }
}
