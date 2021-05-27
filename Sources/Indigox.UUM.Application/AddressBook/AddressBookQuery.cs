using System;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.Application.DTO;
using NHibernate;

namespace Indigox.UUM.Application.AddressBook
{
    public class AddressBookQuery : Indigox.Web.CQRS.GenericSingleQuery<AddressBookDTO>
    {
        public string PrincipalID { get; set; }

        public override AddressBookDTO Single()
        {
            ISession session = SessionFactories.Instance.Get( typeof( AddressBookDTO ).Assembly ).GetCurrentSession();
            {
                string sql = GetSql();

                ISQLQuery query = session.CreateSQLQuery( sql );

                query.AddEntity( typeof( AddressBookDTO ) );

                return query.UniqueResult<AddressBookDTO>();
            }
        }

        private string GetSql()
        {
            string sql = String.Format( @"
SELECT dbo.Principal.*,
        ISNULL(t.FullName,t.displayName) AS OrganizationFullName,
        u.AccountName,
        u.Mobile,
        u.Telephone,
        u.Title,
        u.Fax,
        u.OtherContact,
        u.Profile
FROM dbo.Principal
LEFT JOIN dbo.Principal t on dbo.Principal.Organization=t.ID
JOIN dbo.Users u on dbo.Principal.ID=u.ID
WHERE dbo.Principal.[ID]='{0}'
", this.PrincipalID );

            return sql;
        }
    }
}