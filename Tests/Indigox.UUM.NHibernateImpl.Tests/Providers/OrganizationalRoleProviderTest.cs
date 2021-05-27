using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Providers
{
    [TestFixture]
    public class OrganizationalRoleProviderTest : BaseProviderTestFixture
    {
        [Test]
        public void TestGetOrganizationalRoleByOrganizationalPerson()
        {
            ProviderFactory providerFactory = new ProviderFactory();

            //IOrganizationalUnit org = providerFactory.GetOrganizationProvider().GetOrganizationByID( "" );

            IOrganizationalPerson user = (IOrganizationalPerson)providerFactory.GetUserProvider().GetUserByAccount("yfxue");

            IList<IOrganizationalRole> roles = providerFactory.GetOrganizationalRoleProvider().GetOrganizationalRoleByOrganizationalPerson(user.ID);

        }
    }
}
