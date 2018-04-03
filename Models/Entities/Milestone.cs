﻿using System;

namespace SimplePMServices.Models.Entities
{
    public class Milestone
    {
        public int MilestoneId { get; set; }
        
        public bool Active { get; set; }
        public DateTime SetDateTime { get; set; }
        public int PhaseId { get; set; }
        public DateTime PhaseCompleteDate { get; set; }
        public double PhaseCompleteEstimate { get; set; }

        public int ProjectId { get; set; }
    }
}