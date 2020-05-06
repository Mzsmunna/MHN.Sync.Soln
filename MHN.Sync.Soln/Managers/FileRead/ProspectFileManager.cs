using Cerebro.JWTAuthService.Services.Helper;
using MHN.Sync.Entity;
using MHN.Sync.Entity.Enum;
using MHN.Sync.Entity.MongoEntity;
using MHN.Sync.Helper;
using MHN.Sync.Interface;
using MHN.Sync.Jobs;
using MHN.Sync.ModelView;
using MHN.Sync.MongoInterface;
using MHN.Sync.Utility.CSV;
using MHN.Sync.Utility.FTP;
using MHN.Sync.Utility.SFTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MHN.Sync.Soln.Managers.FileRead
{
    public class ProspectFileManager : IJobManager
    {
        private JWTHelperUtility _jwtHelperUtility;       
        private static IProspectMemberRepository _prospectMemberRepository;
        private static IProspectRepository _prospectRepository;

        public JobManagerResult Result;

        private static StringBuilder output = new StringBuilder();
        private static Stopwatch stopwatch = new Stopwatch();

        int processDataCount = 0;
        int totalDataCount = 0;

        private TextReader fileReadableStream = null;
        //private SFTPUtility sFTPUtility;

        List<ProspectModel> prospectModelList;

        NewJob.DataProcessDelegate<ProspectModel> _dataProcessDelegate;

        public ProspectFileManager(IProspectMemberRepository prospectMemberRepository, IProspectRepository prospectRepository, JWTHelperUtility jWTHelperUtility)
        {
            _prospectMemberRepository = prospectMemberRepository;
            _prospectRepository = prospectRepository;
            _jwtHelperUtility = jWTHelperUtility;

            Result = NewJob.GetJobInstance();
            //sFTPUtility = new SFTPUtility(_jwtHelperUtility, ConstantType.MHN);

            prospectModelList = new List<ProspectModel>();
            _dataProcessDelegate = new NewJob.DataProcessDelegate<ProspectModel>(DataProcess);
        }

        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
            output.AppendLine(str);
        }

        public JobManagerResult Execute()
        {
            try
            {
                Result.Message.CustomAppender(Environment.NewLine + "Prospect Member Service started on " + HelperUtility.GetCurrentTimeInEST());
                FtpsUtility.CustomInitialize(_jwtHelperUtility);
                stopwatch.Start();
                FileProcess();
                stopwatch.Stop();
                WriteLine("\nTotal Minutes : " + stopwatch.Elapsed.TotalMinutes);
                Result.Message.CustomAppender(Environment.NewLine + "Prospect Member Service ended on " + HelperUtility.GetCurrentTimeInEST());
                Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Message.CustomAppender("Exception Occured.");
                Result.Message.CustomAppender(ex.Message);
                if (!string.IsNullOrEmpty(ApplicationConstants.InstrumentationKey))
                {
                    //TelemetryLogger.LogException(ex);
                }
            }

            return Result;
        }

        private void FileProcess()
        {
            if (ApplicationConstants.ManualProcess)
            {
                ManualProcess();
            }
            else
            {
                AutomatedProcess();
            }
        }

        private void AutomatedProcess()
        {
            var contentIdentifier = string.Empty;
            //fileReadableStream = NewJob.AutomatedProcess(contentIdentifier, fileToSearch, Result);
            //fileReadableStream = NewJob.AutomatedProcess(contentIdentifier, fileToSearch, Result, sFTPUtility);

            //isFileExists = Result.IsSearchedFileFound.Value;

            if (fileReadableStream != null)
            {
                FileProcess(fileReadableStream);
            }
            //else
            //    Result.Message.CustomAppender(String.Format("No file found to process. File Name:{0}, Date: {1}", fileToSearch, HelperUtility.GetCurrentTimeInEST()));
        }

        private void ManualProcess()
        {
            fileReadableStream = NewJob.ManualProcess(Result);

            if (fileReadableStream != null)
            {
                FileProcess(fileReadableStream);
            }
        }

        private void FileProcess(TextReader fileReadableStream)
        {
            WriteLine("Processing the file....");

            using (fileReadableStream)
            {
                prospectModelList = CsvUtility.ReadDataFromTextReader<ProspectModel, ProspectModelMap>(fileReadableStream, ',', true);

                //prospectModelList = JsonConvert.DeserializeObject<List<ProspectMember>>(JsonConvert.SerializeObject(prospectList));
                //string jSon = JsonConvert.SerializeObject(prospectList, Formatting.Indented);
                //prospectMmeberList = JsonConvert.DeserializeObject<List<ProspectMember>>(jSon);

                //BlobUtility.DataListUpload<CareplanMailbackViewModel>(careplanMailbackList, BlobConstant.Azure_Connection_String, BlobConstant.MOC_Audit_Container, BlobConstant.Careplan_Mailback, fileToSearch);

                totalDataCount = prospectModelList.Count();

                Result.Message.CustomAppender("Total data : " + totalDataCount);

                WriteLine("Processing the file....");
                NewJob.DataProcessWithTask(prospectModelList, _dataProcessDelegate);

                //DataProcess(prospectModelList);

                Result.IsSuccess = true;
            }
        }

        private void DataProcess(List<ProspectModel> dataList)
        {
            foreach (var prospect in dataList)
            {
                var id = string.Empty;

                var prospectMember = ConvertToProspectMember(prospect);

                //Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / _totalData, processDataCount, _totalData);

                id = _prospectMemberRepository.Save(prospectMember);

                var prospectMeta = ConvertToProspectMeta(prospect);
                prospectMeta.ProspectMemberDataRef = id;

                processDataCount++;
                Result.NoOfRecordsProcessed++;
                Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / totalDataCount, processDataCount, totalDataCount);

                _prospectRepository.Save(prospectMeta);
            }
        }

        private ProspectMember ConvertToProspectMember(ProspectModel prospect)
        {
            ProspectMember prospectMember = new ProspectMember()
            {
                AddressInfo = new Address()
                {
                    residentialAddress = new ResidentialAddress()
                    {
                        Address1 = prospect.Address.address1.ToLower(),
                        //Address2 = prospect.Address.address2.ToLower(),
                        City = prospect.Address.city.ToLower(),
                        State = prospect.Address.state.ToLower(),
                        Zip = prospect.Address.zip.ToLower()
                    },
                    mailingAddress = new MailingAddress()
                    {

                    }
                },
                AgentName = prospect.AgentName.ToLower(),
                FName = prospect.FName.ToLower(),
                LName = prospect.LName.ToLower(),
                ProspectId = prospect.ProspectId.ToLower(),
                //Status = null,
                Type = prospect.Type.ToLower(),
                PTCExpiredOn = HelperUtility.GetDateFromString(prospect.PTCExpiredOn),
                PTC_Date_Stamped = HelperUtility.GetDateFromString(prospect.PTC_Date_Stamped),

                IsMember = false,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };

            return prospectMember;
        }

        private ProspectMeta ConvertToProspectMeta(ProspectModel prospect)
        {
            ProspectMeta prospectMeta = new ProspectMeta
            {
                //ProspectMemberDataRef = id,
                Admin_Notes = prospect.Admin_Notes.ToLower(),
                AgentAssignedOn = HelperUtility.GetDateFromString(prospect.AgentAssignedOn),
                Heard_about_us_through = prospect.Heard_about_us_through.ToLower(),
                ConversionInfo = new Conversion
                {
                    One_Two_DaysConversion = prospect.Conversion.One_Two_DaysConversion.ToLower(),
                    Three_Five_DaysConversion = prospect.Conversion.Three_Five_DaysConversion.ToLower(),
                    Five_Plus_DaysConversion = prospect.Conversion.Five_Plus_DaysConversion.ToLower(),
                },
                LastContactAttemptedOn = HelperUtility.GetDateFromString(prospect.LastContactAttemptedOn),
                Comments = prospect.Comments.ToLower(),
                Compl_by_Name = prospect.Compl_by_Name.ToLower(),
                Compl_by_Organization = prospect.Compl_by_Organization.ToLower(),
                Compl_by_Role = prospect.Compl_by_Role.ToLower(),
                ConvertStatus = prospect.ConvertStatus.ToLower(),
                LastContactedOn = HelperUtility.GetDateFromString(prospect.LastContactedOn),
                PreferredContactMethod = prospect.PreferredContactMethod.ToLower(),
                Email = prospect.Email.ToLower(),
                Language = prospect.Language.ToLower(),
                PhoneNumber = prospect.PhoneNumber.ToLower(),
                Location = prospect.Location.ToLower(),
                MobileNumber = prospect.MobileNumber.ToLower(),
                Origin = prospect.Origin.ToLower(),
                PreferredOutreachDay = prospect.PreferredOutreachDay.ToLower(),
                PreferredOutreachTime = prospect.PreferredOutreachTime.ToLower(),               
                PTCFilledOn = HelperUtility.GetDateFromString(prospect.PTCFilledOn),               
                SpecificDateNoted = HelperUtility.GetDateFromString(prospect.SpecificDateNoted),
                Specific_time_or_range_noted = prospect.Specific_time_or_range_noted.ToLower(),

                IsMember = false,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };

            return prospectMeta;
        }
    }
}
