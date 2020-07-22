using CsvHelper.Configuration;
using MHN.Sync.AzureBlobRepository;
using MHN.Sync.Entity;
using MHN.Sync.Utility.CSV;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Utility.Blob
{
    public static class BlobUtility
    {
        public static void DataListUpload<T>(List<T> dataList, string connectionString, string blobContainerName, string blobFolderName, string fileName, char delimiter = ',', bool hasHeaderRecord = true) where T : class
        {
            try
            {
                if (ApplicationConstants.UploadToAzureBlob)
                {
                    if(dataList != null)
                    {
                        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
                        {
                            HasHeaderRecord = hasHeaderRecord
                        };

                        byte[] data = CsvUtility.ConvertListToBytes(dataList,delimiter,config);
                        AzureBlobRepo.UploadBlob(data, connectionString, blobContainerName, blobFolderName, fileName);
                    }
                    else
                    {
                        Console.WriteLine("List is Empty");
                    }
                    
                }
                else
                {
                    Console.WriteLine("Configuration is disable to upload into Azure Blob!");
                }
            }
            catch (Exception ex) { };
        }

        public static void DataStreamUpload(MemoryStream memoryStream, string connectionStringName, string blobContainerName, string blobFolderName, string fileName)
        {
            try
            {
                if (ApplicationConstants.UploadToAzureBlob)
                {
                    byte[] data = memoryStream.ToArray();
                    AzureBlobRepo.UploadBlob(data, connectionStringName, blobContainerName, blobFolderName, fileName);
                }
                else
                {
                    Console.WriteLine("Configuration Error: file didn't upload to Azure Blob!");
                }

            }
            catch (Exception ex) { };
        }


        public static bool IsFileExist(string connectionStringName, string blobContainerName, string blobFolderName, string fileName)
        {
            try
            {
                return AzureBlobRepo.IsFileExists(connectionStringName, blobContainerName, blobFolderName, fileName);
            }
            catch (Exception ex) { };
            return false;
        }
    }
}
