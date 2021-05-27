using System;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using NHibernate;

namespace Indigox.UUM.NHibernateImpl
{
    public class GroupProvider : IGroupProvider
    {
        public IGroup GetGroupByID( string id )
        {
            if ( String.IsNullOrEmpty( id ) )
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                Group group = session.Get<Group>( id );
                if ( group == null )
                {
                    throw new MemberNotFoundException( id, MemberNotFoundException.TYPE_ID );
                }
                return group;
            }
        }
    }
}