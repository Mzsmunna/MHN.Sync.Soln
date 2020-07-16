using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class ProspectMember : IEntity
    {
        public string Id { get; set; }
        public string ProspectId { get; set; }
        public string MemberId { get; set; }
        public string MBI { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string LOB { get; set; }
        public Address AddressInfo { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string AgentName { get; set; }
        public bool IsMember { get; set; }
        public bool IsApplicant { get; set; }       
        public DateTime? TermDateOn { get; set; }
        public DateTime? EnrolledOn { get; set; }
        public DateTime? DisEnrolledOn { get; set; }
        public DateTime? PTC_Date_Stamped { get; set; }
        public DateTime? PTCExpiredOn { get; set; }
        public DateTime? LastContactAttemptedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<MHNAcquisition> AcquisitionList { get; set; }

        //For MemberAdditional
        public bool? OutreachNeededInd { get; set; }
        public string LISStatusInd { get; set; }
        public bool? DisEnrollmentRequest { get; set; }
        public DateTime? DisEnrollmentEffectiveDate { get; set; }
        public bool? FiledGrievanceIndicator { get; set; }
        public bool? FiledAppealIndicator { get; set; }
        //--new
        public string LastSubmissionDateDifference { get; set; }
        public string Source { get; set; }
        public ExternalSubmitterInfo ExternalSubmitterInfo { get; set; }

    }

    public class MHNAcquisition
    {
        public string FormType { get; set; }
        public DateTime FormSubmissionDate { get; set; }
        public string RequestSubmissionRefId { get; set; }
    }

    public class ExternalSubmitterInfo
    {
        public string OrganizationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string CurrentDate { get; set; }
    }
}
