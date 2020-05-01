using GTW.Sync.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTW.Sync.SQLRepository.EntityConfig
{
    public class MonthlyMedicareSummaryConfiguration : EntityTypeConfiguration<MonthlyMedicareSummary>
    {
        public MonthlyMedicareSummaryConfiguration()
        {
            this.ToTable("MonthlyMedicareSummary");
            this.HasKey(x => new { x.ClientId, x.Month, x.Year });
        }
    }
}
