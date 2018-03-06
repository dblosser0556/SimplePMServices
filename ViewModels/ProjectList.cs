using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.ViewModels
{
    public class ProjectList
    {
        public int ProjectId { get; set; }
        
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string ProjectManager { get; set; }
        public string PlannedStartDate { get; set; }
        public string ActualStartDate { get; set; }
        public int GroupId { get; set; }
        public int StatusId { get; set; }
        public string GroupName { get; set; }
        public string GroupManager { get; set; }
        public string StatusName { get; set; }
        
    }
}
