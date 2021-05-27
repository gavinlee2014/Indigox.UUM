using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class OrganizationalRoleDeletedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            OrganizationalRoleDeletedEvent concreateEvent = (OrganizationalRoleDeletedEvent)this.Event;
            context.Set( "OrganizationalRoleID", GetExternalObject( concreateEvent.OrganizationalRole.ID ) );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportOrganizationalRoleServiceClient ).AssemblyQualifiedName;
            executor.Method = "Delete";
            executor.Url = this.System.OrganizationRoleSyncWebService;
            return executor;
        }

        protected override bool SyncEnabled
        {
            get
            {
                return !string.IsNullOrEmpty( System.OrganizationRoleSyncWebService );
            }
        }
    }
}