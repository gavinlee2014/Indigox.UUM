using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using NHibernate;

namespace Indigox.UUM.NHibernateImpl
{
    public class OrganizationalRoleProvider : IOrganizationalRoleProvider
    {
        public IOrganizationalRole GetOrganizationalRoleByID( string id )
        {
            if ( String.IsNullOrEmpty( id ) )
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                OrganizationalRole position = session.Get<OrganizationalRole>( id );
                if ( position == null )
                {
                    throw new MemberNotFoundException( id, MemberNotFoundException.TYPE_ID );
                }
                return position;
            }
        }

        public IList<IOrganizationalRole> GetOrganizationalRoleByOrganizationalPerson( string personId )
        {
            List<IOrganizationalRole> list = new List<IOrganizationalRole>();
            IUser user = RepositoryFactory.Instance.CreateRepository<IUser>().Get( personId );
            foreach ( IContainer item in user.MemberOf )
            {
                IOrganizationalRole role = item as IOrganizationalRole;
                if ( role != null )
                {
                    list.Add( role );
                }
            }
            return list;
        }
    }
}