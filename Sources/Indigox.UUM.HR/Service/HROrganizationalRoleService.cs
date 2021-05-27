using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Extend;
using Indigox.UUM.Factory;
using Indigox.UUM.HR.Model;
using Indigox.UUM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.HR.Service
{
    public class HROrganizationalRoleService
    {
        private IMutableOrganizationalRole CreateEntity(HROrganizationalRole roleItem)
        {
            IRepository<IOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            IRepository<IOrganizationalUnit> orgRepository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IMutableOrganizationalRole item;
            IOrganizationalUnit parentOrg = null;

            string parentOrgID = null;
            if (!String.IsNullOrEmpty(roleItem.ParentID))
            {
                parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(roleItem.ParentID);
            }
            if (!String.IsNullOrEmpty(parentOrgID))
            {
                parentOrg = orgRepository.Get(parentOrgID);
            }

            OrganizationalRoleFactory factory = new OrganizationalRoleFactory()
            {
                Name = roleItem.Name,
                DisplayName = String.IsNullOrEmpty(roleItem.DisplayName) ? roleItem.Name : roleItem.DisplayName,
                Email = roleItem.Email,
                OrganizationalUnit = parentOrg,
                ExtendProperties = roleItem.CloneExtendProperties(),
            };

            item = factory.Create(factory.GetNextID());
            repository.Add(item);
            return item;
        }
        private void UpdateEntity(HROrganizationalRole role, IMutableOrganizationalRole item)
        {
            IRepository<IOrganizationalUnit> orgRepository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IRepository<IOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            IOrganizationalUnit parentOrg = null;
            string parentOrgID = null;
            if (!String.IsNullOrEmpty(role.ParentID))
            {
                parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(role.ParentID);
            }
            if (!String.IsNullOrEmpty(parentOrgID))
            {
                parentOrg = orgRepository.Get(parentOrgID);
            }

            item.Name = role.Name;
            item.FullName = role.Name;
            item.Email = role.Email;
            item.Organization = parentOrg;
            item.ExtendProperties = role.CloneExtendProperties();

            OrganizationalRoleService service = new OrganizationalRoleService();
            service.Update(item);

            repository.Update(item);
        }
        public void Sync(string roleID)
        {
            HROrganizationalRole item = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>().Get(roleID);
            this.Sync(item);
        }

        public void Sync(HROrganizationalRole item)
        {
            this.Sync(item, null);
        }

        public void Sync(HROrganizationalRole item, IOrganizationalRole bindingItem)
        {
            switch (item.State)
            {
                case HRState.Created: this.Create(item, bindingItem); break;
                case HRState.Changed: this.Update(item); break;
                case HRState.Deleted: this.Delete(item.ID); break;
            }
            item.Synchronized = true;
            RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>().Update(item);
        }

        public void Create(HROrganizationalRole roleItem, IOrganizationalRole principal)
        {
            if (!string.IsNullOrEmpty(MappingUtil.GetPrincipalIDByHRObjectID(roleItem.ID)))
            {
                Update(roleItem);
                return;
            }

            IMutableOrganizationalRole item = principal as IMutableOrganizationalRole;

            /*
             * 修改时间：2018-09-28
             * 修改人：曾勇
             * 修改内容：同步新建的时候，判断所属部门不允许为空，不允许有相同Mail的部门存在
             */
            //if ((principal is null) && (!String.IsNullOrEmpty(org.Email)))
            //{
            //    IList<IOrganizationalUnit> emailExists = repository.Find(
            //        Query.NewQuery.FindByCondition(Specification.Equal("Email", org.Email))
            //    );
            //    if (emailExists.Count > 0)
            //    {
            //        throw new ArgumentException("同步新建部门失败，Email：'" + org.Email + "' 已经存在");
            //    }
            //}

            if (item == null)
            {
                item = CreateEntity(roleItem);

                OperationLogService.LogOperation("从HR同步，创建部门： " + item.Name, item.GetDescription());
            }
            else
            {
                UpdateEntity(roleItem, item);
                OperationLogService.LogOperation("从HR同步，更新部门： " + item.Name, item.GetDescription());
            }

            MappingUtil.CreateMapping(roleItem.ID, item.ID);
        }

        

        public void Update(HROrganizationalRole role)
        {
            string organizationalRoleID = MappingUtil.GetPrincipalIDByHRObjectID(role.ID);
            IRepository<IMutableOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalRole>();

            IMutableOrganizationalRole item = null;
            if (!String.IsNullOrEmpty(organizationalRoleID))
            {
                item = repository.Get(organizationalRoleID);
            }

            if (item == null)
            {
                MappingUtil.DeleteMapping(organizationalRoleID);
                item = CreateEntity(role);
                MappingUtil.CreateMapping(role.ID, item.ID);
            }
            else
            {
                //item.DisplayName = role.DisplayName;
                UpdateEntity(role, item);
            }

            OperationLogService.LogOperation("从HR同步，更新部门： " + item.Name, item.GetDescription());
        }

        

        public void Delete(string roleID)
        {
            IRepository<IOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            string organizationalRoleID = MappingUtil.GetPrincipalIDByHRObjectID(roleID);

            IOrganizationalRole item = repository.Get(organizationalRoleID);

            PrincipalService service = new PrincipalService();
            service.Delete(item);
            OperationLogService.LogOperation("从HR同步，删除部门： " + item.Name, item.GetDescription());
            MappingUtil.DeleteMapping(organizationalRoleID);
        }
    }
}
