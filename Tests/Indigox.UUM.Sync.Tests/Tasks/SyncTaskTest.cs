using System;
using System.Threading;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Tasks;
using Indigox.UUM.Sync.Tests.Model;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.Tasks
{
    internal class SyncTaskTest
    {
        [Test]
        public void TestDependencies()
        {
            string s = "";

            SyncTask dependency = new SyncTask()
            {
                Executor = new CallbackExecutor( () =>
                {
                    Thread.Sleep( 100 );
                    s += "A";
                } )
            };

            SyncTask task = new SyncTask()
            {
                Executor = new CallbackExecutor( () =>
                {
                    Thread.Sleep( 100 );
                    s += "B";
                } ),
                Dependencies = new ISyncTask[] { dependency }
            };

            Thread threadA = new Thread( () =>
            {
                dependency.Execute();
            } );

            Thread threadB = new Thread( () =>
            {
                task.Execute();
            } );

            threadB.Start();

            // wait for execute
            Thread.Sleep( 200 );

            Assert.AreEqual( SyncTaskState.Initiated, task.State, "dependencies is not ready, wait dependencies..." );

            threadA.Start();

            // wait for execute
            Thread.Sleep( 300 );

            Assert.AreEqual( SyncTaskState.Successed, task.State, "task successed." );
            Assert.AreEqual( "AB", s, "task executed sequence is ok." );
        }
    }
}