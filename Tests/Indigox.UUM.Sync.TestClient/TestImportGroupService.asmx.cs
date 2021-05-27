using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Sync.TestClient
{
    [WebService( Name = "ImportGroupService", Namespace = "http://indigox.uum/sync/" )]
    public class TestSyncGroupService : WebService, IImportGroupService
    {
        public void Delete( string groupID )
        {
        }

        public void ChangeProperty( string groupID, PropertyChangeCollection propertyChanges )
        {
        }

        public void AddOrganizationalUnit( string groupID, string organizationalUnitID )
        {
        }

        public void RemoveOrganizationalUnit( string groupID, string organizationalUnitID )
        {
        }

        public void AddOrganizationalRole( string groupID, string organizationalRoleID )
        {
        }

        public void RemoveOrganizationalRole( string groupID, string organizationalRoleID )
        {
        }

        public void AddUser( string groupID, string userID )
        {
        }

        public void RemoveUser( string groupID, string userID )
        {
        }

        public string SyncGroup( string nativeID, string name, string email, string description, double orderNum )
        {
            return Guid.NewGuid().ToString();
        }

        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            return Guid.NewGuid().ToString();
        }
    }
}