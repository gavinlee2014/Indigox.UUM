using System;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders
{
    internal class CreateUserTaskBuilder : AbstractTaskBuilder
    {
        public string NativeID { get; set; }
        public string OrganizationalUnitID { get; set; }
        public string AccountName { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public double OrderNum { get; set; }
        public string Description { get; set; }
        public string OtherContact { get; set; }
        public string Portrait { get; set; }

        protected override string BuildDescription()
        {
            return "新建用户：" + Name;
        }

        protected override ISyncContext BuildTaskContext()
        {
            ISyncContext context = base.BuildTaskContext();
            context.Set( "nativeID", NativeID );
            context.Set( "accountName", null );
            context.Set( "organizationalUnitID", OrganizationalUnitID );
            context.Set( "name", Name );
            context.Set( "fullName", FullName );
            context.Set( "email", Email );
            context.Set( "title", Title );
            context.Set( "mobile", Mobile );
            context.Set( "telephone", Telephone );
            context.Set( "fax", Fax );
            context.Set( "orderNum", 1.0 );
            context.Set( "description", Description );
            context.Set( "otherContact", OtherContact );
            context.Set( "displayName", Name );
            context.Set("portrait", Portrait);
            context.Set("mailDatabase", "");
            return context;
        }

        protected override ISyncExecutor BuildTaskExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor()
            {
                WebServiceType = typeof( ImportUserServiceClient ).AssemblyQualifiedName,
                Method = "Create",
                Url = Source.UserSyncWebService
            };
            return executor;
        }
    }
}