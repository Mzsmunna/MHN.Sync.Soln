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
        public delegate void DataProcessDelegate<T>(List<T> dataList) where T : class;
        public delegate void DataFetchDelegate(int currentPage, int pageSize);

        public static char delimiter = '\0';

        private static void SetDelimiter(string file)
        {
            if(delimiter.Equals('\0'))
            {
                string fitExt = file.Split('.').LastOrDefault();

                if (fitExt.ToLower().Equals("txt"))
                {
                    delimiter = '|';
                }
                else if (fitExt.ToLower().Equals("csv"))
                {
                    delimiter = ',';
                }
            }
        }

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
            TextReader fileReadableStream = AutomatedProcess(contentIdentifier, fileToSearch, Result, null);

            return fileReadableStream;
        }

        public static TextReader AutomatedProcess(string contentIdentifier, string fileToSearch, JobManagerResult Result, SFTPUtility sFTPUtility)
        {
            TextReader fileReadableStream = null;

            //fileToSearch = HelperUtility.GenerateFileName(MocSyncType.MOC_Careplan_Mailback);
            Result.Message.CustomAppender("Searching filename: " + fileToSearch);
            Result.FileLocations.Add(contentIdentifier + "\\" + fileToSearch);
            Result.FileNames.Add(fileToSearch);
            SetDelimiter(fileToSearch);

            if (sFTPUtility != null)
            {
                //var contentIdentifier = ApplicationConstants.Get<string>(ConstantType.MHN);
                Result.IsSearchedFileFound = sFTPUtility.FileExistsInFtps(contentIdentifier, fileToSearch);

                if (Result.IsSearchedFileFound.Value == true)
                    fileReadableStream = sFTPUtility.DownloadFile(contentIdentifier, fileToSearch);
                else
                    Result.Message.CustomAppender(String.Format("No file found to process. File Name:{0}, Date: {1}", fileToSearch, HelperUtility.GetCurrentTimeInEST()));
            }
            else
            {
                //var contentIdentifier = ApplicationConstants.Get<string>(MHNConstantType.ImportMHNContentLocation);
                Result.IsSearchedFileFound = FtpsUtility.FileExistsInFtps(contentIdentifier, fileToSearch);

                if (Result.IsSearchedFileFound.Value == true)
                    fileReadableStream = FtpsUtility.DownloadFile(contentIdentifier, fileToSearch);
                else
                    Result.Message.CustomAppender(String.Format("No file found to process. File Name:{0}, Date: {1}", fileToSearch, HelperUtility.GetCurrentTimeInEST()));
            }
              
            return fileReadableStream;
        }

        public static TextReader ManualProcess(JobManagerResult Result)
        {
            TextReader fileReadableStream = null;

            Console.WriteLine("Provide file location with filename: ");
            string location = Console.ReadLine();

            if (!string.IsNullOrEmpty(location))
            {
                SetDelimiter(location);
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

        #region CSV-With_Class_Map

        public static List<T1> ManualProcess<T1, T2>(JobManagerResult Result) where T1 : class
                                                                                where T2 : ClassMap<T1>
        {
            TextReader fileReadableStream = ManualProcess(Result);
            List<T1> dataList = CsvUtility.ReadDataFromTextReader<T1, T2>(fileReadableStream, delimiter, true);

            return dataList;
        }

        public static void DataProcessWithTask<T1, T2>(TextReader fileReadableStream, DataProcessDelegate<T1> DataProcessDelegate) where T1 : class
                                                                                        where T2 : ClassMap<T1>
        {
            List<T1> dataList = CsvUtility.ReadDataFromTextReader<T1, T2>(fileReadableStream, delimiter, true);

            DataProcessWithTask<T1>(dataList, DataProcessDelegate);
        }

        #endregion

        #region CSV-Without_Class_Map

        public static List<T> ManualProcess<T>(JobManagerResult Result) where T : class
        {
            TextReader fileReadableStream = ManualProcess(Result);
            List<T> dataList = CsvUtility.ReadDataFromTextReader<T>(fileReadableStream, delimiter, true);

            return dataList;
        }

        public static void DataProcessWithTask<T>(TextReader fileReadableStream, DataProcessDelegate<T> DataProcessDelegate) where T : class
        {
            List<T> dataList = CsvUtility.ReadDataFromTextReader<T>(fileReadableStream, delimiter, true);

            DataProcessWithTask<T>(dataList, DataProcessDelegate);
        }

        #endregion

        public static void DataProcessWithTask<T>(List<T> fullDataList, DataProcessDelegate<T> DataProcessDelegate) where T : class
        {
            if(fullDataList.Count > 0 && DataProcessDelegate != null)
            {
                //int loop = fullDataList.Count / 100;

                //Task[] tasks = new Task[loop + 1];

                //for (var i = 0; i <= loop; i++)
                //{
                //    var taskList = fullDataList.GetRange(loop * i, loop);

                //    if (i == loop)
                //    {
                //        int lastCount = fullDataList.Count - loop * i;
                //        taskList = fullDataList.GetRange(loop * i, lastCount);
                //    }

                //    var task = Task.Factory.StartNew(() => { DataProcessDelegate(taskList); });

                //    tasks[i] = task;
                //}

                //Task.WaitAll(tasks);

                int taskDivide = fullDataList.Count / 10;
                var task1List = fullDataList.GetRange(taskDivide * 0, taskDivide);
                var task2List = fullDataList.GetRange(taskDivide * 1, taskDivide);
                var task3List = fullDataList.GetRange(taskDivide * 2, taskDivide);
                var task4List = fullDataList.GetRange(taskDivide * 3, taskDivide);
                var task5List = fullDataList.GetRange(taskDivide * 4, taskDivide);
                var task6List = fullDataList.GetRange(taskDivide * 5, taskDivide);
                var task7List = fullDataList.GetRange(taskDivide * 6, taskDivide);
                var task8List = fullDataList.GetRange(taskDivide * 7, taskDivide);
                var task9List = fullDataList.GetRange(taskDivide * 8, taskDivide);
                int lastTaskCount = fullDataList.Count - taskDivide * 9;
                var task10List = fullDataList.GetRange(taskDivide * 9, lastTaskCount);

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
            else if(fullDataList.Count <= 0)
            {
                Console.WriteLine("No data available to process");
            }
            else if(DataProcessDelegate == null)
            {
                Console.WriteLine("Delegate Method reference is null");
            }
        }

        public static void DataFetchWithTask(int dbCount, DataFetchDelegate DataFetchDelegate)
        {
            Console.WriteLine("\rStart getting data count....");
            //dbCount = memberFulFillmentRepository.CountLastSSBCIMailbackReceiveMembers(ApplicationConstants.ClientId, ApplicationConstants.SSBCIGatewayDileDayRange).Result;
            //result.Message.CustomAppender("Total Data Count : " + dbCount);
            if (dbCount > 0)
            {
                //int pageSize = Convert.ToInt32(ApplicationConstants.DataPullPerRequest);
                //int pageNumber = 0;

                //int loop = dbCount / pageSize;

                //Task[] tasks = new Task[loop + 1];

                //for (var i = 0; i <= loop; i++)
                //{
                //    var task = Task.Factory.StartNew(() => { DataFetchDelegate(pageNumber++, pageSize); });
                //    tasks[i] = task;
                //}

                //Task.WaitAll(tasks);

                int taskDivide = dbCount / 10;
                var task1 = Task.Factory.StartNew(() => { DataFetchDelegate(0, taskDivide); });
                var task2 = Task.Factory.StartNew(() => { DataFetchDelegate(1, taskDivide); });
                var task3 = Task.Factory.StartNew(() => { DataFetchDelegate(2, taskDivide); });
                var task4 = Task.Factory.StartNew(() => { DataFetchDelegate(3, taskDivide); });
                var task5 = Task.Factory.StartNew(() => { DataFetchDelegate(4, taskDivide); });
                var task6 = Task.Factory.StartNew(() => { DataFetchDelegate(5, taskDivide); });
                var task7 = Task.Factory.StartNew(() => { DataFetchDelegate(6, taskDivide); });
                var task8 = Task.Factory.StartNew(() => { DataFetchDelegate(7, taskDivide); });
                var task9 = Task.Factory.StartNew(() => { DataFetchDelegate(8, taskDivide); });
                var task10 = Task.Factory.StartNew(() => { DataFetchDelegate(9, taskDivide); });
                var task11 = Task.Factory.StartNew(() => { DataFetchDelegate(10, taskDivide); });

                Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11);

            }
            //result.Message.CustomAppender("Total Data Found : " + memberFulFillments.Count);
        }
    }
}
