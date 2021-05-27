using System;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Exceptions;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership.Providers;
using Indigox.Common.NHibernateFactories;
using Indigox.Common.Utilities;
using NHibernate;

namespace Indigox.UUM.NHibernateImpl
{
    public class OrganizationalUnitProvider : IOrganizationalUnitProvider
    {
        public IOrganizationalUnit GetOrganizationalUnitByID( string id )
        {
            if ( String.IsNullOrEmpty( id ) )
            {
                return null;
            }

            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                OrganizationalUnit org = session.Get<OrganizationalUnit>( id );
                if ( org == null )
                {
                    throw new MemberNotFoundException( id, MemberNotFoundException.TYPE_ID );
                }
                return org;
            }
        }

        public IPrincipal GetManager( IOrganizationalUnit organization )
        {
            IDepartment dept = organization as IDepartment;
            if ( dept != null )
            {
                return dept.Manager;
            }
            ISection section = organization as ISection;
            if ( section != null )
            {
                return section.Manager;
            }
            return null;
        }

        public IPrincipal GetDirector( IOrganizationalUnit organization )
        {
            IDepartment dept = organization as IDepartment;
            if (dept == null)
            {
                return null;
            }
            else
            {
                return dept.Director;
            }
        }

        public ICorporation GetCorporation()
        {
            ISession session = SessionFactories.Instance.Get( typeof( IPrincipal ).Assembly ).GetCurrentSession();
            {
                Corporation corp = session.CreateCriteria<Corporation>().UniqueResult<Corporation>();
                if ( corp == null )
                {
                    throw new NotFoundCorporationException();
                }
                return corp;
            }
        }
    }
}