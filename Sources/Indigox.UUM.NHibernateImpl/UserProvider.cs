using System;
using System.Collections.Generic;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using Indigox.Common.Utilities;
using NHibernate;

namespace Indigox.UUM.NHibernateImpl
{
    public class UserProvider : IUserProvider
    {
        public IUser GetUserByID( string id )
        {
            if ( String.IsNullOrEmpty( id ) )
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                User user = session.Get<User>( id );
                if ( user == null )
                {
                    throw new MemberNotFoundException( id, MemberNotFoundException.TYPE_ID );
                }
                return user;
            }
        }

        public IUser GetUserByAccount( string account )
        {
            if ( String.IsNullOrEmpty( account ) )
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                User user = (User)session
                    .CreateQuery( "from User where AccountName = :account and IsEnabled = 1 and IsDeleted = 0 " )
                    .SetString( "account", account )
                    .UniqueResult();
                if ( user == null )
                {
                    throw new MemberNotFoundException( account, MemberNotFoundException.TYPE_Account );
                }
                return user;
            }
        }

        public IList<IUser> GetAllUsers()
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                IList<IUser> user = CollectionUtil.ConvertToList<IUser>(
                    session.CreateQuery( "from User where IsEnabled = 1 and IsDeleted = 0 " )
                           .List<User>() );

                return user;
            }
        }
    }
}