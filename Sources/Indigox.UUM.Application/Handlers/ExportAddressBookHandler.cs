using System.Web;
using Indigox.Common.Data;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.StateContainer;
using System.Web.SessionState;
using Indigox.Common.Statistics.Exporters;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.Handlers
{
    public class ExportAddressBookHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly byte[] BOM_UTF8 = new byte[] { 0xEF, 0xBB, 0xBF };
        public string OrganizationalUnitID { get; set; }
        public string QueryString { get; set; }

        private static readonly string DefaultPrincipalID = "OR1000000000";

        public void ProcessRequest(HttpContext context)
        {
            OrganizationalUnitID = context.Request.QueryString["OrganizationalUnit"];
            QueryString = context.Request.QueryString["QueryString"];

            string sql = GetSql();
            Log.Error("导出脚本为：" + sql);
            var dbFactory = new DatabaseFactory();
            var db = dbFactory.CreateDatabase("UUM");
            var recordSet = db.QueryText(sql);
            string downloadFileName = "AdressBook.csv";
            if (context.Request.Browser.Browser == "IE")
            {
                downloadFileName = HttpUtility.UrlEncode(downloadFileName, System.Text.Encoding.UTF8);
            }
            context.Response.Headers.Add("Content-Disposition", "attachment;filename=" + downloadFileName);
            context.Response.ContentType = "application/octet-stream";
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

            context.Response.OutputStream.Write(BOM_UTF8, 0, BOM_UTF8.Length);
            CsvExporter exporter = new CsvExporter(context.Response.Output);
            exporter.Export(recordSet);

        }

        private string GetSql()
        {
            string organizationalUnitID = this.OrganizationalUnitID;
            if (string.IsNullOrEmpty(organizationalUnitID))
            {
                organizationalUnitID = DefaultPrincipalID;
            }
            string org = string.Format("WHERE Parent='{0}'", organizationalUnitID);

            string condition = "";

            if (!string.IsNullOrEmpty(this.QueryString))
            {
                condition += " AND ("
                    + " dbo.Principal.[Name] LIKE '%" + this.QueryString + "%'"
                    + " OR u.AccountName LIKE '%" + this.QueryString + "%'"
                    + " OR u.Mobile LIKE '%" + this.QueryString + "%'"
                    + " OR u.Telephone LIKE '%" + this.QueryString + "%'"
                    + " OR u.Fax LIKE '%" + this.QueryString + "%'"
                    + " ) ";
            }

            string sql = string.Format(@"
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
                select 所属部门,显示名,姓名,登录名,手机,办公电话,电子邮件 from (
                    SELECT distinct p.displayName AS 所属部门,
                        dbo.Principal.DisplayName AS 显示名,
                        dbo.Principal.name AS 姓名,
                        u.AccountName AS 登录名,
                        u.Mobile 手机,
                        u.Telephone 办公电话,
                        dbo.Principal.email 电子邮件
                    FROM dbo.Principal
                    JOIN dbo.Membership on (dbo.Principal.ID=dbo.Membership.Child or dbo.Principal.ID=dbo.Membership.Parent)
                    LEFT JOIN dbo.Principal p on dbo.Principal.Organization=p.ID
                    JOIN dbo.Users u on dbo.Principal.ID=u.ID
                    JOIN CTE as org on dbo.Membership.Parent = org.Child
                    WHERE dbo.Principal.IsEnabled=1 and dbo.Principal.isDeleted=0 and dbo.Principal.[Type] between 200 and 299 {1}
                )as t 
                order by t.所属部门 asc,t.显示名 asc

", org, condition);

            return sql;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}