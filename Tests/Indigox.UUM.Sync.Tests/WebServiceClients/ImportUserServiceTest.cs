using System;
using System.Collections.Generic;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.WebServiceClients;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.WebServiceClients
{
    // 需要 webservice 服务，无法实现自动化单元测试
    [Category( "UserTest" )]
    public class ImportUserServiceTest
    {
        private static object[] WEBS_ERVICE_URLS = new object[] {
            "http://localhost/webservice/TestImportUserService.asmx"
        };

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void Create( string url )
        {
            ImportUserServiceClient service = new ImportUserServiceClient( url );

            string nativeID = "3232";
            string organizationalUnitID = "OR1000000000";
            string accountName = "Admin";
            string name = "管理员";
            string fullName = "总部_管理员";
            string dislpayName = "总部_管理员";
            string email = "admin@indigox.net";
            string title = "管理员";
            string mobile = "13723566773";
            string telephone = "26731323";
            string fax = "23432315";
            double orderNum = 1;
            string description = "";
            string otherContact = "";
            string portrait = "";
            string mailDatabase = "";

            service.Create( nativeID, organizationalUnitID, accountName, name, fullName, dislpayName, email, title, mobile, telephone, fax, orderNum, description, otherContact ,portrait,mailDatabase);
        }

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void ChangeProperty( string url )
        {
            ImportUserServiceClient service = new ImportUserServiceClient( url );

            string nativeID = "3232";
            Dictionary<string, object> changedProperties = new Dictionary<string, object>()
            {
                { "Name", "wangz" },
                { "Email", "wangz@indigox.net" }
            };

            service.ChangeProperty( nativeID, new PropertyChangeCollection( changedProperties ) );
        }
    }
}