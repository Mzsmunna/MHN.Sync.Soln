using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.SQLEntity
{
    public class IntakeWorkflow
    {
        public int Id { get; set; }
        public string ProjectLead { get; set; }
        public string WorkflowStatus { get; set; }
        public int TotalDuration { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Indicator { get; set; }
    }

    public class Indicator
    {
        public string Id { get; set; }
        public string WorkflowStatus { get; set; }
        public string Color { get; set; }
        public int StartRange { get; set; }
        public int EndRange { get; set; }

    }
}
