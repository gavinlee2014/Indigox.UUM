using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class OrganizationalRoleRemovedFromOrganizationalUnitEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            OrganizationalRoleRemovedFromOrganizationalUnitEvent concreateEvent = (OrganizationalRoleRemovedFromOrganizationalUnitEvent)this.Event;
            context.Set( "OrganizationalRoleID", GetExternalObject( concreateEvent.OrganizationalRole.ID ) );
            context.Set( "OrganizationalUnitID", GetExternalObject( concreateEvent.OrganizationalUnit.ID ) );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportOrganizationalUnitServiceClient ).AssemblyQualifiedName;
            executor.Method = "RemoveOrganizationalRole";
            executor.Url = this.System.OrganizationUnitSyncWebService;
            return executor;
        }

        protected override bool SyncEnabled
        {
            get
            {
                return !string.IsNullOrEmpty( System.OrganizationUnitSyncWebService );
            }
        }
    }
}