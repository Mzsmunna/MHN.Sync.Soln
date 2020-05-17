using Cerebro.JWTAuthService.Services.Helper;
using MHN.Sync.Entity;
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
    public class MemberAdditionalFileManager : IJobManager
    {
        private JWTHelperUtility _jwtHelperUtility;
        private static IProspectMemberRepository _prospectMemberRepository;
        private static IMemberAdditionalRepository _memberAdditionalRepository;

        public JobManagerResult Result;

        private static StringBuilder output = new StringBuilder();
        private static Stopwatch stopwatch = new Stopwatch();

        int processDataCount = 0;
        int totalDataCount = 0;

        private TextReader fileReadableStream = null;
        //private SFTPUtility sFTPUtility;

        List<MemberAdditionalModel> memberAdditionalModelList;

        NewJob.DataProcessDelegate<MemberAdditionalModel> _dataProcessDelegate;

        public MemberAdditionalFileManager(IProspectMemberRepository prospectMemberRepository, IMemberAdditionalRepository memberAdditionalRepository, JWTHelperUtility jWTHelperUtility)
        {
            _prospectMemberRepository = prospectMemberRepository;
            _memberAdditionalRepository = memberAdditionalRepository;
            _jwtHelperUtility = jWTHelperUtility;

            Result = NewJob.GetJobInstance();
            //sFTPUtility = new SFTPUtility(_jwtHelperUtility, ConstantType.MHN);

            memberAdditionalModelList = new List<MemberAdditionalModel>();
            _dataProcessDelegate = new NewJob.DataProcessDelegate<MemberAdditionalModel>(DataProcess);
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
                Result.Message.CustomAppender(Environment.NewLine + "Member Additional Service started on " + HelperUtility.GetCurrentTimeInEST());
                FtpsUtility.CustomInitialize(_jwtHelperUtility);
                stopwatch.Start();
                FileProcess();
                stopwatch.Stop();
                WriteLine("Total Minutes : " + stopwatch.Elapsed.TotalMinutes);
                Result.Message.CustomAppender(Environment.NewLine + "Member Additional Service ended on " + HelperUtility.GetCurrentTimeInEST());
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
                memberAdditionalModelList = CsvUtility.ReadDataFromTextReader<MemberAdditionalModel, MemberAdditionalModelMap>(fileReadableStream, ',', true);

                //memberModelList = JsonConvert.DeserializeObject<List<Member>>(JsonConvert.SerializeObject(membersList));
                //string jSon = JsonConvert.SerializeObject(prospectList, Formatting.Indented);
                //prospectMmeberList = JsonConvert.DeserializeObject<List<ProspectMember>>(jSon);

                //BlobUtility.DataListUpload<CareplanMailbackViewModel>(careplanMailbackList, BlobConstant.Azure_Connection_String, BlobConstant.MOC_Audit_Container, BlobConstant.Careplan_Mailback, fileToSearch);

                totalDataCount = memberAdditionalModelList.Count();
                Result.Message.CustomAppender("Total data : " + totalDataCount);

                //NewJob.DataProcessWithTask(memberModelList, _dataProcessDelegate);

                DataProcess(memberAdditionalModelList);

                Result.IsSuccess = true;
            }
        }

        private void DataProcess(List<MemberAdditionalModel> dataList)
        {
            foreach (var member in dataList)
            {
                var id = string.Empty;

                //var prospectMember = JsonConvert.DeserializeObject<MemberMeta>(JsonConvert.SerializeObject(member));

                //var prospectMember = ConvertToProspectMember(member);

                //Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / totalDataCount, processDataCount, totalDataCount);
                
                //id = _prospectMemberRepository.Save(prospectMember);

                var memberAdditional = ConvertToMemberAdditional(member);
                memberAdditional.ProspectMemberDataRef = id;

                processDataCount++;
                Result.NoOfRecordsProcessed++;
                Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / totalDataCount, processDataCount, totalDataCount);

                _memberAdditionalRepository.Save(memberAdditional);
            }
        }

        private ProspectMember ConvertToProspectMember(MemberAdditionalModel member)
        {
            ProspectMember prospectMember = null;

            if(!string.IsNullOrEmpty(member.MBI))
                prospectMember = _prospectMemberRepository.GetAllByField("MBI", member.MBI.ToLower()).Result.ToList().FirstOrDefault();

            
            if (prospectMember == null)
            {
                prospectMember = new ProspectMember()
                {
                    MBI = member.MBI.ToLower(),

                    IsApplicant = false,

                    IsMember = true,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                };
                
            }

            return prospectMember;
        }

        private MemberAdditional ConvertToMemberAdditional(MemberAdditionalModel member)
        {
            //MemberAdditional memberAdditional = new MemberAdditional
            //{

            //    IsActive = true,
            //    CreatedOn = DateTime.UtcNow,
            //};

            var memberAdditional = JsonConvert.DeserializeObject<MemberAdditional>(JsonConvert.SerializeObject(member));

            memberAdditional.OutreachNeededInd = member.OutreachNeededInd.ToLower().StartsWith("t") || member.OutreachNeededInd.Equals("1") ? true : false;
            
            memberAdditional.Supplementary.LISStatusInd = member.Supplementary.LISStatusInd.ToLower().StartsWith("t") || member.Supplementary.LISStatusInd.Equals("1") ? true : false;
            memberAdditional.Supplementary.MedicaidEligibilityInd = member.Supplementary.MedicaidEligibilityInd.ToLower().StartsWith("t") || member.Supplementary.MedicaidEligibilityInd.Equals("1") ? true : false;
            memberAdditional.Supplementary.LISStartDate = HelperUtility.GetDateFromString(member.Supplementary.LISStartDate);
            memberAdditional.Supplementary.LISEndDate = HelperUtility.GetDateFromString(member.Supplementary.LISEndDate);
           
            memberAdditional.CareManagement.LastHRADate = HelperUtility.GetDateFromString(member.CareManagement.LastHRADate);
            memberAdditional.CareManagement.HRADueDate = HelperUtility.GetDateFromString(member.CareManagement.HRADueDate);
            memberAdditional.CareManagement.CareGapInd = member.CareManagement.CareGapInd.ToLower().StartsWith("t") || member.CareManagement.CareGapInd.Equals("1") ? true : false;

            memberAdditional.ContactRelated.PermissionToTextDate = HelperUtility.GetDateFromString(member.ContactRelated.PermissionToTextDate);
            memberAdditional.ContactRelated.TextOptOutDate = HelperUtility.GetDateFromString(member.ContactRelated.TextOptOutDate);

            memberAdditional.RiskActivity.DisEnrollmentRequestDate = HelperUtility.GetDateFromString(member.RiskActivity.DisEnrollmentRequestDate);
            memberAdditional.RiskActivity.DisEnrollmentEffectiveDate = HelperUtility.GetDateFromString(member.RiskActivity.DisEnrollmentEffectiveDate);
            memberAdditional.RiskActivity.LEPInd = member.RiskActivity.LEPInd.ToLower().StartsWith("t") || member.RiskActivity.LEPInd.Equals("1") ? true : false;
            memberAdditional.RiskActivity.LEPLetterSentDate = HelperUtility.GetDateFromString(member.RiskActivity.LEPLetterSentDate);
            memberAdditional.RiskActivity.AppealDate = HelperUtility.GetDateFromString(member.RiskActivity.AppealDate);

            memberAdditional.InteractionHistory.WelcomeCallDate = HelperUtility.GetDateFromString(member.InteractionHistory.WelcomeCallDate);
            memberAdditional.InteractionHistory.MbrNotesDate = HelperUtility.GetDateFromString(member.InteractionHistory.MbrNotesDate);

            memberAdditional.BenefitsActivity.OTCInfo.CardActivated = member.BenefitsActivity.OTCInfo.CardActivated.ToLower().StartsWith("t") || member.BenefitsActivity.OTCInfo.CardActivated.Equals("1") ? true : false;
            memberAdditional.BenefitsActivity.OTCInfo.ActivationDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.ActivationDate);
            memberAdditional.BenefitsActivity.OTCInfo.MailDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.MailDate);
            memberAdditional.BenefitsActivity.OTCInfo.StartDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.StartDate);
            memberAdditional.BenefitsActivity.OTCInfo.ExpirationDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.ExpirationDate);

            memberAdditional.Medication.DayRefillInd90 = member.Medication.DayRefillInd90.ToLower().StartsWith("t") || member.Medication.DayRefillInd90.Equals("1") ? true : false;
            memberAdditional.Medication.MailOrderPharmacyInd = member.Medication.MailOrderPharmacyInd.ToLower().StartsWith("t") || member.Medication.MailOrderPharmacyInd.Equals("1") ? true : false;
            memberAdditional.Medication.TakingMedInd = member.Medication.TakingMedInd.ToLower().StartsWith("t") || member.Medication.TakingMedInd.Equals("1") ? true : false;

            memberAdditional.IsActive = true;
            memberAdditional.CreatedOn = DateTime.UtcNow;

            return memberAdditional;
        }
    }
}
