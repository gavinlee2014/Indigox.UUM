using System;
using System.Threading;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.SyncQueues;
using Indigox.UUM.Sync.Tasks;
using Indigox.UUM.Sync.Tests.Model;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.SyncQueues
{
    internal class AsyncSequenceQueueTest
    {
        [Test]
        public void TestCommonTask()
        {
            AsyncSequenceQueue queue = new AsyncSequenceQueue();
            int taskExecuted = 0;

            for ( int i = 0; i < 3; i++ )
            {
                ISyncTask task = new SyncTask();
                task.Executor = new CallbackExecutor( () =>
                {
                    Console.WriteLine( "sleep 100 ms..." );
                    Thread.Sleep( 100 );
                    taskExecuted++;
                } );
                queue.Push( task );
            }

            Assert.AreEqual( AsyncSequenceQueue.QueueState.Busy, queue.State, "queue is busy." );
            Assert.AreEqual( 0, taskExecuted, "queue has begun, but no task execution completed." );

            // wait for queue processed
            Thread.Sleep( 500 );

            Assert.AreEqual( AsyncSequenceQueue.QueueState.Idle, queue.State, "queue is idle." );
            Assert.AreEqual( 3, taskExecuted, "all tasks executed." );
        }

        [Test]
        public void TestErrorTask()
        {
            int idSeed = 1;

            AsyncSequenceQueue queue = new AsyncSequenceQueue();
            int taskExecuted = 0;

            for ( int i = 0; i < 3; i++ )
            {
                ISyncTask task = new SyncTask();
                task.Executor = new CallbackExecutor( () =>
                {
                    Console.WriteLine( "sleep 100 ms..." );
                    Thread.Sleep( 100 );
                    taskExecuted++;
                } );
                queue.Push( task );
            }

            ISyncTask errorTask = new SyncTask();
            errorTask.Executor = new CallbackExecutor( () =>
            {
                Console.WriteLine( "Occurs some error." );
                taskExecuted++;
                throw new ApplicationException( "Occurs some error." );
            } );
            queue.Push( errorTask );

            ISyncTask followToErrorTask = new SyncTask();
            followToErrorTask.Executor = new CallbackExecutor( () =>
            {
                Console.WriteLine( "A task follows errorTask..." );
                taskExecuted++;
            } );
            queue.Push( followToErrorTask );

            Thread.Sleep(1);

            Assert.AreEqual( AsyncSequenceQueue.QueueState.Busy, queue.State, "queue is busy." );
            Assert.AreEqual( 0, taskExecuted, "queue has begun, but no task execution completed." );

            // wait for queue processed
            Thread.Sleep( 500 );

            Assert.AreEqual( AsyncSequenceQueue.QueueState.Paused, queue.State, "queue is paused." );
            Assert.AreEqual( SyncTaskState.Failed, errorTask.State, "errorTask is failed." );
            Assert.AreEqual( "Occurs some error.", errorTask.ErrorMessage, "errorTask.ErrorMessage is ok." );
            Assert.AreEqual( SyncTaskState.Initiated, followToErrorTask.State, "followToErrorTask is wait queue resume." );
            Assert.AreEqual( 4, taskExecuted );

            // manual reset errorTask successed
            errorTask.SetSuccessed();

            // wait for queue processed
            Thread.Sleep( 100 );

            Assert.AreEqual( AsyncSequenceQueue.QueueState.Idle, queue.State, "queue is idle." );
            Assert.AreEqual( SyncTaskState.Successed, errorTask.State, "errorTask reset as successed." );
            Assert.AreEqual( SyncTaskState.Successed, followToErrorTask.State, "followToErrorTask is executed." );
            Assert.AreEqual( 5, taskExecuted, "all tasks executed." );
        }
    }
}