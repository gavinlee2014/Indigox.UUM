using System;
using System.Collections.Generic;
using Indigox.Common.EventBus;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Service
{
    public class RoleService
    {
        public void Update( IRole role )
        {
            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            propertyChanges.Add( "Name", role.Name );
            propertyChanges.Add( "Email", role.Email );
            propertyChanges.Add( "Description", role.Description );
            propertyChanges.Add( "OrderNum", role.OrderNum );
            propertyChanges.Add( "Level", role.Level );

            EventTrigger.Trigger(role, new RolePropertyChangedEvent(role, propertyChanges));
        }
    }
}