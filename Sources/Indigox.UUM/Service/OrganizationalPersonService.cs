using System;
using System.Collections.Generic;
using Indigox.Common.EventBus;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Util;

namespace Indigox.UUM.Service
{
    public class OrganizationalPersonService
    {
        public void Update(IOrganizationalPerson organizationalPerson)
        {
            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            propertyChanges.Add("Name", organizationalPerson.Name);
            propertyChanges.Add("FullName", organizationalPerson.Name);
            propertyChanges.Add("Email", organizationalPerson.Email);
            propertyChanges.Add("AccountName", organizationalPerson.AccountName);
            propertyChanges.Add("Title", organizationalPerson.Title);
            propertyChanges.Add("Mobile", organizationalPerson.Mobile);
            propertyChanges.Add("Telephone", organizationalPerson.Telephone);
            propertyChanges.Add("Fax", organizationalPerson.Fax);
            propertyChanges.Add("OtherContact", organizationalPerson.OtherContact);
            propertyChanges.Add("OrderNum", organizationalPerson.OrderNum);
            propertyChanges.Add("DisplayName", organizationalPerson.DisplayName);
            propertyChanges.Add("IdCard", organizationalPerson.IdCard);
            propertyChanges.Add("Profile", organizationalPerson.Profile);
            var cloned = new Dictionary<string, string>();
            foreach (var key in organizationalPerson.ExtendProperties.Keys)
            {
                cloned.Add(key, organizationalPerson.ExtendProperties[key]);
            }
            propertyChanges.Add("ExtendProperties", cloned);

            EventTrigger.Trigger(organizationalPerson, new UserPropertyChangedEvent(organizationalPerson, propertyChanges));
        }
        public void UpdateUserPasswordByAccount(string accountName, string pwd)
        {
            PasswordUtil.UpdatePasswordByAccount(accountName, pwd);
        }
        public void UpdateUserPassword(IOrganizationalPerson organizationalPerson, string pwd)
        {
            PasswordUtil.UpdatePassword(organizationalPerson.ID, pwd);
        }

        public string GetPassword(IOrganizationalPerson organizationalPerson)
        {
            return PasswordUtil.GetPassword(organizationalPerson.ID);
        }
    }
}