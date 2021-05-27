using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Repositories
{
    [TestFixture]
    public class UserRepositoryTest : BaseRepositoryTestFixture
    {
        [Test]
        public void AddOrganizationalPerson()
        {
            IRepository<User> repos = RepositoryFactory.Instance.CreateRepository<User>();

            User user = new User()
            {
                ID = "UR9000000001",
                Name = "NUnitTestUser",
                FullName = "NUnitTestUser",
                AccountName = "NUnitTestUser",
                Email = "NUnitTestUser@test.com",
                Mobile = "18522330442"
            };

            repos.Add( user );
        }

        [Test]
        [TestCase( "UR0000000001" )]
        [TestCase( "UR1000000005" )]
        public void GetOrganizationalPersonByID( string id )
        {
            IRepository<User> repos = RepositoryFactory.Instance.CreateRepository<User>();
            User user = repos.Get( id );
            Assert.NotNull( user );
            Assert.IsNotNullOrEmpty( user.AccountName, "AccountName is null or empty." );
            Assert.IsNotNullOrEmpty( user.Mobile, "Mobile is null or empty." );
        }

        [Test]
        [TestCase( "admin" )]
        [TestCase( "hao" )]
        public void GetOrganizationalPersonByAccount( string account )
        {
            IRepository<User> repos = RepositoryFactory.Instance.CreateRepository<User>();
            User user = repos.First( Query.NewQuery.FindByCondition( Specification.Equal( "AccountName", account ) ) );
            Assert.NotNull( user );
        }
    }
}