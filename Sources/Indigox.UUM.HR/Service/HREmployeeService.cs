using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.HR.Model;
using Indigox.UUM.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;
using Indigox.UUM.Sync.Mail;
using Indigox.UUM.Sync.Model;
using Indigox.Common.DomainModels.Queries;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using Indigox.UUM.Naming.Service;
using Indigox.Common.Logging;

namespace Indigox.UUM.HR.Service
{
    public class HREmployeeService
    {
        private IMutableOrganizationalPerson CreateEntity(HREmployee employee)
        {
            IMutableOrganizationalPerson item;
            IOrganizationalUnit parentOrg = null;
            string parentOrgID = null;
            if (!String.IsNullOrEmpty(employee.ParentID))
            {
                parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(employee.ParentID);
            }
            if (!String.IsNullOrEmpty(parentOrgID))
            {
                parentOrg = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(parentOrgID);
            }
            if (String.IsNullOrEmpty(employee.AccountName))
            {
                NameService nameService = new NameService();
                employee.AccountName = nameService.Naming(employee.Name);
            }

            
            var emailSuffix = EmailSettingService.Instance.GetSuffix(parentOrgID);
            employee.Email = string.Format("{0}@{1}", employee.AccountName, emailSuffix);
            /*
             * 修改时间：2018-08-27
             * 修改人：曾勇
             * 修改内容：同步新建的时候，判断所属部门不允许为空，不允许有相同Account和Mail的用户存在
             */

            if (parentOrg == null)
            {
                throw new ArgumentException("同步新建用户" + employee.Name + "失败，所属部门不存在");
            }
            IRepository<IOrganizationalPerson> repos = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IList<IOrganizationalPerson> accountExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("AccountName", employee.AccountName)));
            if (accountExists.Count > 0)
            {
                throw new ArgumentException("同步新建用户失败，AccountName：'" + employee.AccountName + "' 已经存在。");
            }

            if (!String.IsNullOrEmpty(employee.Email))
            {
                IList<IOrganizationalPerson> emailExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("Email", employee.Email)));
                if (emailExists.Count > 0)
                {
                    throw new ArgumentException("同步新建用户失败，Email：'" + employee.Email + "' 已经存在");
                }
            }

            if (!string.IsNullOrEmpty(employee.IdCard))
            {
                IList<IOrganizationalPerson> idCardExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("IdCard", employee.IdCard)));
                if (idCardExists.Count > 0)
                {
                    throw new ArgumentException("同步新建用户失败，IdCard：'" + employee.IdCard + "' 已经存在");
                }
            }
            if (!string.IsNullOrEmpty(employee.Mobile))
            {
                IList<IOrganizationalPerson> mobileExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("Mobile", employee.Mobile)));
                if (mobileExists.Count > 0)
                {
                    throw new ArgumentException("同步新建用户失败，Mobile：'" + employee.Mobile + "' 已经存在");
                }
            }

            //if (ADExists(employee.AccountName))
            //{
            //    throw new ArgumentException("同步新建用户失败，Ad中已经存在AccountName：'" + employee.AccountName + "' 的用户。");
            //}
            IDictionary<string, string> extendProperties = employee.CloneExtendProperties();
            if (extendProperties.ContainsKey("Portrait"))
            {
                extendProperties["Portrait"] = employee.Portrait;
            }
            else
            {
                extendProperties.Add("Portrait", employee.Portrait);
            }

            OrganizationalPersonFactory factory = new OrganizationalPersonFactory()
            {

                AccountName = employee.AccountName,
                IdCard = employee.IdCard,
                DisplayName = String.IsNullOrEmpty(employee.DisplayName) ? employee.Name : employee.DisplayName,
                Name = employee.Name,
                Email = employee.Email,
                Mobile = employee.Mobile,
                Telephone = employee.Tel,
                Fax = employee.Fax,
                Title = employee.Title,
                OrganizationalUnit = parentOrg,
                Profile = employee.Portrait,
                MailDatabase = employee.MailDatabase,
                OrderNum = employee.OrderNum,
                ExtendProperties = extendProperties,
            };

            item = factory.Create(factory.GetNextID());

            RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().Add(item);
            return item;
        }

        private IMutableOrganizationalPerson UpdateEntity(HREmployee employee, IMutableOrganizationalPerson item)
        {
            IOrganizationalUnit parentOrg = UpdateEntityProperties(employee, item);

            if (item.Enabled)
            {
                if (item.Organization.ID != parentOrg.ID)
                {
                    PrincipalService principalService = new PrincipalService();
                    principalService.MoveTo(item, parentOrg);
                }
            }


            OrganizationalPersonService service = new OrganizationalPersonService();
            service.Update(item);
            OperationLogService.LogOperation("从HR同步，更新用户： " + item.Name, item.GetDescription());

            //if (item.Enabled == employee.Enabled)
            //{
            //    if (item.Organization.ID != parentOrg.ID)
            //    {
            //        PrincipalService principalService = new PrincipalService();
            //        principalService.MoveTo(item, parentOrg);
            //    }

            //    OrganizationalPersonService service = new OrganizationalPersonService();
            //    service.Update(item);
            //    OperationLogService.LogOperation("从HR同步，更新用户： " + item.Name, item.GetDescription());
            //}
            //else
            //{
            //    PrincipalService service = new PrincipalService();
            //    if (employee.Enabled == true)
            //    {
            //        service.Enable(item, parentOrg);
            //        OperationLogService.LogOperation("从HR同步，启用用户： " + item.Name, item.GetDescription());
            //    }
            //    else
            //    {
            //        service.Disable(item);
            //        OperationLogService.LogOperation("从HR同步，禁用用户： " + item.Name, item.GetDescription());
            //    }
            //}
            return item;
        }

        private IOrganizationalUnit UpdateEntityProperties(HREmployee employee, IMutableOrganizationalPerson item)
        {
            IRepository<IMutableOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalPerson>();
            if (!string.IsNullOrEmpty(employee.Mobile))
            {
                IList<IMutableOrganizationalPerson> mobileExists = repository.Find(
                    Query.NewQuery.FindByCondition(
                        Specification.And(
                            Specification.Equal("Mobile", employee.Mobile),
                            Specification.NotEqual("ID", item.ID)
                        )
                    )
                );
                if (mobileExists.Count > 0)
                {
                    throw new ArgumentException("从HR同步修改用户失败，Mobile：'" + employee.Mobile + "' 已经存在。");
                }
            }
            if (!string.IsNullOrEmpty(employee.IdCard))
            {
                IList<IMutableOrganizationalPerson> idCardExists = repository.Find(
                    Query.NewQuery.FindByCondition(
                        Specification.And(
                            Specification.Equal("IdCard", employee.IdCard),
                            Specification.NotEqual("ID", item.ID)
                        )
                    )
                );
                if (idCardExists.Count > 0)
                {
                    throw new ArgumentException("从HR同步修改用户失败，IdCard：'" + employee.IdCard + "' 已经存在。");
                }
            }

            IOrganizationalUnit parentOrg = null;
            string parentOrgID = null;
            if (!String.IsNullOrEmpty(employee.ParentID))
            {
                parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(employee.ParentID);
            }
            if (!String.IsNullOrEmpty(parentOrgID))
            {
                parentOrg = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(parentOrgID);
            }

            IDictionary<string, string> extendProperties = employee.CloneExtendProperties();
            if (extendProperties.ContainsKey("Portrait"))
            {
                extendProperties["Portrait"] = employee.Portrait;
            }
            else
            {
                extendProperties.Add("Portrait", employee.Portrait);
            }


            item.Name = employee.Name;
            if ((!String.IsNullOrEmpty(item.DisplayName)) && (item.DisplayName.IndexOf(".") > -1))
            {
                item.DisplayName = item.DisplayName.Substring(0, item.DisplayName.LastIndexOf('.')) + "." + employee.Name;
            }
            else
            {
                item.DisplayName = employee.Name;
            }
            //item.DisplayName = employee.DisplayName;
            item.Mobile = employee.Mobile;
            item.IdCard = employee.IdCard;
            item.Telephone = employee.Tel;
            item.Fax = employee.Fax;
            item.Title = employee.Title;
            item.Profile = employee.Portrait;
            item.MailDatabase = employee.MailDatabase;
            //item.OrderNum = employee.OrderNum;
            item.ExtendProperties = extendProperties;

            repository.Update(item);
            return parentOrg;
        }

        public void Create(HREmployee employee, IOrganizationalPerson principal)
        {
            Log.Debug(String.Format("HREmployeeService: Create Employee with ID:{0}-{1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13}", employee.ID, employee.ParentID, employee.AccountName, employee.Name, employee.DisplayName, employee.IdCard, employee.Email, employee.Title, employee.Mobile, employee.Tel, employee.Fax, employee.OrderNum,  employee.Portrait, employee.MailDatabase));
            String extendString = "";
            if (employee.ExtendProperties != null)
            {
                foreach (var key in employee.ExtendProperties.Keys)
                {
                    extendString += key + ":" + employee.ExtendProperties[key] + " ";
                }

            }
            Log.Debug(String.Format("HREmployeeService: Create Employee with ID:{0} extend:{1}", employee.ID, extendString));

            if (!string.IsNullOrEmpty(MappingUtil.GetPrincipalIDByHRObjectID(employee.ID)))
            {
                Update(employee);
                return;
            }

            IMutableOrganizationalPerson item = principal as IMutableOrganizationalPerson;

            if (item == null)
            {
                item = CreateEntity(employee);
                //emailSysAdmin(item);

                OperationLogService.LogOperation("从HR同步，创建用户： " + item.Name, item.GetDescription());
            }
            else
            {
                UpdateEntity(employee, item);

                OperationLogService.LogOperation("从HR同步，更新用户： " + item.Name, item.GetDescription());
            }

            SyncEmployeeRoleRelation(employee, item);
            MappingUtil.CreateMapping(employee.ID, item.ID);
        }

 
        private void SyncEmployeeRoleRelation(HREmployee employee, IOrganizationalPerson principal)
        {
            IMutableOrganizationalPerson item = principal as IMutableOrganizationalPerson;

            List<string> roleIDList = new List<string>();

            if (employee.OrganizationalRoles != null)
            {
                foreach (var hrRole in employee.OrganizationalRoles)
                {
                    roleIDList.Add(MappingUtil.GetPrincipalIDByHRObjectID(hrRole.ID));
                }
            }

            List<IOrganizationalRole> roleToAdd = new List<IOrganizationalRole>();
            List<IOrganizationalRole> roleToRemove = new List<IOrganizationalRole>();

            foreach (var parent in principal.MemberOf)
            {
                IOrganizationalRole role = parent as IOrganizationalRole;
                if(role != null) 
                {
                    if (!roleIDList.Contains(role.ID))
                    {
                        roleToRemove.Add(role);
                    }
                    else
                    {
                        roleIDList.Remove(role.ID);
                    }
                }
            }

            var roleRep = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            foreach (var principalRoleID in roleIDList)
            {
                var role = roleRep.Get(principalRoleID);
                roleToAdd.Add(role);
            }

            foreach (var role in roleToRemove)
            {
                item.RemoveMemberOf(role);
            }

            foreach (var role in roleToAdd)
            {
                item.AddMemberOf(role);
            }

            RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().Update(item);
        }

        private bool ADExists(string accountName)
        {
            Forest forest = Forest.GetCurrentForest();
            GlobalCatalog gc = forest.FindGlobalCatalog();

            using (DirectorySearcher searcher = gc.GetDirectorySearcher())
            {
                searcher.Filter = String.Format("(sAMAccountName={0})", accountName);
                SearchResultCollection results = searcher.FindAll();
                return results.Count > 0;
            }
        }

        private static void emailSysAdmin(IMutableOrganizationalPerson item)
        {
            MailService mailService = new MailService();
            List<string> sysAdminEmails = new List<string>();
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var list = repository.Find(new Query());
            foreach (var sysConfiguration in list)
            {
                if (sysConfiguration.Enabled == true && !string.IsNullOrEmpty(sysConfiguration.Email))
                {
                    sysAdminEmails.Add(sysConfiguration.Email);
                }
            }
            
            mailService.SendMail(sysAdminEmails, "新员工[" + item.Name + "]入职通知", string.Format(@"
姓名： {0} <br>
部门: {1} <br>
岗位: {2} ", item.Name, item.Organization.DisplayName, item.Title));
        }

        public void Update(HREmployee employee)
        {
            Log.Debug(String.Format("HREmployeeService: Update Employee with ID:{0}-{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", employee.ID, employee.ParentID, employee.AccountName, employee.Name, employee.DisplayName, employee.IdCard, employee.Email, employee.Title, employee.Mobile, employee.Tel, employee.Fax, employee.OrderNum, employee.MailDatabase));
            String extendString = "";
            if (employee.ExtendProperties != null)
            {
                foreach (var key in employee.ExtendProperties.Keys)
                {
                    extendString += key + ":" + employee.ExtendProperties[key] + " ";
                }

            }
            Log.Debug(String.Format("HREmployeeService: Update Employee with ID:{0} extend:{1}", employee.ID, extendString));

            string organizationalPersonID = MappingUtil.GetPrincipalIDByHRObjectID(employee.ID);
            IRepository<IMutableOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalPerson>();
            IMutableOrganizationalPerson item = null;
            if (!String.IsNullOrEmpty(organizationalPersonID))
            {
                item = repository.Get(organizationalPersonID);
            }

            if (item == null)
            {
                MappingUtil.DeleteMapping(organizationalPersonID);
                item = CreateEntity(employee);
                SyncEmployeeRoleRelation(employee, item);
                MappingUtil.CreateMapping(employee.ID, item.ID);
            }
            else
            {
                item = UpdateEntity(employee, item);
                SyncEmployeeRoleRelation(employee, item);
            }
        }

        public void Enable(HREmployee employee)
        {
            Log.Debug(String.Format("HREmployeeService: Enable Employee with ID:{0}-{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", employee.ID, employee.ParentID, employee.AccountName, employee.Name, employee.DisplayName, employee.IdCard, employee.Email, employee.Title, employee.Mobile, employee.Tel, employee.Fax, employee.OrderNum, employee.MailDatabase));
            String extendString = "";
            if (employee.ExtendProperties != null)
            {
                foreach (var key in employee.ExtendProperties.Keys)
                {
                    extendString += key + ":" + employee.ExtendProperties[key] + " ";
                }

            }
            Log.Debug(String.Format("HREmployeeService: Enable Employee with ID:{0} extend:{1}", employee.ID, extendString));

            string organizationalPersonID = MappingUtil.GetPrincipalIDByHRObjectID(employee.ID);
            IRepository<IMutableOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalPerson>();
            IMutableOrganizationalPerson item = null;
            if (!String.IsNullOrEmpty(organizationalPersonID))
            {
                item = repository.Get(organizationalPersonID);
            }

            if (item == null)
            {
                throw new ArgumentNullException("启用用户失败，用户" + employee.ID + "不存在");
            }


            IOrganizationalUnit parentOrg = UpdateEntityProperties(employee, item);
            PrincipalService service = new PrincipalService();
            service.Enable(item, parentOrg);
            SyncEmployeeRoleRelation(employee, item);
            OperationLogService.LogOperation("从HR同步，启用用户： " + item.Name, item.GetDescription());
        }

        public void Disable(HREmployee employee)
        {
            Log.Debug(String.Format("HREmployeeService: Disable Employee with ID:{0}-{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", employee.ID, employee.ParentID, employee.AccountName, employee.Name, employee.DisplayName, employee.IdCard, employee.Email, employee.Title, employee.Mobile, employee.Tel, employee.Fax, employee.OrderNum, employee.MailDatabase));
            String extendString = "";
            if (employee.ExtendProperties != null)
            {
                foreach (var key in employee.ExtendProperties.Keys)
                {
                    extendString += key + ":" + employee.ExtendProperties[key] + " ";
                }

            }
            Log.Debug(String.Format("HREmployeeService: Disable Employee with ID:{0} extend:{1}", employee.ID, extendString));

            string organizationalPersonID = MappingUtil.GetPrincipalIDByHRObjectID(employee.ID);
            IRepository<IMutableOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalPerson>();
            IMutableOrganizationalPerson item = null;
            if (!String.IsNullOrEmpty(organizationalPersonID))
            {
                item = repository.Get(organizationalPersonID);
            }

            if (item == null)
            {
                return;
            }


            UpdateEntityProperties(employee, item);
            PrincipalService service = new PrincipalService();
            service.Disable(item);
            SyncEmployeeRoleRelation(employee, item);

            OperationLogService.LogOperation("从HR同步，禁用用户： " + item.Name, item.GetDescription());
        }

        public void Delete(string employeeID)
        {
            string organizationalPersonID = MappingUtil.GetPrincipalIDByHRObjectID(employeeID);
            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IOrganizationalPerson item = repository.Get(organizationalPersonID);

            repository.Remove(item);
            OperationLogService.LogOperation("从HR同步，删除用户： " + item.Name, item.GetDescription());
            MappingUtil.DeleteMapping(organizationalPersonID);
        }


        public void Sync(string employeeID)
        {
            HREmployee item = RepositoryFactory.Instance.CreateRepository<HREmployee>().Get(employeeID);
            this.Sync(item);
        }

        public void Sync(string employeeID, string bindingPrincipalID)
        {
            HREmployee item = RepositoryFactory.Instance.CreateRepository<HREmployee>().Get(employeeID);
            IOrganizationalPerson bindingPerson = null;

            if (!String.IsNullOrEmpty(bindingPrincipalID))
            {
                bindingPerson =
                    RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().Get(bindingPrincipalID);
            }

            this.Sync(item, bindingPerson);
        }

        public void Sync(HREmployee item)
        {
            this.Sync(item, null);
        }

        public void Sync(HREmployee item, IOrganizationalPerson bindingPerson)
        {
            switch (item.State)
            {
                case HRState.Created: this.Create(item, bindingPerson); break;
                case HRState.Changed: this.Update(item); break;
                //case HRState.Disabled: this.Disable(item); break;
                //case HRState.Enabled: this.Enable(item); break;
                case HRState.Deleted: this.Delete(item.ID); break;
            }
            item.Synchronized = true;
            RepositoryFactory.Instance.CreateRepository<HREmployee>().Update(item);
        }

        public string GetMappedPrincipal(string HREmployeeID)
        {
            return MappingUtil.GetPrincipalIDByHRObjectID(HREmployeeID);
        }
    }
}
