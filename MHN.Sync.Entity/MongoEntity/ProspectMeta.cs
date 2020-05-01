using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class ProspectMeta : IEntity
    {
        public string Id { get; set; }
        public string ProspectMemberDataRef { get; set; }
        public string Language { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string PreferredContactMethod { get; set; }
        public string PreferredOutreachDay { get; set; }
        public DateTime? SpecificDateNoted { get; set; }
        public string PreferredOutreachTime { get; set; }
        public string Specific_time_or_range_noted { get; set; }
        public string Heard_about_us_through { get; set; }
        public string Comments { get; set; }
        public string Compl_by_Name { get; set; }
        public string Compl_by_Organization { get; set; }
        public string Compl_by_Role { get; set; }
        public string Origin { get; set; }
        public string Location { get; set; }
        public DateTime? PTC_Date_Stamped { get; set; }
        public DateTime? AgentAssignedOn { get; set; }
        public DateTime? PTCExpiredOn { get; set; }
        public DateTime? LastContactAttemptedOn { get; set; }
        public DateTime? LastContactedOn { get; set; }
        public DateTime? PTCFilledOn { get; set; }
        public string ConvertStatus { get; set; }
        public Conversion ConversionInfo { get; set; }
        public string Admin_Notes { get; set; }
        public bool IsMember { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class Conversion
    {
        public string One_Two_DaysConversion { get; set; }
        public string Three_Five_DaysConversion { get; set; }
        public string Five_Plus_DaysConversion { get; set; }
    }
}
