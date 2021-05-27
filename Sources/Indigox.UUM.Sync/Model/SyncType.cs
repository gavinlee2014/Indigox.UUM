using System;

namespace Indigox.UUM.Sync.Model
{
    [Flags]
    public enum SyncType
    {
        None = 0,
        Source = 1,
        Target = 2,
        All = 3,
    }

    [Flags]
    public  enum SyncState{
        Running = 0,
        Error = 1,
        Completed = 2,
    }
}