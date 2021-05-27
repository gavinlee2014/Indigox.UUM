using System;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class GroupCreatedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            GroupCreatedEvent concreateEvent = (GroupCreatedEvent)this.Event;

            IGroup group = concreateEvent.Group;

            //context.Set( "GroupID", GetExternalID( group.ID ) );
            context.Set("NativeID", group.ID);
            context.Set( "Email", group.Email );
            context.Set( "Name", group.Name );
            context.Set( "OrderNum", group.OrderNum );
            context.Set( "Description", group.Description );
            context.Set("ExtendProperties", group.ExtendProperties);

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            SaveIDWebServiceExecutor executor = new SaveIDWebServiceExecutor();
            executor.InternalID = Source.ID.ToString();
            executor.SysClientName = System.ClientName;
            executor.WebServiceType = typeof( ImportGroupServiceClient ).AssemblyQualifiedName;
            executor.Method = "Create";
            executor.Url = this.System.GroupSyncWebService;
            return executor;
        }

        protected override bool SyncEnabled
        {
            get
            {
                return !string.IsNullOrEmpty( System.GroupSyncWebService );
            }
        }
    }
}