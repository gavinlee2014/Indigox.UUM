using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.EventBus;
using Indigox.Common.Membership.Events;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Utilities;
using Indigox.Common.Membership;

namespace Indigox.UUM.Service
{
    public class PrincipalService
    {
        private IRepository<IPrincipal> repository = RepositoryFactory.Instance.CreateRepository<IPrincipal>();

        public void Disable(IPrincipal principal)
        {
            ArgumentAssert.NotNull(principal, "principal");

            IMutablePrincipal mutablePrincipal = (IMutablePrincipal)principal;

            if (!mutablePrincipal.Enabled || mutablePrincipal.Deleted)
            {
                return;
            }

            mutablePrincipal.Enabled = false;

            // 禁用时解除所有关系 - Members
            if (mutablePrincipal is IMutableContainer)
            {
                IMutableContainer container = (IMutableContainer)mutablePrincipal;

                List<IPrincipal> members = new List<IPrincipal>(container.Members);

                container.ClearMembers();

                foreach (IPrincipal member in members)
                {
                    repository.Update(member);
                }
            }

            // 禁用时解除所有关系 - MemberOf
            mutablePrincipal.ClearMemberOf();
            //解除关系，最后禁用用户
            TriggerDisabledEvent(principal);
            repository.Update(mutablePrincipal);
        }

        [Obsolete("Deprecated", true)]
        public void Enable(IPrincipal principal)
        {
            ArgumentAssert.NotNull(principal, "principal");

            TriggerEnabledEvent(principal);

            IMutablePrincipal mutablePrincipal = (IMutablePrincipal)principal;

            if (mutablePrincipal.Enabled || mutablePrincipal.Deleted)
            {
                return;
            }

            mutablePrincipal.Enabled = true;

            repository.Update(mutablePrincipal);
        }

        public void Enable(IOrganizationalObject organizationalObject, IOrganizationalUnit organization)
        {
            ArgumentAssert.NotNull(organizationalObject, "organizationalObject");

            TriggerEnabledEvent((IPrincipal)organizationalObject);

            IMutableOrganizationalObject mutableOrganizationalObject = (IMutableOrganizationalObject)organizationalObject;
            IMutableOrganizationalUnit mutableOrganization = (IMutableOrganizationalUnit)organization;
            IMutablePrincipal mutablePrincipal = ((IMutablePrincipal)mutableOrganizationalObject);

            if (mutablePrincipal.Enabled || mutablePrincipal.Deleted)
            {
                return;
            }

            mutableOrganizationalObject.Organization = null; // the organization maybe not changed, but the membership is deleted.

            mutableOrganizationalObject.Organization = organization;

            mutablePrincipal.Enabled = true;

            repository.Update(mutablePrincipal);
        }

        public void Delete(IPrincipal principal)
        {
            ArgumentAssert.NotNull(principal, "principal");



            IMutablePrincipal mutablePrincipal = ((IMutablePrincipal)principal);

            //if ( principal.Deleted )
            //{
            //    return;
            //}

            // 删除时解除所有关系 - Members
            if (mutablePrincipal is IOrganizationalUnit)
            {
                IOrganizationalUnit ou = (IOrganizationalUnit)mutablePrincipal;
                if (ou.Members.Count > 0)
                {
                    throw new ApplicationException("部门下还有部门、用户、组织角色，删除部门之前请删除或移走部门下所有的数据！");
                }
            }
            else if (mutablePrincipal is IRole)
            {
                IRole role = (IRole)mutablePrincipal;
                if (role.Members.Count > 0)
                {
                    throw new ApplicationException("角色下还有组织角色，删除角色之前请删除角色下所有的数据！");
                }
            }
            else if (mutablePrincipal is IMutableContainer)
            {
                IMutableContainer container = (IMutableContainer)mutablePrincipal;

                List<IPrincipal> members = new List<IPrincipal>(container.Members);

                container.ClearMembers();

                foreach (IPrincipal member in members)
                {
                    repository.Update(member);
                }
            }

            // 删除时解除所有关系 - MemberOf
            mutablePrincipal.ClearMemberOf();

            TriggerDeletedEvent(principal);

            repository.Remove(mutablePrincipal);
        }

        public void Unlock(IPrincipal principal)
        {
            
        }

        //public void Recycle( IPrincipal principal )
        //{
        //    throw new NotImplementedException();
        //}

        public void SetMemberOf(IPrincipal principal, IList<IContainer> memberOf)
        {
            ArgumentAssert.NotNull(principal, "principal");

            IMutablePrincipal mutablePrincipal = ((IMutablePrincipal)principal);

            mutablePrincipal.ClearMemberOf();

            foreach (IContainer container in memberOf)
            {
                mutablePrincipal.AddMemberOf(container);
            }

            repository.Update(principal);
        }

        public void SetMembers(IContainer container, IList<IPrincipal> members)
        {
            ArgumentAssert.NotNull(container, "container");

            IMutableContainer mutableContainer = ((IMutableContainer)container);

            List<IPrincipal> oldMembers = new List<IPrincipal>(mutableContainer.Members);

            mutableContainer.ClearMembers();

            foreach (IPrincipal member in members)
            {
                mutableContainer.AddMember(member);
                repository.Update(member);
            }

            foreach (IPrincipal member in oldMembers)
            {
                repository.Update(member);
            }

            repository.Update(mutableContainer);
        }

        public void MoveTo(IOrganizationalObject obj, IOrganizationalUnit target)
        {
            IMutableOrganizationalObject mutableObj = (IMutableOrganizationalObject)obj;
            IMutableOrganizationalUnit origin = (IMutableOrganizationalUnit)obj.Organization;

            mutableObj.Organization = target;

            if (origin != null)
            {
                repository.Update(origin);
            }

            if (target != null)
            {
                repository.Update(target);

                /*
                 * 修改时间：2018-09-14
                 * 修改人：曾勇
                 * 修改原因：HR将人员移动到别的部门时，原来的逻辑是根据移动到的部门，对人员的DisplayName重新赋值，此逻辑不适应，删除
                 *
                
                if (obj is IOrganizationalPerson)
                {
                    OrganizationalPerson persion = (OrganizationalPerson)obj;
                    string displayName = "";
                    if (target.DisplayName.IndexOf(".") != -1)
                    {
                        displayName = target.DisplayName.Substring(0,target.DisplayName.IndexOf("."));
                    }
                    if (persion.Level != -1)
                    {
                        displayName += persion.Level.ToString();
                    }
                    displayName += persion.Name;
                    persion.DisplayName = displayName;

                    repository.Update(persion);
                }
                */                
            }


        }

        private void TriggerEnabledEvent(IPrincipal principal)
        {
            if (principal is IUser)
            {
                EventTrigger.Trigger(principal, new UserEnabledEvent(principal as IUser));
            }
        }

        private void TriggerDisabledEvent(IPrincipal principal)
        {
            if (principal is IUser)
            {
                EventTrigger.Trigger(principal, new UserDisabledEvent(principal as IUser));
            }
        }

        private void TriggerDeletedEvent(IPrincipal principal)
        {
            if (principal is IUser)
            {
                EventTrigger.Trigger(principal, new UserDeletedEvent(principal as IUser));
            }
            else if (principal is IOrganizationalRole)
            {
                EventTrigger.Trigger(principal, new OrganizationalRoleDeletedEvent(principal as IOrganizationalRole));
            }
            else if (principal is IRole)
            {
                EventTrigger.Trigger(principal, new RoleDeletedEvent(principal as IRole));
            }
            else if (principal is IGroup)
            {
                EventTrigger.Trigger(principal, new GroupDeletedEvent(principal as IGroup));
            }
            else if (principal is IOrganizationalUnit)
            {
                EventTrigger.Trigger(principal, new OrganizationalUnitDeletedEvent(principal as IOrganizationalUnit));
            }
        }
    }
}