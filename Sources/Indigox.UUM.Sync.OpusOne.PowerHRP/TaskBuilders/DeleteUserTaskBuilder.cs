using System;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class DeleteUserTaskBuilder : AbstractTaskBuilder
    {
        public string UserID { get; set; }
        public string Name { get; set; }

        protected override string BuildDescription()
        {
            return "删除用户：" + Name;
        }

        protected override ISyncContext BuildTaskContext()
        {
            ISyncContext context = base.BuildTaskContext();
            context.Set( "userID", UserID );
            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportUserServiceClient ).AssemblyQualifiedName,
                Method = "Delete",
                Url = Source.UserSyncWebService
            };
            return executor;
        }
    }
}