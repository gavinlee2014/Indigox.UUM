using System;
using System.Collections.Generic;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.Application.DTO;
using NHibernate;

namespace Indigox.UUM.Application.Principal
{
    public class PrincipalListQuery : Indigox.Web.CQRS.GenericListQuery<PrincipalDTO>
    {
        public string OrganizationalUnitID { get; set; }
        public bool IncludeOrganizationalUnit { get; set; }
        public string[] TypeFilters { get; set; }
        public string QueryString { get; set; }

        public override IList<PrincipalDTO> List()
        {
            //IRepository<IPrincipal> repository = RepositoryFactory.Instance.CreateRepository<IPrincipal>();
            //return PrincipalDTO.ConvertToDTOs(repository.Find(Query.NewQuery));

            ISession session = SessionFactories.Instance.Get( typeof( PrincipalDTO ).Assembly ).GetCurrentSession();
            {
                string sql = GetSql();
                bool includeOrganizationalUnit = IsIncludeOrganizationalUnit();

                if ( this.FetchSize > 0 )
                {
                    int start = this.FirstResult;
                    int end = ( this.FirstResult + this.FetchSize );

                    if ( includeOrganizationalUnit )
                    {
                        start--;
                        end--;
                    }

                    sql += "where a.rownum > " + start + " and a.rownum <= " + end;
                }

                ISQLQuery query = session.CreateSQLQuery( sql );

                query.AddEntity( typeof( PrincipalDTO ) );

                //query.SetString("userid", user.ID);

                IList<PrincipalDTO> list = query.List<PrincipalDTO>();

                if ( includeOrganizationalUnit && this.FirstResult == 0 && !String.IsNullOrEmpty(OrganizationalUnitID))
                {
                    list.Insert( 0, PrincipalDTO.ConvertToDTO(
                        Indigox.Common.Membership.Principal.GetPrincipalByID( OrganizationalUnitID ) ) );
                }

                return list;
            }
        }

        private bool IsIncludeOrganizationalUnit()
        {
            if ( !IncludeOrganizationalUnit )
            {
                return false;
            }
            if ( TypeFilters == null )
            {
                return true;
            }
            foreach ( string typeFilter in TypeFilters )
            {
                if ( typeFilter == "OrganizationalUnit" )
                {
                    return true;
                }
            }
            return false;
        }

        public override int Size()
        {
            ISession session = SessionFactories.Instance.Get( typeof( PrincipalDTO ).Assembly ).GetCurrentSession();
            {
                string sql = GetSql();

                ISQLQuery query = session.CreateSQLQuery( sql );

                query.AddEntity( typeof( PrincipalDTO ) );

                //query.SetString("userid", user.ID);

                return query.List<PrincipalDTO>().Count;
            }
        }

        private string GetSql()
        {
            string sql = @"select * from (select ROW_NUMBER() OVER(ORDER BY (p.[Type] / 100) ASC, p.OrderNum ASC, p.Name ASC) as rownum,
                                                 p.*
                                            from [Principal] p
                                            left join [Users] u on p.ID=u.ID
                                           where p.IsEnabled=1 and p.IsDeleted=0 ";

            if ( !String.IsNullOrEmpty( this.OrganizationalUnitID ) )
            {
                sql = sql + "and Organization='" + this.OrganizationalUnitID + "' ";
            }

            if ( this.TypeFilters != null && this.TypeFilters.Length > 0 )
            {
                List<string> typeFilterSqls = new List<string>();
                foreach ( string typeFilter in this.TypeFilters )
                {
                    int[] range = PrincipalTypes.GetDiscriminatorRange( typeFilter );
                    if ( range == null )
                    {
                        continue;
                    }
                    else if ( range.Length == 1 )
                    {
                        typeFilterSqls.Add( "p.[Type] = " + range[ 0 ] + " " );
                    }
                    else if ( range.Length == 2 )
                    {
                        typeFilterSqls.Add( "p.[Type] between " + range[ 0 ] + " and " + range[ 1 ] + " " );
                    }
                }
                sql += " and ((" + string.Join( ") or (", typeFilterSqls.ToArray() ) + "))";
            }

            if ( !String.IsNullOrEmpty( this.QueryString ) )
            {
                sql += "and (p.[Name] like '%" + this.QueryString + "%' or u.accountname like '%" + this.QueryString + "%' ) ";
            }

            sql += ") a ";
            return sql;
        }
    }
}