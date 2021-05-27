using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.WebServiceClients;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal class UserPropertyChangedEventTaskBuilder : AbstractPrincipalEventTaskBuilder
    {
        protected override ISyncContext BuildSyncContext()
        {
            ISyncContext context = base.BuildSyncContext();

            UserPropertyChangedEvent concreateEvent = (UserPropertyChangedEvent)this.Event;
            IUser user = concreateEvent.User;

            string title = "";
            IDictionary<string, object> propertyChanges = concreateEvent.PropertyChanges;
            IOrganizationalPerson organizationalPerson = concreateEvent.User as IOrganizationalPerson;
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
                            if (propertyChanges.ContainsKey("RoleLevel"))
                            {
                                propertyChanges["RoleLevel"] = role.ExtendProperties["RoleLevel"];
                            }
                            else
                            {
                                propertyChanges.Add("RoleLevel", role.ExtendProperties["RoleLevel"]);
                            }
                        }
                        


                        break;
                    }
                    
                }
            }
            Dictionary<string, string> extendProperties = (Dictionary<string, string>)propertyChanges["ExtendProperties"];
            if (extendProperties.ContainsKey("IdCard"))
            {
                extendProperties["IdCard"] = organizationalPerson.IdCard;
            }
            else
            {
                extendProperties.Add("IdCard", organizationalPerson.IdCard);
            }
            foreach (var key in extendProperties.Keys)
            {
                if (!propertyChanges.ContainsKey(key))
                {
                    propertyChanges.Add(key, extendProperties[key]);
                }
            }
            if (propertyChanges.ContainsKey("Title"))
            {
                propertyChanges["Title"] = title;
            }
            else
            {
                propertyChanges.Add("Title", title);
            }


            context.Set( "UserID", GetExternalObject( user.ID ) );
            context.Set( "PropertyChanges", propertyChanges);

            return context;
        }

        protected override ISyncExecutor BuildSyncExecutor()
        {
            WebServiceExecutor executor = new WebServiceExecutor();
            executor.WebServiceType = typeof( ImportUserServiceClient ).AssemblyQualifiedName;
            executor.Method = "ChangeProperty";
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