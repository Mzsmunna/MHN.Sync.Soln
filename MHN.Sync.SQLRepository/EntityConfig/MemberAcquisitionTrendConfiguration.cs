using GTW.Sync.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTW.Sync.SQLRepository.Config
{
    public class MemberAcquisitionTrendConfiguration : EntityTypeConfiguration<MemberAcquisitionTrend>
    {
        public MemberAcquisitionTrendConfiguration()
        {
            this.ToTable("MemberAcquisitionTrend");
            this.HasKey(x => new { x.PlanId, x.State, x.County, x.LOB });
        }
    }
}
