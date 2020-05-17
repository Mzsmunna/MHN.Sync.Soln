using Cerebro.JWTAuthService.Services.Helper;
using MHN.Sync.Entity;
using MHN.Sync.Entity.Enum;
using MHN.Sync.Helper;
using MHN.Sync.Interface;
using MHN.Sync.IOC.Module;
using MHN.Sync.MongoInterface;
using MHN.Sync.Soln.Managers.FileRead;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Soln
{
    class Program
    {
        private static IKernel kernel;
        private static IProspectMemberRepository _prospectMemberRepository;
        private static IProspectRepository _prospectRepository;
        private static IMemberRepository _memberRepository;
        private static IMemberAdditionalRepository _memberAdditionalRepository;
        private static JWTHelperUtility _jwtHelperUtility;

        //private static readonly string BaseAddress = Path.GetDirectoryName(Application.ExecutablePath);

        static void Main(string[] args)
        {
            //var decrypt = Cryptography.Decrypt("5mFNpR8lPddyNHu6Eu57wHzZZdDWvowEgzhgNGNMHg4=");
            //var encrypt = Cryptography.Encrypt("SMITH");

            //if(encrypt.Equals("5mFNpR8lPddyNHu6Eu57wHzZZdDWvowEgzhgNGNMHg4="))
            //{
            //    Console.WriteLine("Matched");
            //}

            Console.WriteLine("Initiating MHN Jobs...");
            // Creating File Export Path Directory
            //Directory.CreateDirectory(ApplicationConstants.ExportFilePath);

            try
            {
                ReadConfigurations();
                RunSyncProcess();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception [Main]:" + ex.Message);
                if (!string.IsNullOrEmpty(ApplicationConstants.InstrumentationKey))
                {
                    //TelemetryLogger.LogException(ex);
                }
            }

            Console.WriteLine("Exiting Job...");
        }

        #region Configurations
        private static IKernel GetNinjectKernel()
        {
            IKernel kernel = new StandardKernel();

            //kernel.Bind<IProspectMemberRepository>().To<ProspectMemberRepository>();

            var modules = new List<INinjectModule>
            {
                new ConfigModule(),
                new RepositoryModule(),
                //new ManagerModule()
            };

            kernel.Load(modules);

            return kernel;
        }
        private static void ReadConfigurations()
        {
            kernel = GetNinjectKernel();
            _prospectMemberRepository = kernel.Get<IProspectMemberRepository>();
            _prospectRepository = kernel.Get<IProspectRepository>();
            _memberRepository = kernel.Get<IMemberRepository>();
            _memberAdditionalRepository = kernel.Get<IMemberAdditionalRepository>();
            //_dataSyncReportRepository = kernel.Get<IDataSyncReportRepository>();
            //_dataSyncJobListRepository = kernel.Get<IDataSyncJobListRepository>();

            _jwtHelperUtility = kernel.Get<JWTHelperUtility>();

        }

        #endregion

        private static void RunSyncProcess()
        {
            if (ApplicationConstants.ManualProcess)
            {
                ManualProcess();
            }
            else
            {
                AutomaticProcess();
            }
        }

        private static void ManualProcess()
        {
            JobManagerResult results = null;
            IJobManager manager;
            string type = string.Empty;

            var syncTypes = GetAllSyncTypes();

            int listCount = 1;
            Console.WriteLine("** Select A Process To Run Manually:");
            foreach (var synctype in syncTypes)
            {
                Console.WriteLine(listCount + ". " + synctype.Value);
                listCount++;
            }

            Console.WriteLine("So Your Choice: ");

            var input = Console.ReadLine();

            listCount = 1;
            MHNSyncType selectedSyncType = new MHNSyncType();
            bool inputTypeMatch = false;


            foreach (var syncType in syncTypes)
            {
                if (listCount.ToString() == input)
                {
                    inputTypeMatch = true;
                    selectedSyncType = syncType.Key;
                }

                listCount++;
            }
            if (inputTypeMatch)
            {
                type = selectedSyncType.ToString();
                manager = GetInstance(selectedSyncType);
                results = manager.Execute();

                if (ApplicationConstants.ManualProcessDataSyncReport)
                {
                    //DataSyncReport report = new DataSyncReport()
                    //{
                    //    CampaignId = ApplicationConstants.CampaignId,
                    //    ClientId = ApplicationConstants.ClientId,
                    //    CreatedOn = Helper.Helper.GetCurrentTimeInEST(),
                    //    Message = results.Message.ToString(),
                    //    Status = results.IsSuccess ? SyncStatus.SUCCESS.ToString() : SyncStatus.FAILED.ToString(),
                    //    SyncType = type,
                    //    SyncExecutionProcess = SyncExecutionProcess.AUTO.ToString(),
                    //    IsSearchedFileFound = results.IsSearchedFileFound,
                    //    NoOfMatchedRecords = results.NoOfMatchedRecords ?? 0,
                    //    NoOfMismatchedRecords = results.NoOfMismatchedRecords ?? 0,
                    //    ProspectIds = results.ProspectIds,
                    //    NoOfRecordsProcessed = results.NoOfRecordsProcessed ?? 0,

                    //    NoOfSpecialData = results.NoOfSpecialData ?? 0,
                    //    NoOfCorruptedData = results.NoOfCorruptedData ?? 0,
                    //    NoOfValidData = ((results.NoOfRecordsProcessed ?? 0) - (results.NoOfCorruptedData ?? 0) - (results.NoOfSpecialData ?? 0))
                    //};
                    //_dataSyncReportRepository.Save(report);
                }
            }
            else Console.WriteLine("Input type not match");

            ManualProcess();
        }
        private static IJobManager GetInstance(MHNSyncType type, List<string> fileLocations = null)
        {
            IJobManager manager = null;
            switch (type)
            {
                case MHNSyncType.ProspectMember:
                    manager = new ProspectFileManager(_prospectMemberRepository, _prospectRepository, _jwtHelperUtility);
                    break;
                case MHNSyncType.Member:
                    manager = new MemberFileManager(_prospectMemberRepository, _memberRepository, _jwtHelperUtility);
                    break;

                    //case MocSyncType.Moc_Audit_CoverageStartDate:
                    //    manager = new CoverageDateCheckingManager(_memberEnrollmentInfoRepository, _memberHraRepository);
                    //    break;
            }
            return manager;
        }
        private static void AutomaticProcess()
        {
            //List<DataSyncJobList> jobList =
            //    _dataSyncJobListRepository.GetActiveJobs(ApplicationConstants.ClientId,
            //        ApplicationConstants.CampaignId);
            //jobList = JobUtility.FindExecutableJobs(jobList);


            //Console.WriteLine("Total Jobs: " + jobList.Count);
            //foreach (var job in jobList)
            //{
            //    Console.WriteLine("Processing : " + job.SyncType + "...");

            //    IJobManager manager = GetInstance((MHNSyncType)Enum.Parse(typeof(MHNSyncType), job.SyncType));
            //    var results = manager.Execute();

            //    DataSyncReport report = new DataSyncReport()
            //    {
            //        CampaignId = ApplicationConstants.CampaignId,
            //        ClientId = ApplicationConstants.ClientId,
            //        CreatedOn = Helper.Helper.GetCurrentTimeInEST(),
            //        Message = results.Message.ToString(),
            //        Status = results.IsSuccess ? SyncStatus.SUCCESS.ToString() : SyncStatus.FAILED.ToString(),
            //        SyncType = job.SyncType,
            //        SyncExecutionProcess = SyncExecutionProcess.AUTO.ToString(),
            //        IsSearchedFileFound = results.IsSearchedFileFound,
            //        NoOfMatchedRecords = results.NoOfMatchedRecords ?? 0,
            //        NoOfMismatchedRecords = results.NoOfMismatchedRecords ?? 0,
            //        ProspectIds = results.ProspectIds,
            //        NoOfRecordsProcessed = results.NoOfRecordsProcessed ?? 0,

            //        NoOfSpecialData = results.NoOfSpecialData ?? 0,
            //        NoOfCorruptedData = results.NoOfCorruptedData ?? 0,
            //        NoOfValidData = ((results.NoOfRecordsProcessed ?? 0) - (results.NoOfCorruptedData ?? 0) -
            //                         (results.NoOfSpecialData ?? 0))
            //    };

            //    if (!results.IsSuccess)
            //    {
            //        // AutomatedFailureMailer(report);
            //    }

            //    _dataSyncReportRepository.Save(report);
            //    Console.WriteLine(job.SyncType + " ended with " + report.Status);
            //}
        }

        private static Dictionary<MHNSyncType, string> GetAllSyncTypes()
        {
            return new Dictionary<MHNSyncType, string>
            {
                { MHNSyncType.ProspectMember, "ProspectMember data Parse" },
                { MHNSyncType.Member, "Member data Parse" },
            };
        }
    }
}
