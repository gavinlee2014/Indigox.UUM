using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.Web.CQRS.Interface;
using Indigox.UUM.Application.Comparers;
using Indigox.Common.DomainModels.Factory;

namespace Indigox.UUM.Application.Role
{
    public abstract class RoleCommand : ICommand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public int Level { get; set; }
        public IList<SimplePrincipalDTO> Members { get; set; }

        public abstract void Execute();

        protected void FillPropery( IRole item )
        {
            IMutableRole mutableItem = (IMutableRole)item;
            mutableItem.Name = this.Name;
            mutableItem.Email = this.Email;
            mutableItem.Description = this.Description;
            mutableItem.OrderNum = this.OrderNum;
            mutableItem.Level = (RoleLevel)this.Level;

            SetMembers(mutableItem);
        }

        private HashSet<IPrincipal> ConvertFromMembers(IList<SimplePrincipalDTO> members)
        {
            HashSet<IPrincipal> set = new HashSet<IPrincipal>(new PrincipalComparer());
            var repos = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            for (int i = 0; i < members.Count; i++)
            {
                set.Add(repos.Get(members[i].UserID));
            }
            return set;
        }

        protected void SetMembers(IMutableRole mutableItem)
        {
            HashSet<IPrincipal> membersSet = ConvertFromMembers(Members);
            HashSet<IPrincipal> oldMemberSet = new HashSet<IPrincipal>(mutableItem.Members, new PrincipalComparer());
            HashSet<IPrincipal> oldMemberSetCopy = new HashSet<IPrincipal>(new PrincipalComparer());
            foreach (var v in oldMemberSet)
            {
                oldMemberSetCopy.Add(v);
            }

            oldMemberSet.ExceptWith(membersSet);
            membersSet.ExceptWith(oldMemberSetCopy);

            foreach (var v in oldMemberSet)
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