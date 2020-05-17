using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class MemberAdditionalModel
    {
        public string Id { get; set; }
        public string MBI { get; set; }
        public string OutreachNeededInd { get; set; }
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
        //public string MedicaidEligibilityInd { get; set; }
        public string LISStatusInd { get; set; }
        public string LISStartDate { get; set; }
        public string LISEndDate { get; set; }
        public string LISLevel { get; set; }
        public string LISRXCoPaymentAmt { get; set; }
    }

    public class CareManagement
    {
        public string LastHRADate { get; set; }
        public string HRADueDate { get; set; }
        public string CareGapInd { get; set; }
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
        public string PermissionToTextDate { get; set; }
        public string TextOptOutDate { get; set; }
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
        public string DisEnrollmentRequestDate { get; set; }
        public string DisEnrollmentRequestReason { get; set; }
        public string DisEnrollmentEffectiveDate { get; set; }
        public string LEPInd { get; set; }
        public string LEPAmount { get; set; }
        public string LEPLetterSentDate { get; set; }
        public string FiledGrievanceIndicator { get; set; }
        public string GrievanceDate1 { get; set; }
        public string GrievanceSource1 { get; set; }
        public string GrievanceDescription1{ get; set; }
        public string GrievanceOutcome1 { get; set; }
        public string GrievanceDate2 { get; set; }
        public string GrievanceSource2 { get; set; }
        public string GrievanceDescription2 { get; set; }
        public string GrievanceOutcome2 { get; set; }
        public string FiledAppealIndicator { get; set; }
        public string AppealDate { get; set; }
        public string AppealType { get; set; }
        public string AppealDescription { get; set; }
        public string AppealOutcome { get; set; }
    }

    public class InteractionHistory
    {
        public string WelcomeCallMadeInd { get; set; }
        public string WelcomeCallDate { get; set; }
        public string MbrNotesDate { get; set; }
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
        public string CardActivated { get; set; }
        public string CardBalance { get; set; }
        public string ActivationDate { get; set; }
        public string MailDate { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }
    }

    public class Medication
    {
        public string DayRefillInd90 { get; set; }
        public string MailOrderPharmacyInd { get; set; }
        public string MedAdherenceRate { get; set; }
        public string GenericUtilizationRate { get; set; }
        public string TakingMedInd { get; set; }
        public string MedicationNumber1 { get; set; }
        public string MedicationName1 { get; set; }
        public string MedicationQuantity1 { get; set; }
        public string MedicationNumber2 { get; set; }
        public string MedicationName2 { get; set; }
        public string MedicationQuantity2 { get; set; }
    }


    public sealed class MemberAdditionalModelMap : ClassMap<MemberAdditionalModel>
    {
        public MemberAdditionalModelMap()
        {
            //Map(m => m.Id).Ignore();
            Map(m => m.MBI).Name("MBI");
            Map(m => m.OutreachNeededInd).Name("Outreach Needed Ind");
            Map(m => m.OutreachPriorityInd).Name("Outreach Priority Ind");
            Map(m => m.OutreachReason).Name("Outreach Reason");
            Map(m => m.AgentName).Name("Agent Name");

            //Supplementary
            Map(m => m.Supplementary.SocialSecurityNumber).Name("Social Security Number");
            Map(m => m.Supplementary.ApplicationID).Name("Application ID");
            Map(m => m.Supplementary.MedicaidNumber).Name("Medicaid Number");
            Map(m => m.Supplementary.LISStatusInd).Name("LIS Receive Status Ind");
            Map(m => m.Supplementary.LISStartDate).Name("LIS Start Date");
            Map(m => m.Supplementary.LISEndDate).Name("LIS End Date");
            Map(m => m.Supplementary.LISLevel).Name("LIS Level");
            Map(m => m.Supplementary.LISRXCoPaymentAmt).Name("LIS RX Co-Payment amt");

            //CareManagement
            Map(m => m.CareManagement.LastHRADate).Name("Last HRA Date");
            Map(m => m.CareManagement.HRADueDate).Name("HRA Due Date");
            Map(m => m.CareManagement.CareGapInd).Name("Care Gap Ind");
            Map(m => m.CareManagement.CareGapName).Name("Care Gap Name");
            Map(m => m.CareManagement.CareGapDescription).Name("Care Gap Description");
            Map(m => m.CareManagement.HealthRiskCategory).Name("Health Risk Category");
            Map(m => m.CareManagement.PCPName).Name("PCP Name");
            Map(m => m.CareManagement.CMEName).Name("CME Name");
            Map(m => m.CareManagement.SpecialistProvider).Name("Specialist Provider Name");

            //ContactRelated
            Map(m => m.ContactRelated.PermissionToText).Name("Permission To Text");
            Map(m => m.ContactRelated.PermissionToTextDate).Name("Permission to Text Date");
            Map(m => m.ContactRelated.TextOptOutDate).Name("Text Opt Out Date");
            Map(m => m.ContactRelated.EmailAddress).Name("Email Address");
            Map(m => m.ContactRelated.POAName).Name("POA Name", "POI Name");
            Map(m => m.ContactRelated.POAPhoneNumber).Name("POA Phone Number", "POI Phone Number");
            Map(m => m.ContactRelated.POAAddress).Name("POA Address", "POI Address");
            Map(m => m.ContactRelated.EmergencyInfo.ContactName).Name("Emergency Contact Name");
            Map(m => m.ContactRelated.EmergencyInfo.ContactAddress).Name("Emergency Contact Address");
            Map(m => m.ContactRelated.EmergencyInfo.ContactPhoneNumber).Name("Emergency Contact Phone Number");
            Map(m => m.ContactRelated.EmergencyInfo.ContactRelationshipToMember).Name("Emergency Contact Relationship to Member");

            //RiskActivity
            Map(m => m.RiskActivity.DisEnrollmentRequest).Name("DisEnrollment Requested");
            Map(m => m.RiskActivity.DisEnrollmentRequestDate).Name("DisEnrollment Request Date");
            Map(m => m.RiskActivity.DisEnrollmentRequestReason).Name("DisEnrollment Request Reason");
            Map(m => m.RiskActivity.DisEnrollmentEffectiveDate).Name("DisEnrollment Effective Date");
            Map(m => m.RiskActivity.LEPInd).Name("LEP Ind");
            Map(m => m.RiskActivity.LEPAmount).Name("LEP Amount");
            Map(m => m.RiskActivity.LEPLetterSentDate).Name("LEP Letter Sent Date");
            Map(m => m.RiskActivity.FiledGrievanceIndicator).Name("Filed Grievance Indicator");
            Map(m => m.RiskActivity.GrievanceDate1).Name("Grievance Date (1)");
            Map(m => m.RiskActivity.GrievanceSource1).Name("Grievance Source (1)", "'Grievance Source (1)");
            Map(m => m.RiskActivity.GrievanceDescription1).Name("Grievance Description (1)");
            Map(m => m.RiskActivity.GrievanceOutcome1).Name("Grievance Outcome (1)");
            Map(m => m.RiskActivity.GrievanceDate2).Name("Grievance Date (2)");
            Map(m => m.RiskActivity.GrievanceSource2).Name("Grievance Source (2)");
            Map(m => m.RiskActivity.GrievanceDescription2).Name("Grievance Description (2)");
            Map(m => m.RiskActivity.GrievanceOutcome2).Name("Grievance Outcome (2)");
            Map(m => m.RiskActivity.FiledAppealIndicator).Name("Filed Appeal Indicator");
            Map(m => m.RiskActivity.AppealDate).Name("Appeal Date");
            Map(m => m.RiskActivity.AppealType).Name("Appeal Type");
            Map(m => m.RiskActivity.AppealDescription).Name("Appeal Description");
            Map(m => m.RiskActivity.AppealOutcome).Name("Appeal Outcome");

            //InteractionHistory
            Map(m => m.InteractionHistory.WelcomeCallMadeInd).Name("Welcome_Call_Made_Ind");
            Map(m => m.InteractionHistory.WelcomeCallDate).Name("Welcome_Call_Date");
            Map(m => m.InteractionHistory.MbrNotesDate).Name("Mbr Notes Date");
            Map(m => m.InteractionHistory.MbrNotesProviderName).Name("Mbr Notes Provider Name");
            Map(m => m.InteractionHistory.MbrNotesSource).Name("Mbr Notes Source");
            Map(m => m.InteractionHistory.MbrNotesDescription).Name("Mbr Notes Description");

            //BenefitsActivity
            Map(m => m.BenefitsActivity.FoodRecipientInd).Name("Food Recipient Ind");
            Map(m => m.BenefitsActivity.NumberOfMealsProvided).Name("Number of Meals Provided");
            Map(m => m.BenefitsActivity.FitnessUseIndicator).Name("Fitness Use Indicator");
            Map(m => m.BenefitsActivity.FitnessLocationName).Name("Fitness Location Name");
            Map(m => m.BenefitsActivity.OTCInfo.CardActivated).Name("OTC Card Activated");
            Map(m => m.BenefitsActivity.OTCInfo.CardBalance).Name("OTC Card Balance");
            Map(m => m.BenefitsActivity.OTCInfo.ActivationDate).Name("OTC Activation Date");
            Map(m => m.BenefitsActivity.OTCInfo.MailDate).Name("OTC Mail Date");
            Map(m => m.BenefitsActivity.OTCInfo.StartDate).Name("OTC Start Date");
            Map(m => m.BenefitsActivity.OTCInfo.ExpirationDate).Name("OTC Expiration Date");
            Map(m => m.BenefitsActivity.TransportationUseIndicator).Name("Transportation Use Indicator");
            Map(m => m.BenefitsActivity.NumberOfRidesUtilized).Name("Number of Rides Utilized");

            //Medication
            Map(m => m.Medication.DayRefillInd90).Name("90_day_refill_ind");
            Map(m => m.Medication.MailOrderPharmacyInd).Name("mail_order_pharmacy_ind");
            Map(m => m.Medication.MedAdherenceRate).Name("med_adherence_rate");
            Map(m => m.Medication.GenericUtilizationRate).Name("generic_utilization_rate");
            Map(m => m.Medication.TakingMedInd).Name("Taking-Med-Ind", "Taking-Med-Ind ");
            Map(m => m.Medication.MedicationNumber1).Name("Medication-Number(1)");
            Map(m => m.Medication.MedicationName1).Name("Medication-Name(1)", "Medication-Name (1)");
            Map(m => m.Medication.MedicationQuantity1).Name("Medication-Quantity(1)", "Medication-Quantity (1)");
            Map(m => m.Medication.MedicationNumber2).Name("Medication-Number(2)");
            Map(m => m.Medication.MedicationName2).Name("Medication-Name(2)", "Medication-Name (2)");
            Map(m => m.Medication.MedicationQuantity2).Name("Medication-Quantity(2)", "Medication-Quantity (2)");
         
        }
    }

}
