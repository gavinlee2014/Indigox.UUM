using System;
using Indigox.Common.EventBus;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    public class OrganizationalUnitFactory : PrincipalFactory
    {
        public IOrganizationalUnit ParentOrganizationalUnit { get; set; }

        public IMutableOrganizationalUnit Create()
        {
            return this.Create(this.GetNextID(), typeof(Department).Name, false);
        }

        public IMutableOrganizationalUnit Create(string id, string type)
        {
            return Create(id, type, true);
        }

        private IMutableOrganizationalUnit Create(string id, string type, bool triggleEvent)
        {
            IMutableOrganizationalUnit mutableItem;

            if (type == typeof(OrganizationalUnit).Name)
            {
                mutableItem = this.ParentOrganizationalUnit != null ? new OrganizationalUnit(this.ParentOrganizationalUnit) : new OrganizationalUnit(); 
            }
            else if (type == typeof(Corporation).Name)
            {
                mutableItem = this.ParentOrganizationalUnit != null ? new Corporation(this.ParentOrganizationalUnit) : new Corporation();
            }
            else if (type == typeof(Company).Name)
            {
                mutableItem = this.ParentOrganizationalUnit != null ? new Company(this.ParentOrganizationalUnit) : new Company();
            }
            else if (type == typeof(Department).Name)
            {
                mutableItem = this.ParentOrganizationalUnit != null ? new Department(this.ParentOrganizationalUnit) : new Department();
            }
            else if (type == typeof(Section).Name)
            {
                mutableItem = this.ParentOrganizationalUnit != null ? new Section(this.ParentOrganizationalUnit) : new Section();
            }
            else
            {
                throw new ApplicationException("不支持的类型：" + type);
            }

            mutableItem.ID = id;
            mutableItem.Deleted = false;

            this.SetBaseProperties(mutableItem);

            if (triggleEvent)
            {
                EventTrigger.Trigger(mutableItem, new OrganizationalUnitCreatedEvent(mutableItem));
            }

            return mutableItem;
        }

        public override string GetNextID()
        {
            return "OR" + UUMIdGernerator.GetNext();
        }
    }
}