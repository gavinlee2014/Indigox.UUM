using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Repositories
{
    [TestFixture]
    public class OrganizationalUnitRepositoryTest : BaseRepositoryTestFixture
    {
        [Test]
        public void AddOrganizationalUnit()
        {
            IRepository<OrganizationalUnit> repos = RepositoryFactory.Instance.CreateRepository<OrganizationalUnit>();

            OrganizationalUnit org = new OrganizationalUnit()
            {
                ID = "OR9000000001",
                Name = "NUnitTest",
                FullName = "NUnitTest"
            };

            repos.Add( org );
        }

        [Test]
        [TestCase( "OR1000000000" )]
        public void ListOrganizationalUnit( string orgId )
        {
            IRepository<OrganizationalUnit> repos = RepositoryFactory.Instance.CreateRepository<OrganizationalUnit>();

            OrganizationalUnit root = repos.Get( orgId );

            IList<OrganizationalUnit> list = repos.Find( Query.NewQuery
                .FindByCondition( Specification.Equal( "Organization", root ) )
                .OrderByAsc( "Name" ) );
        }

        [Test]
        [TestCase("OR1000000113")]
        public void TestUpdte(string orgId)
        {
            IRepository<OrganizationalUnit> ouRepos = RepositoryFactory.Instance.CreateRepository<OrganizationalUnit>();
            IRepository<OrganizationalPerson> opRepos = RepositoryFactory.Instance.CreateRepository<OrganizationalPerson>();

            Department root = ouRepos.Get(orgId) as Department;

            root.Manager = opRepos.Get("UR0000000001");
            root.Director = opRepos.Get("UR0000000001");

            ouRepos.Update(root);
        }
    }
}