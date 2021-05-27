using System;
using System.Collections.Generic;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class ChangeOrganizationTaskBuilder : AbstractTaskBuilder
    {
        public string OrganizationalUnitID { get; set; }
        public IDictionary<string, object> PropertyChanges { get; set; }
        public string Name { get; set; }

        protected override string BuildDescription()
        {
            return "修改部门：" + Name;
        }

        protected override ISyncContext BuildTaskContext()
        {
            ISyncContext context = base.BuildTaskContext();
            context.Set( "organizationalUnitID", OrganizationalUnitID );
            context.Set( "propertyChanges", PropertyChanges );
            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportOrganizationalUnitServiceClient ).AssemblyQualifiedName,
                Method = "ChangeProperty",
                Url = Source.OrganizationUnitSyncWebService
            };
            return executor;
        }
    }
}