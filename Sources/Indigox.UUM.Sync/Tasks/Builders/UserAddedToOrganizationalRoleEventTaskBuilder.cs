using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class UserAddedToOrganizationalRoleEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            UserAddedToOrganizationaRoleEvent concreateEvent = (UserAddedToOrganizationaRoleEvent)this.Event;
            context.Set( "UserID", GetExternalObject( concreateEvent.User.ID ) );
            context.Set( "OrganizationalRoleID", GetExternalObject( concreateEvent.OrganizationalRole.ID ) );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportOrganizationalRoleServiceClient ).AssemblyQualifiedName;
            executor.Method = "AddUser";
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