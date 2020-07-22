using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure; //Namespace for CloudConfigurationManager

namespace MHN.Sync.AzureBlobRepository
{
    public static class AzureBlobRepo
    {
        public static void UploadBlob(string connectionStringName, string containerName, string fileLocation, string fileName, string folderName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                
                var directory = container.GetDirectoryReference(folderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);

                using (var fileStream = File.OpenRead(fileLocation))
                {
                    blockBlob.UploadFromStream(fileStream);
                    Console.WriteLine("Successfully Uploaded : " + fileName + " in " + containerName + "/" + folderName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static void UploadBlob(byte[] data, string connectionStringName, string blobContainerName, string blobFolderName, string fileName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(blobContainerName);
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                var directory = container.GetDirectoryReference(blobFolderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);
                using (Stream stream = new MemoryStream(data))
                {
                    blockBlob.UploadFromStream(stream);
                    Console.WriteLine("Successfully uploaded file : " + fileName + " in " + connectionStringName + "/" + blobContainerName + "/" + blobFolderName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static List<string> ListCloudBlockBlob(string connectionStringName, string containerName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                var blobs = new List<string>();
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;

                        blobs.Add(string.Format("{0}", blob.Name));

                    }
                }
                return blobs;
            }
            catch
            {
                throw;
            }
        }
        public static List<string> ListBlobs(string connectionStringName, string containerName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                var blobs = new List<string>();
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;

                        blobs.Add(string.Format("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri));

                    }
                    else if (item.GetType() == typeof(CloudPageBlob))
                    {
                        CloudPageBlob pageBlob = (CloudPageBlob)item;

                        blobs.Add(string.Format("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri));

                    }
                    else if (item.GetType() == typeof(CloudBlobDirectory))
                    {
                        CloudBlobDirectory directory = (CloudBlobDirectory)item;

                        blobs.Add(string.Format("Directory: {0}", directory.Uri));
                    }
                }
                return blobs;
            }
            catch
            {
                throw;
            }
        }
        public static void PrintBlobs(string connectionStringName, string containerName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                BlobContinuationToken blobContinuationToken = null;

                do
                {
                    var results = container.ListBlobsSegmented(null, false, BlobListingDetails.None, 50, blobContinuationToken, null, null);
                    // Get the value of the continuation token returned by the listing call.
                    blobContinuationToken = results.ContinuationToken;
                    foreach (IListBlobItem item in results.Results)
                    {
                        Console.WriteLine(item.Uri);
                    }
                } while (blobContinuationToken != null); // Loop while the continuation token is not null. 

            }
            catch
            {
                throw;
            }
        }
        public static bool DownloadBlob(string connectionStringName, string containerName, string folderName, string fileName, string localFileSaveLocation)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                var directory = container.GetDirectoryReference(folderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);
                blockBlob.DownloadToFile(localFileSaveLocation, FileMode.CreateNew);
                if (File.Exists(localFileSaveLocation))
                {
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public static byte[] DownloadAsByteArray(string connectionStringName, string containerName, string folderName, string fileName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlobDirectory directory = container.GetDirectoryReference(folderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);

                if (blockBlob.Exists())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        blockBlob.DownloadToStream(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Message: " + exception.Message);
                Console.WriteLine("Trace: " + exception.StackTrace);
                return null;
            }
        }
        public static bool DownloadFromBlob(string connectionStringName, string containerName, string folderName, string fileName, string localFileSaveLocation)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlobDirectory directory = container.GetDirectoryReference(folderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);

                var fileNameWithLocation = localFileSaveLocation + fileName;

                if (blockBlob.Exists())
                {
                    blockBlob.DownloadToFile(fileNameWithLocation, FileMode.Create);
                    if (File.Exists(fileNameWithLocation))
                    {
                        Console.WriteLine("File Downloaded From {0}", blockBlob.Uri.AbsoluteUri);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("File Downloaded Failed");
                    }
                }
                return false;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static bool IsFileExists(string connectionStringName, string containerName, string folderName, string fileName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlobDirectory directory = container.GetDirectoryReference(folderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);

                return blockBlob.Exists();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Message: " + exception.Message);
                Console.WriteLine("Trace: " + exception.StackTrace);
                return false;
            }            
        }

        public static void UploadBlobV2(string connectionStringName, string containerName, string fileLocation, string fileName, string folderName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringName));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                var directory = container.GetDirectoryReference(folderName);
                CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);

                using (var fileStream = File.Open(fileLocation, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    blockBlob.UploadFromStream(fileStream);
                    Console.WriteLine("Successfully Uploaded : " + fileName + " in " + containerName + "/" + folderName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
