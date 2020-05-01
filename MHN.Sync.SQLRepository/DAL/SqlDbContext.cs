using MHN.Sync.Entity.SQLEntity;
using MHN.Sync.SQLRepository.EntityConfig;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MHN.Sync.SQLRepository.DAL
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext() : base("name=DatabaseContextContainer")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        #region Database Collection

        //public DbSet<MemberAcquisitionTrend> MemberAcquisitionTrend { get; set; }
        //public DbSet<GatewayMemberSummary> GatewayMemberSummary { get; set; }
        //public DbSet<MonthlyMedicareSummary> MonthlyMedicareSummary { get; set; }
        //public DbSet<MemberDisenrollmentSummery> MemberDisenrollmentSummery { get; set; }
        //public DbSet<MemberDisenrollmentRecord> MemberDisenrollmentRecord { get; set; }
        public DbSet<IntakeWorkflow> IntakeDashboardEnhancement { get; set; }
        //public DbSet<NavigatorSummary> NavigatorSummary { get; set; }
        //public DbSet<NavigatorDashboardSummary> NavigatorDashboardSummary { get; set; }
        //public DbSet<MemberRawResponse> MemberRawResponse { get; set; }
        //public DbSet<PomSnpReportTemp> PomSnpReportTemp { get; set; }

        //public DbSet<HraQuestionMap> HraQuestionMap { get; set; }
        //public DbSet<NavigatorQuestionSummary> NavigatorQuestionSummary { get; set; }
        //public DbSet<HraMemberOverview> HraMemberOverview { get; set; }
        
        //public DbSet<POMOutcome> POMOutcomes { get; set; }

        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SqlDbContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.Add(new MemberAcquisitionTrendConfiguration());
            //modelBuilder.Configurations.Add(new GatewayMemberSummaryConfiguration());
            //modelBuilder.Configurations.Add(new MonthlyMedicareSummaryConfiguration());
            //modelBuilder.Configurations.Add(new MemberDisenrollmentSummeryConfiguration());
            //modelBuilder.Configurations.Add(new MemberDisenrollmentRecordConfiguration());
            modelBuilder.Configurations.Add(new IntakeWorkflowConfiguration());
        }
    }
}
