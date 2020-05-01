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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Soln.Managers.FileRead
{
    public class MemberFileManager : IJobManager
    {
        private JWTHelperUtility _jwtHelperUtility;
        private static IProspectMemberRepository _prospectMemberRepository;
        private static IMemberRepository _memberRepository;

        public JobManagerResult Result;

        private static StringBuilder output = new StringBuilder();
        private static Stopwatch stopwatch = new Stopwatch();

        private bool isFileExists = false;
        private string fileToSearch = string.Empty;
        int processDataCount = 0;

        private TextReader fileReadableStream = null;
        private SFTPUtility sFTPUtility;

        List<MemberModel> memberModelList;

        public MemberFileManager(IProspectMemberRepository prospectMemberRepository, IMemberRepository memberRepository, JWTHelperUtility jWTHelperUtility)
        {
            _prospectMemberRepository = prospectMemberRepository;
            _memberRepository = memberRepository;
            _jwtHelperUtility = jWTHelperUtility;

            Result = NewJob.GetJobInstance();
            //sFTPUtility = new SFTPUtility(_jwtHelperUtility, ConstantType.MHN);

            memberModelList = new List<MemberModel>();
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
                Result.Message.CustomAppender(Environment.NewLine + "Member Service started on " + HelperUtility.GetCurrentTimeInEST());
                FtpsUtility.CustomInitialize(_jwtHelperUtility);
                stopwatch.Start();
                FileProcess();
                stopwatch.Stop();
                WriteLine("Total Minutes : " + stopwatch.Elapsed.TotalMinutes);
                Result.Message.CustomAppender(Environment.NewLine + "Member Service ended on " + HelperUtility.GetCurrentTimeInEST());
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
                memberModelList = CsvUtility.ReadDataFromTextReader<MemberModel, MemberModelMap>(fileReadableStream, ',', true);

                //memberModelList = JsonConvert.DeserializeObject<List<Member>>(JsonConvert.SerializeObject(membersList));
                //string jSon = JsonConvert.SerializeObject(prospectList, Formatting.Indented);
                //prospectMmeberList = JsonConvert.DeserializeObject<List<ProspectMember>>(jSon);

                //BlobUtility.DataListUpload<CareplanMailbackViewModel>(careplanMailbackList, BlobConstant.Azure_Connection_String, BlobConstant.MOC_Audit_Container, BlobConstant.Careplan_Mailback, fileToSearch);

                var _totalData = memberModelList.Count();
                Result.Message.CustomAppender("Total data : " + _totalData);

                foreach (var member in memberModelList)
                {
                    var id = string.Empty;
                    //var prospectMember = JsonConvert.DeserializeObject<MemberMeta>(JsonConvert.SerializeObject(member));

                    var prospectMember = ConvertToProspectMember(member);

                    //Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / _totalData, processDataCount, _totalData);
                    id = _prospectMemberRepository.Save(prospectMember);

                    var memberMeta = ConvertToMemberMeta(member);
                    memberMeta.ProspectMemberDataRef = id;

                    processDataCount++;
                    Result.NoOfRecordsProcessed++;
                    Console.Write("\rProcessing : {0}% ({1}) | {2}", (processDataCount * 100) / _totalData, processDataCount, _totalData);
                    _memberRepository.Save(memberMeta);
                }

                Result.IsSuccess = true;
            }
        }

        private ProspectMember ConvertToProspectMember(MemberModel member)
        {
            ProspectMember prospectMember = new ProspectMember()
            {
                MBI = member.MBI.ToLower(),
                MemberId = member.MemberId.ToLower(),
                AddressInfo = new Address()
                {
                    residentialAddress = new ResidentialAddress()
                    {
                        Address1 = member.Address.address1.ToLower(),
                        Address2 = member.Address.address2.ToLower(),
                        City = member.Address.city.ToLower(),
                        State = member.Address.state.ToLower(),
                        Zip = member.Address.zip.ToLower()
                    },
                    mailingAddress = new MailingAddress()
                    {

                    }
                },
                DOB = member.DOB.ToLower(),
                LOB = member.LOB.ToLower(),
                EnrolledOn = HelperUtility.GetDateFromString(member.EnrolledOn),
                FName = member.FName.ToLower(),
                LName = member.LName.ToLower(),
                Gender = member.Gender.ToLower(),
                //Status = null,
                TermDateOn = HelperUtility.GetDateFromString(member.TermDateOn),

                IsMember = true,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };

            return prospectMember;
        }

        private MemberMeta ConvertToMemberMeta(MemberModel member)
        {
            MemberMeta memberMeta = new MemberMeta
            {
                //ProspectMemberDataRef = id,
                Part_A_RF = member.Part_A_RF.ToLower(),
                Part_D_RF = member.Part_D_RF.ToLower(),
                PCPId = member.PCPId.ToLower(),
                PCPName = member.PCPName.ToLower(),
                PCP_NPI = member.PCP_NPI.ToLower(),
                PhoneNumber = member.PhoneNumber.ToLower(),
                LIPS_Code = member.LIPS_Code.ToLower(),
                CareManagementEntity = member.CareManagementEntity.ToLower(),
                DualCode = member.DualCode.ToLower(),
                Email = member.Email.ToLower(),
                INST_Code = member.INST_Code.ToLower(),
                LIPS_PCT = member.LIPS_PCT.ToLower(),

                IsMember = true,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };

            return memberMeta;
        }
    }
}
