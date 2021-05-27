using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;
using Indigox.UUM.Sync.Model;

namespace Indigox.UUM.Application.DTO
{
    public class SysConfigurationDTO
    {
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string UserSyncWebService { get; set; }
        public string Email { get; set; }
        public string RoleSyncWebService { get; set; }
        public string OrganizationUnitSyncWebService { get; set; }
        public string GroupSyncWebService { get; set; }
        public string OrganizationRoleSyncWebService { get; set; }
        public IList<int> Dependencies { get; set; }
        public bool Enabled { get; set; }
        public SyncType SyncType { get; set; }
        public SyncState SyncState { get; set; }
        public int SyncTaskID { get; set; }

        public static SysConfigurationDTO ConvertSysConfigurationToDTO(Indigox.UUM.Sync.Model.SysConfiguration config)
        {
            var configDTO = new SysConfigurationDTO();
            configDTO.ID = config.ID;
            configDTO.SyncType = config.SyncType;
            configDTO.ClientName = config.ClientName;
            configDTO.Email = config.Email;
            configDTO.UserSyncWebService = config.UserSyncWebService;
            configDTO.RoleSyncWebService = config.RoleSyncWebService;
            configDTO.OrganizationRoleSyncWebService = config.OrganizationRoleSyncWebService;
            configDTO.OrganizationUnitSyncWebService = config.OrganizationUnitSyncWebService;
            configDTO.GroupSyncWebService = config.GroupSyncWebService;
            configDTO.Enabled = config.Enabled;
            configDTO.Dependencies = new List<int>();
            foreach (var v in config.Dependencies)
            {
                configDTO.Dependencies.Add(v.ID);
            }
            return configDTO;
        }

        public static SysConfigurationDTO ConvertSysConfigurationToDTOWithState(Indigox.UUM.Sync.Model.SysConfiguration config)
        {
            var dto = new SysConfigurationDTO();
            dto.ID = config.ID;
            dto.SyncType = config.SyncType;
            dto.ClientName = config.ClientName;

            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText(string.Format(@"select id From synctask where state = 2 and Tag = '{0}'", config.ClientName));

            if (recordset.Records.Count > 0)
            {
                dto.SyncState = SyncState.Error;
                dto.SyncTaskID = recordset.Records[0].GetInt("id");
            }
            else
            {
                recordset = database.QueryText(string.Format(@"select id From synctask where state = 0 and Tag = '{0}'", config.ClientName));
                if (recordset.Records.Count > 0)
                {
                    dto.SyncState = SyncState.Running;
                }
                else
                {
                    dto.SyncState = SyncState.Completed;
                }
            }

            return dto;
        }
    }
}
