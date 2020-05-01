using GTW.Sync.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTW.Sync.SQLRepository.EntityConfig
{
    public class GatewayMemberSummaryConfiguration : EntityTypeConfiguration<GatewayMemberSummary>
    {
        public GatewayMemberSummaryConfiguration()
        {
            this.ToTable("GatewayMemberSummary");
            this.HasKey(x => new { x.Year, x.Month });
        }
    }
}
