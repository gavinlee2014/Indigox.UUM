using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Sync.TestClient
{
    [WebService( Name = "ImportOrganizationalRoleService", Namespace = "http://indigox.uum/sync/" )]
    public class TestSyncOrganizationalRoleService : WebService, IImportOrganizationalRoleService
    {
        public string Create(string nativeID, string organizationalRoleID, string name, string fullName, string displayName, string email, string description, double orderNum)
        {
            return Guid.NewGuid().ToString();
        }

        public void Delete( string organizationalRoleID )
        {
        }

        public void ChangeProperty( string organizationalRoleID, PropertyChangeCollection propertyChanges )
        {
        }

        public void AddUser( string organizationalRoleID, string userID )
        {
        }

        public void RemoveUser( string organizationalRoleID, string userID )
        {
        }

        public string SyncOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum)
        {
            return Guid.NewGuid().ToString();
        }
    }
}