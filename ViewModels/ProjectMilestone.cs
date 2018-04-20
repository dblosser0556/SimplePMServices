using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.ViewModels
{
    public class ProjectMilestone
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public DateTime ActualStart { get; set; }
        public int ActualStartYear { get; set; }
        public DateTime PlannedStart { get; set; }
        public int PlannedStartYear { get; set; }
        public string PhaseName { get; set; }
        public DateTime PlannedPhaseComplete { get; set; }
        public string MilestonePhaseComplete { get; set; }
        public double PhaseCapitalTarget { get; set; }
        public double PhaseExpenseTarget { get; set; }
        public double ActualCapital { get; set; }
        public double ActualExpense { get; set; }
        public double EACCapital { get; set; }
        public double EACExpense { get; set; }
    }
}
