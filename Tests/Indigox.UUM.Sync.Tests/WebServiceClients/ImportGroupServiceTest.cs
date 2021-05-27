using System;
using Indigox.UUM.Sync.WebServiceClients;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.WebServiceClients
{
    // 需要 webservice 服务，无法实现自动化单元测试
    [Category( "UserTest" )]
    public class ImportGroupServiceTest
    {
        private static object[] WEBS_ERVICE_URLS = new object[] {
            "http://localhost/webservice/TestImportGroupService.asmx"
        };

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void Create( string url )
        {
            ImportGroupServiceClient service = new ImportGroupServiceClient( url );

            string nativeID = "dfd";
            string name = "测试用户";
            string email = "testgp@indigox.net";
            string description = "";
            double orderNum = 1;

            service.Create( nativeID, name, email, description, orderNum );
        }
    }
}