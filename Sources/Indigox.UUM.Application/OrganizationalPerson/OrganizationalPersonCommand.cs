using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.Web.CQRS.Interface;
using Indigox.UUM.Application.Comparers;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public abstract class OrganizationalPersonCommand : ICommand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MailDatabase { get; set; }
        public string AccountName { get; set; }
        public string IdCard { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string OtherContact { get; set; }
        public double OrderNum { get; set; }
        public string Organization { get; set; }
        public string StaffNo { get; set; }
        public string StaffType { get; set; }
        public string Gender { get; set; }
        public string ContractCompanyTexNo { get; set; }
        public string ContractCompanyName { get; set; }
        public string LastJoinDate { get; set; }
        public string NeedClockOn { get; set; }
        public IList<SimplePrincipalDTO> MemberOfOrganizationalRoles { get; set; }
        public IList<SimplePrincipalDTO> MemberOfGroups { get; set; }
        public IList<ProfileDTO> Profile { get; set; }
        public string Level { get; set; }
        //public IDictionary<string, string> ExtendProperties { get; set; }

        public abstract void Execute();

        private HashSet<IContainer> ConvertFromMemberOfOrganizationalRoles(IList<SimplePrincipalDTO> memberOfOrganizationalRoles)
        {
            ContainerComparer cmp=new ContainerComparer();
            HashSet<IContainer> set = new HashSet<IContainer>(cmp);
            var repos = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            for (int i = 0; i < memberOfOrganizationalRoles.Count; i++)
            {
                set.Add(repos.Get(memberOfOrganizationalRoles[i].UserID));
            }            
            return set;
        }

        private HashSet<IContainer> ConvertFromMemberOfGroups(IList<SimplePrincipalDTO> MemberOfGroups)
        {
            ContainerComparer cmp = new ContainerComparer();
            HashSet<IContainer> set = new HashSet<IContainer>(cmp);
            var repos = RepositoryFactory.Instance.CreateRepository<IGroup>();
            for (int i = 0; i < MemberOfGroups.Count; i++)
            {
                set.Add(repos.Get(MemberOfGroups[i].UserID));
            }           
            return set;
        }

        protected void FillPropery( IOrganizationalPerson item )
        {
            IMutableOrganizationalPerson mutableItem = (IMutableOrganizationalPerson)item;
            mutableItem.Name = this.Name;
            mutableItem.FullName = this.Name;
            mutableItem.Email = this.Email;
            mutableItem.AccountName = this.AccountName;
            mutableItem.Title = this.Title;
            mutableItem.IdCard = this.IdCard;
            mutableItem.Mobile = this.Mobile;
            mutableItem.Telephone = this.Telephone;
            mutableItem.Fax = this.Fax;
            mutableItem.OtherContact = this.OtherContact;
            mutableItem.OrderNum = this.OrderNum;
            mutableItem.DisplayName = this.DisplayName;
            mutableItem.Profile = this.Profile.Count>0 ? this.Profile[0].GetFileUrl() : "";
            mutableItem.Level = string.IsNullOrEmpty(this.Level) ? 0 : Int32.Parse(this.Level);
            mutableItem.MailDatabase = this.MailDatabase;
            this.SetMembers(mutableItem);
            mutableItem.ExtendProperties = new Dictionary<string, string>();
            mutableItem.ExtendProperties.Add("StaffNo", this.StaffNo);
            mutableItem.ExtendProperties.Add("StaffType", this.StaffType);
            mutableItem.ExtendProperties.Add("Gender", this.Gender);
            mutableItem.ExtendProperties.Add("NeedClockOn", this.NeedClockOn);
            mutableItem.ExtendProperties.Add("LastJoinDate", this.LastJoinDate);
            mutableItem.ExtendProperties.Add("ContractCompanyTexNo", this.ContractCompanyTexNo);
            mutableItem.ExtendProperties.Add("ContractCompanyName", this.ContractCompanyName);
            mutableItem.ExtendProperties.Add("Portrait", mutableItem.Profile);
        }

        protected void SetMembers(IMutableOrganizationalPerson mutableItem)
        {
            HashSet<IContainer> memberOfSet = new HashSet<IContainer>(mutableItem.MemberOf);
            HashSet<IContainer> memberOfCopySet = new HashSet<IContainer>(new ContainerComparer());
            foreach (var v in memberOfSet)
            {
                memberOfCopySet.Add(v);
            }

            HashSet<IContainer> memberOfOrganizationalRolesSet = ConvertFromMemberOfOrganizationalRoles(MemberOfOrganizationalRoles);
            HashSet<IContainer> allSet = ConvertFromMemberOfGroups(MemberOfGroups);

            allSet.UnionWith(memberOfOrganizationalRolesSet);//求memberOfOrganizationalRolesSet和memberOfGroupsSet的并集，结果在memberOfGroupsSet中
            allSet.Add(RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(this.Organization));

            memberOfSet.ExceptWith(allSet);//需要删除的
            allSet.ExceptWith(memberOfCopySet);//需要添加的

            foreach (var v in memberOfSet)
            {
                mutableItem.RemoveMemberOf(v);
            }
            foreach (var v in allSet)
            {
                mutableItem.AddMemberOf(v);
            }
        }
    }
}