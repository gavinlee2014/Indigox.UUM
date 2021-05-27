using System;
using System.Collections.Generic;
using Indigox.Common.EventBus;
using Indigox.Common.Membership;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    public class OrganizationalPersonFactory : PrincipalFactory
    {

        public string AccountName { get; set; }
        public string Title { get; set; }
        public string IdCard { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string OtherContact { get; set; }
        public IOrganizationalUnit OrganizationalUnit { get; set; }
        public string Profile { get; set; }
        public string MailDatabase { get; set; }

        public IMutableOrganizationalPerson Create()
        {
            return this.Create(this.GetNextID(), false);
        }

        public IMutableOrganizationalPerson Create(string id)
        {
            return this.Create(id, true);
        }

        private IMutableOrganizationalPerson Create(string id, bool triggleEvent)
        {
            IMutableOrganizationalPerson mutableItem = this.OrganizationalUnit != null ? new OrganizationalPerson(this.OrganizationalUnit): new OrganizationalPerson();

            mutableItem.ID = id;
            mutableItem.Deleted = false;

            this.SetBaseProperties(mutableItem);

            mutableItem.AccountName = this.AccountName;
            mutableItem.IdCard = this.IdCard;
            mutableItem.Title = this.Title;
            mutableItem.Mobile = this.Mobile;
            mutableItem.Telephone = this.Telephone;
            mutableItem.Fax = this.Fax;
            mutableItem.OtherContact = this.OtherContact;
            mutableItem.Profile = this.Profile;
            mutableItem.MailDatabase = this.MailDatabase;
            mutableItem.ExtendProperties = this.ExtendProperties;

            if (triggleEvent)
            {
                EventTrigger.Trigger(mutableItem, new UserCreatedEvent(mutableItem));
            }

            return mutableItem;
        }

        public override string GetNextID()
        {
            return "UR" + UUMIdGernerator.GetNext();
        }
    }
}