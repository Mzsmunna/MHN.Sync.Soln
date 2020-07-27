using Cerebro.JWTAuthService.Services.Helper;
using MHN.Sync.Entity;
using MHN.Sync.Entity.MongoEntity;
using MHN.Sync.Helper;
using MHN.Sync.Interface;
using MHN.Sync.Jobs;
using MHN.Sync.MongoInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Export.Soln.Managers.FileGenerate
{
    public class PTCFileManager : IJobManager
    {
        private JWTHelperUtility _jwtHelperUtility;
        private static IProspectMemberRepository _prospectMemberRepository;
        private static IPTCRequestRepository _ptcRequestRepository;

        public JobManagerResult Result;
        private static char delimiter;

        private static StringBuilder output = new StringBuilder();
        private static Stopwatch stopwatch = new Stopwatch();

        int processDataCount = 0;
        int totalDataCount = 0;

        //private TextReader fileReadableStream = null;
        //private SFTPUtility sFTPUtility;

        List<PTCRequest> ptcRequestList;

        //NewJob.DataProcessDelegate<PTCRequest> _dataProcessDelegate;
        NewJob.DataFetchDelegate _dataFetchDelegate;

        public PTCFileManager(IProspectMemberRepository prospectMemberRepository, IPTCRequestRepository ptcRequestRepository, JWTHelperUtility jWTHelperUtility)
        {
            _prospectMemberRepository = prospectMemberRepository;
            _ptcRequestRepository = ptcRequestRepository;
            _jwtHelperUtility = jWTHelperUtility;

            Result = NewJob.GetJobInstance();
            //sFTPUtility = new SFTPUtility(_jwtHelperUtility, ConstantType.MHN);

            ptcRequestList = new List<PTCRequest>();
            //_dataProcessDelegate = new NewJob.DataProcessDelegate<PTCRequest>(DataProcess);
            //_dataFetchDelegate = new NewJob.DataProcessDelegate(100,20);
        }
        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
            output.AppendLine(str);
        }
        public JobManagerResult Execute()
        {
            try
            {
                Result.Message.CustomAppender(Environment.NewLine + "PTC Request Export PDF Service started on " + HelperUtility.GetCurrentTimeInEST());
                //FtpsUtility.CustomInitialize(_jwtHelperUtility);
                stopwatch.Start();
                FileGenerate();
                stopwatch.Stop();
                WriteLine("Total Minutes : " + stopwatch.Elapsed.TotalMinutes);
                Result.Message.CustomAppender(Environment.NewLine + "PTC Request Export PDF Service ended on " + HelperUtility.GetCurrentTimeInEST());
                Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Message.CustomAppender("Exception Occured.");
                Result.Message.CustomAppender(ex.Message);
                if (!string.IsNullOrEmpty(ApplicationConstants.InstrumentationKey))
                {
                    //TelemetryLogger.LogException(ex);
                }
            }
            return Result;
        }

        private void FileGenerate()
        {
            ptcRequestList = _ptcRequestRepository.GetAll().Result;

            foreach(var ptcRequest in ptcRequestList)
            {

            }
        }
    }
}
