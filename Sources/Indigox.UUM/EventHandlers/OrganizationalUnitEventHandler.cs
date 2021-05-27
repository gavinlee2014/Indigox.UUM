using System;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;

namespace Indigox.UUM.EventHandlers
{
    internal class OrganizationalUnitEventHandler
    {
        public void OnDelete( object source, IEvent e )
        {
            IOrganizationalUnit org = source as IOrganizationalUnit;

            PrincipalService service = new PrincipalService();

            foreach ( IPrincipal child in org.Members )
            {
                // delete child
                service.Delete( child );
            }
        }
    }
}