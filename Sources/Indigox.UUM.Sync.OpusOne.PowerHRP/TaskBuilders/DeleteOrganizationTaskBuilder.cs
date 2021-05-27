using System;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class DeleteOrganizationTaskBuilder : AbstractTaskBuilder
    {
        public string Name { get; set; }
        public string OrganizationalUnitID { get; set; }

        protected override string BuildDescription()
        {
            return "删除部门：" + Name;
        }

        protected override ISyncContext BuildTaskContext()
        {
            ISyncContext context = base.BuildTaskContext();
            context.Set( "organizationalUnitID", OrganizationalUnitID );
            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportOrganizationalUnitServiceClient ).AssemblyQualifiedName,
                Method = "Delete",
                Url = Source.OrganizationUnitSyncWebService
            };
            return executor;
        }
    }
}