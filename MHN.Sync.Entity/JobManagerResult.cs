using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity
{
    public class JobManagerResult
    {
        public JobManagerResult()
        {
            this.Message = new StringBuilder();
            this.ProspectIds = new List<String>();
            this.FileLocations = new List<string>();
            this.FileNames = new List<string>();
        }

        public StringBuilder Message { get; set; }
        public Boolean IsSuccess { get; set; }
        public int? NoOfRecordsProcessed { get; set; }
        public List<String> ProspectIds { get; set; }
        public Boolean? IsSearchedFileFound { get; set; }
        public int? NoOfMatchedRecords { get; set; }
        public int? NoOfMismatchedRecords { get; set; }

        public int? NoOfCorruptedData { get; set; }
        public int? NoOfSpecialData { get; set; }
        public int? NoOfValidData { get; set; }

        public List<string> FileLocations { get; set; }
        public List<string> FileNames { get; set; }
    }
}
