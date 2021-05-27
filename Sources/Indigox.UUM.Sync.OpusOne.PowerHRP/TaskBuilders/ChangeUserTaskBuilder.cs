using System;
using System.Collections.Generic;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class ChangeUserTaskBuilder : AbstractTaskBuilder
    {
        public string UserID { get; set; }
        public IDictionary<string, object> PropertyChanges { get; set; }
        public string Name { get; set; }

        protected override string BuildDescription()
        {
            return "修改用户：" + Name;
        }

        protected override ISyncContext BuildTaskContext()
        {
            ISyncContext context = base.BuildTaskContext();
            context.Set( "userID", UserID );
            context.Set( "propertyChanges", PropertyChanges );
            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportUserServiceClient ).AssemblyQualifiedName,
                Method = "ChangeProperty",
                Url = Source.UserSyncWebService
            };
            return executor;
        }
    }
}