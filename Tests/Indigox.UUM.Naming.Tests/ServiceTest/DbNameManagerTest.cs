using System;
using Indigox.TestUtility.TestFixtures;
using Indigox.UUM.Naming.Model;
using Indigox.UUM.Naming.Tests.TestFixtureProxies;
using NUnit.Framework;

namespace Indigox.UUM.Naming.Tests.ServiceTest
{
    [Category( "UserTest" )]
    [TestFixtureProxy(
         typeof( NHibernateRepositoryTestFixtureProxy ),
         typeof( StateContextTestFixtureProxy ) )]
    public class DbNameManagerTest : BaseTestFixture
    {
        [Test]
        [TestCase( "admin" )]
        public void ContainsTest( string name )
        {
            INameManager nameManager = new DbNameManager();
            Assert.True( nameManager.Contains( name ) );
        }
    }
}