using System;
using System.Collections.Generic;
using Indigox.Common.EventBus;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Service
{
    public class GroupService
    {
        public void Update(IGroup group)
        {
            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            propertyChanges.Add("Name", group.Name);
            propertyChanges.Add("DisplayName", group.DisplayName);
            propertyChanges.Add("Email", group.Email);
            propertyChanges.Add("Description", group.Description);
            propertyChanges.Add("OrderNum", group.OrderNum);

            EventTrigger.Trigger(group, new GroupPropertyChangedEvent(group, propertyChanges));
        }
    }
}