using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Logging;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Application.Sync
{
    internal class FullSyncCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int SystemID { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var externalSystem = repository.Get( SystemID );
            FullSyncTask task = new FullSyncTask( externalSystem );
            task.Process();
        }

        private class FullSyncTask
        {
            #region fields

            private Indigox.UUM.Sync.Model.SysConfiguration externalSystem = null;
            private ImportGroupServiceClient importGroupServiceClient = null;
            private ImportOrganizationalRoleServiceClient importOrganizationalRoleServiceClient = null;
            private ImportOrganizationalUnitServiceClient importOrganizationalUnitServiceClient = null;
            private ImportRoleServiceClient importRoleServiceClient = null;
            private ImportUserServiceClient importUserServiceClient = null;

            #endregion fields

            #region constructors

            public FullSyncTask( Indigox.UUM.Sync.Model.SysConfiguration externalSystem )
            {
                this.externalSystem = externalSystem;
                if ( !string.IsNullOrEmpty( externalSystem.GroupSyncWebService ) )
                {
                    this.importGroupServiceClient = new ImportGroupServiceClient( externalSystem.GroupSyncWebService );
                }
                if ( !string.IsNullOrEmpty( externalSystem.OrganizationRoleSyncWebService ) )
                {
                    this.importOrganizationalRoleServiceClient = new ImportOrganizationalRoleServiceClient( externalSystem.OrganizationRoleSyncWebService );
                }
                if ( !string.IsNullOrEmpty( externalSystem.OrganizationUnitSyncWebService ) )
                {
                    this.importOrganizationalUnitServiceClient = new ImportOrganizationalUnitServiceClient( externalSystem.OrganizationUnitSyncWebService );
                }
                if ( !string.IsNullOrEmpty( externalSystem.RoleSyncWebService ) )
                {
                    this.importRoleServiceClient = new ImportRoleServiceClient( externalSystem.RoleSyncWebService );
                }
                if ( !string.IsNullOrEmpty( externalSystem.UserSyncWebService ) )
                {
                    this.importUserServiceClient = new ImportUserServiceClient( externalSystem.UserSyncWebService );
                }
            }

            #endregion constructors

            #region main entry

            public void Process()
            {
                TraverseAllRoles();

                TraverseOrganizationTree();

                TraverseOrganizationalRoleToRoleRelations();
                TraverseUserToOrganizationalRoleRelations();

                TraverseAllGroups();
            }

            #endregion main entry

            #region traversals

            private void TraverseAllGroups()
            {
                IRepository<IGroup> repository = RepositoryFactory.Instance.CreateRepository<IGroup>();
                IList<IGroup> groups = repository.Find( Query.NewQuery );
                foreach ( IGroup group in groups )
                {
                    string externalGroupID = ProcessGroup( group );

                    foreach ( IPrincipal member in group.Members )
                    {
                        if ( member is IOrganizationalPerson )
                        {
                            IOrganizationalPerson user = (IOrganizationalPerson)member;
                            ProcessUserToGroup( user, group, externalGroupID );
                        }
                        else if ( member is IOrganizationalRole )
                        {
                            IOrganizationalRole organizationalRole = (IOrganizationalRole)member;
                            ProcessOrganizationalRoleToGroup( organizationalRole, group, externalGroupID );
                        }
                    }
                }
            }

            private void TraverseAllRoles()
            {
                IRepository<IRole> repository = RepositoryFactory.Instance.CreateRepository<IRole>();
                IList<IRole> roles = repository.Find( Query.NewQuery );
                foreach ( IRole role in roles )
                {
                    ProcessRole( role );
                }
            }

            private void TraverseOrganizationalRoleToRoleRelations()
            {
                IRepository<IRole> repository = RepositoryFactory.Instance.CreateRepository<IRole>();
                IList<IRole> roles = repository.Find( Query.NewQuery );
                foreach ( IRole role in roles )
                {
                    string externalRoleID = GetExternalID( role.ID );

                    foreach ( IPrincipal member in role.Members )
                    {
                        if ( member is IOrganizationalRole )
                        {
                            IOrganizationalRole organizationalRole = (IOrganizationalRole)member;
                            ProcessOrganizationalRoleToRole( organizationalRole, role, externalRoleID );
                        }
                    }
                }
            }

            private void TraverseOrganizationalUnitNode( IOrganizationalUnit organizationalUnit )
            {
                string externalOrganizationalUnitID = ProcessOrganizationalUnit( organizationalUnit );

                foreach ( IPrincipal member in organizationalUnit.Members )
                {
                    if (member.Deleted)
                    {
                        continue;
                    }
                    if ( member is IOrganizationalPerson )
                    {
                        IOrganizationalPerson user = (IOrganizationalPerson)member;
                        if (user.Enabled)
                        {
                            ProcessUser(user);
                        }
                        //ProcessUserToOrganizationalUnit( user, organizationalUnit, externalOrganizationalUnitID );
                    }
                    else if ( member is IOrganizationalRole )
                    {
                        IOrganizationalRole organizationalRole = (IOrganizationalRole)member;
                        ProcessOrganizationalRole( organizationalRole );

                        //ProcessOrganizationalRoleToOrganizationalUnit( organizationalRole, organizationalUnit, externalOrganizationalUnitID );
                    }
                    else if ( member is IOrganizationalUnit )
                    {
                        IOrganizationalUnit childOrganizationalUnit = (IOrganizationalUnit)member;
                        TraverseOrganizationalUnitNode( childOrganizationalUnit );

                        //ProcessOrganizationalUnitToOrganizationalUnit( childOrganizationalUnit, organizationalUnit, externalOrganizationalUnitID );
                    }
                }
            }

            private void TraverseOrganizationTree()
            {
                IOrganizationalUnit corporation = Indigox.Common.Membership.Corporation.GetCorporation();
                Log.Debug( String.Format( "集团部门：{0}", corporation.Name ) );
                TraverseOrganizationalUnitNode( corporation );
            }

            private void TraverseUserToOrganizationalRoleRelations()
            {
                IRepository<IOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
                IList<IOrganizationalRole> organizationalRoles = repository.Find( Query.NewQuery );
                foreach ( IOrganizationalRole organizationalRole in organizationalRoles )
                {
                    string externalOrganizationalRoleID = GetExternalID( organizationalRole.ID );

                    foreach ( IPrincipal member in organizationalRole.Members )
                    {
                        if ( member is IOrganizationalPerson )
                        {
                            IOrganizationalPerson user = (IOrganizationalPerson)member;
                            ProcessUserToOrganizationalRole( user, organizationalRole, externalOrganizationalRoleID );
                        }
                    }
                }
            }

            #endregion traversals

            #region process member

            private string ProcessGroup(IGroup group)
            {
                if (importGroupServiceClient == null)
                {
                    return null;
                }

                string externalID = GetExternalID(group.ID);

                if (String.IsNullOrEmpty(externalID))
                {
                    externalID = importGroupServiceClient.SyncGroup(group.ID,
                        group.Name, group.Email, group.Description, group.OrderNum);

                    SetExternalID(group.ID, externalID);

                    Log.Debug(String.Format("群组：{0}", group.Name));
                }

                return externalID;
            }

            private string ProcessOrganizationalRole(IOrganizationalRole organizationalRole)
            {
                if (importOrganizationalRoleServiceClient == null)
                {
                    return null;
                }

                string externalOrganizationalUnitID = null;

                if (organizationalRole.Organization != null)
                {
                    externalOrganizationalUnitID = GetExternalID(organizationalRole.Organization.ID);
                }

                string externalID = GetExternalID(organizationalRole.ID);

                if (String.IsNullOrEmpty(externalID))
                {
                    externalID = importOrganizationalRoleServiceClient.SyncOrganizationalRole(organizationalRole.ID,
                       externalOrganizationalUnitID, organizationalRole.Name, organizationalRole.FullName,
                       organizationalRole.DisplayName, organizationalRole.Email,
                       organizationalRole.Description, organizationalRole.OrderNum,
                       new PropertyChangeCollection(organizationalRole.ExtendProperties));

                    SetExternalID(organizationalRole.ID, externalID);

                    Log.Debug(String.Format("组织角色：{0}", organizationalRole.Name));
                }

                return externalID;
            }

            private string ProcessOrganizationalUnit( IOrganizationalUnit organization )
            {
                if ( importOrganizationalUnitServiceClient == null )
                {
                    return null;
                }

                string externalParentOrganizationalUnitID = null;

                if ( organization.Organization != null )
                {
                    externalParentOrganizationalUnitID = GetExternalID( organization.Organization.ID );
                }

                string externalOrganizationalUnitID = GetExternalID(organization.ID);

                if (String.IsNullOrEmpty(externalOrganizationalUnitID))
                {
                    externalOrganizationalUnitID = importOrganizationalUnitServiceClient.SyncOrganizationalUnit(
                        organization.ID, externalParentOrganizationalUnitID, organization.Name, organization.FullName,
                        organization.DisplayName, organization.Email, organization.Description,
                        organization.OrderNum, organization.GetType().Name, 
                        new PropertyChangeCollection(organization.ExtendProperties));

                    Log.Debug(String.Format("部门：{0}, {1}", organization.Name, organization.FullName));
                    Log.Debug(String.Format("External OrganizationalUnitID is {0}", externalOrganizationalUnitID));

                    SetExternalID(organization.ID, externalOrganizationalUnitID);
                }

                return externalOrganizationalUnitID;
            }

            private string ProcessRole( IRole role )
            {
                if ( importRoleServiceClient == null )
                {
                    return null;
                }

                string externalID = GetExternalID(role.ID);

                if (String.IsNullOrEmpty(externalID))
                {
                    externalID = importRoleServiceClient.SyncRole(role.ID,
                    role.Name, role.Email, role.Description, role.OrderNum);

                    SetExternalID(role.ID, externalID);

                    Log.Debug(String.Format("角色：{0}", role.Name));
                }

                return externalID;
            }

            private string ProcessUser( IOrganizationalPerson user )
            {
                if ( importUserServiceClient == null )
                {
                    return null;
                }

                string externalOrganizationID = null;

                if ( user.Organization != null )
                {
                    externalOrganizationID = GetExternalID( user.Organization.ID );
                }

                string title = "";
                var extendProperties = user.ExtendProperties;
                if(extendProperties == null)
                {
                    extendProperties = new Dictionary<string, string>();
                }
                if (extendProperties.ContainsKey("IdCard"))
                {
                    extendProperties["IdCard"] = user.IdCard;
                }
                else
                {
                    extendProperties.Add("IdCard", user.IdCard);
                }
                if (user.MemberOf != null)
                {
                    foreach (var parent in user.MemberOf)
                    {
                        IOrganizationalRole role = parent as IOrganizationalRole;
                        if ((role != null) && (role.Organization != null) && (user.Organization != null)
                            && (role.Organization.ID.Equals(user.Organization.ID)))
                        {
                            title = role.Name;
                            if (role.ExtendProperties.ContainsKey("RoleLevel"))
                            {
                                if (extendProperties.ContainsKey("RoleLevel"))
                                {
                                    extendProperties["RoleLevel"] = role.ExtendProperties["RoleLevel"];
                                }
                                else
                                {
                                    extendProperties.Add("RoleLevel", role.ExtendProperties["RoleLevel"]);
                                }
                            }
                            
                            
                            break;
                        }
                    }
                }

                string externalID = GetExternalID(user.ID);
                if (String.IsNullOrEmpty(externalID))
                {
                    externalID = importUserServiceClient.SyncUser(user.ID,
                        externalOrganizationID, user.AccountName, user.Name, user.FullName, user.DisplayName, user.Email, title, user.Mobile,
                        user.Telephone, user.Fax, user.OrderNum, user.Description, user.OtherContact, user.Profile, user.MailDatabase, new PropertyChangeCollection(extendProperties));

                    SetExternalID(user.ID, externalID);

                    Log.Debug(String.Format("用户：{0}, {1}", user.Name, user.AccountName));
                }
                return externalID;
            }

            #endregion process member

            #region process membership

            private void ProcessOrganizationalRoleToGroup( IOrganizationalRole organizationalRole, IGroup group, string externalGroupID )
            {
                if ( importGroupServiceClient == null )
                {
                    return;
                }
                string externalOrganizationRoleID = GetExternalID( organizationalRole.ID );
                importGroupServiceClient.AddOrganizationalRole( externalGroupID, externalOrganizationRoleID );
                Log.Debug( String.Format( "[关系] 组织角色：{0} --> 群组：{1}", organizationalRole.Name, group.Name ) );
            }

            private void ProcessOrganizationalRoleToOrganizationalUnit( IOrganizationalRole organizationalRole, IOrganizationalUnit organizationalUnit, string externalOrganizationalUnitID )
            {
                if ( importOrganizationalUnitServiceClient == null )
                {
                    return;
                }
                string externalOrganizationRoleID = GetExternalID( organizationalRole.ID );
                importOrganizationalUnitServiceClient.AddOrganizationalRole( externalOrganizationalUnitID, externalOrganizationRoleID );
                Log.Debug( String.Format( "[关系] 组织角色：{0} --> 部门：{1}", organizationalRole.Name, organizationalUnit.Name ) );
            }

            private void ProcessOrganizationalRoleToRole( IOrganizationalRole organizationalRole, IRole role, string externalRoleID )
            {
                if ( importRoleServiceClient == null )
                {
                    return;
                }
                string externalOrganizationRoleID = GetExternalID( organizationalRole.ID );
                importRoleServiceClient.AddOrganizationalRole( externalRoleID, externalOrganizationRoleID );
                Log.Debug( String.Format( "[关系] 组织角色：{0} --> 角色：{1}", organizationalRole.Name, role.Name ) );
            }

            private void ProcessOrganizationalUnitToOrganizationalUnit( IOrganizationalUnit organizationalUnit, IOrganizationalUnit parentOrganizationalUnit, string externalParentOrganizationalUnitID )
            {
                if ( importOrganizationalUnitServiceClient == null )
                {
                    return;
                }
                string externalOrganizationalUnitID = GetExternalID( organizationalUnit.ID );
                importOrganizationalUnitServiceClient.AddOrganizationalUnit( externalParentOrganizationalUnitID, externalOrganizationalUnitID );
                Log.Debug( String.Format( "[关系] 部门：{0} --> 部门：{1}", organizationalUnit.Name, parentOrganizationalUnit.Name ) );
            }

            private void ProcessUserToGroup( IOrganizationalPerson user, IGroup group, string externalGroupID )
            {
                if ( importGroupServiceClient == null )
                {
                    return;
                }
                string externalUserID = GetExternalID( user.ID );
                importGroupServiceClient.AddUser( externalGroupID, externalUserID );
                Log.Debug( String.Format( "[关系] 用户：{0} --> 群组：{1}", user.Name, group.Name ) );
            }

            private void ProcessUserToOrganizationalRole(IOrganizationalPerson user, IOrganizationalRole organizationalRole, string externalOrganizationalRoleID)
            {
                if (importOrganizationalRoleServiceClient == null)
                {
                    return;
                }
                string externalUserID = GetExternalID(user.ID);
                if (String.IsNullOrEmpty(externalOrganizationalRoleID) || String.IsNullOrEmpty(externalUserID))
                {
                    return;
                }
                importOrganizationalRoleServiceClient.AddUser(externalOrganizationalRoleID, externalUserID);
                Log.Debug(String.Format("[关系] 用户：{0} --> 组织角色：{1}", user.Name, organizationalRole.Name));
            }

            private void ProcessUserToOrganizationalUnit( IOrganizationalPerson user, IOrganizationalUnit organizationalUnit, string externalOrganizationalUnitID )
            {
                if ( importOrganizationalUnitServiceClient == null )
                {
                    return;
                }
                string externalUserID = GetExternalID( user.ID );
                importOrganizationalUnitServiceClient.AddUser( externalOrganizationalUnitID, externalUserID );
                Log.Debug( String.Format( "[关系] 用户：{0} --> 部门：{1}", user.Name, organizationalUnit.Name ) );
            }

            #endregion process membership

            #region external id methods

            private string GetExternalID( string internalID )
            {
                string externalID = SysKeyMappingService.Instance.GetExternalID( internalID, externalSystem );
                return externalID;
            }

            private void SetExternalID( string internalID, string externalID )
            {
                if(String.IsNullOrEmpty(externalID))
                {
                    return;
                }
                SysKeyMappingService.Instance.SetExternalID( internalID, externalID, externalSystem );
            }

            #endregion external id methods
        }
    }
}