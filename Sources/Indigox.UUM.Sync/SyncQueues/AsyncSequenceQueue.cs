using System;
using System.Collections.Generic;
using System.Threading;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Sync.SyncQueues
{
    /// <summary>
    /// 异步地，顺序的执行任务
    /// </summary>
    public class AsyncSequenceQueue : ISyncQueue
    {
        private bool isBusy = false;
        private bool isPaused = false;
        private Thread processQueueThread;
        private Queue<ISyncTask> tasks = new Queue<ISyncTask>();

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <remarks>
        /// 添加任务完任务后，立即尝试异步线程开始执行队列
        /// </remarks>
        public void Push( ISyncTask task )
        {
            tasks.Enqueue( task );
            TryBeginProcess();
        }

        public QueueState State
        {
            get
            {
                if ( isPaused )
                {
                    return QueueState.Paused;
                }
                if ( isBusy )
                {
                    return QueueState.Busy;
                }
                return QueueState.Idle;
            }
        }

        public enum QueueState
        {
            Idle,
            Busy,
            Paused
        }

        /// <summary>
        /// 尝试异步线程开始执行队列
        /// </summary>
        /// <remarks>
        /// 队列正在忙或已暂停时不会再开始队列
        /// </remarks>
        private void TryBeginProcess()
        {
            Log.Debug("TryBeginProcess...");
            if ( !isBusy && !isPaused )
            {
                BeginProcess();
            }
        }

        /// <summary>
        /// 异步线程开始执行队列
        /// </summary>
        private void BeginProcess()
        {
            Log.Debug("BeginProcess...");
            isBusy = true;
            processQueueThread = new Thread( Process );
            processQueueThread.Start();
        }

        /// <summary>
        /// 顺序执行队列
        /// </summary>
        private void Process()
        {
            while ( tasks.Count > 0 )
            {
                ISyncTask task = tasks.Dequeue();
                if (task.State == SyncTaskState.Ignore)
                {
                    continue;
                }
                if ( !TryExecuteTask( task ) )
                {
                    Pause();
                    task.Successed += new SyncTaskCompletedEvent( TaskSuccessed );
                    task.Ignored += new SyncTaskCompletedEvent(TaskSuccessed);
                    break;
                }
            }

            isBusy = false;
        }

        private bool TryExecuteTask( ISyncTask task )
        {
            Log.Debug( string.Format( "Begin execute task {{ ID:{0}, Tag:{1}, Desc:{2} }}.", task.ID, task.Tag, task.Description ) );
            task.Execute();

            if ( task.State == SyncTaskState.Failed )
            {
                Log.Debug( string.Format( "Execute task failed {{ ID:{0}, Tag:{1}, Desc:{2} }}.", task.ID, task.Tag, task.Description ) );
                return false;
            }
            else
            {
                Log.Debug( string.Format( "Execute task successed {{ ID:{0}, Tag:{1}, Desc:{2} }}.", task.ID, task.Tag, task.Description ) );
                return true;
            }
        }

        private void TaskIgnored(ISyncTask task)
        {
            Resume();
        }

        private void TaskSuccessed( ISyncTask task )
        {
            Resume();
        }

        /// <summary>
        /// 因出错暂停队列
        /// </summary>
        private void Pause()
        {
            if ( !isBusy )
            {
                throw new ApplicationException( "队列还未开始。" );
            }
            isPaused = true;
        }

        /// <summary>
        /// 修复错误后恢复队列
        /// </summary>
        private void Resume()
        {
            isPaused = false;
            BeginProcess();
        }
    }
}