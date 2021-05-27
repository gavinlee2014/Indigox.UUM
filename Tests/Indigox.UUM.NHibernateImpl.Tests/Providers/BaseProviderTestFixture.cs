using System;
using Indigox.TestUtility.TestFixtures;
using Indigox.UUM.NHibernateImpl.NUnitTest.TestFixtureProxies;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Providers
{
    [Category( "UserTest" )]
    [TestFixtureProxy( typeof( StateContextTestFixtureProxy ),
                       typeof( NHibernateRepositoryTestFixtureProxy ) )]
    public class BaseProviderTestFixture : BaseTestFixture
    {
        protected override void SetUp()
        {
            base.SetUp();
        }
    }
}