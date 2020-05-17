using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class MemberAdditional : IEntity
    {
        public string Id { get; set; }
        public string MBI { get; set; }
        public bool? OutreachNeededInd { get; set; }
        public string OutreachPriorityInd { get; set; }
        public string OutreachReason { get; set; }
        public string AgentName { get; set; }
        public Supplementary Supplementary { get; set; }
        public CareManagement CareManagement { get; set; }
        public ContactRelated ContactRelated { get; set; }
        public RiskActivity RiskActivity { get; set; }
        public InteractionHistory InteractionHistory { get; set; }
        public BenefitsActivity BenefitsActivity { get; set; }
        public Medication Medication { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

    public class Supplementary
    {
        public string SocialSecurityNumber { get; set; }
        public string ApplicationID { get; set; }
        public string MedicaidNumber { get; set; }
        public bool? MedicaidEligibilityInd { get; set; }
        public bool? LISStatusInd { get; set; }
        public DateTime? LISStartDate { get; set; }
        public DateTime? LISEndDate { get; set; }
        public string LISLevel { get; set; }
        public string LISRXCoPaymentAmt { get; set; }
    }

    public class CareManagement
    {
        public DateTime? LastHRADate { get; set; }
        public DateTime? HRADueDate { get; set; }
        public bool? CareGapInd { get; set; }
        public string CareGapName { get; set; }
        public string CareGapDescription { get; set; }
        public string PCPName { get; set; }
        public string CMEName { get; set; }
        public string HealthRiskCategory { get; set; }
        public string SpecialistProvider { get; set; }
    }

    public class ContactRelated
    {
        public string PermissionToText { get; set; }
        public DateTime? PermissionToTextDate { get; set; }
        public DateTime? TextOptOutDate { get; set; }
        public string EmailAddress { get; set; }
        public string POAName { get; set; }
        public string POAPhoneNumber { get; set; }
        public string POAAddress { get; set; }
        public Emergency EmergencyInfo { get; set; }

    }

    public class Emergency
    {
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactRelationshipToMember { get; set; }
    }

    public class RiskActivity
    {
        public string DisEnrollmentRequest { get; set; }
        public DateTime? DisEnrollmentRequestDate { get; set; }
        public string DisEnrollmentRequestReason { get; set; }
        public DateTime? DisEnrollmentEffectiveDate { get; set; }
        public bool? LEPInd { get; set; }
        public string LEPAmount { get; set; }
        public DateTime? LEPLetterSentDate { get; set; }
        public string FiledGrievanceIndicator { get; set; }
        public string GrievanceDate1 { get; set; }
        public string GrievanceSource1 { get; set; }
        public string GrievanceDescription1 { get; set; }
        public string GrievanceOutcome1 { get; set; }
        public string GrievanceDate2 { get; set; }
        public string GrievanceSource2 { get; set; }
        public string GrievanceDescription2 { get; set; }
        public string GrievanceOutcome2 { get; set; }
        public string FiledAppealIndicator { get; set; }
        public DateTime? AppealDate { get; set; }
        public string AppealType { get; set; }
        public string AppealDescription { get; set; }
        public string AppealOutcome { get; set; }
    }

    public class InteractionHistory
    {
        public bool? WelcomeCallMadeInd { get; set; }
        public DateTime? WelcomeCallDate { get; set; }
        public DateTime? MbrNotesDate { get; set; }
        public string MbrNotesProviderName { get; set; }
        public string MbrNotesSource { get; set; }
        public string MbrNotesDescription { get; set; }
    }

    public class BenefitsActivity
    {
        public string FoodRecipientInd { get; set; }
        public string NumberOfMealsProvided { get; set; }
        public string FitnessUseIndicator { get; set; }
        public string FitnessLocationName { get; set; }
        public OTC OTCInfo { get; set; }
        public string TransportationUseIndicator { get; set; }
        public string NumberOfRidesUtilized { get; set; }
    }

    public class OTC
    {
        public bool? CardActivated { get; set; }
        public string CardBalance { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? MailDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class Medication
    {
        public string DayRefillInd90 { get; set; }
        public bool? MailOrderPharmacyInd { get; set; }
        public string MedAdherenceRate { get; set; }
        public string GenericUtilizationRate { get; set; }
        public bool? TakingMedInd { get; set; }
        public string MedicationNumber1 { get; set; }
        public string MedicationName1 { get; set; }
        public string MedicationQuantity1 { get; set; }
        public string MedicationNumber2 { get; set; }
        public string MedicationName2 { get; set; }
        public string MedicationQuantity2 { get; set; }
    }

}
