using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ADGroup = Indigox.Common.ADAccessor.ObjectModel.Group;
using ADOrganizationalUnit = Indigox.Common.ADAccessor.ObjectModel.OrganizationalUnit;
using Indigox.Common.Logging;
using Indigox.Common.ADAccessor;

namespace Indigox.UUM.AD.Application.WebServices
{
    [WebService(Name = "ImportOrganizationalUnitService", Namespace = Consts.Namespace)]
    public class ImportOrganizationalUnitService : IImportOrganizationalUnitService
    {
        private static List<string> CreateOURequiredType = new List<string>();
        private string GetGroupName(string name, string displayName)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z]+\.");
            return reg.Replace(displayName, "");
        }
        private string GetGroupDisplayName(string name, string displayName)
        {
            return displayName + "全体";
        }
        private string GetOUName(string name, string displayName)
        {
            return name;
        }

        static ImportOrganizationalUnitService()
        {
            string createOURequiredType = ConfigurationManager.AppSettings["CreateOURequiredType"];
            if (!String.IsNullOrEmpty(createOURequiredType))
            {
                CreateOURequiredType.AddRange(createOURequiredType.Split(','));
            }
        }

        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType)
        {
            ADGroup parentOrgGroup = null;
            int index = parentOrganizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                parentOrganizationalUnitID = parentOrganizationalUnitID.Substring(0, index);
            }
            if (String.IsNullOrEmpty(parentOrganizationalUnitID))
            {
                parentOrgGroup = Accessor.GetDefaultGroup();
            }
            else
            {
                parentOrgGroup = Accessor.GetGroupByID(parentOrganizationalUnitID);
            }

            ADOrganizationalUnit parentOrgOU = Accessor.GetOrganizationByByID(parentOrgGroup.Parent.ToString());

            string containerID = null;
            if (parentOrgOU != null)
            {
                containerID = parentOrgOU.ID.ToString();
            }

            string groupName = GetGroupName(name, displayName);
            string groupDisplayName = GetGroupDisplayName(name, displayName);

            ADGroup createdGroup = null;
            if (this.IsCreateOURequired(organizationalUnitType))
            {
                ADOrganizationalUnit createdOU = CreateOU(containerID, GetOUName(name,displayName));
                createdGroup = CreateGroup(createdOU.ID.ToString(), parentOrgGroup, groupName, groupDisplayName, email, organizationalUnitType);
               // ADAccessorUtil.MappingGroupAndOU(createdGroup.ID.ToString(), createdOU.ID.ToString());
                return createdGroup.ID.ToString() + "," + createdOU.ID.ToString();
            }
            else
            {
                createdGroup = CreateGroup(containerID, parentOrgGroup, groupName, groupDisplayName, email, organizationalUnitType);
                return createdGroup.ID.ToString() ;
            }
        }

        private ADGroup CreateGroup(string groupContainerID, ADGroup parentOrgGroup, string name, string displayName, string email, string organizationalUnitType)
        {
            ADGroup group = new ADGroup()
            {
                Name = name,
                DisplayName = displayName,
                Mail = email
            };

            ADGroup createdGroup = Accessor.CreateGroup(groupContainerID, group);
            Accessor.AddToGroup(createdGroup.ID.ToString(), parentOrgGroup.ID.ToString());
            return createdGroup;
        }

        private ADOrganizationalUnit CreateOU(string containerID, string name)
        {
            ADOrganizationalUnit ou = new ADOrganizationalUnit()
            {
                Name = name
            };
            ADOrganizationalUnit createdOU = Accessor.CreateOrganization(containerID, ou);
            return createdOU;
        }

        private bool IsCreateOURequired(string organizationalUnitType)
        {
            return CreateOURequiredType.Contains(organizationalUnitType);
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType)
        {
            Log.Debug(String.Format("SyncOrganizationalUnit {0},{1},{2},{3},{4},{5} begin", nativeID, parentOrganizationalUnitID, name, fullName, displayName, email));
            ADGroup orgGroup = Accessor.GetGroupByAccount(email.Substring(0, email.IndexOf("@")));
            if (orgGroup != null)
            {
                ADGroup parentOrgGroup = null;
                int index = parentOrganizationalUnitID.IndexOf(',');
                string ADGroupID = null;
                if (index > 0)
                {
                    ADGroupID = parentOrganizationalUnitID.Substring(0, index);
                }
                if (String.IsNullOrEmpty(ADGroupID))
                {
                    parentOrgGroup = Accessor.GetDefaultGroup();
                }
                else
                {
                    parentOrgGroup = Accessor.GetGroupByID(ADGroupID);
                }

                string containerID = parentOrgGroup.Parent.ToString();
                string ouName = GetOUName(name, displayName);
                ADOrganizationalUnit createdOU = null;

                if (IsCreateOURequired(organizationalUnitType))
                {
                    createdOU = CreateOU(parentOrgGroup.Parent.ToString(), ouName);
                   // ADAccessorUtil.MappingGroupAndOU(orgGroup.ID.ToString(), createdOU.ID.ToString());
                    containerID = createdOU.ID.ToString();
                }

                Accessor.MoveTo(orgGroup.ID.ToString(), containerID);
                Accessor.AddToGroup(orgGroup.ID.ToString(), parentOrgGroup.ID.ToString());

                return orgGroup.ID.ToString() + (createdOU==null? "":","+createdOU.ID.ToString());
            }
            string t = Create(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType);
            Log.Debug(String.Format("SyncOrganizationalUnit {0},{1},{2},{3},{4},{5} begin", nativeID, parentOrganizationalUnitID, name, fullName, displayName, email));

            return t;
        }

        public void Delete(string organizationalUnitID)
        {
            try
            {
                Log.Error("*************************");
                ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
                Accessor.DeleteGroup(group.ID.ToString());

                ADOrganizationalUnit ou = ADAccessorUtil.GetADObjectByID<ADOrganizationalUnit>(organizationalUnitID);
                if (ou != null)
                {
                    Accessor.DeleteOrganizationalUnit(ou.ID.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw ex;
            }
        }

        public void ChangeProperty(string organizationalUnitID, PropertyChangeCollection propertyChanges)
        {
            ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalUnitID);
            ADOrganizationalUnit ou = ADAccessorUtil.GetADObjectByID<ADOrganizationalUnit>(organizationalUnitID);
            string name=Convert.ToString(propertyChanges.Get("Name"));
            string displayName = Convert.ToString(propertyChanges.Get("DisplayName"));
            
            group.Name=GetGroupName(name,displayName);

            group.DisplayName = GetGroupDisplayName(name,displayName);
            group.Mail = Convert.ToString(propertyChanges.Get("Email"));

            Log.Debug("Change Group Property [name]:[" + group.Name + "], [DisplayName]:[" + group.DisplayName + "]");

            Accessor.UpdateGroup(group);

            if (ou != null)
            {
                ou.Name = GetOUName(name,displayName);
                Accessor.UpdateOrganizationalUnit(ou);
            }
        }

        public void AddOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            Accessor.AddToGroup(organizationalUnitID, parentOrganizationalUnitID);

            ADOrganizationalUnit ou = ADAccessorUtil.GetADObjectByID<ADOrganizationalUnit>(organizationalUnitID);
            if (ou != null)
            {
                ADGroup group = Accessor.GetGroupByID(parentOrganizationalUnitID);
                string parentOUID = group.Parent.ToString();
                Accessor.MoveTo(ou.ID.ToString(), parentOUID);
                Accessor.MoveTo(organizationalUnitID, ou.ID.ToString());
            }
            else
            {
                MoveToParentOU(parentOrganizationalUnitID, organizationalUnitID);
            }
        }

        public void RemoveOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            int index = organizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                organizationalUnitID = organizationalUnitID.Substring(0, index);
            }
            index = parentOrganizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                parentOrganizationalUnitID = parentOrganizationalUnitID.Substring(0, index);
            }
            Accessor.RemoveFromGroup(organizationalUnitID, parentOrganizationalUnitID);
            MoveToRootOU(organizationalUnitID);
        }

        public void AddOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            int index = organizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                organizationalUnitID = organizationalUnitID.Substring(0, index);
            }
            Accessor.AddToGroup(organizationalRoleID, organizationalUnitID);
            MoveToParentOU(organizationalUnitID, organizationalRoleID);
        }

        public void RemoveOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            int index = organizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                organizationalUnitID = organizationalUnitID.Substring(0, index);
            }
            Accessor.RemoveFromGroup(organizationalRoleID, organizationalUnitID);
            MoveToRootOU(organizationalRoleID);
        }

        public void AddUser(string organizationalUnitID, string userID)
        {
            int index = organizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                organizationalUnitID = organizationalUnitID.Substring(0, index);
            }
            Accessor.AddToGroup(userID, organizationalUnitID);
            MoveToParentOU(organizationalUnitID, userID);
        }

        public void RemoveUser(string organizationalUnitID, string userID)
        {
            int index = organizationalUnitID.IndexOf(',');
            if (index > 0)
            {
                organizationalUnitID = organizationalUnitID.Substring(0, index);
            }
            Accessor.RemoveFromGroup(userID, organizationalUnitID);
            MoveToLeaveOU(userID);
        }

        private string MoveToParentOU(string parentOrganizationalUnitID, string entryID)
        {
            ADGroup group = Accessor.GetGroupByID(parentOrganizationalUnitID);
            // ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(parentOrganizationalUnitID);

            Accessor.MoveTo(entryID, group.Parent.ToString());
            return group.Parent.ToString();
        }

        private void MoveToRootOU(string entryID)
        {
            Accessor.MoveTo(entryID, null);
        }

        private void MoveToLeaveOU(string entryID)
        {
            string leaveOU = Accessor.GetLeaveOU();
            Accessor.MoveTo(entryID, leaveOU);
        }
    }
}
