using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class SOARequest : IEntity
    {
        public string Id { get; set; }
        public string ProspectMemberDataRef { get; set; }
        public string AgentId { get; set; }
        public string MedicareAdvantagePrescriptionDrugPlan { get; set; }
        public string RepresentativeName { get; set; }
        public string RelationshipBeneficiary { get; set; }
        public string CompletedByAgent { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryPhone { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string InitialContactMethod { get; set; }
        public string RepresentedPlanInMeeting { get; set; }
        public string ExplanationWhySoaNotDocumented { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
