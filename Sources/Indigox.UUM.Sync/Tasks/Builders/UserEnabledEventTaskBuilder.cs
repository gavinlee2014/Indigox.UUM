using System;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Model;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class UserEnabledEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            UserEnabledEvent concreateEvent = (UserEnabledEvent)this.Event;

            IUser user = concreateEvent.User;
            IOrganizationalPerson organizationalPerson = concreateEvent.User as IOrganizationalPerson;

            ExternalObjectDescriptor externalOrganizationalUnitID = null;

            if (organizationalPerson != null && organizationalPerson.Organization != null)
            {
                externalOrganizationalUnitID = GetExternalObject(organizationalPerson.Organization.ID);
            }

            context.Set( "UserID", GetExternalID( user.ID ) );
            context.Set("OrganizationalUnitID", externalOrganizationalUnitID);
            context.Set("Name", user.Name);
            context.Set("FullName", user.FullName);
            context.Set("Email", user.Email);
            context.Set("Description", user.Description);
            context.Set("OrderNum", user.OrderNum);
            context.Set("AccountName", user.AccountName);
            context.Set("Title", user.Title);
            context.Set("Mobile", user.Mobile);
            context.Set("Telephone", user.Telephone);
            context.Set("Fax", user.Fax);
            context.Set("OtherContact", user.OtherContact);
            context.Set("DisplayName", user.DisplayName);
            context.Set("Description", user.Description);
            context.Set("Portrait", user.Profile);
            context.Set("MailDatabase", user.MailDatabase);

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportUserServiceClient ).AssemblyQualifiedName;
            executor.Method = "Enable";
            executor.Url = this.System.UserSyncWebService;
            return executor;
        }

        protected override bool SyncEnabled
        {
            get
            {
                return !string.IsNullOrEmpty( System.UserSyncWebService );
            }
        }
    }
}