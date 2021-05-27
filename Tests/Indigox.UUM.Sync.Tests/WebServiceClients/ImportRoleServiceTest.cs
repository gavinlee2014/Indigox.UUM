using System;
using Indigox.UUM.Sync.WebServiceClients;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.WebServiceClients
{
    // 需要 webservice 服务，无法实现自动化单元测试
    [Category( "UserTest" )]
    public class ImportRoleServiceTest
    {
        private static object[] WEBS_ERVICE_URLS = new object[] {
            "http://localhost/webservice/TestImportRoleService.asmx"
        };

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void Create( string url )
        {
            ImportRoleServiceClient service = new ImportRoleServiceClient( url );

            string nativeID = "";
            string name = "总监";
            string email = "director@indigox.net";
            string description = "";
            double orderNum = 1;

            service.Create( nativeID, name, email, description, orderNum );
        }
    }
}