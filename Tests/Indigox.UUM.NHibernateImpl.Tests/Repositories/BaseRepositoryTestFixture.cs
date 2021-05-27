using System;
using Indigox.TestUtility;
using Indigox.TestUtility.Expressions;
using Indigox.TestUtility.TestFixtures;
using Indigox.UUM.NHibernateImpl.NUnitTest.TestFixtureProxies;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Repositories
{
    [Category( "UserTest" )]
    [TestFixtureProxy( typeof( StateContextTestFixtureProxy ),
                       typeof( NHibernateRepositoryTestFixtureProxy ) )]
    public class BaseRepositoryTestFixture : BaseTestFixture
    {
        protected override void SetUp()
        {
            base.SetUp();
            ClearTestData();
        }

        protected virtual void ClearTestData()
        {
            DbUtil.Get( "UUM" ).ClearData( "Users", new SqlExpression( "substring(id,3,5)='90000'" ) );
            DbUtil.Get( "UUM" ).ClearData( "Principal", new SqlExpression( "substring(id,3,5)='90000'" ) );
        }
    }
}