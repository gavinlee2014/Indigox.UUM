using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Sync.TestClient
{
    [WebService( Name = "ImportRoleService", Namespace = "http://indigox.uum/sync/" )]
    public class TestSyncRoleService : WebService, IImportRoleService
    {
        public void Delete( string roleID )
        {
        }

        public void ChangeProperty( string roleID, PropertyChangeCollection propertyChanges )
        {
        }

        public void AddOrganizationalRole( string roleID, string organizationalRoleID )
        {
        }

        public void RemoveOrganizationalRole( string roleID, string organizationalRoleID )
        {
        }

        public string SyncRole( string nativeID, string name, string email, string description, double orderNum )
        {
            return Guid.NewGuid().ToString();
        }

        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            return Guid.NewGuid().ToString();
        }
    }
}