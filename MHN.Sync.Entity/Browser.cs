using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity
{
    public class Browser
    {
        public string os { get; set; }
        public string os_version { get; set; }
        public string browser { get; set; }
        public object device { get; set; }
        public string browser_version { get; set; }
        public object real_mobile { get; set; }
    }
}
