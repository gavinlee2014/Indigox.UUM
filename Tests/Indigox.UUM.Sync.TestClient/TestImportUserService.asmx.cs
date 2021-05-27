using System;
using System.Collections.Generic;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Sync.TestClient
{
    [WebService( Name = "ImportUserService", Namespace = "http://indigox.uum/sync/" )]
    public class TestSyncDataService : WebService, IImportUserService
    {
        public string Create(string nativeID, string userID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            return Guid.NewGuid().ToString();
        }

        public void Delete( string userID )
        {
        }

        public void Disable( string userID )
        {
        }

        public void Enable(string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
        }

        public void ChangeProperty( string userID, PropertyChangeCollection propertyChanges )
        {
            List<string> temp = new List<string>();
            if ( propertyChanges != null )
            {
                foreach ( PropertyChange item in propertyChanges.PropertyChanges )
                {
                    temp.Add( item.Name );
                }
            }
            string properties = string.Join( ", ", temp.ToArray() );
        }

        public string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            return Guid.NewGuid().ToString();
        }
    }
}