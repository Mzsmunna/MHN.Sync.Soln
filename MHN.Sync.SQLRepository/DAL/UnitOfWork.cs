using MHN.Sync.SQLInterface;

namespace MHN.Sync.SQLRepository.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlDbContext _dbContext;
        public UnitOfWork(SqlDbContext dbContext, IIntakeWorkflowRepository intakeWorkflowRepository)
        {
            _dbContext = dbContext;
            //GatewayMemberSummary = gatewayMemberSummaryRepository;
            //MemberAcquisitionTrend = disEnrollmentSummaryRepository;
            //MonthlyMedicareSummary = monthlyMedicareSummaryRepository;
            //DisenrollmentSummary = memberDisenrollmentSummaryRepository;
            IntakeWorkflow = intakeWorkflowRepository;
            //MemberDisenrollmentRecord = memberDisenrollmentRecordRepository;
        }

        //public IGatewayMemberSummaryRepository GatewayMemberSummary { get; private set; }
        //public IMemberAcquisitionTrendRepository MemberAcquisitionTrend { get; private set; }
        //public IMonthlyMedicareSummaryRepository MonthlyMedicareSummary { get; private set; }
        //public IMemberDisenrollmentSummaryRepository DisenrollmentSummary { get; private set; }
        public IIntakeWorkflowRepository IntakeWorkflow { get; private set; }
        //public IMemberDisenrollmentRecordRepository MemberDisenrollmentRecord { get; private set; }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
