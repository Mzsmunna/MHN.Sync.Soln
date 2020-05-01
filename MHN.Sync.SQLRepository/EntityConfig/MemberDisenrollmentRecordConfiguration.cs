using GTW.Sync.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTW.Sync.SQLRepository.EntityConfig
{
    public class MemberDisenrollmentRecordConfiguration : EntityTypeConfiguration<MemberDisenrollmentRecord>
    {
        public MemberDisenrollmentRecordConfiguration()
        {
            this.ToTable("MemberDisenrollmentRecord");
            this.Ignore(x => x.MemberId);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(x => x.Id);
        }
    }
}
