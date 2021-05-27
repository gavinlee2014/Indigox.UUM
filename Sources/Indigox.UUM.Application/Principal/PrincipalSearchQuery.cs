using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using NHibernate;
using Indigox.Common.NHibernateFactories;

namespace Indigox.UUM.Application.Principal
{
    public class PrincipalSearchQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string QueryString { get; set; }

        public override IList<OrganizationalPersonDTO> List()
        {
            if (string.IsNullOrEmpty(QueryString)) return new List<OrganizationalPersonDTO>();

            ISession session = SessionFactories.Instance.Get(typeof(OrganizationalPersonDTO).Assembly).GetCurrentSession();
            {
                string sql = GetSql();

                if (this.FetchSize > 0)
                {
                    sql += "WHERE rownum > " + this.FirstResult + " AND rownum <= " + (this.FirstResult + this.FetchSize);
                }

                ISQLQuery query = session.CreateSQLQuery(sql);

                query.AddEntity(typeof(OrganizationalPersonDTO));

                return query.List<OrganizationalPersonDTO>();
            }
        }

        public override int Size()
        {
            if (string.IsNullOrEmpty(QueryString)) return 0;
            ISession session = SessionFactories.Instance.Get(typeof(OrganizationalPersonDTO).Assembly).GetCurrentSession();
            {
                string sql = GetSql();

                ISQLQuery query = session.CreateSQLQuery(sql);

                query.AddEntity(typeof(OrganizationalPersonDTO));

                return query.List<OrganizationalPersonDTO>().Count;
            }
        }

        private string GetSql()
        {
            string condition = "";

            if (!String.IsNullOrEmpty(this.QueryString))
            {
                condition += " AND ("
                    + " p.[Name] LIKE '%" + this.QueryString + "%'"
                    + " OR u.AccountName LIKE '%" + this.QueryString + "%'"
                    + " OR u.Mobile LIKE '%" + this.QueryString + "%'"
                    + " OR u.Telephone LIKE '%" + this.QueryString + "%'"
                    + " OR u.Fax LIKE '%" + this.QueryString + "%'"
                    + " OR p.Email LIKE '%" + this.QueryString + "%'"
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
                     WHERE p.IsEnabled=1 AND p.IsDeleted=0  {0}
                ) a
                ", condition);

            return sql;
        }
    }
}