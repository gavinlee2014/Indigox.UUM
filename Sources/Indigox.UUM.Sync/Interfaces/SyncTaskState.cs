using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.Sync.Interfaces
{
    public enum SyncTaskState
    {
        Initiated = 0,
        Successed = 1,
        Failed = 2,
        Ignore = 3,
    }
}
