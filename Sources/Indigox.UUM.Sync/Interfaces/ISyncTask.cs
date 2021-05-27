using System;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.Interfaces
{
    public interface ISyncTask
    {
        int ID { get; }
        string Tag { get; set; }
        string Description { get; set; }
        string ErrorMessage { get; set; }
        SyncTaskState State { get; }
        ISyncContext Context { get; set; }
        ISyncExecutor Executor { get; set; }
        DateTime CreateTime { get; set; }
        DateTime? ExecuteTime { get; set; }
        IList<ISyncTask> Dependencies { get; set; }

        event SyncTaskCompletedEvent Failed;

        event SyncTaskCompletedEvent Successed;

        event SyncTaskCompletedEvent Ignored;

        void Execute();

        void SetSuccessed();

        void SetFailed();

        void SetIgnore();
    }

    public delegate void SyncTaskCompletedEvent( ISyncTask task );
}