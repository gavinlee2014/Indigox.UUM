using System;
using Indigox.UUM.Application.Sync.WebServices.AD;
using NUnit.Framework;

namespace Indigox.UUM.Application.Tests.Sync.Client
{
    [Category( "UserTest" )]
    public class ADImportUserServiceTest
    {
        [Test]
        public void TestCreate()
        {
            ADImportUserService service = new ADImportUserService();
            service.Create("", "2B469FCC-61F8-4B67-8CFC-A559D68869C6", "smxia", "夏淑旻", "夏淑旻", "夏淑旻", "smxia@indigox.dmo", "", "13500012345", "", "", 1, "", "","","");
            service.Create("", "330DCB40-A8F2-4E02-8F34-0AEEB2F39D0D", "wjzhang", "张伟杰", "张伟杰", "张伟杰", "wjzhang@indigox.dmo", "", "", "", "", 1, "", "","","");
            service.Create("", "8DA687F1-76E6-453C-9685-9A406043A3A5", "cmzhou", "周崇明", "周崇明", "周崇明", "cmzhou@indigox.dmo", "", "", "", "", 1, "", "","","");
        }

        [Test]
        public void TestDisable()
        {
            ADImportUserService service = new ADImportUserService();
            service.Disable( "2B469FCC-61F8-4B67-8CFC-A559D68869C6" );
        }

        [Test]
        public void TestEnable()
        {
            ADImportUserService service = new ADImportUserService();
            service.Enable("2B469FCC-61F8-4B67-8CFC-A559D68869C6","","","","","","","","","","",0,"","","","");
        }
    }
}