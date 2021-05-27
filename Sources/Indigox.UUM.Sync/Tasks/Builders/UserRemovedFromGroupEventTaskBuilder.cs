﻿using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class UserRemovedFromGroupEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            UserRemovedFromGroupEvent concreateEvent = (UserRemovedFromGroupEvent)this.Event;
            context.Set( "GroupID", GetExternalObject( concreateEvent.Group.ID ) );
            context.Set( "UserID", GetExternalObject( concreateEvent.User.ID ) );

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportGroupServiceClient ).AssemblyQualifiedName;
            executor.Method = "RemoveUser";
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