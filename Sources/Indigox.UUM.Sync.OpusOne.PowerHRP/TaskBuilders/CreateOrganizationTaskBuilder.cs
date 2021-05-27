using System;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class CreateOrganizationTaskBuilder : AbstractTaskBuilder
    {
        public string NativeID { get; set; }
        public string ParentOrganizationalUnitID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public string OrganizationalUnitType { get; set; }

        protected override string BuildDescription()
        {
            return "添加部门：" + Name + "(" + NativeID + ")";
        }

        protected override ISyncContext BuildTaskContext()
        {
            var context = base.BuildTaskContext();

            var type = this.GetType();
            var props = type.GetProperties();
            foreach ( var prop in props )
            {
                if ( prop.DeclaringType == type )
                {
                    context.Set( prop.Name, prop.GetValue( this, null ) );
                }
            }

            //context.Set("displayName", this.Name);

            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportOrganizationalUnitServiceClient ).AssemblyQualifiedName,
                Method = "Create",
                Url = Source.OrganizationUnitSyncWebService
            };
            return executor;
        }
    }
}