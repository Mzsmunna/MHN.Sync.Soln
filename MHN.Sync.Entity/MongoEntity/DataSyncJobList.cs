using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class DataSyncJobList: IEntity
    {
        public string Id { get; set; }
        public string CampaignId { get; set; }
        public string ClientId { get; set; }
        public string SyncType { get; set; } //SyncType enum
        public string SyncStatus { get; set; } //SyncStatus enum
        public int ExecutionStartHour { get; set; } // this will be in 24 hour
        public int ExecutionEndHour { get; set; } // this will be in 24 hour
        public string ScheduleExpression { get; set; }
        public string FileLocation { get; set; }
        public string FileSource { get; set; }
        public Boolean IsActive { get; set; }
        public int OrderBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
