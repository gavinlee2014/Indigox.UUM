using System;
using System.Collections.Generic;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using NHibernate;

namespace Indigox.UUM.NHibernateImpl
{
    public class RoleProvider : IRoleProvider
    {
        public IRole GetRoleByID(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get(typeof(IPrincipal).Assembly).GetCurrentSession();
            {
                Role role = session.Get<Role>(id);
                if (role == null)
                {
                    throw new MemberNotFoundException(id, MemberNotFoundException.TYPE_ID);
                }
                return role;
            }
        }

        public IList<IOrganizationalRole> GetOrganizationalRoleFromRole(IOrganizationalHolder holder, IRole role)
        {
            List<IOrganizationalRole> list = new List<IOrganizationalRole>();
            if (holder.Organization == null)
            {
                return list;
            }

            Type organizationalUnitType = Role.GetRoleLevelType(role.Level);

            IOrganizationalUnit organizationalUnit = holder.Organization;

            while (list.Count == 0)
            {
                while (!organizationalUnit.GetType().Equals(organizationalUnitType))
                {
                    organizationalUnit = organizationalUnit.Organization;
                    if (organizationalUnit == null)
                    {
                        return list;
                    }
                }

                foreach (IOrganizationalRole item in role.Members)
                {
                    if (this.InOrganization(item, organizationalUnit))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;

            //ISession session = SessionFactories.Instance.Get(typeof(IPrincipal).Assembly).GetCurrentSession();
            //{
            //    IList<IOrganizationalRole> list = session
            //        .CreateQuery("from OrganizationalRole where Organization=:org and Role=:role and IsEnabled=1 and IsDeleted=0 ")
            //        .SetString("org", holder.Organization.ID)
            //        .SetString("role", role.ID)
            //        .List<IOrganizationalRole>();

            //    return list;
            //}
        }

        private bool InOrganization(IOrganizationalRole role, IOrganizationalUnit container)
        {
            IOrganizationalUnit temp = role.Organization;
            while (!temp.Equals(container))
            {
                temp = temp.Organization;
                if (temp == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}