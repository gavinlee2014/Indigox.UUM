using System;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;
using Indigox.UUM.Sync.Model;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class OrganizationalRoleCreatedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            OrganizationalRoleCreatedEvent concreateEvent = (OrganizationalRoleCreatedEvent)this.Event;

            IOrganizationalRole organizationalRole = concreateEvent.OrganizationalRole;

            string externalOrganizationalUnitID = null;

            if (organizationalRole.Organization != null)
            {
                externalOrganizationalUnitID = GetExternalID(organizationalRole.Organization.ID);
            }

            //context.Set( "OrganizationalRoleID", GetExternalID( organizationalRole.ID ) );
            context.Set("OrganizationalUnitID", externalOrganizationalUnitID);
            context.Set("NativeID", organizationalRole.ID);
            context.Set("Name", organizationalRole.Name);
            context.Set("FullName", organizationalRole.FullName);
            context.Set("Email", organizationalRole.Email);
            context.Set("Description", organizationalRole.Description);
            context.Set("OrderNum", organizationalRole.OrderNum);
            context.Set("DisplayName", organizationalRole.DisplayName);
            context.Set("ExtendProperties", organizationalRole.ExtendProperties);
            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            SaveIDWebServiceExecutor executor = new SaveIDWebServiceExecutor();
            executor.InternalID = Source.ID.ToString();
            executor.SysClientName = System.ClientName;
            executor.WebServiceType = typeof( ImportOrganizationalRoleServiceClient ).AssemblyQualifiedName;
            executor.Method = "Create";
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