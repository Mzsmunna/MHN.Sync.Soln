using Cerebro.JWTAuthService.Services.Helper;
using MHN.Export.Soln.Managers.FileGenerate;
using MHN.Sync.Entity;
using MHN.Sync.Entity.Enum;
using MHN.Sync.Interface;
using MHN.Sync.IOC.Module;
using MHN.Sync.MongoInterface;
//using MHN.Sync.Export.Managers.FileGenerate;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Export.Soln
{
    class Program
    {
        private static IKernel kernel;
        private static IProspectMemberRepository _prospectMemberRepository;
        private static IPTCRequestRepository _ptcRequestRepository;
        private static IEnrollmentRequestRepository _enrollmentRequestRepository;

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

            Console.WriteLine("Initiating MHN Data Export Jobs...");
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
            _ptcRequestRepository = kernel.Get<IPTCRequestRepository>();
            _enrollmentRequestRepository = kernel.Get<IEnrollmentRequestRepository>();

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

            var exportTypes = GetAllExportTypes();

            int listCount = 1;
            Console.WriteLine("** Select A Process To Run Manually:");
            foreach (var exportType in exportTypes)
            {
                Console.WriteLine(listCount + ". " + exportType.Value);
                listCount++;
            }

            Console.WriteLine("So Your Choice: ");

            var input = Console.ReadLine();

            listCount = 1;
            MHNExportType selectedExportType = new MHNExportType();
            bool inputTypeMatch = false;


            foreach (var exportType in exportTypes)
            {
                if (listCount.ToString() == input)
                {
                    inputTypeMatch = true;
                    selectedExportType = exportType.Key;
                }

                listCount++;
            }
            if (inputTypeMatch)
            {
                type = selectedExportType.ToString();
                manager = GetInstance(selectedExportType);
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
        private static IJobManager GetInstance(MHNExportType type, List<string> fileLocations = null)
        {
            IJobManager manager = null;
            switch (type)
            {
                case MHNExportType.EnrollmentExport:
                    manager = new EnrollmentFileManager(_prospectMemberRepository, _enrollmentRequestRepository, _jwtHelperUtility);
                    break;
                case MHNExportType.PTCExport:
                    manager = new PTCFileManager(_prospectMemberRepository, _ptcRequestRepository, _jwtHelperUtility);
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

        private static Dictionary<MHNExportType, string> GetAllExportTypes()
        {
            return new Dictionary<MHNExportType, string>
            {
                { MHNExportType.PTCExport, "PTC Form data Export to PDF" },
                { MHNExportType.EnrollmentExport, "Enrollment Form data Export to PDF" },
            };
        }
    }
}
