using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.ModelView
{
    public class ProspectModel
    {
        public string Type { get; set; }
        public string Language { get; set; }
        public string MobileNumber { get; set; }
        public string PreferredContactMethod { get; set; }
        public string SpecificDateNoted { get; set; }
        public string PreferredOutreachTime { get; set; }
        public string PreferredOutreachDay { get; set; }
        public string Specific_time_or_range_noted { get; set; }
        public string Heard_about_us_through { get; set; }
        public string Comments { get; set; }
        public string Compl_by_Name { get; set; }
        public string Compl_by_Organization { get; set; }
        public string Compl_by_Role { get; set; }
        public string Origin { get; set; }
        public string Location { get; set; }
        public string PTC_Date_Stamped { get; set; }
        public string AgentAssignedOn { get; set; }
        public string PTCExpiredOn { get; set; }
        public string LastContactedOn { get; set; }
        public string LastContactAttemptedOn { get; set; }
        public string PTCFilledOn { get; set; }
        public string ConvertStatus { get; set; }
        public ConversionModel Conversion { get; set; }
        public string Admin_Notes { get; set; }
        public string ProspectId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressModel Address { get; set; }
        public string AgentName { get; set; }

        public bool IsMember { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
    }

    public class ConversionModel
    {
        public string One_Two_DaysConversion { get; set; }
        public string Three_Five_DaysConversion { get; set; }
        public string Five_Plus_DaysConversion { get; set; }
    }

    public sealed class ProspectModelMap : ClassMap<ProspectModel>
    {
        public ProspectModelMap()
        {
            Map(m => m.ProspectId).Name("ptc_number");
            Map(m => m.Type).Name("type_of_ptc");
            Map(m => m.Language).Name("Pref_Language");
            Map(m => m.FName).Name("prospect_first_name");
            Map(m => m.LName).Name("prospect_last_name");
            Map(m => m.Address.address1).Name("address");
            Map(m => m.Address.city).Name("city");
            Map(m => m.Address.state).Name("state");
            Map(m => m.Address.zip).Name("zip");
            Map(m => m.PhoneNumber).Name("phone_number");
            Map(m => m.MobileNumber).Name("mobile_number");
            Map(m => m.Email).Name("email_address");
            Map(m => m.PreferredContactMethod).Name("preferred_method_of_contact");
            Map(m => m.PreferredOutreachTime).Name("preferred_time_of_outreach");
            Map(m => m.SpecificDateNoted).Name("specific_date_noted");           
            Map(m => m.PreferredOutreachDay).Name("preferred_day_of_outreach"); 
            Map(m => m.Specific_time_or_range_noted).Name("specific_time_or_range_noted");
            Map(m => m.Heard_about_us_through).Name("heard_about_us_through");
            Map(m => m.Comments).Name("additional_comments");
            Map(m => m.Compl_by_Name).Name("Compl_by_Name");
            Map(m => m.Compl_by_Organization).Name("Compl_by_Organization");
            Map(m => m.Compl_by_Role).Name("Compl_by_Role");
            Map(m => m.AgentName).Name("assigned_agent");
            Map(m => m.Origin).Name("prospect_origin");
            Map(m => m.Location).Name("prospect_location");
            Map(m => m.PTC_Date_Stamped).Name("PTC_date_stamped");
            Map(m => m.AgentAssignedOn).Name("agent_assigned_date");
            Map(m => m.PTCExpiredOn).Name("ptc_expiration_date");
            Map(m => m.LastContactAttemptedOn).Name("last_contact_attempt_date");
            Map(m => m.LastContactedOn).Name("last_contact_made_date");
            Map(m => m.PTCFilledOn).Name("appt_date");
            Map(m => m.ConvertStatus).Name("converted_not converted");
            Map(m => m.Conversion.One_Two_DaysConversion).Name("1-2 day_conversion");
            Map(m => m.Conversion.Three_Five_DaysConversion).Name("3-5 day_conversion");
            Map(m => m.Conversion.Five_Plus_DaysConversion).Name("5+days_conversion");
            Map(m => m.Admin_Notes).Name("Admin_Notes");
            //Map(m => m.Id).Ignore();
        }
    }
}
