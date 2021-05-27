using System;
using System.Diagnostics;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;

namespace Indigox.UUM.Sync.TestClient
{
    [WebService( Name = "ImportDataService", Namespace = "http://indigox.uum/sync/" )]
    [WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class SampleSyncDataService : WebService
    {
        [WebMethod]
        public void AddOrganization( OrganizationalUnit organization )
        {
            Debug.WriteLine( organization.ID );
        }

        [WebMethod]
        public void AddUser( OrganizationalPerson user )
        {
            Debug.WriteLine( user.ID );
        }
    }
}