using MHN.Sync.Entity.MongoEntity;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Jobs
{
    public static class JobUtility
    {

        public static List<DataSyncJobList> FindExecutableJobs(List<DataSyncJobList> jobList)
        {
            List<DataSyncJobList> executableJobList = new List<DataSyncJobList>();

            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            var oneHourBefore = easternTime.AddHours(-1);
            foreach (var job in jobList)
            {
                if (!string.IsNullOrEmpty(job.ScheduleExpression))
                {
                    var schedule = CrontabSchedule.Parse(job.ScheduleExpression);
                    var exicutionTime = schedule.GetNextOccurrence(oneHourBefore);
                    var exicutionHour = exicutionTime.ToString("MM-dd-yyyy-")+ exicutionTime.Hour;
                    var currentHour = easternTime.ToString("MM-dd-yyyy-") + easternTime.Hour;
                    if (exicutionHour == currentHour)
                    {
                        executableJobList.Add(job);
                        Console.WriteLine("Name : " + job.SyncType + ", Time : " + job.ScheduleExpression);
                    }
                }
                else
                    if (easternTime.Hour == job.ExecutionStartHour)
                    executableJobList.Add(job);
            }
            return executableJobList;
        }
    }
}
