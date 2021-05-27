using System;
using System.Collections.Generic;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using Indigox.Common.Utilities;
using Indigox.UUM.NHibernateImpl.Model;
using NHibernate;
using NHibernate.Criterion;

namespace Indigox.UUM.NHibernateImpl
{
    public class ReportingHierarchyProvider : IReportingHierarchyProvider
    {
        public IOrganizationalHolder GetManagerCrossLevel( IReportingHierarchy hierarchy, IOrganizationalHolder user, int level )
        {
            string managerId = user.ID;
            for ( int i = level; i >= 1; i-- )
            {
                managerId = GetMangerID( hierarchy, user );
                user = (IOrganizationalHolder)Principal.GetPrincipalByID( managerId );
                if ( managerId == null )
                {
                    return null;
                }
            }
            return (IOrganizationalHolder)Principal.GetPrincipalByID( managerId );
        }

        public IList<IOrganizationalHolder> GetDirectReporters( IReportingHierarchy hierarchy, IOrganizationalHolder user )
        {
            List<IOrganizationalHolder> users = new List<IOrganizationalHolder>();
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                ICriteria criteria = session.CreateCriteria( typeof( ReportingHierarchyUser ) );
                criteria.Add( Restrictions.And(
                        Restrictions.Eq( "ManagerID", user.ID ),
                        Restrictions.Eq( "ReportingHierarchyID", hierarchy.ID )
                    ) );
                var list = criteria.List<ReportingHierarchyUser>();
                foreach ( ReportingHierarchyUser item in list )
                {
                    users.Add( (IOrganizationalHolder)Principal.GetPrincipalByID( item.UserID ) );
                }
            }
            return users;
        }

        public IList<IOrganizationalHolder> GetReportingLine( IReportingHierarchy hierarchy, IOrganizationalHolder user )
        {
            List<IOrganizationalHolder> users = new List<IOrganizationalHolder>();
            users.Add( user );
            string managerId = GetMangerID( hierarchy, user );
            while ( managerId != null )
            {
                users.Add( (IOrganizationalHolder)Principal.GetPrincipalByID( managerId ) );
                managerId = GetMangerID( hierarchy, user );
            }
            return users;
        }

        public void SetManager( IReportingHierarchy hierarchy, IOrganizationalHolder user, IOrganizationalHolder manager )
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                session.SaveOrUpdate( user );
            }
        }

        public IReportingHierarchy GetReportingHierarchyById( int id )
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                ReportingHierarchy r = session.Get<ReportingHierarchy>( id );
                if ( r == null )
                {
                    throw new MemberNotFoundException( id, MemberNotFoundException.TYPE_ID );
                }
                return r;
            }
        }

        public IList<IReportingHierarchy> GetAllReportingHierarchy()
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                return CollectionUtil.ConvertToList<IReportingHierarchy>(
                    session.CreateCriteria<ReportingHierarchy>().List<ReportingHierarchy>() );
            }
        }

        public void AddReportingHierarchy( IReportingHierarchy hierarchy )
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                session.Save( hierarchy );
            }
        }

        public void RemoveReportingHierarchy( IReportingHierarchy hierarchy )
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                session.Delete( hierarchy );
            }
        }

        public void UpdateReportingHierarchy( IReportingHierarchy hierarchy )
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                session.Update( hierarchy );
            }
        }

        private string GetMangerID( IReportingHierarchy hierarchy, IOrganizationalHolder user )
        {
            string managerId = null;
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                ICriteria criteria = session.CreateCriteria( typeof( ReportingHierarchyUser ) );
                criteria.Add( Restrictions.And(
                        Restrictions.Eq( "UserID", user.ID ),
                        Restrictions.Eq( "ReportingHierarchyID", hierarchy.ID )
                    ) );
                var list = criteria.List<ReportingHierarchyUser>();
                if ( list.Count == 1 )
                {
                    managerId = list[ 0 ].ManagerID;
                }
            }
            return managerId;
        }
    }
}