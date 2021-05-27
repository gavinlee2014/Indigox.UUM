using System;
using System.Collections.Generic;
using Indigox.Common.EventBus;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Service
{
    public class OrganizationalUnitService
    {
        public void Update( IOrganizationalUnit organizationalUnit )
        {
            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            propertyChanges.Add( "Name", organizationalUnit.Name );
            propertyChanges.Add( "FullName", organizationalUnit.FullName );
            propertyChanges.Add( "Email", organizationalUnit.Email );
            propertyChanges.Add( "Description", organizationalUnit.Description );
            propertyChanges.Add( "OrderNum", organizationalUnit.OrderNum );
            propertyChanges.Add("DisplayName", organizationalUnit.DisplayName);

            EventTrigger.Trigger(organizationalUnit, new OrganizationalUnitPropertyChangedEvent(organizationalUnit, propertyChanges));
        }
    }
}