using System;
using System.Collections.Generic;
using System.Threading;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Sync.Tasks
{
    public class SyncTask : ISyncTask
    {
        private int id;
        private string tag;
        private string description;
        private string errorMessage;
        private SyncTaskState state = SyncTaskState.Initiated;
        private ISyncContext context;
        private ISyncExecutor executor;
        private DateTime createTime = DateTime.Now;
        private DateTime? executeTime;
        private IList<ISyncTask> dependencies;

        public int ID
        {
            get { return this.id; }
        }

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public SyncTaskState State
        {
            get { return state; }
        }

        public ISyncContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public ISyncExecutor Executor
        {
            get { return executor; }
            set { executor = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public DateTime? ExecuteTime
        {
            get { return executeTime; }
            set { executeTime = value; }
        }

        public IList<ISyncTask> Dependencies
        {
            get { return dependencies; }
            set { dependencies = value; }
        }

        public event SyncTaskCompletedEvent Failed;

        public event SyncTaskCompletedEvent Successed;

        public event SyncTaskCompletedEvent Ignored;

        public void Execute()
        {
            try
            {
                WaitDependencies();
                executor.Execute( context );
                SetSuccessed();
            }
            catch ( Exception ex )
            {
                ApplicationException wrappedEx = new ApplicationException( string.Format( "执行任务失败！Tag: {0}, 任务：{1}", this.Tag, this.Description ), ex );
                Log.Error( wrappedEx.ToString() );
                errorMessage = ex.Message;
                SetFailed();
            }
        }

        public void SetFailed()
        {
            executeTime = DateTime.Now;
            state = SyncTaskState.Failed;

            if ( Failed != null )
            {
                Failed.Invoke( this );
            }
        }

        public void SetSuccessed()
        {
            executeTime = DateTime.Now;
            state = SyncTaskState.Successed;
            errorMessage = null;

            if ( Successed != null )
            {
                Successed.Invoke( this );
            }
        }

        public void SetIgnore()
        {
            this.state = SyncTaskState.Ignore;
            errorMessage = null;

            if (Ignored != null)
            {
                Ignored.Invoke(this);
            }
        }

        private void WaitDependencies()
        {
            if ( dependencies == null )
            {
                return;
            }

            List<WaitHandle> waitHandlers = new List<WaitHandle>();

            foreach ( ISyncTask dependency in dependencies )
            {
                if(dependency.State == SyncTaskState.Ignore)
                {
                    return;
                }
                if ( dependency.State != SyncTaskState.Successed )
                {
                    // create WaitHandler
                    AutoResetEvent waitHandler = new AutoResetEvent( false );

                    dependency.Successed += new SyncTaskCompletedEvent( ( task ) =>
                    {
                        // release WaitHandler
                        waitHandler.Set();
                    } );

                    /*
                     * 修改时间：2018-09-19
                     * 修改人：曾勇
                     * 修改内容：任务A的依赖任务B如果被忽略，那么任务A也将被执行
                     * 避免情况：AD任务忽略，所有的同步队列将变成等待状态，除非应用启动
                     */
                    dependency.Ignored += new SyncTaskCompletedEvent((task) =>
                    {
                        waitHandler.Set();
                    });


                    // add WaitHandler
                    waitHandlers.Add( waitHandler );
                }
            }

            if ( waitHandlers.Count > 0 )
            {
                // wait for all WaitHandler release
                WaitHandle.WaitAll( waitHandlers.ToArray() );
            }
        }
    }
}