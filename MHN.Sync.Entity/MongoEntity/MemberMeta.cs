using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class MemberMeta : IEntity
    {
        public string Id { get; set; }
        public string ProspectMemberDataRef { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
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

        public bool IsMember { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
