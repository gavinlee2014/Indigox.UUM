using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.Web.CQRS.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Application.Comparers;

namespace Indigox.UUM.Application.Group
{
    public abstract class GroupCommand : ICommand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public IList<SimplePrincipalDTO> Members { get; set; }

        public abstract void Execute();

        protected void FillPropery( IGroup item )
        {
            IMutableGroup mutableItem = (IMutableGroup)item;
            mutableItem.Name = this.Name;
            mutableItem.Email = this.Email;
            mutableItem.Description = this.Description;
            mutableItem.OrderNum = this.OrderNum;
            mutableItem.DisplayName = this.DisplayName;

            this.SetMembers(mutableItem);
        }

        private HashSet<IPrincipal> ConvertFromMembers(IList<SimplePrincipalDTO> members)
        {
            HashSet<IPrincipal> set = new HashSet<IPrincipal>(new PrincipalComparer());
            for (int i = 0; i < members.Count; i++)
            {
                set.Add(Indigox.Common.Membership.Principal.GetPrincipalByID(members[i].UserID));
            }
            return set;
        }

        protected void SetMembers(IMutableGroup mutableItem)
        {
            HashSet<IPrincipal> membersSet = ConvertFromMembers(Members);
            HashSet<IPrincipal> oldMemberSet = new HashSet<IPrincipal>(new PrincipalComparer());
            HashSet<IPrincipal> oldMemberSetCopy = new HashSet<IPrincipal>(new PrincipalComparer());
            foreach (var v in mutableItem.Members)
            {
                oldMemberSet.Add((IPrincipal)v);
                oldMemberSetCopy.Add((IPrincipal)v);
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