using System;
using Indigox.Common.Logging;
using Indigox.UUM.Sync;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Application.SyncTask
{
    public class RetrySyncTaskCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }

        public void Execute()
        {
            var task = SyncManager.GetTaskByID( ID );

            if ( task != null )
            {
                if ( !TryExecuteTask( task ) )
                {
                    throw new ApplicationException( "Retry execute task failed." );
                }
            }
            else
            {
                Log.Debug( string.Format( "Can't find task [{0}].", ID ) );
            }
        }

        private bool TryExecuteTask( ISyncTask task )
        {
            Log.Debug( string.Format( "Begin retry execute task {{ ID:{0}, Tag:{1}, Desc:{2} }}.", task.ID, task.Tag, task.Description ) );
            task.Execute();

            if ( task.State == SyncTaskState.Failed )
            {
                Log.Debug( string.Format( "Retry execute task failed {{ ID:{0}, Tag:{1}, Desc:{2} }}.", task.ID, task.Tag, task.Description ) );
                return false;
            }
            else
            {
                Log.Debug( string.Format( "Retry execute task successed {{ ID:{0}, Tag:{1}, Desc:{2} }}.", task.ID, task.Tag, task.Description ) );
                return true;
            }
        }
    }
}