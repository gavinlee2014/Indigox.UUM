using System;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class RoleCreatedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            RoleCreatedEvent concreateEvent = (RoleCreatedEvent)this.Event;

            IRole role = concreateEvent.Role;

            //context.Set( "GroupID", GetExternalID( role.ID ) );
            context.Set("NativeID", role.ID);
            context.Set("Description", role.Description);
            context.Set( "Email", role.Email );
            context.Set( "Name", role.Name );
            context.Set( "OrderNum", role.OrderNum );
            context.Set("ExtendProperties", role.ExtendProperties);

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            SaveIDWebServiceExecutor executor = new SaveIDWebServiceExecutor();
            executor.InternalID = Source.ID.ToString();
            executor.SysClientName = System.ClientName;
            executor.WebServiceType = typeof( ImportRoleServiceClient ).AssemblyQualifiedName;
            executor.Method = "Create";
            executor.Url = this.System.RoleSyncWebService;
            return executor;
        }

        protected override bool SyncEnabled
        {
            get
            {
                return !string.IsNullOrEmpty( System.RoleSyncWebService );
            }
        }
    }
}