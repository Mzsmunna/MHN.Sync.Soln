using MHN.Sync.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHN.Sync.Entity
{
    public class ApplicationConstants
    {
        private ApplicationConstants() { }

        public static string ConnectionString
        {
            get
            {
                string key = "ConnectionString";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DatabaseName
        {
            get
            {
                string key = "DatabaseName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DatabaseContextContainer
        {
            get
            {
                string key = "DatabaseContextContainer";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SSOUserFileName
        {
            get
            {
                string key = "SSOUserFileName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static int SQLServerCommandTimeout
        {
            get
            {
                string key = "SQLServerCommandTimeout";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings[key]);
                }
                else
                {
                    return 600;
                }
            }
        }

        public static int NumberOfRecordsProcessPerPass
        {
            get
            {
                string key = "NumberOfRecordsProcessPerPass";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings[key]);
                }
                else
                {
                    return 1000;
                }
            }
        }

        public static bool EnableSsl
        {
            get
            {
                string key = "EnableSsl";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool TruncateMemberTable
        {
            get
            {
                string key = "TruncateMemberTable";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool TruncateProspectTable
        {
            get
            {
                string key = "TruncateProspectTable";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DropMemberCollection
        {
            get
            {
                string key = "DropMemberCollection";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }

        public static string LocalExportedFileLocation
        {
            get
            {
                string key = "LocalExportedFileLocation";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string LocalImportedFileLocation
        {
            get
            {
                string key = "LocalImportedFileLocation";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SFTPUploadFileLocation
        {
            get
            {
                string key = "SFTPUploadFileLocation";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SFTPDownloadFileLocation
        {
            get
            {
                string key = "SFTPDownloadFileLocation";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SFTPHost
        {
            get
            {
                string key = "SFTPHost";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SFTPUsername
        {
            get
            {
                string key = "SFTPUsername";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SFTPPassword
        {
            get
            {
                string key = "SFTPPassword";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ClientId
        {
            get
            {
                string key = "ClientId";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ExpectedUserType
        {
            get
            {
                string key = "ExpectedUserType";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string CampaignId
        {
            get
            {
                string key = "CampaignId";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string CampaignName
        {
            get
            {
                string key = "CampaignName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportEmailTemplatePath
        {
            get
            {
                string key = "SyncReportEmailTemplatePath";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportEmailTo
        {
            get
            {
                string key = "SyncReportEmailTo";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportEmailCC
        {
            get
            {
                string key = "SyncReportEmailCC";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportEmailBCC
        {
            get
            {
                string key = "SyncReportEmailBCC";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportToClientEmailTo
        {
            get
            {
                string key = "SyncReportToClientEmailTo";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportToClientEmailCC
        {
            get
            {
                string key = "SyncReportToClientEmailCC";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportToClientEmailBCC
        {
            get
            {
                string key = "SyncReportToClientEmailBCC";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string Source
        {
            get
            {
                string key = "Source";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string EmailContentsToClientPath
        {
            get
            {
                string key = "EmailContentsToClientPath";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportEmailFromName
        {
            get
            {
                string key = "SyncReportEmailFromName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SyncReportEmailSubject
        {
            get
            {
                string key = "SyncReportEmailSubject";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool ManualProcess
        {
            get
            {
                string key = "ManualProcess";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ManualProcessDataSyncReport
        {
            get
            {
                string key = "ManualProcessDataSyncReport";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }
        

        #region Directory Path

        public static string ExportFilePath
        {
            get
            {
                string key = "ExportFilePath";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Path.GetDirectoryName(Application.ExecutablePath) + @"\" + ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DirectoryPathofCSExport
        {
            get
            {
                string key = "DirectoryPathofCSExport";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DirectoryPathofCSImport
        {
            get
            {
                string key = "DirectoryPathofCSImport";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Directory Path

        public static string WebJobsTempLocation
        {
            get
            {
                string key = "WebJobsTempLocation";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string WebJobsName
        {
            get
            {
                string key = "WebJobsName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string LiveConnectionString
        {
            get
            {
                string key = "LiveConnectionString";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string LiveDatabaseName
        {
            get
            {
                string key = "LiveDatabaseName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DevConnectionString
        {
            get
            {
                string key = "DevConnectionString";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DevDatabaseName
        {
            get
            {
                string key = "DevDatabaseName";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string AuthId
        {
            get
            {
                string key = "AuthId";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                {
                    return ConfigurationManager.AppSettings[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static string AuthToken
        {
            get
            {
                string key = "AuthToken";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                {
                    return ConfigurationManager.AppSettings[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static string InstrumentationKey
        {
            get
            {
                string key = "InstrumentationKey";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                {
                    return ConfigurationManager.AppSettings[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool IsManualDateForExport
        {
            get
            {
                string key = "IsManualDateForExport";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings[key] as string);
                }
                else
                {
                    return false;
                }
            }
        }

        public static string SetDateForExport
        {
            get
            {
                string key = "SetDateForExport";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                {
                    return ConfigurationManager.AppSettings[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ApiUrl
        {
            get
            {
                string key = "ApiUrl";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                {
                    return ConfigurationManager.AppSettings[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static int PassExpiryDay
        {
            get
                => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PassExpiryDay"]) ? default(int) : Convert.ToInt32(ConfigurationManager.AppSettings["PassExpiryDay"]);
        }

        public static string DirectoryPathManualProcess
        {
            get
            {
                string key = "DirectoryPathManualProcess";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string EmailIntervalDay
        {
            get
                => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailIntervalDay"]) ? default(string) : ConfigurationManager.AppSettings["EmailIntervalDay"];
        }

        public static string PassExpiredEmailTemplatePath
        {
            get
                => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PassExpiredEmailTemplatePath"]) ? null : ConfigurationManager.AppSettings["PassExpiredEmailTemplatePath"];
        }

        public static string PassExpiredReportEmailTemplatePath
        {
            get
                => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PassExpiredReportEmailTemplatePath"]) ? null : ConfigurationManager.AppSettings["PassExpiredReportEmailTemplatePath"];
        }

        public static string PasswordResetBaseUrl
        {
            get
                => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PasswordResetBaseUrl"]) ? null : ConfigurationManager.AppSettings["PasswordResetBaseUrl"];
        }

        public static T Get<T>(ConstantType applicationConstantsEnum)
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[applicationConstantsEnum.ToString()]))
            {
                try
                {
                    return (T)Convert.ChangeType(ConfigurationManager.AppSettings[applicationConstantsEnum.ToString()], typeof(T));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return default(T);
        }

        public static ApplicationConstants Constant
        {
            get => new Lazy<ApplicationConstants>(() => new ApplicationConstants()).Value;
        }

        public string this[ConstantType appConstant]
        {
            get => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[appConstant.ToString()]) ? null : ConfigurationManager.AppSettings[appConstant.ToString()];
        }

        public static string JiraTicket
        {
            get
            {
                string key = "JiraTicket";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ProspectType
        {
            get
            {
                string key = "ProspectType";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DataCount
        {
            get
            {
                string key = "DataCount";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DropNumber
        {
            get
            {
                string key = "DropNumber";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DropDate
        {
            get
            {
                string key = "DropDate";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string DataPullPerRequest
        {
            get
            {
                string key = "DataPullPerRequest";
                if (ConfigurationManager.AppSettings[key] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key] as string))
                {
                    return ConfigurationManager.AppSettings[key] as string;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
