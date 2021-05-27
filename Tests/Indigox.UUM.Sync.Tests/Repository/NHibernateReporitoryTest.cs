using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.TestUtility.TestFixtures;
using Indigox.UUM.Sync.Contexts;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.Tasks;
using Indigox.UUM.Sync.Tests.Model;
using Indigox.UUM.Sync.Tests.TestFixtureProxies;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.Repository
{
    [Category( "UserTest" )]
    [TestFixtureProxy(
        typeof( NHibernateRepositoryTestFixtureProxy ),
        typeof( StateContextTestFixtureProxy ) )]
    public class NHibernateReporitoryTest : BaseTestFixture
    {
        [Test]
        public void TestSyncTask()
        {
            HashSyncContext context = new HashSyncContext();
            context.Set( "kk", "v1" );
            context.Set( "dd", new Dictionary<string, object>()
            {
                { "i1", 190 },
                { "s1", "beijing" }
            } );

            SyncTask task1 = new SyncTask()
            {
                Tag = "test",
                Description = "add user",
                Context = context,
                CreateTime = DateTime.Now.AddHours( -4 ),
                ExecuteTime = DateTime.Now.AddHours( -3 ),
                Executor = new SerializableExecutor( "user", "create", "http://a.cn/user.asmx" )
            };
            task1.SetFailed();

            SyncTask task2 = new SyncTask()
            {
                Tag = "test",
                Description = "add user 2",
                Context = context,
                CreateTime = DateTime.Now,

                //ExecuteTime = new DateTime( 1900, 1, 1 ),
                Executor = new SerializableExecutor( "user", "create", "http://a.cn/user.asmx" ),
                Dependencies = new ISyncTask[] { task1 }
            };

            IRepository<ISyncTask> repository = RepositoryFactory.Instance.CreateRepository<ISyncTask>();
            repository.Add( task1 );
            repository.Add( task2 );
            Assert.Pass();
        }

        [Test]
        public void TestSysKeyMapping()
        {
            SysKeyMapping mapping = new SysKeyMapping( "100", "x988", new SysConfiguration() { ID = 1000 } );

            IRepository<SysKeyMapping> repository = RepositoryFactory.Instance.CreateRepository<SysKeyMapping>();
            repository.Add( mapping );
            Assert.Pass();
        }
    }
}