using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.ModelView
{
    public class MemberModel
    {
        public string LOB { get; set; }
        public string TermDateOn { get; set; }
        public string CareManagementEntity { get; set; }
        public string PCPId { get; set; }
        public string PCPName { get; set; }
        public string PCP_NPI { get; set; }
        public string Part_A_RF { get; set; }
        public string Part_D_RF { get; set; }
        public string DualCode { get; set; }
        public string LIPS_PCT { get; set; }
        public string LIPS_Code { get; set; }
        public string INST_Code { get; set; }

        public string MBI { get; set; }
        public string EnrolledOn { get; set; }
        public string MemberId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressModel Address { get; set; }

        public bool IsMember { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
    }

    public class MemberModelMap : ClassMap<MemberModel>
    {
        public MemberModelMap()
        {
            Map(m => m.LOB).Name("PRODUCT");
            Map(m => m.MBI).Name("MBI");
            Map(m => m.EnrolledOn).Name("START DATE");
            Map(m => m.TermDateOn).Name("TERM DATE");
            Map(m => m.MemberId).Name("INTERNAL ID");
            Map(m => m.LName).Name("LAST NAME");
            Map(m => m.FName).Name("FIRST_NAME");                        
            Map(m => m.DOB).Name("DOB");            
            Map(m => m.Gender).Name("SEX");
            Map(m => m.Address.address1).Name("ADDRESS LINE 1");
            Map(m => m.Address.address2).Name("ADDRESS LINE 2");
            Map(m => m.Address.city).Name("CITY");
            Map(m => m.Address.state).Name("STATE");
            Map(m => m.Address.zip).Name("ZIP"); 
            Map(m => m.PhoneNumber).Name("PHONE");
            Map(m => m.Email).Name("EMAIL");
            Map(m => m.CareManagementEntity).Name("CARE-MANAGEMENT ENTITY");
            Map(m => m.PCPId).Name("INTERNAL PCP ID");
            Map(m => m.PCPName).Name("PCP NAME");
            Map(m => m.PCP_NPI).Name("PCP NPI");
            Map(m => m.Part_A_RF).Name("PART A RF");
            Map(m => m.Part_D_RF).Name("PART D RF");
            Map(m => m.DualCode).Name("DUAL CODE");
            Map(m => m.LIPS_PCT).Name("LIPS PCT");
            Map(m => m.LIPS_Code).Name("LIPS CODE");
            Map(m => m.INST_Code).Name("INST CODE");
            //Map(m => m.Id).Ignore();
        }   
    }
}
