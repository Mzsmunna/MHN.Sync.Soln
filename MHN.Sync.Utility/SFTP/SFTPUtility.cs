using Cerebro.JWTAuthService.Services.Models;
using Cerebro.JWTAuthService.Services.Helper;
using MHN.Sync.Entity;
using MHN.Sync.Entity.Enum;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
//using System.Windows.Forms;

namespace MHN.Sync.Utility.SFTP
{
    public class SFTPUtility
    {
        private static ConnectionInfo conncectionInfo = null;

        public SFTPUtility(JWTHelperUtility utility, ConstantType ftpIdentifier)
        {
            var contentIdentifier = ftpIdentifier.ToString();
            var FTPCredential = utility.GetSFTPCred((ApplicationConstants.Get<bool>(ConstantType.TestEnvironment) ? $"{contentIdentifier}-ftp-dev" : $"{contentIdentifier}-ftp-live"));
            conncectionInfo = new ConnectionInfo(FTPCredential.Host, Convert.ToInt32(FTPCredential.Port), FTPCredential.UserName,
            new AuthenticationMethod[]{
                new PasswordAuthenticationMethod(FTPCredential.UserName, FTPCredential.Password)
            });
        }

        public SFTPUtility(JWTHelperUtility utility, string ftpIdentifier)
        {
            var contentIdentifier = ftpIdentifier.ToString();
            var FTPCredential = utility.GetSFTPCred((ApplicationConstants.Get<bool>(ConstantType.TestEnvironment) ? $"{contentIdentifier}-ftp-dev" : $"{contentIdentifier}-ftp-live"));
            conncectionInfo = new ConnectionInfo(FTPCredential.Host, Convert.ToInt32(FTPCredential.Port), FTPCredential.UserName,
            new AuthenticationMethod[]{
                new PasswordAuthenticationMethod(FTPCredential.UserName, FTPCredential.Password)
            });
        }

        public bool FileExistsInFtps(string downloadLocation, string fileName)
        {
            Console.WriteLine(Environment.NewLine + "Getting file : " + conncectionInfo.Host + downloadLocation + fileName + Environment.NewLine);
            bool isFileFound = false;
            try
            {
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();
                    isFileFound = sftp.Exists(downloadLocation + fileName);

                    if (isFileFound)
                    {
                        Console.WriteLine(fileName + " File found");
                    }
                    else
                    {
                        Console.WriteLine(fileName + " File Not found");
                    }

                    sftp.Disconnect();
                    return isFileFound;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TextReader DownloadFile(string location, string fileName)
        {
            try
            {
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();
                    string fileSaveLocation = string.Format(@"{0}\{1}", ApplicationConstants.ExportFilePath, fileName);
                    Console.WriteLine("File Save Location: " + fileSaveLocation);

                    using (var fs = new FileStream(fileSaveLocation, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        sftp.DownloadFile(location + fileName, fs);
                        Console.WriteLine("Download Complete");
                        fs.Flush();
                        fs.Close();
                        fs.Dispose();
                    }

                    sftp.Disconnect();
                    TextReader reader = File.OpenText(fileSaveLocation);
                    return reader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UploadFile(byte[] fileContents, string uploadLocation, string filename)
        {
            try
            {
                Console.WriteLine("Uploading File Into : " + uploadLocation + filename);
                var fileStream = new MemoryStream(fileContents);
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();
                    sftp.UploadFile(fileStream, uploadLocation + filename);

                    sftp.Disconnect();
                }
                Console.WriteLine("Upload File Complete, status");
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }


        public MemoryStream DownloadAsMemoryStream(string downloadLocation, string fileName)
        {
            string relativePath = downloadLocation + fileName;
            try
            {
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();

                    using (var memoryStream = new MemoryStream())
                    {
                        sftp.DownloadFile(relativePath, memoryStream);

                        sftp.Disconnect();
                        return memoryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DownloadAsString(string downloadLocation, string fileName)
        {
            string relativePath = downloadLocation + fileName;
            try
            {
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();

                    var files = sftp.ListDirectory(downloadLocation);

                    foreach (var file in files)
                    {
                        if (file.Name.Equals(fileName))
                        {
                            return sftp.ReadAllText(file.FullName);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
