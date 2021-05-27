using System;
using System.Collections.Generic;
using Indigox.Common.Data;
using Indigox.Common.Data.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Gateways;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.SyncQueues;

namespace Indigox.UUM.Sync
{
    public class SyncManager
    {
        private static Dictionary<string, ISyncQueue> queues = new Dictionary<string, ISyncQueue>();

        private static Dictionary<int, ISyncTask> tasks = new Dictionary<int, ISyncTask>();

        public static void AddTask( string tag, ISyncTask task )
        {
            AddTask( tag, task, true );
        }

        private static void AddTask( string tag, ISyncTask task, bool isNew )
        {
            if ( isNew )
            {
                AddToRepository( task );
            }

            ISyncQueue queue = GetQueue( tag );

            task.Successed += new SyncTaskCompletedEvent( SyncTaskCompleted );
            task.Failed += new SyncTaskCompletedEvent( SyncTaskCompleted );
            task.Ignored += new SyncTaskCompletedEvent(SyncTaskCompleted);

            queue.Push( task );
            tasks.Add( task.ID, task );
        }

        public static ISyncTask GetTaskByID( int id )
        {
            if ( tasks.ContainsKey( id ) )
            {
                return tasks[ id ];
            }
            return null;
        }

        /// <remarks>
        /// 在应用启动的时候调用，恢复系统终止时的同步队列
        /// </remarks>
        public static void RestoreFromRepository()
        {
            IRepository<ISyncTask> repos = RepositoryFactory.Instance.CreateRepository<ISyncTask>();
            IList<ISyncTask> tasks = repos.Find( Query.NewQuery
                .FindByCondition(
                    Specification.Or(
                        Specification.Equal( "State", SyncTaskState.Initiated ),
                        Specification.Equal( "State", SyncTaskState.Failed )
                    )
                )
                .OrderByAsc( "ID" )
            );

            int restoredCount = 0;

            foreach ( ISyncTask task in tasks )
            {
                if ( string.IsNullOrEmpty( task.Tag ) )
                {
                    Log.Debug( string.Format( "Task[{0}].Tag is null.", task.ID ) );
                    continue;
                }

                AddTask( task.Tag, task, false );

                restoredCount++;
            }

            Log.Debug( string.Format( "Restored {0} tasks from repository.", restoredCount ) );
        }

        [Obsolete( "Deprecated, instead by EventGateway.Notify(object,IEvent)" )]
        public void ProcessEvent( object source, IEvent evt )
        {
            IEventGateway eventGateway = new EventGateway();
            eventGateway.Notify( source, evt );
        }

        private static ISyncQueue GetQueue( string tag )
        {
            if ( !queues.ContainsKey( tag ) )
            {
                queues.Add( tag, new AsyncSequenceQueue() );
            }
            return queues[ tag ];
        }

        private static void AddToRepository( ISyncTask task )
        {
            IRepository<ISyncTask> repos = RepositoryFactory.Instance.CreateRepository<ISyncTask>();
            repos.Add( task );

            //            IDatabase db = new DatabaseFactory().CreateDatabase( "UUM" );
            //
            //            string sql = @"update SyncTask
            //set State =  @State,
            //    ExecuteTime = @ExecuteTime,
            //    ErrorMessage = @ErrorMessage
            //where ID = @ID";
            //
            //            ICommand cmd = db.CreateTextCommand( sql )
            //                .AddParameter( "@State int", (int)task.State )
            //                .AddParameter( "@ExecuteTime datetime", task.ExecuteTime )
            //                .AddParameter( "@ErrorMessage nvarchar(max)", task.ErrorMessage )
            //                .AddParameter( "@ID int", task.ID );
            //
            //            db.Execute( cmd );
        }

        private static void SyncTaskCompleted( ISyncTask task )
        {
            //IRepository<ISyncTask> repository = RepositoryFactory.Instance.CreateRepository<ISyncTask>();
            //repository.Update( task );
            //NHibernateFactoryInvoker.Instance.FlushSession();

            //TODO: temp solution
            IDatabase db = new DatabaseFactory().CreateDatabase( "UUM" );

            string sql = @"update SyncTask
set State =  @State,
    ExecuteTime = @ExecuteTime,
    ErrorMessage = @ErrorMessage
where ID = @ID";

            ICommand cmd = db.CreateTextCommand( sql )
                .AddParameter( "@State int", (int)task.State )
                .AddParameter( "@ExecuteTime datetime", task.ExecuteTime )
                .AddParameter( "@ErrorMessage nvarchar(max)", task.ErrorMessage )
                .AddParameter( "@ID int", task.ID );

            db.Execute( cmd );
        }
    }
}