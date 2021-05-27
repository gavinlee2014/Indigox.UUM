﻿using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class OrganizationalUnitAddedToOrganizationalUnitEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            OrganizationalUnitAddedToOrganizationalUnitEvent concreateEvent = (OrganizationalUnitAddedToOrganizationalUnitEvent)this.Event;
            context.Set( "OrganizationalUnitID", GetExternalObject( concreateEvent.OrganizationalUnit.ID ) );
            context.Set( "ParentOrganizationalUnitID", GetExternalObject( concreateEvent.ParentOrganizationalUnit.ID ) );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportOrganizationalUnitServiceClient ).AssemblyQualifiedName;
            executor.Method = "AddOrganizationalUnit";
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