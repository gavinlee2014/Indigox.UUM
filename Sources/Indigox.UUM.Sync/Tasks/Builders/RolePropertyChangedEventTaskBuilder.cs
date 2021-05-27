﻿using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class RolePropertyChangedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            RolePropertyChangedEvent concreateEvent = (RolePropertyChangedEvent)this.Event;
            context.Set( "RoleID", GetExternalObject( concreateEvent.Role.ID ) );
            context.Set( "PropertyChanges", concreateEvent.PropertyChanges );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportRoleServiceClient ).AssemblyQualifiedName;
            executor.Method = "ChangeProperty";
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