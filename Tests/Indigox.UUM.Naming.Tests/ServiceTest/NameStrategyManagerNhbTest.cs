using System;
using Indigox.TestUtility.TestFixtures;
using Indigox.UUM.Naming.Factory;
using Indigox.UUM.Naming.Tests.TestFixtureProxies;
using NUnit.Framework;

namespace Indigox.UUM.Naming.Tests.ServiceTest
{
    [Category( "UserTest" )]
    [TestFixtureProxy(
         typeof( NHibernateRepositoryTestFixtureProxy ),
         typeof( StateContextTestFixtureProxy ) )]
    public class NameStrategyManagerNhbTest : BaseTestFixture
    {
        [Test]
        public void Test()
        {
            var strategies = NameStrategyManager.Instance.GetNameStrategies();
            Assert.Greater( strategies.Count, 0 );
        }
    }
}