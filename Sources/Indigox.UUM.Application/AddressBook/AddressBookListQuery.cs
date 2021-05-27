using System;
using System.Collections.Generic;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.Application.DTO;
using NHibernate;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.AddressBook
{
    public class AddressBookListQuery : Indigox.Web.CQRS.GenericListQuery<AddressBookDTO>
    {
        public string OrganizationalUnitID { get; set; }
        public string QueryString { get; set; }

        private static readonly string DefaultPrincipalID = "OR1000000000";

        public override IList<AddressBookDTO> List()
        {
            ISession session = SessionFactories.Instance.Get(typeof(AddressBookDTO).Assembly).GetCurrentSession();
            {
                string sql = GetSql();

                if (this.FetchSize > 0)
                {
                    sql += "WHERE rownum > " + this.FirstResult + " AND rownum <= " + (this.FirstResult + this.FetchSize);
                }

                ISQLQuery query = session.CreateSQLQuery(sql);

                query.AddEntity(typeof(AddressBookDTO));

                return query.List<AddressBookDTO>();
            }
        }

        public override int Size()
        {
            ISession session = SessionFactories.Instance.Get(typeof(AddressBookDTO).Assembly).GetCurrentSession();
            {
                string sql = GetSql();

                ISQLQuery query = session.CreateSQLQuery(sql);

                query.AddEntity(typeof(AddressBookDTO));

                return query.List<AddressBookDTO>().Count;
            }
        }

        private string GetSql()
        {
            string organizationalUnitID = this.OrganizationalUnitID;
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                organizationalUnitID = DefaultPrincipalID;
            }
            string org = String.Format("WHERE Parent='{0}'", organizationalUnitID);

            string condition = "";

            if (!String.IsNullOrEmpty(this.QueryString))
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
WITH CTE AS
(
    SELECT Parent,Child
    FROM dbo.Membership
    {0}
    UNION ALL
    SELECT dbo.Membership.Parent,dbo.Membership.Child
    FROM CTE JOIN dbo.Membership
    ON dbo.Membership.Parent = CTE.Child
)

SELECT * FROM
(
    SELECT *,ROW_NUMBER() OVER(ORDER BY (a.[Type] / 100) ASC, a.OrderNum ASC, a.DisplayName, a.Name ASC) AS rownum FROM
    (
        SELECT DISTINCT 
            dbo.Principal.*,
            ISNULL(t.FullName,t.DisplayName) AS OrganizationFullName,
            u.AccountName,
            u.Mobile,
            u.Telephone,
            u.Title,
            u.Fax,
            u.OtherContact,
            u.Profile
        FROM dbo.Principal
        --JOIN dbo.Membership on (dbo.Principal.ID=dbo.Membership.Child or dbo.Principal.ID=dbo.Membership.Parent)
        LEFT JOIN dbo.Principal t on dbo.Principal.Organization=t.ID
        JOIN dbo.Users u on dbo.Principal.ID=u.ID
        JOIN CTE as org on dbo.Principal.ID = org.Child
        WHERE dbo.Principal.IsEnabled=1 and dbo.Principal.[Type] between 200 and 299 {1}
    ) a
) b
", org, condition);

            return sql;
        }
    }
}