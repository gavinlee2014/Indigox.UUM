using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.Utils;
using Indigox.UUM.Sync.Model;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.SysConfiguration
{
    public class UpdateSysConfigurationCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }
        public IList<int> Dependencies { get; set; }
        public string ClientName { get; set; }
        public SyncType SyncType { get; set; }
        public string UserSyncWebService { get; set; }
        public string RoleSyncWebService { get; set; }
        public string OrganizationUnitSyncWebService { get; set; }
        public string GroupSyncWebService { get; set; }
        public string OrganizationRoleSyncWebService { get; set; }
        public bool Enabled { get; set; }
        public string Email { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            
            var sysConfiguration = repository.Get(this.ID);

            sysConfiguration.Dependencies = new List<Indigox.UUM.Sync.Model.SysConfiguration>();
            if (Dependencies != null)
            {
                foreach (int dependency in Dependencies)
                {
                    sysConfiguration.Dependencies.Add(SysConfigurationService.Instance.GetSysConfiguration(dependency));
                }
            }
            sysConfiguration.ClientName = ClientName;
            sysConfiguration.SyncType = SyncType;
            sysConfiguration.RoleSyncWebService = RoleSyncWebService;
            sysConfiguration.UserSyncWebService = UserSyncWebService;
            sysConfiguration.OrganizationRoleSyncWebService = OrganizationRoleSyncWebService;
            sysConfiguration.OrganizationUnitSyncWebService = OrganizationUnitSyncWebService;
            sysConfiguration.GroupSyncWebService = GroupSyncWebService;
            sysConfiguration.Enabled = Enabled;
            sysConfiguration.Email = Email;

            repository.Update( sysConfiguration );
        }
    }
}