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

                var prospectMember = ConvertToProspectMember(member);

                //Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / totalDataCount, processDataCount, totalDataCount);
                
                id = _prospectMemberRepository.Save(prospectMember);

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

            prospectMember.OutreachNeededInd = ConvertToBool(member.OutreachNeededInd);
            prospectMember.LISStatusInd = ConvertToBool(member.Supplementary.LISStatusInd);
            prospectMember.DisEnrollmentRequest = ConvertToBool(member.RiskActivity.DisEnrollmentRequest);
            prospectMember.DisEnrollmentEffectiveDate = HelperUtility.GetDateFromString(member.RiskActivity.DisEnrollmentEffectiveDate);
            prospectMember.FiledGrievanceIndicator = ConvertToBool(member.RiskActivity.FiledGrievanceIndicator);
            prospectMember.FiledAppealIndicator = ConvertToBool(member.RiskActivity.FiledAppealIndicator);

            return prospectMember;
        }

        private bool ConvertToBool(string value)
        {
            bool trueOrFalse = value.ToLower().StartsWith("t") || value.ToLower().StartsWith("y") || value.Equals("1") ? true : false;
            return trueOrFalse;
        }

        private MemberAdditional ConvertToMemberAdditional(MemberAdditionalModel member)
        {
            //MemberAdditional memberAdditional = new MemberAdditional
            //{

            //    IsActive = true,
            //    CreatedOn = DateTime.UtcNow,
            //};

            var config = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };

            var memberAdditional = JsonConvert.DeserializeObject<MemberAdditional>(JsonConvert.SerializeObject(member), config);

            memberAdditional.OutreachNeededInd = ConvertToBool(member.OutreachNeededInd);
            
            memberAdditional.Supplementary.LISStatusInd = ConvertToBool(member.Supplementary.LISStatusInd);
            //memberAdditional.Supplementary.MedicaidEligibilityInd = ConvertToBool(member.Supplementary.MedicaidEligibilityInd);
            memberAdditional.Supplementary.LISStartDate = HelperUtility.GetDateFromString(member.Supplementary.LISStartDate);
            memberAdditional.Supplementary.LISEndDate = HelperUtility.GetDateFromString(member.Supplementary.LISEndDate);
           
            memberAdditional.CareManagement.LastHRADate = HelperUtility.GetDateFromString(member.CareManagement.LastHRADate);
            memberAdditional.CareManagement.HRADueDate = HelperUtility.GetDateFromString(member.CareManagement.HRADueDate);
            memberAdditional.CareManagement.CareGapInd = ConvertToBool(member.CareManagement.CareGapInd);

            memberAdditional.ContactRelated.PermissionToText = ConvertToBool(member.ContactRelated.PermissionToText);
            memberAdditional.ContactRelated.PermissionToTextDate = HelperUtility.GetDateFromString(member.ContactRelated.PermissionToTextDate);
            memberAdditional.ContactRelated.TextOptOutDate = HelperUtility.GetDateFromString(member.ContactRelated.TextOptOutDate);

            memberAdditional.RiskActivity.DisEnrollmentRequest = ConvertToBool(member.RiskActivity.DisEnrollmentRequest);
            memberAdditional.RiskActivity.DisEnrollmentRequestDate = HelperUtility.GetDateFromString(member.RiskActivity.DisEnrollmentRequestDate);
            memberAdditional.RiskActivity.DisEnrollmentEffectiveDate = HelperUtility.GetDateFromString(member.RiskActivity.DisEnrollmentEffectiveDate);
            memberAdditional.RiskActivity.LEPInd = ConvertToBool(member.RiskActivity.LEPInd);
            memberAdditional.RiskActivity.LEPLetterSentDate = HelperUtility.GetDateFromString(member.RiskActivity.LEPLetterSentDate);
            memberAdditional.RiskActivity.FiledGrievanceIndicator = ConvertToBool(member.RiskActivity.FiledGrievanceIndicator);
            memberAdditional.RiskActivity.FiledAppealIndicator = ConvertToBool(member.RiskActivity.FiledAppealIndicator);
            memberAdditional.RiskActivity.AppealDate = HelperUtility.GetDateFromString(member.RiskActivity.AppealDate);

            memberAdditional.InteractionHistory.WelcomeCallMadeInd = ConvertToBool(member.InteractionHistory.WelcomeCallMadeInd);
            memberAdditional.InteractionHistory.WelcomeCallDate = HelperUtility.GetDateFromString(member.InteractionHistory.WelcomeCallDate);
            memberAdditional.InteractionHistory.MbrNotesDate = HelperUtility.GetDateFromString(member.InteractionHistory.MbrNotesDate);

            memberAdditional.BenefitsActivity.FoodRecipientInd = ConvertToBool(member.BenefitsActivity.FoodRecipientInd);
            memberAdditional.BenefitsActivity.FitnessUseIndicator = ConvertToBool(member.BenefitsActivity.FitnessUseIndicator);
            memberAdditional.BenefitsActivity.OTCInfo.CardActivated = ConvertToBool(member.BenefitsActivity.OTCInfo.CardActivated);
            memberAdditional.BenefitsActivity.OTCInfo.ActivationDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.ActivationDate);
            memberAdditional.BenefitsActivity.OTCInfo.MailDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.MailDate);
            memberAdditional.BenefitsActivity.OTCInfo.StartDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.StartDate);
            memberAdditional.BenefitsActivity.OTCInfo.ExpirationDate = HelperUtility.GetDateFromString(member.BenefitsActivity.OTCInfo.ExpirationDate);
            memberAdditional.BenefitsActivity.TransportationUseIndicator = ConvertToBool(member.BenefitsActivity.TransportationUseIndicator);

            memberAdditional.Medication.DayRefillInd90 = ConvertToBool(member.Medication.DayRefillInd90);
            memberAdditional.Medication.MailOrderPharmacyInd = ConvertToBool(member.Medication.MailOrderPharmacyInd);
            memberAdditional.Medication.TakingMedInd = ConvertToBool(member.Medication.TakingMedInd);

            memberAdditional.IsActive = true;
            memberAdditional.CreatedOn = DateTime.UtcNow;

            return memberAdditional;
        }
    }
}
