using System;
using System.Collections.Generic;
using Indigox.Common.EventBus;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Service
{
    public class OrganizationalRoleService
    {
        public void Update( IOrganizationalRole organizationalRole )
        {
            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            propertyChanges.Add( "Name", organizationalRole.Name );
            propertyChanges.Add( "FullName", organizationalRole.FullName );
            propertyChanges.Add( "Email", organizationalRole.Email );
            propertyChanges.Add( "Description", organizationalRole.Description );
            propertyChanges.Add( "OrderNum", organizationalRole.OrderNum );
            propertyChanges.Add("DisplayName", organizationalRole.DisplayName);

            EventTrigger.Trigger(organizationalRole, new OrganizationalRolePropertyChangedEvent(organizationalRole, propertyChanges));
        }
    }
}