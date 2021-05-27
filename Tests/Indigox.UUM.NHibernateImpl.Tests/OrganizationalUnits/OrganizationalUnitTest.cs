using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Interfaces;
using Indigox.TestUtility.TestFixtures;
using Indigox.UUM.NHibernateImpl.NUnitTest.TestFixtureProxies;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.OrganizationalUnits
{
    [Category( "UserTest" )]
    [TestFixture]
    [TestFixtureProxy( typeof( NHibernateRepositoryTestFixtureProxy ),
                typeof( StateContextTestFixtureProxy ) )]
    public class OrganizationalUnitTest : BaseTestFixture
    {
        [Test]
        public void TestGetAllUsers()
        {
            IRepository<OrganizationalUnit> repos = RepositoryFactory.Instance.CreateRepository<OrganizationalUnit>();

            OrganizationalUnit root = repos.Get( "OR1000000000" );
            IList<IOrganizationalPerson> list = root.GetAllUsers();

            foreach ( IOrganizationalPerson item in list )
            {
                Console.WriteLine( item.AccountName );
            }
        }
    }
}