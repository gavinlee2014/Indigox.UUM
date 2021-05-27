using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [WebServiceBinding(Name = "ImportHROrganizationalRoleService", Namespace = Consts.Namespace_HR + "organizationalrole/")]
    public class ImportHROrganizationalRoleService : IImportHROrganizationalRoleService
    {
        private bool OrganizationalRoleExists(string nativeID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", nativeID);
            var role = repository.First(query);
            return role != null;
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
        private HROrganizationalRole CreateOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, HRPropertyChangeCollection extendProperties)
        {
            
            var nameService = new NameService();
            var emailSuffix = EmailSettingService.Instance.GetSuffix("");
            var accountName = nameService.Naming(name);
            email = string.Format("{0}@{1}", accountName, emailSuffix);

            var role = new HROrganizationalRole();
            role.ID = nativeID;
            role.Name = name;
            role.ParentID = organizationalUnitID;
            role.Email = email;
            role.DisplayName = displayName;
            role.Synchronized = false;
            role.ModifyTime = DateTime.Now;
            role.State = HRState.Created;
            role.Description = "新建组织岗位" + name;
            role.ExtendProperties = extendProperties == null ? new Dictionary<string, string>() : extendProperties.ToDictionary();
            return role;
        }

        private HROrganizationalRole UpdateOrganizationalRole(HROrganizationalRole role, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, HRPropertyChangeCollection extendProperties)
        {
            role.Name = name;
            role.ParentID = organizationalUnitID;
            //role.Email = email;
            role.DisplayName = displayName;
            role.Synchronized = false;
            role.ModifyTime = DateTime.Now;
            role.State = HRState.Changed;
            role.Description = "修改组织岗位" + name;
            role.ExtendProperties = extendProperties == null ? new Dictionary<string, string>() : extendProperties.ToDictionary();
            return role;
        }

        public void AddUser(string organizationalRoleID, string userID)
        {
            if (String.IsNullOrEmpty( organizationalRoleID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }
            if (String.IsNullOrEmpty(userID))
            {
                throw new ArgumentNullException("userID should not be null");
            }
            IRepository<HROrganizationalRole> roleRepository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            HROrganizationalRole role = roleRepository.Get(organizationalRoleID);
            if (role == null)
            {
                throw new ArgumentException("Cannot find OrganizationalRole of " + organizationalRoleID);
            }
            IRepository<HREmployee> userRepository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            HREmployee user =  userRepository.Get(userID);
            if (user == null)
            {
                throw new ArgumentException("Cannot find HREmployee of " + userID);
            }
            user.OrganizationalRoles.Add(role);
            user.State = HRState.Changed;
            user.Synchronized = false;
            userRepository.Update(user);
            Log.Debug(String.Format(@"ImportHROrganizationalRoleService AddUser {0} {1}", organizationalRoleID, userID));
            SyncToUUM(user);
        }

        public void ChangeProperty(string organizationalRoleID, HRPropertyChangeCollection propertyChanges)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", organizationalRoleID);
            var roleObj = repository.First(query);
            if (roleObj != null)
            {
                string description = "修改部门属性：";
                HROrganizationalRole role = roleObj as HROrganizationalRole;
                foreach (var v in propertyChanges.PropertyChanges)
                {
                    string fieldName = v.Name;
                    if (fieldName.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    description += fieldName + "：" + v.Value + " ，";
                    switch (fieldName)
                    {
                        case "Name":
                            role.Name = v.Value.ToString();
                            break;
                        case "ParentID":
                            role.ParentID = v.Value.ToString();
                            break;

                        case "Email":
                            role.Email = v.Value.ToString();
                            break;
                        default:
                            if (role.ExtendProperties.ContainsKey(fieldName))
                            {
                                role.ExtendProperties[fieldName] = Convert.ToString(v.Value);
                            }
                            else
                            {
                                role.ExtendProperties.Add(fieldName, Convert.ToString(v.Value));
                            }
                            break;
                    }
                }

                if (!(!role.Synchronized && role.State == HRState.Created))
                {
                    role.State = HRState.Changed;
                    role.Description = propertyChanges.PropertyChanges.Count > 0 ? description.TrimEnd('，') : description;
                }
                role.Synchronized = false;
                role.ModifyTime = DateTime.Now;
                repository.Update(role);

                SyncToUUM(role);
            }
        }

        public string Create(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, HRPropertyChangeCollection extendProperties)
        {
            IRepository<HROrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            if (!OrganizationalRoleExists(nativeID))
            {
                try
                {
                    HROrganizationalRole role = CreateOrganizationalRole(nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum, extendProperties);
                    repository.Add(role);

                    SyncToUUM(role);
                }
                catch (Exception e)
                {
                    throw new Exception("创建组织岗位失败，编号:" + nativeID + ", error:" + e.Message);
                }
            }
            else
            {
                try
                {
                    HROrganizationalRole role = repository.Get(nativeID);
                    UpdateOrganizationalRole(role, organizationalUnitID, name, fullName, displayName, email, description, orderNum, extendProperties);
                    repository.Update(role);

                    SyncToUUM(role);
                }
                catch (Exception e)
                {
                    throw new Exception("创建（修改）组织岗位失败，编号:" + nativeID + ", error:" + e.Message);
                }
            }
            Log.Debug(String.Format(@"ImportHROrganizationalRoleUnitService SyncOrganizationalRole {0} {1} {2} {3} {4} {5} {6} {7}", nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum));
            return nativeID;
        }

        public void Delete(string organizationalRoleID)
        {
            if (String.IsNullOrEmpty(organizationalRoleID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }
            IRepository roleRepository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            HROrganizationalRole role = (HROrganizationalRole)roleRepository.Get(organizationalRoleID);
            if (role == null)
            {
                throw new ArgumentException("Cannot find OrganizationalRole of " + organizationalRoleID);
            }

            if (!role.Synchronized && role.State == HRState.Created)
            {
                roleRepository.Remove(role);
            }
            else {
                role.State = HRState.Deleted;
                role.Synchronized = false;
                roleRepository.Update(role);
            }
            
            Log.Debug(String.Format(@"ImportHROrganizationalRoleService Delete {0}", organizationalRoleID));
            SyncToUUM(role);
        }

        public void RemoveUser(string organizationalRoleID, string userID)
        {
            if (String.IsNullOrEmpty(organizationalRoleID))
            {
                throw new ArgumentNullException("organizationalRoleID should not be null");
            }
            if (String.IsNullOrEmpty(userID))
            {
                throw new ArgumentNullException("userID should not be null");
            }
            IRepository<HROrganizationalRole> roleRepository = RepositoryFactory.Instance.CreateRepository<HROrganizationalRole>();
            HROrganizationalRole role = roleRepository.Get(organizationalRoleID);
            if (role == null)
            {
                throw new ArgumentException("Cannot find OrganizationalRole of " + organizationalRoleID);
            }
            IRepository<HREmployee> userRepository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            HREmployee user = userRepository.Get(userID);
            if (user == null)
            {
                throw new ArgumentException("Cannot find Employee of " + userID);
            }
            user.OrganizationalRoles.Remove(role);
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
            Log.Debug(String.Format(@"ImportHROrganizationalRoleService RemoveUser {0} {1}", organizationalRoleID, userID));
            SyncToUUM(user);
        }

        public string SyncOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, HRPropertyChangeCollection extendProperties)
        {
            return Create(nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum, extendProperties);
        }
    }
}
