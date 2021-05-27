using System;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;
using Indigox.UUM.Sync.Model;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class UserCreatedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            UserCreatedEvent concreateEvent = (UserCreatedEvent)this.Event;

            IUser user = concreateEvent.User;
            IOrganizationalPerson organizationalPerson = concreateEvent.User as IOrganizationalPerson;

            ExternalObjectDescriptor externalOrganizationalUnitID = null;

            if (organizationalPerson != null && organizationalPerson.Organization != null)
            {
                externalOrganizationalUnitID = GetExternalObject(organizationalPerson.Organization.ID);
            }

            string title = "";
            var extendProperties = organizationalPerson.ExtendProperties;
            if (extendProperties == null)
            {
                extendProperties = new Dictionary<string, string>();
            }
            if (extendProperties.ContainsKey("IdCard"))
            {
                extendProperties["IdCard"] = user.IdCard;
            }
            else
            {
                extendProperties.Add("IdCard", user.IdCard);
            }
            if (organizationalPerson.MemberOf != null)
            {
                foreach (var parent in organizationalPerson.MemberOf)
                {
                    IOrganizationalRole role = parent as IOrganizationalRole;
                    if ((role != null) && (role.Organization.ID.Equals(organizationalPerson.Organization.ID)))
                    {
                        title = role.Name;
                        if (role.ExtendProperties.ContainsKey("RoleLevel"))
                        {
                            if (extendProperties.ContainsKey("RoleLevel"))
                            {
                                extendProperties["RoleLevel"] = role.ExtendProperties["RoleLevel"];
                            }
                            else
                            {
                                extendProperties.Add("RoleLevel", role.ExtendProperties["RoleLevel"]);
                            }
                        }


                        break;
                    }
                }
            }
            //context.Set( "UserID", GetExternalID( user.ID ) );
            context.Set("OrganizationalUnitID", externalOrganizationalUnitID);
            context.Set("NativeID", user.ID);
            context.Set("Name", user.Name);
            context.Set("FullName", user.FullName);
            context.Set("Email", user.Email);
            context.Set("OrderNum", user.OrderNum);
            context.Set("AccountName", user.AccountName);
            context.Set("Title", title);
            context.Set("Mobile", user.Mobile);
            context.Set("Telephone", user.Telephone);
            context.Set("Fax", user.Fax);
            context.Set("OtherContact", user.OtherContact);
            context.Set("DisplayName", user.DisplayName);
            context.Set("Portrait", user.Profile);
            context.Set("MailDatabase", user.MailDatabase);
            context.Set("Description", user.Description);
            context.Set("ExtendProperties", extendProperties);

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            SaveIDWebServiceExecutor executor = new SaveIDWebServiceExecutor();
            executor.InternalID = Source.ID.ToString();
            executor.SysClientName = System.ClientName;
            executor.WebServiceType = typeof(ImportUserServiceClient).AssemblyQualifiedName;
            executor.Method = "Create";
            executor.Url = this.System.UserSyncWebService;
            return executor;
        }

        protected override bool SyncEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(System.UserSyncWebService);
            }
        }
    }
}