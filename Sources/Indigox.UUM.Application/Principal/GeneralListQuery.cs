using System;
using System.Collections.Generic;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.Application.DTO;
using NHibernate;

namespace Indigox.UUM.Application.Principal
{
    public class GeneralListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string OrganizationalUnitID { get; set; }
        public string[] TypeFilters { get; set; }
        public string QueryString { get; set; }

        private static readonly string DefaultPrincipalID = "OR1000000000";

        public override IList<OrganizationalPersonDTO> List()
        {
            ISession session = SessionFactories.Instance.Get( typeof( OrganizationalPersonDTO ).Assembly ).GetCurrentSession();
            {
                string sql = GetSql();

                if ( this.FetchSize > 0 )
                {
                    sql += "WHERE rownum > " + this.FirstResult + " AND rownum <= " + ( this.FirstResult + this.FetchSize );
                }

                ISQLQuery query = session.CreateSQLQuery( sql );

                query.AddEntity( typeof( OrganizationalPersonDTO ) );

                return query.List<OrganizationalPersonDTO>();
            }
        }

        public override int Size()
        {
            ISession session = SessionFactories.Instance.Get( typeof( OrganizationalPersonDTO ).Assembly ).GetCurrentSession();
            {
                string sql = GetSql();

                ISQLQuery query = session.CreateSQLQuery( sql );

                query.AddEntity( typeof( OrganizationalPersonDTO ) );

                return query.List<OrganizationalPersonDTO>().Count;
            }
        }

        private string GetSql()
        {
            string condition = "";

            string organizationalUnitID = this.OrganizationalUnitID;
            if ( String.IsNullOrEmpty( organizationalUnitID ) )
            {
                organizationalUnitID = DefaultPrincipalID;
            }
            condition += " AND Organization='" + organizationalUnitID + "' ";

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
                condition += " and ((" + string.Join( ") or (", typeFilterSqls.ToArray() ) + "))";
            }

            if ( !String.IsNullOrEmpty( this.QueryString ) )
            {
                condition += " AND ("
                    + " dbo.Principal.[Name] LIKE '%" + this.QueryString + "%'"
                    + " OR u.AccountName LIKE '%" + this.QueryString + "%'"
                    + " OR u.Mobile LIKE '%" + this.QueryString + "%'"
                    + " OR u.Telephone LIKE '%" + this.QueryString + "%'"
                    + " OR u.Fax LIKE '%" + this.QueryString + "%'"
                    + " ) ";
            }

            string sql = String.Format(@"
SELECT * FROM (
    SELECT ROW_NUMBER() OVER(ORDER BY (p.[Type] / 100) ASC, p.DisplayName ASC, p.OrderNum ASC, p.Name ASC) AS rownum,
           p.*,
           u.AccountName,
           u.Mobile,
           u.Telephone,
           u.Title,
           u.Fax,
           u.OtherContact,
           u.Profile
      FROM dbo.Principal p
      LEFT JOIN dbo.Users u ON p.ID=u.ID
     WHERE p.IsEnabled=1 AND p.IsDeleted=0 {0}
) a
", condition );

            return sql;
        }
    }
}