using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Providers
{
    [TestFixture]
    public class RoleProviderTest : BaseProviderTestFixture
    {
        [Test]
        public void GetPositionsFromRelativePosition()
        {
            ProviderFactory providerFactory = new ProviderFactory();

            //IOrganizationalUnit org = providerFactory.GetOrganizationProvider().GetOrganizationByID( "" );

            IOrganizationalPerson user = (IOrganizationalPerson)providerFactory.GetUserProvider().GetUserByAccount( "yfxue" );

            IRole role = providerFactory.GetRoleProvider().GetRoleByID( "PS1000000159" );

            IList<IOrganizationalRole> organizationalRoles = providerFactory.GetRoleProvider().GetOrganizationalRoleFromRole( user, role );

            Assert.AreEqual( 1, organizationalRoles.Count );
        }
    }
}