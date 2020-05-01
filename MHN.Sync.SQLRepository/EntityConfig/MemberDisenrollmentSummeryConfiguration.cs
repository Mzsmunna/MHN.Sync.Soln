using GTW.Sync.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTW.Sync.SQLRepository.EntityConfig
{
    public class MemberDisenrollmentSummeryConfiguration : EntityTypeConfiguration<MemberDisenrollmentSummery>
    {
        public MemberDisenrollmentSummeryConfiguration()
        {
            this.ToTable("MemberDisenrollmentSummery");
            this.HasKey(x => x.Id);
            //this.HasKey(x => new { x.State, x.Month, x.Year, x.ReasonCode });
        }
    }
}
