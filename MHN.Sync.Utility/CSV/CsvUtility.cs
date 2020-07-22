using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Utility.CSV
{
    public static class CsvUtility
    {
        public static char Delimiter = '\t';

        private static void SetDelimiter (char delimiter)
        {
            if(!string.IsNullOrEmpty(delimiter.ToString()))
            {
                Delimiter = delimiter;
            }
        }

        #region With_Class_Map

        public static List<T1> ReadDataFromFile<T1, T2>(string fileLocation, char delimiter = ',', bool validateHeader = true) where T1 : class //new()
                                                                            where T2 : ClassMap<T1>//, new()
        {
            List<T1> csvResult = new List<T1>();

            using (var reader = new StreamReader(fileLocation))
            {
                csvResult = ReadDataFromStream<T1, T2>(reader, delimiter, validateHeader);
            }

            //var fileStream = File.ReadAllText(fileLocation);
            //using (TextReader reader = new StringReader(fileStream))
            //{
            //    csvResult = ReadDataFromTextReader<T1, T2>(reader, delimiter, validateHeader);
            //}

            return csvResult;
        }

        public static List<T1> ReadDataFromStream<T1, T2>(StreamReader reader, char delimiter = ',', bool validateHeader = true) where T1 : class //new()
                                                                            where T2 : ClassMap<T1>//, new()
        {
            List<T1> csvResult = ReadDataFromTextReader<T1, T2>(reader, delimiter, validateHeader);
            return csvResult;
        }

        public static List<T1> ReadDataFromTextReader<T1, T2>(TextReader reader, char delimiter = ',', bool validateHeader = true) where T1 : class //new()
                                                                            where T2 : ClassMap<T1>//, new()
        {
            SetDelimiter(delimiter);

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<T2>();
                csv.Configuration.Delimiter = Delimiter.ToString();

                if(!validateHeader)
                {
                    csv.Configuration.HeaderValidated = null;
                }

                csv.Configuration.MissingFieldFound = null;
                //csv.Configuration.BadDataFound = null;
                List<T1> csvResult = csv.GetRecords<T1>().ToList();

                return csvResult;
            }
        }

        #endregion

        #region Without_Class_Map

        public static List<T> ReadDataFromFile<T>(string fileLocation, char delimiter = ',', bool validateHeader = true) where T : class //new()                                                                           
        {
            List<T> csvResult = new List<T>();

            using (var reader = new StreamReader(fileLocation))
            {
                csvResult = ReadDataFromStream<T>(reader, delimiter, validateHeader);
            }

            //var fileStream = File.ReadAllText(fileLocation);
            //using (TextReader reader = new StringReader(fileStream))
            //{
            //    csvResult = ReadDataFromTextReader<T>(reader, delimiter, validateHeader);
            //}

            return csvResult;
        }

        public static List<T> ReadDataFromStream<T>(StreamReader reader, char delimiter = ',', bool validateHeader = true) where T : class //new()
        {
            List<T> csvResult = ReadDataFromTextReader<T>(reader, delimiter, validateHeader);
            return csvResult;
        }

        public static List<T> ReadDataFromTextReader<T>(TextReader reader, char delimiter = ',', bool validateHeader = true) where T : class //new()
        {
            SetDelimiter(delimiter);

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Delimiter.ToString();

                if (!validateHeader)
                {
                    csv.Configuration.HeaderValidated = null;
                }

                csv.Configuration.MissingFieldFound = null;
                //csv.Configuration.BadDataFound = null;
                List<T> csvResult = csv.GetRecords<T>().ToList();

                return csvResult;
            }
        }

        #endregion

        public static bool WriteDataToFile<T>(List<T> dataList, string fileLocation, char delimiter = ',') where T : class
        {
            SetDelimiter(delimiter);

            using (var writer = new StreamWriter(fileLocation))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = Delimiter.ToString();
                csv.WriteRecords(dataList);
                return true;
            }
        }

        public static bool AppendManyDataToFile<T>(List<T> dataList, string fileLocation, char delimiter) where T : class
        {
            SetDelimiter(delimiter);

            // Do not include the header row if the file already exists
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = !File.Exists(fileLocation)
            };

            // WARNING: This will throw an error if the file is open in Excel!
            using (FileStream fileStream = new FileStream(fileLocation, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    using (var csv = new CsvWriter(streamWriter, csvConfig))
                    {
                        csv.Configuration.Delimiter = Delimiter.ToString();
                        // Append records to csv OR txt
                        csv.WriteRecords(dataList);
                        return true;
                    }
                }
            }
        }

        public static bool AppendDataToFile<T>(T data, string filePath, char delimiter) where T : class
        {
            var dataList = new List<T>();

            dataList.Add(data);

            var isSuccess = AppendManyDataToFile<T>(dataList, filePath, delimiter);

            return isSuccess;
        }

        // List To Byte Conversion
        public static byte[] ConvertListToBytes<T>(List<T> dataList, char delimiter = ',', CsvConfiguration configuration = null) where T : class
        {
            SetDelimiter(delimiter);

            if (configuration == null)
                configuration = new CsvConfiguration(CultureInfo.CurrentCulture);

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.Unicode))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, configuration))
                    {
                        csvWriter.Configuration.Delimiter = Delimiter.ToString();
                        csvWriter.WriteRecords(dataList);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
