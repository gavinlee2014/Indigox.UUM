using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ADGroup = Indigox.Common.ADAccessor.ObjectModel.Group;
using ADOrganizationalUnit = Indigox.Common.ADAccessor.ObjectModel.OrganizationalUnit;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.Sync.WebServices.AD
{
    [WebService(Name = "ImportOrganizationalUnitService", Namespace = Consts.Namespace)]
    public class ADImportOrganizationalUnitService : IImportOrganizationalUnitService
    {
        private static List<string> CreateOURequiredType = new List<string>();

        static ADImportOrganizationalUnitService()
        {
            string createOURequiredType = ConfigurationManager.AppSettings["CreateOURequiredType"];
            if (!String.IsNullOrEmpty(createOURequiredType))
            {
                CreateOURequiredType.AddRange(createOURequiredType.Split(','));
            }
        }

        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, PropertyChangeCollection extendProperties)
        {
            ADGroup parentOrgGroup = null;
            if (String.IsNullOrEmpty(parentOrganizationalUnitID))
            {
                parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
            }
            else
            {
                parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByID(parentOrganizationalUnitID);
            }

            ADOrganizationalUnit parentOrgOU = Indigox.Common.ADAccessor.Accessor.GetOrganizationByByID(parentOrgGroup.Parent.ToString());

            string containerID = null;
            if (parentOrgOU != null)
            {
                containerID = parentOrgOU.ID.ToString();
            }

            ADGroup createdGroup = null;
            if (this.IsCreateOURequired(organizationalUnitType))
            {
                string ouName = displayName;
                if (String.IsNullOrEmpty(ouName))
                {
                    ouName = name;
                }
                ADOrganizationalUnit createdOU = CreateOU(containerID, ouName);
                createdGroup = CreateGroup(createdOU.ID.ToString(), parentOrgGroup, name, displayName, email, organizationalUnitType);
                ADAccessorUtil.MappingGroupAndOU(createdGroup.ID.ToString(), createdOU.ID.ToString());
            }
            else
            {
                createdGroup = CreateGroup(containerID, parentOrgGroup, name, displayName, email, organizationalUnitType);
            }

            return createdGroup.ID.ToString();
        }

        private ADGroup CreateGroup(string groupContainerID, ADGroup parentOrgGroup, string name, string displayName, string email, string organizationalUnitType)
        {
            ADGroup group = new ADGroup()
            {
                Name = name,
                DisplayName = displayName + "全体",
                Mail = email
            };
            if (this.IsCreateOURequired(organizationalUnitType))
            {
                group.Name += "全体";
            }

            ADGroup createdGroup = Indigox.Common.ADAccessor.Accessor.CreateGroup(groupContainerID, group);
            Indigox.Common.ADAccessor.Accessor.AddToGroup(createdGroup.ID.ToString(), parentOrgGroup.ID.ToString());
            return createdGroup;
        }

        private ADOrganizationalUnit CreateOU(string containerID, string name)
        {
            ADOrganizationalUnit ou = new ADOrganizationalUnit()
            {
                Name = name
            };
            ADOrganizationalUnit createdOU = Indigox.Common.ADAccessor.Accessor.CreateOrganization(containerID, ou);
            return createdOU;
        }

        private bool IsCreateOURequired(string organizationalUnitType)
        {
            //return organizationalUnitType.Equals("Company");
            return CreateOURequiredType.Contains(organizationalUnitType);
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, PropertyChangeCollection extendProperties)
        {
            Log.Debug(String.Format("SyncOrganizationalUnit {0},{1},{2},{3},{4},{5} begin", nativeID, parentOrganizationalUnitID, name, fullName, displayName, email));
            ADGroup orgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByAccount(email.Substring(0, email.IndexOf("@")));
            if (orgGroup != null)
            {
                ADGroup parentOrgGroup = null;
                if (String.IsNullOrEmpty(parentOrganizationalUnitID))
                {
                    parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
                }
                else
                {
                    parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByID(parentOrganizationalUnitID);
                }

                string containerID = parentOrgGroup.Parent.ToString();

                if (IsCreateOURequired(organizationalUnitType))
                {
                    ADOrganizationalUnit createdOU = CreateOU(parentOrgGroup.Parent.ToString(), displayName);
                    ADAccessorUtil.MappingGroupAndOU(orgGroup.ID.ToString(), createdOU.ID.ToString());
                    containerID = createdOU.ID.ToString();
                }

                Indigox.Common.ADAccessor.Accessor.MoveTo(orgGroup.ID.ToString(), containerID);
                Indigox.Common.ADAccessor.Accessor.AddToGroup(orgGroup.ID.ToString(), parentOrgGroup.ID.ToString());

                return orgGroup.ID.ToString();
            }
            string t = Create(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties);
            Log.Debug(String.Format("SyncOrganizationalUnit {0},{1},{2},{3},{4},{5} begin", nativeID, parentOrganizationalUnitID, name, fullName, displayName, email));

            return t;
        }

        public void Delete(string organizationalUnitID)
        {
            ADOrganizationalUnit ou = null;
            try
            {
                Log.Error(organizationalUnitID);
                ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
                Indigox.Common.ADAccessor.Accessor.DeleteGroup(group.ID.ToString());

                ou = ADAccessorUtil.GetADObjectByID<ADOrganizationalUnit>(organizationalUnitID);
                if (ou != null)
                {                    
                    Indigox.Common.ADAccessor.Accessor.DeleteOrganizationalUnit(ou.ID.ToString());
                }
                Log.Error("done" + organizationalUnitID);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                Log.Error(organizationalUnitID);
                Log.Error(ou.ID.ToString());
                throw ex;
            }
        }

        public void ChangeProperty(string organizationalUnitID, PropertyChangeCollection propertyChanges)
        {
            ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
            ADOrganizationalUnit ou = ADAccessorUtil.GetADObjectByID<ADOrganizationalUnit>(organizationalUnitID);
            if (ou != null)
            {
                group.Name = Convert.ToString(propertyChanges.Get("FullName") + "全体");
            }
            else
            {
                group.Name = Convert.ToString(propertyChanges.Get("FullName"));
            }

            group.DisplayName = Convert.ToString(propertyChanges.Get("DisplayName") + "全体");
            group.Mail = Convert.ToString(propertyChanges.Get("Email"));
            
            if (ou != null)
            {
                ou.Name = Convert.ToString(propertyChanges.Get("DisplayName"));
                Indigox.Common.ADAccessor.Accessor.UpdateOrganizationalUnit(ou);
            }

            Indigox.Common.ADAccessor.Accessor.UpdateGroup(group);
        }

        public void AddOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            Indigox.Common.ADAccessor.Accessor.AddToGroup(organizationalUnitID, parentOrganizationalUnitID);

            ADOrganizationalUnit ou = ADAccessorUtil.GetADObjectByID<ADOrganizationalUnit>(organizationalUnitID);
            if (ou != null)
            {
                ADGroup group = Indigox.Common.ADAccessor.Accessor.GetGroupByID(parentOrganizationalUnitID);
                string parentOUID = group.Parent.ToString();
                Indigox.Common.ADAccessor.Accessor.MoveTo(ou.ID.ToString(), parentOUID);
                Indigox.Common.ADAccessor.Accessor.MoveTo(organizationalUnitID, ou.ID.ToString());
            }
            else
            {
                MoveToParentOU(parentOrganizationalUnitID, organizationalUnitID);
            }
        }

        public void RemoveOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(parentOrganizationalUnitID);
            //ADGroup ou = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);

            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup(organizationalUnitID, parentOrganizationalUnitID);
            MoveToRootOU(organizationalUnitID);
        }

        public void AddOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
            //ADGroup role = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalRoleID);
            if (String.IsNullOrEmpty(organizationalRoleID))
            {
                return;
            }
            Indigox.Common.ADAccessor.Accessor.AddToGroup(organizationalRoleID, organizationalUnitID);
            MoveToParentOU(organizationalUnitID, organizationalRoleID);
        }

        public void RemoveOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
            //ADGroup role = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalRoleID);
            if (String.IsNullOrEmpty(organizationalRoleID))
            {
                return;
            }
            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup(organizationalRoleID, organizationalUnitID);
            MoveToRootOU(organizationalRoleID);
        }

        public void AddUser(string organizationalUnitID, string userID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
            //ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);

            Indigox.Common.ADAccessor.Accessor.AddToGroup(userID, organizationalUnitID);
            MoveToParentOU(organizationalUnitID, userID);
        }

        public void RemoveUser(string organizationalUnitID, string userID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
            //ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);

            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup(userID, organizationalUnitID);
            MoveToRootOU(userID);
        }

        private string MoveToParentOU(string parentOrganizationalUnitID, string entryID)
        {
            ADGroup group = Indigox.Common.ADAccessor.Accessor.GetGroupByID(parentOrganizationalUnitID);
            Indigox.Common.ADAccessor.Accessor.MoveTo(entryID, group.Parent.ToString());
            return group.Parent.ToString();
        }

        private void MoveToRootOU(string entryID)
        {
            Indigox.Common.ADAccessor.Accessor.MoveTo(entryID, null);
        }
    }
}