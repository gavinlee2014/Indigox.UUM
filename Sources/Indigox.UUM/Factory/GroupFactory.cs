using System;
using Indigox.Common.EventBus;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    public class GroupFactory : PrincipalFactory
    {
        //private static readonly GroupFactory instance = new GroupFactory();

        //public static GroupFactory Instance
        //{
        //    get { return instance; }
        //}

        //private GroupFactory()
        //{
        //}

        public IMutableGroup Create()
        {
            return this.Create(this.GetNextID(), false);
        }

        public IMutableGroup Create(string id)
        {
            return Create(id, true);
        }

        private IMutableGroup Create(string id, bool triggleEvent)
        {
            IMutableGroup mutableItem = new Group();
            mutableItem.ID = id;
            mutableItem.Deleted = false;

            this.SetBaseProperties(mutableItem);

            if (triggleEvent)
            {
                EventTrigger.Trigger(mutableItem, new GroupCreatedEvent(mutableItem));
            }

            return mutableItem;
        }

        public override string GetNextID()
        {
            return "DG" + UUMIdGernerator.GetNext();
        }
    }
}