using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class EnrollmentRequest : IEntity
    {
        public string Id { get; set; }
        public string ProspectMemberDataRef { get; set; }
        public List<string> EnrollInPlan { get; set; }
        public string EnrollPCPName { get; set; }
        public string EnrollPCPId { get; set; }
        public string MedicareCardName { get; set; }
        public string MedicareNumber { get; set; }
        public string MedicarePartA { get; set; }
        public string MedicarePartB { get; set; }
        public string PremiumPaymentOption { get; set; }
        public string MonthlyBenefitFrom { get; set; }
        public bool HaveESRD { get; set; }
        public bool HaveOtherDrugCoverage { get; set; }
        public DrugCoverage DrugCoverage { get; set; }
        public bool IsLongTermCareResident { get; set; }
        public LongTermCareResident LongTermCareResident { get; set; }
        public bool IsEnrolledStateMedicaidProgram { get; set; }
        public string MedicaidNumber { get; set; }
        public bool IsSpouseWork { get; set; }
        public List<string> PreferedLanguage { get; set; }
        public AuthorizedRepresentative AuthorizedRepresentative { get; set; }
        public string Signature { get; set; }

        public DateTime? TodayDate { get; set; }
        public string AgentName { get; set; }
        public string AgentId { get; set; }

        public DateTime? EffectiveDateCoverage { get; set; }
        public string EventId { get; set; }
        public List<PreferredDayOutreach> PreferredDayOutreach { get; set; }

        //public bool IsActive { get; set; }
        public string EnrollmentFormNo { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PreferredDayOutreach
    {
        public string QuestionId { get; set; }
        public string DateWithQuestion { get; set; }
    }

    public class DrugCoverage
    {
        public string CoverageName { get; set; }
        public string CoverageId { get; set; }
        public string CoverageGroup { get; set; }

    }

    public class LongTermCareResident
    {
        public string InstitutionName { get; set; }
        public string InstitutionAddress { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class AuthorizedRepresentative
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string RelationshipToEnrollee { get; set; }
    }
}
