using System;
using Indigox.Common.Membership.Providers;

namespace Indigox.UUM.NHibernateImpl
{
    public class ProviderFactory : IProviderFactory
    {
        public IUserProvider GetUserProvider()
        {
            return new UserProvider();
        }

        public IOrganizationalRoleProvider GetOrganizationalRoleProvider()
        {
            return new OrganizationalRoleProvider();
        }

        public IGroupProvider GetGroupProvider()
        {
            return new GroupProvider();
        }

        public IOrganizationalUnitProvider GetOrganizationalUnitProvider()
        {
            return new OrganizationalUnitProvider();
        }

        public IReportingHierarchyProvider GetReportingHierarchyProvider()
        {
            return new ReportingHierarchyProvider();
        }

        public IRoleProvider GetRoleProvider()
        {
            return new RoleProvider();
        }

        public IPrincipalProvider GetPrincipalProvider()
        {
            return new PrincipalProvider();
        }
    }
}