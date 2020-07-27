using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class PTCRequest : IEntity
    {
        public string Id { get; set; }
        public string ProspectMemberDataRef { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public List<string> PreferredContactMethod { get; set; }
        public List<string> PreferredOutReachDay { get; set; }
        public List<string> PreferredOutReachTime { get; set; }
        public List<string> PreferredLanguage { get; set; }
        public string SpecificTimeOrRangeNoted { get; set; }
        public DateTime? SpecificDateNoted { get; set; }
        public string OtherLanguage { get; set; }
        public string HeardAboutUsThrough { get; set; }
        public string AdditionalComments { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public string Source { get; set; }
    }
}
