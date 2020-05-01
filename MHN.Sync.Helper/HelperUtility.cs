using MHN.Sync.Entity;
using MHN.Sync.Utility.SFTP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MHN.Sync.Helper
{
    public static class HelperUtility
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static string GenerateSubmitId(string type)
        {
            string submitId = string.Empty;

            while (submitId.Length != 8)
            {
                Thread.Sleep(1);
                submitId = (DateTime.UtcNow.Subtract(new DateTime(2000, 1, 1))).TotalSeconds.GetHashCode().ToString("X");
            }
            return submitId;
        }

        public static String GenerateEmailToClientCintentName()
        {
            String fileName = string.Empty;
            DateTime currentDate = GetCurrentTimeInEST();

            fileName = String.Format("EmailToClientContent_{0}.txt", currentDate.ToString("yyyy_MM_dd"));

            return fileName;
        }

        public static Boolean HasSpecialCharacter(String stringToMatch)
        {
            return !Regex.IsMatch(stringToMatch, @"^[a-z A-Z 0-9 \s]+$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.CultureInvariant);
        }

        public static string CleanText(String stringToclean)
        {
            return stringToclean.Trim();
        }

        public static DateTime? GetDateFromString(string date)
        {
            DateTime parsed;
            if (DateTime.TryParse(date, out parsed))
            {
                parsed = DateTime.Parse(date, new CultureInfo("en-US", true));
                return parsed;
            }
            else
            {
                // Log error, perhaps?
                return null;
            }
        }

        public static DateTime GetCurrentTimeInEST()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
        }

        public static string GetReportingPropertyValueByKey(List<Field> reportingProperties, string key)
        {
            foreach (var item in reportingProperties)
            {
                if (item.Key == key)
                {
                    return item.Values;
                }
            }
            return "";
        }

        public static bool UploadFilesToGtwFtp(byte[] data, string fileLocation, string fileName)
        {
            bool isSuccess;
            if (ApplicationConstants.ManualProcess)
            {
                using (Stream stream = new MemoryStream(data))
                {
                    string location = ApplicationConstants.ExportFilePath;
                    using (var fileStream = File.Create(location + "/" + fileName))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }
                }
                isSuccess = true;
            }
            else
            {
                using (Stream stream = new MemoryStream(data))
                {
                    //isSuccess = Sync.Helper.GtwFtpsUtility.UploadFile(data, fileLocation, fileName);
                    isSuccess = false;
                }
            }
            Console.WriteLine($"File uploaded successfully through { (ApplicationConstants.ManualProcess ? "Manual Process" : "Automatic Process")}");
            return isSuccess;
        }

        public static bool UploadFilesToInsightinSFTP(byte[] data, string fileLocation, string fileName, SFTPUtility sFTPUtility)
        {
            bool isSuccess = false;
            if (ApplicationConstants.ManualProcess)
            {
                using (Stream stream = new MemoryStream(data))
                {
                    //Console.WriteLine("Where do you want to export this file? ");
                    //string location = Console.ReadLine();

                    string location = ApplicationConstants.ExportFilePath;

                    using (var fileStream = File.Create(location + "/" + fileName))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }
                }
                isSuccess = true;
            }
            else
            {
                using (Stream stream = new MemoryStream(data))
                {
                    isSuccess = sFTPUtility.UploadFile(data, fileLocation, fileName);
                }
            }
            Console.WriteLine($"File uploaded successfully through { (ApplicationConstants.ManualProcess ? "Manual Process" : "Automatic Process")}");
            return isSuccess;
        }

        public static DateTime[] GetStartEndDate(DateTime dateTime, bool isMongo)
        {
            DateTime startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
            DateTime endDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
            if (isMongo)
            {
                return new[]
                {
                    TimeZoneInfo.ConvertTimeToUtc(startDate).AddHours(-4),
                    TimeZoneInfo.ConvertTimeToUtc(endDate).AddHours(-4)
                };
            }
            else
            {
                return new[]
                {
                    startDate,
                    endDate
                };
            }
        }

        public static DateTime[] GetPreviousDayDate()
        {
            DateTime start = DateTime.UtcNow.AddDays(-1);

            DateTime previousStartDate = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            DateTime previousEndDate = new DateTime(start.Year, start.Month, start.Day, 23, 59, 59);

            DateTime[] dateTimes = { previousStartDate, previousEndDate };
            return dateTimes;
        }

        public static DateTime[] GetCurrentYearStartAndEndDate(bool isMongo)
        {
            DateTime startDate = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59);

            if (isMongo)
            {
                return new[]
                {
                    TimeZoneInfo.ConvertTimeToUtc(startDate).AddHours(-5),
                    TimeZoneInfo.ConvertTimeToUtc(endDate).AddHours(-5)
                };
            }
            else
            {
                return new[]
                {
                    startDate,
                    endDate
                };
            }
        }
        public static string GetReportingPropertiesValue(List<Field> fields, string key)
        {
            foreach (var field in fields)
            {
                if (key == field.Key)
                    return field.Values;
            }
            return "";
        }

        public static void LogExport(List<string> items, string fileName)
        {
            StringBuilder messages = new StringBuilder();
            items.ForEach(item => messages.AppendLine(item));
            var location = Path.Combine(ApplicationConstants.ExportFilePath + "/" + fileName);
            File.AppendAllText(location, messages.ToString());
            messages.Clear();
        }

        public static string GetSortedStringForCommaSeparatedNumbers(string list)
        {
            if (!string.IsNullOrEmpty(list))
            {
                var splitedString = list.Split(',');
                int[] response = new int[0];
                response = splitedString.Select(i => int.Parse(i)).ToArray();
                Array.Sort(response);
                return string.Join(",", response.Select(element => element.ToString()));
            }
            return list;
        }
        public static void PrintAndAddToResult(JobManagerResult result, string message)
        {
            Console.WriteLine(message);
            result.Message.Append(message);
        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate);
        }
    }
}
