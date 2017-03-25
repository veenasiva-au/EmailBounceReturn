using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBounceBack.Core
{
    public enum EmailStatus
    {
        InProgress,
        Complete,
        Error,
        Empty,
        Ignored,
        Rejected,
        Escalated,
        Purged
    }
}
