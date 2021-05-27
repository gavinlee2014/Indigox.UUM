using System;
using System.Collections.Generic;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.WebServiceClients;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.WebServiceClients
{
    // 需要 webservice 服务，无法实现自动化单元测试
    [Category( "UserTest" )]
    public class ImportOrganizationalRoleServiceTest
    {
        private static object[] WEBS_ERVICE_URLS = new object[] {
            "http://localhost/webservice/TestImportOrganizationalRoleService.asmx"
        };

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void Create( string url )
        {
            ImportOrganizationalRoleServiceClient service = new ImportOrganizationalRoleServiceClient( url );

            string nativeID = "xsss";
            string organizationalUnitID = "";
            string name = "财务总监";
            string fullName = "";
            string displayName = "";
            string email = "testfd@indigox.net";
            string description = "";
            double orderNum = 1;

            service.Create(nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum);
        }

        [Test]
        [TestCaseSource( "WEBS_ERVICE_URLS" )]
        public void ChangeProperty( string url )
        {
            ImportOrganizationalRoleServiceClient service = new ImportOrganizationalRoleServiceClient( url );

            PropertyChangeCollection changes = new PropertyChangeCollection( new Dictionary<string, object>()
            {
                { "k1", "v1" },
                { "k2", 10 }
            } );
            Assert.AreEqual( 2, changes.PropertyChanges.Count );

            service.ChangeProperty( "RO10002302", changes );
        }
    }
}