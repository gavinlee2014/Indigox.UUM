using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Sync.TestClient
{
    [WebService( Name = "ImportOrganizationalUnitService", Namespace = "http://indigox.uum/sync/" )]
    public class TestSyncOrganizationalUnitService : WebService, IImportOrganizationalUnitService
    {
        public void Delete( string organizationalUnitID )
        {
        }

        public void ChangeProperty( string organizationalUnitID, PropertyChangeCollection propertyChanges )
        {
        }

        public void AddOrganizationalUnit( string parentOrganizationalUnitID, string organizationalUnitID )
        {
        }

        public void RemoveOrganizationalUnit( string parentOrganizationalUnitID, string organizationalUnitID )
        {
        }

        public void AddOrganizationalRole( string organizationalUnitID, string organizationalRoleID )
        {
        }

        public void RemoveOrganizationalRole( string organizationalUnitID, string organizationalRoleID )
        {
        }

        public void AddUser( string organizationalUnitID, string userID )
        {
        }

        public void RemoveUser( string organizationalUnitID, string userID )
        {
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType)
        {
            return Guid.NewGuid().ToString();
        }

        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType)
        {
            return Guid.NewGuid().ToString();
        }
    }
}