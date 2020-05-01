using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.SQLInterface
{
    public interface IUnitOfWork : IDisposable
    {
        IIntakeWorkflowRepository IntakeWorkflow { get; }
        int Complete();
    }
}
