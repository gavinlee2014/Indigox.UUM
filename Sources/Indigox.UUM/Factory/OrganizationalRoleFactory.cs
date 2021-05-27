using System;
using Indigox.Common.EventBus;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    public class OrganizationalRoleFactory : PrincipalFactory
    {
        public IRole Role { get; set; }
        public IOrganizationalUnit OrganizationalUnit { get; set; }

        public IMutableOrganizationalRole Create()
        {
            return this.Create(this.GetNextID(), false);
        }

        public IMutableOrganizationalRole Create(string id)
        {
            return Create(id, true);
        }

        private IMutableOrganizationalRole Create(string id, bool triggleEvent)
        {
            IMutableOrganizationalRole mutableItem = this.OrganizationalUnit != null ? new OrganizationalRole(this.OrganizationalUnit) : new OrganizationalRole();

            mutableItem.ID = id;
            mutableItem.Deleted = false;

            this.SetBaseProperties(mutableItem);
            
            if (triggleEvent)
            {
                EventTrigger.Trigger(mutableItem, new OrganizationalRoleCreatedEvent(mutableItem));
            }

            mutableItem.Role = this.Role;

            return mutableItem;
        }

        public override string GetNextID()
        {
            return "DP" + UUMIdGernerator.GetNext();
        }
    }
}