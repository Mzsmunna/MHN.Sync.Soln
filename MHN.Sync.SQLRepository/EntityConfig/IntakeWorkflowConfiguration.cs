using MHN.Sync.Entity.SQLEntity;
using System.Data.Entity.ModelConfiguration;

namespace MHN.Sync.SQLRepository.EntityConfig
{
    public class IntakeWorkflowConfiguration : EntityTypeConfiguration<IntakeWorkflow>
    {
        public IntakeWorkflowConfiguration()
        {
            this.ToTable("IntakeDashboardEnhancementStage");
            this.HasKey(x => x.Id);
        }
    }
}
