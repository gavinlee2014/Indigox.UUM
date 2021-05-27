using System;
using Indigox.UUM.Sync.Contexts;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.Tasks;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class AbstractTaskBuilder : ISyncTaskBuilder
    {
        public SysConfiguration Source { get; set; }

        public ISyncTask Build()
        {
            if ( !SyncEnabled )
            {
                return null;
            }

            SyncTask task = new SyncTask();
            task.Description = BuildDescription();
            task.Context = BuildTaskContext();
            task.Executor = BuildTaskExecutor();
            task.Tag = Source.ClientName;
            return task;
        }

        protected virtual string BuildDescription()
        {
            return "";
        }

        protected virtual ISyncContext BuildTaskContext()
        {
            return new HashSyncContext();
        }

        protected virtual ISyncExecutor BuildTaskExecutor()
        {
            return new WebServiceExecutor();
        }

        protected virtual bool SyncEnabled
        {
            get
            {
                return true;
            }
        }
    }
}