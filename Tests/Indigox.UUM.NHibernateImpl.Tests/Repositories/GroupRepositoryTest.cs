using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Repositories
{
    [TestFixture]
    public class GroupRepositoryTest : BaseRepositoryTestFixture
    {
        [Test]
        public void AddGroup()
        {
            IRepository<Group> repos = RepositoryFactory.Instance.CreateRepository<Group>();

            Group group = new Group()
            {
                ID = "DG9000000001",
                Name = "NUnitTestUser",
                FullName = "NUnitTestUser",
                Email = "NUnitTestUser@test.com"
            };

            repos.Add( group );
        }
    }
}