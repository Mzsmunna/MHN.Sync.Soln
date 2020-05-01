using CsvHelper.Configuration;
using MHN.Sync.Entity;
using MHN.Sync.Helper;
using MHN.Sync.Utility.CSV;
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
        //public delegate void StreamDataProcessDelegate(TextReader fileReadableStream);
        public delegate void ListDataProcessDelegate<T>(List<T> dataList) where T : class;

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

        public static List<T1> ManualProcess<T1, T2>(JobManagerResult Result) where T1 : class
                                                                                where T2 : ClassMap<T1>
        {
            TextReader fileReadableStream = ManualProcess(Result);
            List<T1> dataList = CsvUtility.ReadDataFromTextReader<T1, T2>(fileReadableStream, ',', true);

            return dataList;
        }

        public static void DataProcessWithTask<T1, T2>(TextReader fileReadableStream) where T1 : class
                                                                                        where T2 : ClassMap<T1>
        {
            List<T1> dataList = CsvUtility.ReadDataFromTextReader<T1, T2>(fileReadableStream, ',', true);

            //DataProcessWithTask<T1>(dataList);
        }

        public static void DataProcessWithTask<T>(List<T> dataList, ListDataProcessDelegate<T> DataProcessDelegate) where T : class
        {
            int taskDivide = dataList.Count / 10;
            var task1List = dataList.GetRange(taskDivide * 0, taskDivide);
            var task2List = dataList.GetRange(taskDivide * 1, taskDivide);
            var task3List = dataList.GetRange(taskDivide * 2, taskDivide);
            var task4List = dataList.GetRange(taskDivide * 3, taskDivide);
            var task5List = dataList.GetRange(taskDivide * 4, taskDivide);
            var task6List = dataList.GetRange(taskDivide * 5, taskDivide);
            var task7List = dataList.GetRange(taskDivide * 6, taskDivide);
            var task8List = dataList.GetRange(taskDivide * 7, taskDivide);
            var task9List = dataList.GetRange(taskDivide * 8, taskDivide);
            int lastTaskCount = dataList.Count - taskDivide * 9;
            var task10List = dataList.GetRange(taskDivide * 9, lastTaskCount);

            var task1 = Task.Factory.StartNew(() => { DataProcessDelegate(task1List); });
            var task2 = Task.Factory.StartNew(() => { DataProcessDelegate(task2List); });
            var task3 = Task.Factory.StartNew(() => { DataProcessDelegate(task3List); });
            var task4 = Task.Factory.StartNew(() => { DataProcessDelegate(task4List); });
            var task5 = Task.Factory.StartNew(() => { DataProcessDelegate(task5List); });
            var task6 = Task.Factory.StartNew(() => { DataProcessDelegate(task6List); });
            var task7 = Task.Factory.StartNew(() => { DataProcessDelegate(task7List); });
            var task8 = Task.Factory.StartNew(() => { DataProcessDelegate(task8List); });
            var task9 = Task.Factory.StartNew(() => { DataProcessDelegate(task9List); });
            var task10 = Task.Factory.StartNew(() => { DataProcessDelegate(task10List); });

            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
        }
    }
}
