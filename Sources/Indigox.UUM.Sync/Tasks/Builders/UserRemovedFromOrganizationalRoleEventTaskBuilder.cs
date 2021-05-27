using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class UserRemovedFromOrganizationalRoleEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            UserRemovedFromOrganizationalRoleEvent concreateEvent = (UserRemovedFromOrganizationalRoleEvent)this.Event;
            context.Set( "OrganizationalRoleID", GetExternalObject( concreateEvent.OrganizationalRole.ID ) );
            context.Set( "UserID", GetExternalObject( concreateEvent.User.ID ) );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportOrganizationalRoleServiceClient ).AssemblyQualifiedName;
            executor.Method = "RemoveUser";
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