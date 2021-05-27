using System;
using Indigox.UUM.Application.Sync.WebServices.AD;
using NUnit.Framework;

namespace Indigox.UUM.Application.Tests.Sync.Client
{
    [Category("UserTest")]
    public class ADImportOrganizationalUnitServiceTest
    {
        [Test]
        public void TestCreate()
        {
            ADImportOrganizationalUnitService service = new ADImportOrganizationalUnitService();
            //service.Create( "OR1000000012", "集团总部", "集团总部", "company@indigox.dmo", "", 1, "Company" );
            string company = service.Create("sa1", null, "集团总部", "集团总部", "集团总部", "company@indigox.dmo", "", 1, "Company");
            string department = service.Create("sa2", company, "财务部", "集团总部_财务部", "集团总部_财务部", "financial@indigox.dmo", "", 1, "Department");
            string section = service.Create("sa3", department, "会计", "集团总部_财务部_会计", "集团总部_财务部_会计", "account@indigox.dmo", "", 1, "Section");
        }

        [Test]
        public void TestDelete()
        {
            ADImportOrganizationalUnitService service = new ADImportOrganizationalUnitService();
            //service.Create( "OR1000000012", "集团总部", "集团总部", "company@indigox.dmo", "", 1, "Company" );
            string created = service.Create("SDF", null, "广州公司", "广州公司", "广州公司", "gz@indigox.dmo", "", 1, "Company");
            service.Delete(created);
        }
    }
}