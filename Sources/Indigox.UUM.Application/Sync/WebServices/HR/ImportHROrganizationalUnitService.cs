using System;
using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Logging;
using Indigox.UUM.HR.Model;
using Indigox.UUM.HR.Service;
using Indigox.UUM.HR.Setting;
using Indigox.UUM.Naming.Service;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Application.Sync.WebServices.HR
{
    [WebServiceBinding(Name = "ImportHROrganizationalUnitService", Namespace = Consts.Namespace_HR + "organizationalunit/")]
    public class ImportHROrganizationalUnitService : IImportHROrganizationalUnitService
    {
        private bool OrganizationalExists(string nativeID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", nativeID);
            var employee = repository.First(query);
            return employee != null;
        }

        private void SyncToUUM(HROrganizational organizational)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var service = new HROrganizationalService();
                service.Sync(organizational);
                IRepository<HROrganizational> repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
                organizational.Synchronized = true;
                repository.Update(organizational);
            }
        }
        private void SyncToUUM(HREmployee user)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var service = new HREmployeeService();
                service.Sync(user);
                IRepository<HREmployee> repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
                user.Synchronized = true;
                repository.Update(user);
            }
        }
        private void SyncToUUM(HROrganizationalRole role)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var service = new HROrganizationalRoleService();
                service.Sync(role);
                IRepository<HROrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
                role.Synchronized = true;
                repository.Update(role);
            }
        }
        private HROrganizational CreateOrganizational(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, HRPropertyChangeCollection extendProperties)
        {
            var nameService = new NameService();
            var emailSuffix = EmailSettingService.Instance.GetSuffixByHRID(parentOrganizationalUnitID);
            var accountName = nameService.Naming(name);
            email = string.Format("{0}@{1}", accountName, emailSuffix);

            var org = new HROrganizational();
            org.ID = nativeID;
            org.Name = name;
            org.ParentID = parentOrganizationalUnitID;
            org.Email = email;
            org.Type = organizationalUnitType;
            org.Synchronized = false;
            org.ModifyTime = DateTime.Now;
            org.State = HRState.Created;
            org.Description = "新建部门" + name;
            org.ExtendProperties = extendProperties == null ? new Dictionary<string, string>() : extendProperties.ToDictionary();
            return org;
        }

        private HROrganizational UpdateOrganizational(HROrganizational org, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, HRPropertyChangeCollection extendProperties)
        {
            org.Name = name;
            org.ParentID = parentOrganizationalUnitID;
            //org.Email = email;
            //org.Type = organizationalUnitType;
            org.Synchronized = false;
            org.ModifyTime = DateTime.Now;
            org.State = HRState.Changed;
            org.Description = "修改部门" + name;
            org.ExtendProperties = extendProperties == null ? new Dictionary<string, string>() : extendProperties.ToDictionary(); 
            return org;
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, HRPropertyChangeCollection extendProperties)
        {
            
            return Create(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties);
        }

        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, HRPropertyChangeCollection extendProperties)
        {
            IRepository<HROrganizational> repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            if (!OrganizationalExists(nativeID))
            {
                try
                {
                    HROrganizational org = CreateOrganizational(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties);
                    repository.Add(org);

                    SyncToUUM(org);
                }
                catch (Exception e)
                {
                    throw new Exception("创建部门失败，编号:" + nativeID + ", error:" + e.ToString());
                }
            }
            else
            {
                try
                {
                    HROrganizational org = repository.Get(nativeID);
                    UpdateOrganizational(org, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties);

                    repository.Update(org);
                    SyncToUUM(org);
                }
                catch (Exception e)
                {
                    throw new Exception("创建（修改）部门失败，编号:" + nativeID + ", error:" + e.ToString());
                }
            }
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService SyncOrganizationalUnit {0} {1} {2} {3} {4} {5} {6} {7} {8}", nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType));
            return nativeID;
        }

        public void Delete(string organizationalUnitID)
        {
            Log.Error("删除部门，id：" + organizationalUnitID);
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", organizationalUnitID);
            object orgObj = repository.First(query);
            if (orgObj != null)
            {
                HROrganizational org = orgObj as HROrganizational;
                if (!org.Synchronized && org.State == HRState.Created)
                {
                    repository.Remove(org);
                }
                else
                {
                    org.Description = "删除部门" + org.Name;
                    org.State = HRState.Deleted;
                    org.Synchronized = false;
                    org.ModifyTime = DateTime.Now;
                    repository.Update(org);
                }

                SyncToUUM(org);
            }
        }

        public void ChangeProperty(string organizationalUnitID, HRPropertyChangeCollection propertyChanges)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", organizationalUnitID);
            var oorgObj = repository.First(query);
            if (oorgObj != null)
            {
                string description = "修改部门属性：";
                HROrganizational org = oorgObj as HROrganizational;
                foreach (var v in propertyChanges.PropertyChanges)
                {
                    string fieldName = v.Name;
                    if (fieldName.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    description += fieldName+"："+v.Value + " ，";
                    switch (fieldName)
                    {
                        case "Name":
                            org.Name = v.Value.ToString();
                            break;

                        case "OrganizationalUnitID":
                            org.ParentID = v.Value.ToString();
                            break;

                        case "DisplayName":
                            org.DisplayName = v.Value.ToString();
                            break;

                        case "Email":
                            org.Email = v.Value.ToString();
                            break;

                        case "Type":
                            org.Type = v.Value.ToString();
                            break;
                        default:
                            if (org.ExtendProperties.ContainsKey(fieldName))
                            {
                                org.ExtendProperties[fieldName] = Convert.ToString(v.Value);
                            }
                            else
                            {
                                org.ExtendProperties.Add(fieldName, Convert.ToString(v.Value));
                            }
                            break;
                    }
                }

                if (!(!org.Synchronized && org.State == HRState.Created))
                {
                    org.State = HRState.Changed;
                    org.Description = propertyChanges.PropertyChanges.Count > 0 ? description.TrimEnd('，') : description;
                }
                org.Synchronized = false;
                org.ModifyTime = DateTime.Now;
                repository.Update(org);

                SyncToUUM(org);
            }
        }

        public void AddOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("parentOrganizationalUnitID should not be null");
            }
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }

            IRepository orgRepository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational org = (HROrganizational)orgRepository.Get(organizationalUnitID);
            if (org == null)
            {
                throw new ArgumentException("Cannot find OrganizationalUnit of " + organizationalUnitID);
            }
            HROrganizational parentOrg = (HROrganizational)orgRepository.Get(parentOrganizationalUnitID);
            if (parentOrg == null)
            {
                throw new ArgumentException("Cannot find parent OrganizationalUnitID of " + parentOrganizationalUnitID);
            }

            org.ParentID = parentOrg.ID;
            org.ParentName = parentOrg.Name;
            if (!org.Synchronized && org.State == HRState.Created)
            {
                org.State = HRState.Created;
            }
            else
            {
                org.State = HRState.Changed;
            }
            org.Synchronized = false;
            orgRepository.Update(org);
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService AddOrganizationalUnit {0} {1}", parentOrganizationalUnitID, organizationalUnitID));
            SyncToUUM(org);
        }

        public void RemoveOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("parentOrganizationalUnitID should not be null");
            }
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }

            IRepository orgRepository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational org = (HROrganizational)orgRepository.Get(organizationalUnitID);
            if (org == null)
            {
                throw new ArgumentException("Cannot find OrganizationalUnit of " + organizationalUnitID);
            }
            HROrganizational parentOrg = (HROrganizational)orgRepository.Get(parentOrganizationalUnitID);
            if (parentOrg == null)
            {
                throw new ArgumentException("Cannot find parent OrganizationalUnitID of " + parentOrganizationalUnitID);
            }

            org.ParentID = null;
            org.ParentName = null;
            if (!org.Synchronized && org.State == HRState.Created)
            {
                org.State = HRState.Created;
            }
            else
            {
                org.State = HRState.Changed;
            }
            org.Synchronized = false;
            orgRepository.Update(org);
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService RemoveOrganizationalUnit {0} {1}", parentOrganizationalUnitID, organizationalUnitID));
            SyncToUUM(org);
        }

        public void AddOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("organizationalUnitID should not be null");
            }
            if (String.IsNullOrEmpty(organizationalRoleID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }

            IRepository orgRepository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational org = (HROrganizational)orgRepository.Get(organizationalUnitID);
            if (org == null)
            {
                throw new ArgumentException("Cannot find OrganizationalUnit of " + organizationalUnitID);
            }
            IRepository roleRepository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            HROrganizationalRole role = (HROrganizationalRole)roleRepository.Get(organizationalRoleID);
            if (role == null)
            {
                throw new ArgumentException("Cannot find OrganizationalRole of " + organizationalRoleID);
            }

            role.ParentID = org.ID;
            role.ParentName = org.Name;
            if (!role.Synchronized && role.State == HRState.Created)
            {
                role.State = HRState.Created;
            }
            else
            {
                role.State = HRState.Changed;
            }
            role.Synchronized = false;
            roleRepository.Update(role);
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService AddOrganizationalRole {0} {1}", organizationalUnitID, organizationalRoleID));
            SyncToUUM(role);
        }

        public void RemoveOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("organizationalUnitID should not be null");
            }
            if (String.IsNullOrEmpty(organizationalRoleID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }

            IRepository orgRepository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational org = (HROrganizational)orgRepository.Get(organizationalUnitID);
            if (org == null)
            {
                throw new ArgumentException("Cannot find OrganizationalUnit of " + organizationalUnitID);
            }
            IRepository roleRepository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            HROrganizationalRole role = (HROrganizationalRole)roleRepository.Get(organizationalRoleID);
            if (role == null)
            {
                throw new ArgumentException("Cannot find OrganizationalRole of " + organizationalRoleID);
            }

            role.ParentID = null;
            role.ParentName = null;
            if (!role.Synchronized && role.State == HRState.Created)
            {
                role.State = HRState.Created;
            }
            else
            {
                role.State = HRState.Changed;
            }
            role.Synchronized = false;
            roleRepository.Update(role);
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService AddOrganizationalRole {0} {1}", organizationalUnitID, organizationalRoleID));
            SyncToUUM(role);
        }

        public void AddUser(string organizationalUnitID, string userID)
        {
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("organizationalUnitID should not be null");
            }
            if (String.IsNullOrEmpty(userID))
            {
                throw new ArgumentNullException("userID should not be null");
            }

            IRepository orgRepository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational org = (HROrganizational)orgRepository.Get(organizationalUnitID);
            if (org == null)
            {
                throw new ArgumentException("Cannot find OrganizationalUnit of " + organizationalUnitID);
            }
            IRepository<HREmployee> userRepository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            HREmployee user = userRepository.Get(userID);
            if (user == null)
            {
                throw new ArgumentException("Cannot find Employee of " + userID);
            }

            user.ParentID = org.ID;
            user.ParentName = org.Name;
            if (!user.Synchronized && user.State == HRState.Created)
            {
                user.State = HRState.Created;
            }
            else
            {
                user.State = HRState.Changed;
            }
            user.Synchronized = false;
            userRepository.Update(user);
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService AddUser {0} {1}", organizationalUnitID, userID));
            SyncToUUM(user);
        }

        public void RemoveUser(string organizationalUnitID, string userID)
        {
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                throw new ArgumentNullException("organizationalUnitID should not be null");
            }
            if (String.IsNullOrEmpty(userID))
            {
                throw new ArgumentNullException("userID should not be null");
            }

            IRepository orgRepository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational org = (HROrganizational)orgRepository.Get(organizationalUnitID);
            if (org == null)
            {
                throw new ArgumentException("Cannot find OrganizationalUnit of " + organizationalUnitID);
            }
            IRepository<HREmployee> userRepository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            HREmployee user = userRepository.Get(userID);
            if (user == null)
            {
                throw new ArgumentException("Cannot find Employee of " + userID);
            }

            user.ParentID = null;
            user.ParentName = null;
            if (!user.Synchronized && user.State == HRState.Created)
            {
                user.State = HRState.Created;
            }
            else
            {
                user.State = HRState.Changed;
            }
            user.Synchronized = false;
            userRepository.Update(user);
            Log.Debug(String.Format(@"ImportHROrganizationalUnitService RemoveUser {0} {1}", organizationalUnitID, userID));
            SyncToUUM(user);
        }
    }
}