using System;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using NHibernate;

namespace Indigox.UUM.NHibernateImpl
{
    public class PrincipalProvider : IPrincipalProvider
    {
        public IPrincipal GetPrincipalByID( string id )
        {
            if ( String.IsNullOrEmpty( id ) )
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                Principal principal = session.Get<Principal>( id );
                if ( principal == null )
                {
                    throw new MemberNotFoundException( id, MemberNotFoundException.TYPE_ID );
                }
                return principal;
            }
        }
    }
}