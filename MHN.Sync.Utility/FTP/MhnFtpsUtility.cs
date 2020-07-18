using Cerebro.JWTAuthService.Services.Helper;
using Cerebro.JWTAuthService.Services.Models;
using MHN.Sync.Entity;
using MHN.Sync.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MHN.Sync.Utility.FTP
{
    public static class MhnFtpsUtility
    {

        private static FTPCredential ftpCredential { get; set; }

        private static bool isEnableSsl = ApplicationConstants.Get<bool>(ConstantType.isEnableSsl);
        public static FTPCredential CustomInitialize(JWTHelperUtility utility)
        {
            var contentIdentifier = ApplicationConstants.Get<string>(ConstantType.ContentIdentifier);
            ftpCredential = utility.GetSFTPCred((ApplicationConstants.Get<bool>(ConstantType.TestEnvironment)
                ? $"{contentIdentifier}-ftp-dev"
                : $"{contentIdentifier}-ftp-live"));
            return ftpCredential;
        }

        //public static FTPCredential CustomInitialize2(JWTHelperUtility utility)
        //{
        //    var contentIdentifier = ApplicationConstants.Get<string>(ConstantType.ContentIdentifier2);
        //    ftpCredential2 = utility.GetSFTPCred((ApplicationConstants.Get<bool>(ConstantType.TestEnvironment) ? $"{contentIdentifier}-ftp-dev" : $"{contentIdentifier}-ftp-live"));
        //    return ftpCredential2;
        //}

        public static FTPCredential CustomInitialize(JWTHelperUtility utility, ConstantType ftpIdentifier)
        {
            var contentIdentifier = ftpIdentifier.ToString();
            ftpCredential = utility.GetSFTPCred((ApplicationConstants.Get<bool>(ConstantType.TestEnvironment) 
                            ? $"{contentIdentifier}-ftp-dev" : 
                            $"{contentIdentifier}-ftp-live"));
            return ftpCredential;
        }

        public static TextReader DownloadFile(string location, string fileName)
        {
            try
            {
                //Console.WriteLine("Attempting to download the file :" + fileName);
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + fileName);
                //request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);

                Console.WriteLine("Attempting to download the file :" + fileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpCredential.Host + location + fileName);
                request.Credentials = new NetworkCredential(ftpCredential.UserName, ftpCredential.Password);

                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = isEnableSsl;

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                string fileSaveLocation = String.Format(@"{0}\{1}\{2}", ApplicationConstants.WebJobsTempLocation, ApplicationConstants.WebJobsName, fileName);
                Console.WriteLine("File Save Location: " + fileSaveLocation);


                Console.WriteLine("Before Saving the file in Disk");
                //Save the stream in DISK
                using (var fileStream = new FileStream(fileSaveLocation, FileMode.Create, FileAccess.Write))
                {
                    Stream responseStream = response.GetResponseStream();
                    responseStream.CopyTo(fileStream);
                    Console.WriteLine("After Saving the file in Disk");
                }

                Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

                response.Close();

                TextReader reader = File.OpenText(fileSaveLocation);
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //method for Care Plan service
        public static string DownloadFileWithLocation(string location, string fileName)
        {
            try
            {
                //Console.WriteLine(Environment.NewLine + "Attempting to download the file : " + ApplicationConstants.SFTPHost + location + fileName);
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + fileName);
                //request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);

                Console.WriteLine(Environment.NewLine + "Attempting to download the file : " + ftpCredential.Host + location + fileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpCredential.Host + location + fileName);
                request.Credentials = new NetworkCredential(ftpCredential.UserName, ftpCredential.Password);

                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = isEnableSsl;

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                string fileSaveLocation = String.Format(@"{0}\{1}\{2}", ApplicationConstants.WebJobsTempLocation, ApplicationConstants.WebJobsName, fileName);

                Console.WriteLine("Save file location: " + fileSaveLocation);
                Console.WriteLine("Before Saving the file in Disk");
                //Save the stream in DISK
                using (var fileStream = new FileStream(fileSaveLocation, FileMode.Create, FileAccess.Write))
                {
                    Stream responseStream = response.GetResponseStream();
                    responseStream.CopyTo(fileStream);
                    Console.WriteLine("After Saving the file in Disk");
                }

                Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

                response.Close();
                return fileSaveLocation;
                //TextReader reader = File.OpenText(fileSaveLocation);
                //return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UploadFile(byte[] fileContents, string location, string fileName)
        {
            try
            {
                Console.WriteLine(Environment.NewLine + "Attempting to UploadFile the file : " + ftpCredential.Host +
                                  ", Location :" + location + ", Filename :" + fileName);
                var path = ftpCredential.Host + location + fileName;

                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(path);
                ftp.Credentials = new NetworkCredential(ftpCredential.UserName, ftpCredential.Password);
                ftp.UseBinary = true;
                ftp.UsePassive = true;
                ftp.EnableSsl = isEnableSsl;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.ContentLength = fileContents.Length;
                var requestStream = ftp.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.ToString());
                }
                throw ex;
            }
        }

        //public static bool UploadFile(byte[] fileContents, string location, string fileName, bool isUseFTPCredential)
        //{
        //    try
        //    {
        //        FTPCredential ftpCredentialTmp;

        //        if (isUseFTPCredential)
        //        {
        //            ftpCredentialTmp = ftpCredential;
        //        }
        //        else
        //        {
        //            ftpCredentialTmp = ftpCredential2;
        //        }

        //        Console.WriteLine(Environment.NewLine + "Attempting to UploadFile the file : " + ftpCredentialTmp.Host + ", Location :" + location + ", Filename :" + fileName);
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpCredentialTmp.Host + location + fileName);
        //        request.Credentials = new NetworkCredential(ftpCredentialTmp.UserName, ftpCredentialTmp.Password);
        //        request.UseBinary = true;
        //        request.UsePassive = true;
        //        request.EnableSsl = isEnableSsl;
        //        request.Method = WebRequestMethods.Ftp.UploadFile;
        //        request.ContentLength = fileContents.Length;


        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(fileContents, 0, fileContents.Length);
        //        requestStream.Close();

        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //        Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

        //        response.Close();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception " + ex);
        //        throw ex;
        //    }
        //}

        public static bool CreateDirectory(string location, string folderName)
        {
            try
            {
                //Console.WriteLine("Attempting to create directory : " + ApplicationConstants.SFTPHost + location + folderName);
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + folderName);
                //request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);
                Console.WriteLine("Attempting to create directory : " + ftpCredential.Host + location + folderName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpCredential.Host + location + folderName);
                request.Credentials = new NetworkCredential(ftpCredential.UserName, ftpCredential.Password);

                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = isEnableSsl;

                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Console.WriteLine("Create Folder Complete, status {0}", response.StatusDescription);

                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> DirectoryList(string fileName)
        {
            try
            {
                Console.WriteLine("Attempting to get directory List: " + "Host : " + ftpCredential.Host + " UserName : " + ftpCredential.UserName + " Password : " + ftpCredential.Password);
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + fileName);
                //request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpCredential.Host + fileName);
                request.Credentials = new NetworkCredential(ftpCredential.UserName, ftpCredential.Password);

                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = isEnableSsl;

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    //Console.WriteLine(line);

                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool FileExistsInFtps(string location, string fileName)
        {
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + fileName);
            //request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);
            Console.WriteLine("Checking File Exists In Ftp: " + ftpCredential.Host + location + fileName);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpCredential.Host + location + fileName);
            request.Credentials = new NetworkCredential(ftpCredential.UserName, ftpCredential.Password);

            request.UseBinary = true;
            request.UsePassive = true;
            request.EnableSsl = isEnableSsl;

            request.Method = WebRequestMethods.Ftp.GetFileSize;

            bool res = false;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Yes File Exists In Ftps");
                res = true;
                return res;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    res = false;
                    return res;
                }
            }
            return res;
        }

        public static Tuple<bool, string> FileExistsInDirectory(List<string> directoryList, string pattern)
        {
            Console.WriteLine("check File Exists In Directory: " + ftpCredential.Host);
            bool res = false;
            string fileName = "";
            foreach (var directory in directoryList)
            {
                res = directory.Contains(pattern);
                fileName = directory;
                if (res)
                {
                    break;
                }
            }

            return new Tuple<bool, string>(res, fileName);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
