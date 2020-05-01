using MHN.Sync.Entity;
using MHN.Sync.Helper;
using MHN.Sync.Utility.FTP;
using MHN.Sync.Utility.SFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Jobs
{
    public static class NewJob
    {
        public static JobManagerResult GetJobInstance()
        {
            JobManagerResult Result = new JobManagerResult();

            Result.NoOfRecordsProcessed = 0;
            Result.NoOfCorruptedData = 0;
            Result.NoOfMatchedRecords = 0;
            Result.NoOfMismatchedRecords = 0;
            Result.NoOfSpecialData = 0;
            Result.NoOfValidData = 0;

            Result.IsSuccess = false;

            return Result;
        }

        public static TextReader AutomatedProcess(string contentIdentifier, string fileToSearch, JobManagerResult Result)
        {
            TextReader fileReadableStream = null;
            //fileToSearch = HelperUtility.GenerateFileName(MocSyncType.MOC_DoNotCall);
            Result.Message.CustomAppender("Searching filename: " + fileToSearch);
            Result.FileLocations.Add(contentIdentifier + "\\" + fileToSearch);
            Result.FileNames.Add(fileToSearch);
            //var contentIdentifier = ApplicationConstants.Get<string>(MHNConstantType.ImportMHNContentLocation);
            Result.IsSearchedFileFound = FtpsUtility.FileExistsInFtps(contentIdentifier, fileToSearch);

            if (Result.IsSearchedFileFound.Value == true)
            {
                fileReadableStream = FtpsUtility.DownloadFile(contentIdentifier, fileToSearch);
            }
            else
                Result.Message.CustomAppender(String.Format("No file found to process. File Name:{0}, Date: {1}", fileToSearch, HelperUtility.GetCurrentTimeInEST()));

            return fileReadableStream;
        }

        public static TextReader AutomatedProcess(string contentIdentifier, string fileToSearch, JobManagerResult Result, SFTPUtility sFTPUtility)
        {
            TextReader fileReadableStream = null;
            //fileToSearch = HelperUtility.GenerateFileName(MocSyncType.MOC_Careplan_Mailback);
            Result.Message.CustomAppender("Searching filename: " + fileToSearch);
            Result.FileLocations.Add(contentIdentifier + "\\" + fileToSearch);
            Result.FileNames.Add(fileToSearch);
            //var contentIdentifier = ApplicationConstants.Get<string>(ConstantType.MHN);
            Result.IsSearchedFileFound = sFTPUtility.FileExistsInFtps(contentIdentifier, fileToSearch);

            if (Result.IsSearchedFileFound.Value == true)
            {
                fileReadableStream = sFTPUtility.DownloadFile(contentIdentifier, fileToSearch);
            }
            else
                Result.Message.CustomAppender(String.Format("No file found to process. File Name:{0}, Date: {1}", fileToSearch, HelperUtility.GetCurrentTimeInEST()));
                
            return fileReadableStream;
        }

        public static TextReader ManualProcess(JobManagerResult Result)
        {
            TextReader fileReadableStream = null;

            Console.WriteLine("Provide file location with filename: ");
            string location = Console.ReadLine();

            if (!string.IsNullOrEmpty(location))
            {
                Result.FileLocations.Add(location);
                Result.FileNames.Add(location.Split('\\').LastOrDefault());
                Result.IsSearchedFileFound = true;
                var fileStream = File.ReadAllText(location);
                fileReadableStream = new StringReader(fileStream);
            }
            else
            {
                Result.IsSuccess = false;
                Result.Message.CustomAppender("No File Found");
            }

            return fileReadableStream;
        }
    }
}
