using System;
using Indigox.Common.EventBus;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    public class RoleFactory : PrincipalFactory
    {   
        public RoleLevel Level { get; set; }

        public IMutableRole Create()
        {
            return this.Create(this.GetNextID(), false);
        }

        public IMutableRole Create(string id)
        {
            return Create(id, true);
        }

        private IMutableRole Create(string id, bool triggleEvent)
        {
            IMutableRole mutableItem = new Role();
            mutableItem.ID = id;
            mutableItem.Deleted = false;

            this.SetBaseProperties(mutableItem);

            mutableItem.Level = this.Level;

            if (triggleEvent)
            {
                EventTrigger.Trigger(mutableItem, new RoleCreatedEvent(mutableItem));
            }

            return mutableItem;
        }

        public override string GetNextID()
        {
            return "PS" + UUMIdGernerator.GetNext();
        }
    }
}