using System;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;
using Indigox.UUM.Sync.Model;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class OrganizationalUnitCreatedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            OrganizationalUnitCreatedEvent concreateEvent = (OrganizationalUnitCreatedEvent)this.Event;

            IOrganizationalUnit organizationalUnit = concreateEvent.OrganizationalUnit;

            ExternalObjectDescriptor externalOrganizationalUnitID = null;

            if ( organizationalUnit.Organization != null )
            {
                externalOrganizationalUnitID = GetExternalObject( organizationalUnit.Organization.ID );
            }

            //context.Set( "OrganizationalUnitID", GetExternalID( organizationalUnit.ID ) );
            context.Set( "ParentOrganizationalUnitID", externalOrganizationalUnitID );
            context.Set("NativeID", organizationalUnit.ID);
            context.Set("OrganizationalUnitType", organizationalUnit.GetType().Name);
            context.Set( "Name", organizationalUnit.Name );
            context.Set( "FullName", organizationalUnit.FullName );
            context.Set( "Email", organizationalUnit.Email );
            context.Set( "Description", organizationalUnit.Description );
            context.Set( "OrderNum", organizationalUnit.OrderNum );
            context.Set("DisplayName", organizationalUnit.DisplayName);
            context.Set("ExtendProperties", organizationalUnit.ExtendProperties);

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            SaveIDWebServiceExecutor executor = new SaveIDWebServiceExecutor();
            executor.InternalID = Source.ID.ToString();
            executor.SysClientName = System.ClientName;
            executor.WebServiceType = typeof( ImportOrganizationalUnitServiceClient ).AssemblyQualifiedName;
            executor.Method = "Create";
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