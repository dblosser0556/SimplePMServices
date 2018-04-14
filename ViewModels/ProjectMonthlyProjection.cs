using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.ViewModels
{
    public class ProjectMonthlyProjection
    {
        public string GroupName { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string ProjectMgr { get; set; }
        public string StatusName { get; set; }
        public int MonthNo { get; set; }
        public double TotalActualCapital { get; set; }
        public double TotalActualExpense { get; set; }
        public double TotalPlannedCapital { get; set; }
        public double TotalPlannedExpense { get; set; }
        public string PhaseName { get; set; }
        public Boolean IsTemplate { get; set; }
        public DateTime Month { get; set; }
        public int Year { get; set; }
        public string UserName { get; set; }
        public string GroupMgr { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }

    }
}
