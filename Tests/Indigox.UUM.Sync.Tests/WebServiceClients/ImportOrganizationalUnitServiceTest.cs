using System;
using Indigox.UUM.Sync.WebServiceClients;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.WebServiceClients
{
    // 需要 webservice 服务，无法实现自动化单元测试
    [Category( "UserTest" )]
    public class ImportOrganizationalUnitServiceTest
    {
        private static object[] WEBS_ERVICE_URLS = new object[] {
            "http://localhost/webservice/TestImportOrganizationalUnitService.asmx"
        };

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void Create( string url )
        {
            ImportOrganizationalUnitServiceClient service = new ImportOrganizationalUnitServiceClient( url );

            string nativeID = "";
            string parentOrganizationalUnitID = "";
            string name = "Indigox";
            string fullName = "";
            string displayName = "";
            string email = "testorg@indigox.net";
            string description = "";
            double orderNum = 1;
            string organizationalUnitType = "Corporation";

            service.Create(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType);
        }
    }
}