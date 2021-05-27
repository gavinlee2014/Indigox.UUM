using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Repositories
{
    [TestFixture]
    public class PrincipalRepositoryTest : BaseRepositoryTestFixture
    {
        [Test]
        [TestCase( "UR0000000001" )]
        public void GetOrganizationalPersonByID( string id )
        {
            IRepository<IPrincipal> repos = RepositoryFactory.Instance.CreateRepository<IPrincipal>();
            IPrincipal principal = repos.Get( id );
            Assert.NotNull( principal );
        }
    }
}