using MHN.Sync.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Interface
{
    public interface IJobManager
    {
        JobManagerResult Execute();
    }
}
