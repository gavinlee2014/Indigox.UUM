using System;
using Indigox.CMS.Security.Factory;
using Indigox.CMS.Security.Interfaces;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.Membership.Interfaces;
using System.Data.SqlTypes;
using Indigox.Common.Logging;

namespace Indigox.UUM.EventHandlers
{
    internal class PrincipalEventHandler
    {
        public void OnAdded( object source, IEvent e )
        {
            IPrincipal pincipal = (IPrincipal)source;
            IPrincipal parent = GetParentObject(pincipal);
            IAcl acl = AclFactory.Instance.Create(source, parent, null);
        }

        private IPrincipal GetParentObject( IPrincipal source )
        {
            IPrincipal pincipal = (IPrincipal)source;

            if ( pincipal is IOrganizationalObject )
            {
                IOrganizationalObject organizationalObject = (IOrganizationalObject)pincipal;
                return organizationalObject.Organization;
            }
            else
            {
                return null;
            }
        }

        public void OnUpdate( object source, IEvent e )
        {
            IMutablePrincipal principal = source as IMutablePrincipal;
            principal.ModifyTime = DateTime.Now;
        }
    }
}