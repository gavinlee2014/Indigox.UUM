using System;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.Model
{
    public class SysConfiguration
    {
        public SysConfiguration()
        {
            Dependencies = new List<SysConfiguration>();
        }

        public int ID { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string UserSyncWebService { get; set; }
        public string RoleSyncWebService { get; set; }
        public string OrganizationUnitSyncWebService { get; set; }
        public string GroupSyncWebService { get; set; }
        public string OrganizationRoleSyncWebService { get; set; }
        public IList<SysConfiguration> Dependencies { get; set; }
        public bool Enabled { get; set; }
        public SyncType SyncType { get; set; }
    }
}