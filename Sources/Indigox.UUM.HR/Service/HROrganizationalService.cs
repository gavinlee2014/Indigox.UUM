using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.HR.Model;
using Indigox.UUM.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;
using Indigox.UUM.HR.Setting;
using Indigox.UUM.Naming.Service;
using Indigox.Common.Membership;

namespace Indigox.UUM.HR.Service
{
    public class HROrganizationalService
    {
        private IMutableOrganizationalUnit CreateEntity(HROrganizational org)
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IMutableOrganizationalUnit item;
            IOrganizationalUnit parentOrg = null;
            string parentOrgID = null;
            if (!String.IsNullOrEmpty(org.ParentID))
            {
                parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(org.ParentID);
            }
            if (!String.IsNullOrEmpty(parentOrgID))
            {
                parentOrg = repository.Get(parentOrgID);
            }

            string accountName;
            if (String.IsNullOrEmpty(org.Email))
            {
                var nameService = new NameService();
                accountName = nameService.Naming(org.Name);
                
            }
            else
            {
                accountName = org.Email.Substring(0, org.Email.IndexOf("@"));
            }
            var emailSuffix = EmailSettingService.Instance.GetSuffix(parentOrgID);
            org.Email = string.Format("{0}@{1}", accountName, emailSuffix);

            OrganizationalUnitFactory factory = new OrganizationalUnitFactory()
            {
                Name = org.Name,
                DisplayName = String.IsNullOrEmpty(org.DisplayName) ? org.Name : org.DisplayName,
                Email = org.Email,
                ParentOrganizationalUnit = parentOrg,
                ExtendProperties = org.CloneExtendProperties(),
            };

            item = factory.Create(factory.GetNextID(), org.Type);
            repository.Add(item);
            return item;
        }
        private static void UpdateEntity(HROrganizational org,  IMutableOrganizationalUnit item)
        {
            IRepository<IMutableOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalUnit>();
            IRepository<IOrganizationalUnit> orgRepository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IOrganizationalUnit parentOrg = null;
            string parentOrgID = null;
            if (!String.IsNullOrEmpty(org.ParentID))
            {
                parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(org.ParentID);
            }
            if (!String.IsNullOrEmpty(parentOrgID))
            {
                parentOrg = orgRepository.Get(parentOrgID);
            }

            item.Name = org.Name;
            if ((!String.IsNullOrEmpty(item.DisplayName)) && (item.DisplayName.IndexOf(".") > -1))
            {
                item.DisplayName = item.DisplayName.Substring(0, item.DisplayName.LastIndexOf('.')) + "." + org.Name;
            }
            else
            {
                item.DisplayName = org.Name;
            }
            item.FullName = org.Name;
            item.Email = org.Email;
            item.Organization = parentOrg;
            item.ExtendProperties = org.CloneExtendProperties();

            OrganizationalUnitService service = new OrganizationalUnitService();
            service.Update(item);

            repository.Update(item);
        }

        public void Create(HROrganizational org, IOrganizationalUnit principal)
        {
            if (!string.IsNullOrEmpty(MappingUtil.GetPrincipalIDByHRObjectID(org.ID)))
            {
                Update(org);
                return;
            }

            IMutableOrganizationalUnit item = principal as IMutableOrganizationalUnit;

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
                item = CreateEntity(org);

                OperationLogService.LogOperation("从HR同步，创建部门： " + item.Name, item.GetDescription());
            }
            else
            {
                UpdateEntity(org, item);
                OperationLogService.LogOperation("从HR同步，更新部门： " + item.Name, item.GetDescription());
            }

            MappingUtil.CreateMapping(org.ID, item.ID);
        }

        

        public void Update(HROrganizational org)
        {
            string organizationalUnitID = MappingUtil.GetPrincipalIDByHRObjectID(org.ID);
            IRepository<IMutableOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalUnit>();

            IMutableOrganizationalUnit item = null;
            if (!String.IsNullOrEmpty(organizationalUnitID))
            {
                item = repository.Get(organizationalUnitID);
            }

            if (item == null)
            {
                MappingUtil.DeleteMapping(organizationalUnitID);
                item = CreateEntity(org);
                MappingUtil.CreateMapping(org.ID, item.ID);
            }
            else
            {
                UpdateEntity(org, item);
            }
            //item.DisplayName = org.DisplayName;
            
            OperationLogService.LogOperation("从HR同步，更新部门： " + item.Name, item.GetDescription());
        }



        public void Delete(string orgID)
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            string organizationalUnitID = MappingUtil.GetPrincipalIDByHRObjectID(orgID);

            IOrganizationalUnit item = repository.Get(organizationalUnitID);
            if ((item.Members != null) && (item.Members.Count > 0))
            {
                foreach (IPrincipal principal in item.Members)
                {
                    IMutableOrganizationalObject organizationalObject = principal as IMutableOrganizationalObject;
                    if (organizationalObject != null)
                    {
                        organizationalObject.Organization = null;
                    }
                }
            }
            if (item.MemberOf != null)
            {
                Container con = item as Container;
                IList<IContainer> list = new List<IContainer>();
                foreach (IContainer parent in item.MemberOf)
                {
                    list.Add(parent);
                }
                foreach (IContainer temp in list)
                {
                    con.RemoveMemberOf(temp);
                }
            }

            PrincipalService service = new PrincipalService();
            service.Delete(item);
            OperationLogService.LogOperation("从HR同步，删除部门： " + item.Name, item.GetDescription());
            MappingUtil.DeleteMapping(organizationalUnitID);
            /***
             * 修改时间：2018-11-25
             * 修改人：曾勇
             * 修改内容：删除部门后，将Organization为部门ID的人员的Organization属性设为null
             **/
            IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
            db.ExecuteText("update Principal set Organization = null where Type = '201' and Organization = '" + organizationalUnitID + "'");
        }

        public void Sync(string orgID)
        {
            HROrganizational item = RepositoryFactory.Instance.CreateRepository<HROrganizational>().Get(orgID);
            this.Sync(item);
        }

        public void Sync(string orgID, string bindingPrincipalID)
        {
            HROrganizational item = RepositoryFactory.Instance.CreateRepository<HROrganizational>().Get(orgID);

            IOrganizationalUnit bindingOrg = null;
            if (!String.IsNullOrEmpty(bindingPrincipalID))
            {
                bindingOrg = 
                    RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(bindingPrincipalID);

            }

            this.Sync(item, bindingOrg);
        }

        public void Sync(HROrganizational org)
        {
            this.Sync(org, null);
        }

        public void Sync(HROrganizational org, IOrganizationalUnit bindingOrg)
        {
            switch (org.State)
            {
                case HRState.Created: this.Create(org, bindingOrg); break;
                case HRState.Changed: this.Update(org); break;
                case HRState.Deleted: this.Delete(org.ID); break;
            }
            org.Synchronized = true;
            RepositoryFactory.Instance.CreateRepository<HROrganizational>().Update(org);
        }

        public IOrganizationalUnit GetMappedPrincipal(string HROrganizationalID)
        {
            string orgID = MappingUtil.GetPrincipalIDByHRObjectID(HROrganizationalID);
            if (string.IsNullOrEmpty(orgID))
            {
                return null;
            }

            return RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(orgID);
        }
    }
}
