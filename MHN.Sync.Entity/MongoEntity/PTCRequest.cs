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
        public string PreferredOutreachDay { get; set; }
        public string PreferredOutreachTime { get; set; }
        public string PreferredLanguage { get; set; }
        public string SpecificTimeOrRangeNoted { get; set; }
        public string SpecificDateNoted { get; set; }
        public string OtherLanguage { get; set; }
        public string HeardAboutUsThrough { get; set; }
        public string AdditionalComments { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
    }
}
