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
        public AddressChanged AddressChanged { get; set; }
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
        public string GrievanceDate { get; set; }
        public string GrievanceSource { get; set; }
        public string GrievanceDescription{ get; set; }
        public string GrievanceOutcome { get; set; }
        //public string GrievanceDate2 { get; set; }
        //public string GrievanceSource2 { get; set; }
        //public string GrievanceDescription2 { get; set; }
        //public string GrievanceOutcome2 { get; set; }
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
        public string MedicationNumber { get; set; }
        public string MedicationName { get; set; }
        public string MedicationQuantity { get; set; }
        //public string MedicationNumber2 { get; set; }
        //public string MedicationName2 { get; set; }
        //public string MedicationQuantity2 { get; set; }
    }

    public class AddressChanged
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneOrLandLineNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PreferredPhoneNumber { get; set; }
    }


    public sealed class MemberAdditionalModelMap : ClassMap<MemberAdditionalModel>
    {
        public MemberAdditionalModelMap()
        {
            //Map(m => m.Id).Ignore();
            Map(m => m.MBI).Name("MBI", "GEN_MBI");
            Map(m => m.OutreachNeededInd).Name("Outreach Needed Ind", "GEN_Outreach_Needed_Ind");
            Map(m => m.OutreachPriorityInd).Name("Outreach Priority Ind", "GEN_Outreach_Priority_Ind");
            Map(m => m.OutreachReason).Name("Outreach Reason", "GEN_Outreach_Reason");
            Map(m => m.AgentName).Name("Agent Name", "GEN_Agent_Name");

            //Supplementary
            Map(m => m.Supplementary.SocialSecurityNumber).Name("Social Security Number", "SUP_Social_Security_Number");
            Map(m => m.Supplementary.ApplicationID).Name("Application ID", "SUP_Application_ID");
            Map(m => m.Supplementary.MedicaidNumber).Name("Medicaid Number", "SUP_Medicaid_Number");
            Map(m => m.Supplementary.LISStatusInd).Name("LIS Receive Status Ind", "SUP_LIS_Receive_Status_Ind");
            Map(m => m.Supplementary.LISStartDate).Name("LIS Start Date", "SUP_LIS_Start_Date");
            Map(m => m.Supplementary.LISEndDate).Name("LIS End Date", "SUP_LIS_End_Date");
            Map(m => m.Supplementary.LISLevel).Name("LIS Level", "SUP_LIS_Level");
            Map(m => m.Supplementary.LISRXCoPaymentAmt).Name("LIS RX Co-Payment amt", "SUP_LIS_RX_Co_Payment_amt");

            //CareManagement
            Map(m => m.CareManagement.LastHRADate).Name("Last HRA Date", "CM_Last_HRA_Date");
            Map(m => m.CareManagement.HRADueDate).Name("HRA Due Date", "CM_HRA_Due_Date");
            Map(m => m.CareManagement.CareGapInd).Name("Care Gap Ind", "CM_Care_Gap_Ind");
            Map(m => m.CareManagement.CareGapName).Name("Care Gap Name", "CM_Care_Gap_Name");
            Map(m => m.CareManagement.CareGapDescription).Name("Care Gap Description", "CM_Care_Gap_Description");
            Map(m => m.CareManagement.HealthRiskCategory).Name("Health Risk Category", "CM_Health_Risk_Category");
            Map(m => m.CareManagement.PCPName).Name("PCP Name", "CM_PCP_Name");
            Map(m => m.CareManagement.CMEName).Name("CME Name", "CM_CME_Name");
            Map(m => m.CareManagement.SpecialistProvider).Name("Specialist Provider Name", "CM_Specialist_Provider_Name");

            //ContactRelated
            Map(m => m.ContactRelated.PermissionToText).Name("Permission To Text", "CR_Permission_To_Text");
            Map(m => m.ContactRelated.PermissionToTextDate).Name("Permission to Text Date", "CR_Permission_to_Text_Date");
            Map(m => m.ContactRelated.TextOptOutDate).Name("Text Opt Out Date", "CR_Text_Opt_Out_Date");
            Map(m => m.ContactRelated.EmailAddress).Name("Email Address", "CR_Email_Address");
            Map(m => m.ContactRelated.POAName).Name("POA Name", "CR_POA_Name");
            Map(m => m.ContactRelated.POAPhoneNumber).Name("POA Phone Number", "CR_POA_Phone_Number");
            Map(m => m.ContactRelated.POAAddress).Name("POA Address", "CR_POA_Address");
            Map(m => m.ContactRelated.EmergencyInfo.ContactName).Name("Emergency Contact Name", "CR_Emergency_Contact_Name");
            Map(m => m.ContactRelated.EmergencyInfo.ContactAddress).Name("Emergency Contact Address", "CR_Emergency_Contact_Address");
            Map(m => m.ContactRelated.EmergencyInfo.ContactPhoneNumber).Name("Emergency Contact Phone Number", "CR_Emergency_Contact_Phone_Numbe", "CR_Emergency_Contact_Phone_Number");
            Map(m => m.ContactRelated.EmergencyInfo.ContactRelationshipToMember).Name("Emergency Contact Relationship to Member", "CR_Emergency_Contact_Relationshi", "CR_Emergency_Contact_Relationship");

            //RiskActivity
            Map(m => m.RiskActivity.DisEnrollmentRequest).Name("DisEnrollment Requested", "RA_DisEnrollment_Requested");
            Map(m => m.RiskActivity.DisEnrollmentRequestDate).Name("DisEnrollment Request Date", "RA_DisEnrollment_Request_Date");
            Map(m => m.RiskActivity.DisEnrollmentRequestReason).Name("DisEnrollment Request Reason", "RA_DisEnrollment_Request_Reason");
            Map(m => m.RiskActivity.DisEnrollmentEffectiveDate).Name("DisEnrollment Effective Date", "RA_DisEnrollment_Effective_Date");
            Map(m => m.RiskActivity.LEPInd).Name("LEP Ind", "RA_LEP_Ind");
            Map(m => m.RiskActivity.LEPAmount).Name("LEP Amount", "RA_LEP_Amount");
            Map(m => m.RiskActivity.LEPLetterSentDate).Name("LEP Letter Sent Date", "RA_LEP_Letter_Sent_Date");
            Map(m => m.RiskActivity.FiledGrievanceIndicator).Name("Filed Grievance Indicator", "RA_Filed_Grievance_Indicator");
            Map(m => m.RiskActivity.GrievanceDate).Name("Grievance Date (1)", "RA_Grievance_Date");
            Map(m => m.RiskActivity.GrievanceSource).Name("Grievance Source (1)", "RA_Grievance_Source");
            Map(m => m.RiskActivity.GrievanceDescription).Name("Grievance Description (1)", "RA_Grievance_Description");
            Map(m => m.RiskActivity.GrievanceOutcome).Name("Grievance Outcome (1)", "RA_Grievance_Outcome");
            //Map(m => m.RiskActivity.GrievanceDate2).Name("Grievance Date (2)", "");
            //Map(m => m.RiskActivity.GrievanceSource2).Name("Grievance Source (2)", "");
            //Map(m => m.RiskActivity.GrievanceDescription2).Name("Grievance Description (2)", "");
            //Map(m => m.RiskActivity.GrievanceOutcome2).Name("Grievance Outcome (2)", "");
            Map(m => m.RiskActivity.FiledAppealIndicator).Name("Filed Appeal Indicator", "RA_Filed_Appeal_Indicator");
            Map(m => m.RiskActivity.AppealDate).Name("Appeal Date", "RA_Appeal_Date");
            Map(m => m.RiskActivity.AppealType).Name("Appeal Type", "RA_Appeal_Type");
            Map(m => m.RiskActivity.AppealDescription).Name("Appeal Description", "RA_Appeal_Description");
            Map(m => m.RiskActivity.AppealOutcome).Name("Appeal Outcome", "RA_Appeal_Outcome");

            //InteractionHistory
            Map(m => m.InteractionHistory.WelcomeCallMadeInd).Name("Welcome_Call_Made_Ind", "IH_Welcome_Call_Made_Ind");
            Map(m => m.InteractionHistory.WelcomeCallDate).Name("Welcome_Call_Date", "IH_Welcome_Call_Date");
            Map(m => m.InteractionHistory.MbrNotesDate).Name("Mbr Notes Date", "IH_Mbr_Notes_Date");
            Map(m => m.InteractionHistory.MbrNotesProviderName).Name("Mbr Notes Provider Name", "IH_Mbr_Notes_Provider_Name");
            Map(m => m.InteractionHistory.MbrNotesSource).Name("Mbr Notes Source", "IH_Mbr_Notes_Source");
            Map(m => m.InteractionHistory.MbrNotesDescription).Name("Mbr Notes Description", "IH_Mbr_Notes_Description");

            //BenefitsActivity
            Map(m => m.BenefitsActivity.FoodRecipientInd).Name("Food Recipient Ind", "BA_Food_Recipient_Ind");
            Map(m => m.BenefitsActivity.NumberOfMealsProvided).Name("Number of Meals Provided", "BA_Number_of_Meals_Provided");
            Map(m => m.BenefitsActivity.FitnessUseIndicator).Name("Fitness Use Indicator", "BA_Fitness_Use_Indicator");
            Map(m => m.BenefitsActivity.FitnessLocationName).Name("Fitness Location Name", "BA_Fitness_Location_Name");
            Map(m => m.BenefitsActivity.OTCInfo.CardActivated).Name("OTC Card Activated", "BA_OTC_Card_Activated");
            Map(m => m.BenefitsActivity.OTCInfo.CardBalance).Name("OTC Card Balance", "BA_OTC_Card_Balance");
            Map(m => m.BenefitsActivity.OTCInfo.ActivationDate).Name("OTC Activation Date", "BA_OTC_Activation_Date");
            Map(m => m.BenefitsActivity.OTCInfo.MailDate).Name("OTC Mail Date", "BA_OTC_Mail_Date");
            Map(m => m.BenefitsActivity.OTCInfo.StartDate).Name("OTC Start Date", "BA_OTC_Start_Date");
            Map(m => m.BenefitsActivity.OTCInfo.ExpirationDate).Name("OTC Expiration Date", "BA_OTC_Expiration_Date");
            Map(m => m.BenefitsActivity.TransportationUseIndicator).Name("Transportation Use Indicator", "BA_Transportation_Use_Indicator");
            Map(m => m.BenefitsActivity.NumberOfRidesUtilized).Name("Number of Rides Utilized", "BA_Number_of_Rides_Utilized");

            //Medication
            Map(m => m.Medication.DayRefillInd90).Name("90_day_refill_ind", "RX_90_day_refill_ind");
            Map(m => m.Medication.MailOrderPharmacyInd).Name("mail_order_pharmacy_ind", "RX_mail_order_pharmacy_ind");
            Map(m => m.Medication.MedAdherenceRate).Name("med_adherence_rate", "RX_med_adherence_rate");
            Map(m => m.Medication.GenericUtilizationRate).Name("generic_utilization_rate", "RX_generic_utilization_rate");
            Map(m => m.Medication.TakingMedInd).Name("Taking-Med-Ind", "Taking-Med-Ind ", "RX_Taking_Med_Ind");
            Map(m => m.Medication.MedicationNumber).Name("Medication-Number(1)", "RX_Medication_Number");
            Map(m => m.Medication.MedicationName).Name("Medication-Name(1)", "Medication-Name (1)", "RX_Medication_Name");
            Map(m => m.Medication.MedicationQuantity).Name("Medication-Quantity(1)", "Medication-Quantity (1)", "RX_Medication_Quantity");
            //Map(m => m.Medication.MedicationNumber2).Name("Medication-Number(2)", "");
            //Map(m => m.Medication.MedicationName2).Name("Medication-Name(2)", "Medication-Name (2)", "");
            //Map(m => m.Medication.MedicationQuantity2).Name("Medication-Quantity(2)", "Medication-Quantity (2)", "");

            Map(m => m.AddressChanged.Address1).Name("ADR_Changed_Address_1");
            Map(m => m.AddressChanged.Address2).Name("ADR_Changed_Address_2");
            Map(m => m.AddressChanged.CityName).Name("ADR_Changed_City_Name");
            Map(m => m.AddressChanged.State).Name("ADR_Changed_State");
            Map(m => m.AddressChanged.ZipCode).Name("ADR_Changed_Zip_Code");
            Map(m => m.AddressChanged.PhoneOrLandLineNumber).Name("ADR_Changed_Phone_Nbr_Land_Line");
            Map(m => m.AddressChanged.MobileNumber).Name("ADR_Changed_Mobile_Phone_Nbr");
            Map(m => m.AddressChanged.PreferredPhoneNumber).Name("ADR_Preferred_Phone_Number");
        }
    }

}
