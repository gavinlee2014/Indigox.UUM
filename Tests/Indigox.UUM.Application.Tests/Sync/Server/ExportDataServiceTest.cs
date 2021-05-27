//using System;
//using Indigox.UUM.Sync.Interface.Server;
//using NUnit.Framework;

//using DTO_Group = Indigox.UUM.Sync.Interface.Group;
//using DTO_OrganizationalRole = Indigox.UUM.Sync.Interface.OrganizationalRole;
//using DTO_OrganizationalUnit = Indigox.UUM.Sync.Interface.OrganizationalUnit;
//using DTO_OrganizationalPerson = Indigox.UUM.Sync.Interface.OrganizationalPerson;
//using DTO_Role = Indigox.UUM.Sync.Interface.Role;

//namespace Indigox.UUM.Application.Tests.Sync.Server
//{
//    // 需要 webservice 服务，无法实现自动化单元测试
//    [Category( "UserTest" )]
//    public class ExportDataServiceTest
//    {
//        private static object[] WEBS_ERVICE_URLS = new object[] {
//            "http://xiangwang.dev.indigox.net:8020/UUM/WebServices/ExportDataService.asmx"
//        };

//        [Test]
//        [TestCaseSource( "WEBS_ERVICE_URLS" )]
//        public void TestFullSync( string url )
//        {
//            ExportDataServiceClient service = new ExportDataServiceClient( url );
//            FullSyncTask task = new FullSyncTask( service );
//            task.Process();
//            Assert.Pass();
//        }

//        [Test]
//        [TestCaseSource( "WEBS_ERVICE_URLS" )]
//        public void TestGetAllGroups( string url )
//        {
//            ExportDataServiceClient service = new ExportDataServiceClient( url );
//            DTO_Group[] users = service.GetAllGroups();
//            Assert.Greater( users.Length, 0 );
//        }

//        [Test]
//        [TestCaseSource( "WEBS_ERVICE_URLS" )]
//        public void TestGetAllOrganizationalRoles( string url )
//        {
//            ExportDataServiceClient service = new ExportDataServiceClient( url );
//            DTO_OrganizationalRole[] users = service.GetAllOrganizationalRoles();
//            Assert.Greater( users.Length, 0 );
//        }

//        [Test]
//        [TestCaseSource( "WEBS_ERVICE_URLS" )]
//        public void TestGetAllRoles( string url )
//        {
//            ExportDataServiceClient service = new ExportDataServiceClient( url );
//            DTO_Role[] users = service.GetAllRoles();
//            Assert.Greater( users.Length, 0 );
//        }

//        [Test]
//        [TestCaseSource( "WEBS_ERVICE_URLS" )]
//        public void TestGetAllUsers( string url )
//        {
//            ExportDataServiceClient service = new ExportDataServiceClient( url );
//            DTO_OrganizationalPerson[] users = service.GetAllUsers();
//            Assert.Greater( users.Length, 0 );
//        }

//        [Test]
//        [TestCaseSource( "WEBS_ERVICE_URLS" )]
//        public void TestGetCorporation( string url )
//        {
//            ExportDataServiceClient service = new ExportDataServiceClient( url );
//            DTO_OrganizationalUnit corporation = service.GetCorporation();
//            Assert.NotNull( corporation );
//        }

//        private class FullSyncTask
//        {
//            private ExportDataServiceClient service;

//            public FullSyncTask( ExportDataServiceClient service )
//            {
//                this.service = service;
//            }

//            public void Process()
//            {
//                ProcessAllRoles();

//                ProcessOrganizationTree();

//                ProcessOrganizationalRoleToRoleRelations();
//                ProcessUserToOrganizationalRoleRelations();

//                ProcessAllGroups();
//            }

//            private void ProcessAllGroups()
//            {
//                DTO_Group[] groups = service.GetAllGroups();
//                foreach ( DTO_Group group in groups )
//                {
//                    //TODO:
//                    Console.WriteLine( "群组：{0}", group.Name );

//                    DTO_OrganizationalPerson[] users = service.GetChildUsersByGroup( group.ID );
//                    foreach ( DTO_OrganizationalPerson user in users )
//                    {
//                        //TODO:
//                        Console.WriteLine( "[关系] 用户：{0} --> 群组：{1}", user.Name, group.Name );
//                    }

//                    DTO_OrganizationalRole[] organizationalRoles = service.GetChildOrganizationalRolesByGroup( group.ID );
//                    foreach ( DTO_OrganizationalRole organizationalRole in organizationalRoles )
//                    {
//                        //TODO:
//                        Console.WriteLine( "[关系] 组织角色：{0} --> 群组：{1}", organizationalRole.Name, group.Name );
//                    }
//                }
//            }

//            private void ProcessAllRoles()
//            {
//                DTO_Role[] roles = service.GetAllRoles();
//                foreach ( DTO_Role role in roles )
//                {
//                    //TODO:
//                    Console.WriteLine( "角色：{0}", role.Name );
//                }
//            }

//            private void ProcessOrganizationalRole( DTO_OrganizationalRole organizationalRole )
//            {
//                //TODO:
//                Console.WriteLine( "组织角色：{0}", organizationalRole.Name );
//            }

//            private void ProcessOrganizationalRoleToRoleRelations()
//            {
//                DTO_Role[] roles = service.GetAllRoles();
//                foreach ( DTO_Role role in roles )
//                {
//                    DTO_OrganizationalRole[] organizationalRoles = service.GetChildOrganizationalRolesByRole( role.ID );
//                    foreach ( DTO_OrganizationalRole organizationalRole in organizationalRoles )
//                    {
//                        //TODO:
//                        Console.WriteLine( "[关系] 组织角色：{0} --> 角色：{1}", organizationalRole.Name, role.Name );
//                    }
//                }
//            }

//            private void ProcessOrganizationalUnit( DTO_OrganizationalUnit organization )
//            {
//                //TODO:
//                Console.WriteLine( "部门：{0}, {1}", organization.Name, organization.FullName );

//                DTO_OrganizationalPerson[] users = service.GetChildUsersByOrganization( organization.ID );
//                foreach ( DTO_OrganizationalPerson user in users )
//                {
//                    ProcessUser( user );
//                    Console.WriteLine( "[关系] 用户：{0} --> 部门：{1}", user.Name, organization.Name );
//                }

//                DTO_OrganizationalRole[] organizationalRoles = service.GetChildOrganizationalRolesByOrganization( organization.ID );
//                foreach ( DTO_OrganizationalRole organizationalRole in organizationalRoles )
//                {
//                    ProcessOrganizationalRole( organizationalRole );
//                    Console.WriteLine( "[关系] 组织角色：{0} --> 部门：{1}", organizationalRole.Name, organization.Name );
//                }

//                DTO_OrganizationalUnit[] childOrganizations = service.GetChildOrganizations( organization.ID );
//                foreach ( DTO_OrganizationalUnit childOrganization in childOrganizations )
//                {
//                    ProcessOrganizationalUnit( childOrganization );
//                    Console.WriteLine( "[关系] 部门：{0} --> 部门：{1}", childOrganization.Name, organization.Name );
//                }
//            }

//            private void ProcessOrganizationTree()
//            {
//                DTO_OrganizationalUnit corporation = service.GetCorporation();
//                Console.WriteLine( "集团部门：{0}", corporation.Name );
//                ProcessOrganizationalUnit( corporation );
//            }

//            private void ProcessUser( DTO_OrganizationalPerson user )
//            {
//                //TODO:
//                Console.WriteLine( "用户：{0}, {1}", user.Name, user.Account );
//            }

//            private void ProcessUserToOrganizationalRoleRelations()
//            {
//                DTO_OrganizationalRole[] organizationalRoles = service.GetAllOrganizationalRoles();
//                foreach ( DTO_OrganizationalRole organizationalRole in organizationalRoles )
//                {
//                    DTO_OrganizationalPerson[] users = service.GetChildUsersByOrganizationalRole( organizationalRole.ID );
//                    foreach ( DTO_OrganizationalPerson user in users )
//                    {
//                        //TODO:
//                        Console.WriteLine( "[关系] 用户：{0} --> 组织角色：{1}", user.Name, organizationalRole.Name );
//                    }
//                }
//            }
//        }
//    }
//}