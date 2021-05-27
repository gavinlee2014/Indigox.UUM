using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.Web.CQRS.Interface;
using Indigox.UUM.Application.Comparers;

namespace Indigox.UUM.Application.OrganizationalRole
{
    public abstract class OrganizationalRoleCommand : ICommand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public string Organization { get; set; }
        public IList<SimplePrincipalDTO> Members { get; set; }
        public string DisplayName { get; set; }
        public string Level { get; set; }
        public string RoleLevel { get; set; }

        public abstract void Execute();

        private HashSet<IPrincipal> ConvertFromMembers(IList<SimplePrincipalDTO> members)
        {
            HashSet<IPrincipal> set = new HashSet<IPrincipal>(new PrincipalComparer());
            var repos = RepositoryFactory.Instance.CreateRepository<IPrincipal>();
            for (int i = 0; i < members.Count; i++)
            {
                set.Add(repos.Get(members[i].UserID));
            }
            return set;
        }

        protected void FillPropery( IOrganizationalRole item )
        {
            IMutableOrganizationalRole mutableItem = (IMutableOrganizationalRole)item;
            mutableItem.Name = this.Name;
            mutableItem.FullName = this.Name;
            mutableItem.Email = this.Email;
            mutableItem.Description = this.Description;
            mutableItem.OrderNum = this.OrderNum;
            mutableItem.DisplayName = this.DisplayName;
            mutableItem.ExtendProperties = new Dictionary<string, string>();
            mutableItem.ExtendProperties.Add("Level", this.Level);
            mutableItem.ExtendProperties.Add("RoleLevel", this.RoleLevel);
            SetMembers(mutableItem);
        }

        protected void SetMembers(IMutableOrganizationalRole mutableItem)
        {

            HashSet<IPrincipal> memberOfSet = new HashSet<IPrincipal>(mutableItem.Members);
            HashSet<IPrincipal> memberOfSetCopy = new HashSet<IPrincipal>(new PrincipalComparer());
            foreach (var v in memberOfSet)
            {
                memberOfSetCopy.Add(v);
            }

            HashSet<IPrincipal> membersSet = ConvertFromMembers(Members);
            memberOfSet.ExceptWith(membersSet);
            membersSet.ExceptWith(memberOfSetCopy);

            foreach (var v in memberOfSet)
            {
                mutableItem.RemoveMember(v);
            }
            foreach (var v in membersSet)
            {
                mutableItem.AddMember(v);
            }

        }
    }
}