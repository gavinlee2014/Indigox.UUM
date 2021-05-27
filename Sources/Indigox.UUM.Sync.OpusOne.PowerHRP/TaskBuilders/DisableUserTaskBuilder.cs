using System;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class DisableUserTaskBuilder : AbstractTaskBuilder
    {
        public string UserID { get; set; }
        public string Name { get; set; }

        protected override string BuildDescription()
        {
            return "禁用用户：" + Name;
        }

        protected override ISyncContext BuildTaskContext()
        {
            ISyncContext context = base.BuildTaskContext();
            context.Set( "UserID", UserID );
            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportUserServiceClient ).AssemblyQualifiedName,
                Method = "Disable",
                Url = Source.UserSyncWebService
            };
            return executor;
        }
    }
}