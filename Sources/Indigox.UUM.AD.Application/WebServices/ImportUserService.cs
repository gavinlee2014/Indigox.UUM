using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ADGroup = Indigox.Common.ADAccessor.ObjectModel.Group;
using ADOrganizationalUnit = Indigox.Common.ADAccessor.ObjectModel.OrganizationalUnit;
using ADUser = Indigox.Common.ADAccessor.ObjectModel.User;
using Indigox.Common.Logging;

namespace Indigox.UUM.AD.Application.WebServices
{
    [WebService( Name = "ImportUserService", Namespace = Consts.Namespace )]
    public class ImportUserService : IImportUserService
    {
        public string Create(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            ADGroup parentOrgGroup = null;
            if (String.IsNullOrEmpty(organizationalUnitID))
            {
                parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
            }
            else
            {
                int index = organizationalUnitID.IndexOf(',');
                string ADGroupID = null;
                if (index > 0)
                {
                    ADGroupID = organizationalUnitID.Substring(0, index);
                }
                parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByID(ADGroupID);
            }

            ADOrganizationalUnit parentOrgOU = Indigox.Common.ADAccessor.Accessor.GetOrganizationByByID(parentOrgGroup.Parent.ToString());

            string containerID = null;
            if (parentOrgOU != null)
            {
                containerID = parentOrgOU.ID.ToString();
            }

            string groupID = null;
            if (parentOrgGroup != null)
            {
                groupID = parentOrgGroup.ID.ToString();
            }
            NameService nameService = new NameService(name);
            ADUser user = new ADUser()
            {
                Account = accountName,
                Name = name,
                DisplayName = displayName.Trim(),
                Mail = email,
                Phone = telephone,
                Mobile = mobile,
                Fax = fax,
                Title = title,
                Portrait = portrait,
                GivenName = nameService.GetGivenName(),
                FamilyName = nameService.GetLastName()
            };
            ADUser createdUser = Indigox.Common.ADAccessor.Accessor.CreateUser(containerID, user);
            Indigox.Common.ADAccessor.Accessor.EnableUser(createdUser.ID.ToString());
            Indigox.Common.ADAccessor.Accessor.AddToGroup(createdUser.ID.ToString(), groupID);

            return createdUser.ID.ToString();
        }

        public string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            Log.Debug(String.Format("SyncUser {0} {1} {2} begin", nativeID, accountName, fullName));
            ADUser user = Indigox.Common.ADAccessor.Accessor.GetUserByAccount(accountName);
            if (user != null)
            {
                Log.Debug(String.Format("User {0} {1} {2} exist", nativeID, accountName, fullName));
                int index = organizationalUnitID.IndexOf(',');
                string ADGroupID = null;
                if (index > 0)
                {
                    ADGroupID = organizationalUnitID.Substring(0, index);
                }
                ADGroup parentOrgGroup = null;
                if (String.IsNullOrEmpty(organizationalUnitID))
                {
                    parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
                }
                else
                {
                    parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByID(ADGroupID);
                }
                Indigox.Common.ADAccessor.Accessor.MoveTo(user.ID.ToString(), parentOrgGroup.Parent.ToString());
                Indigox.Common.ADAccessor.Accessor.AddToGroup(user.ID.ToString(), parentOrgGroup.ID.ToString());

                return user.ID.ToString();
            }
            Log.Debug(String.Format("User {0} {1} {2} need create", nativeID, accountName, fullName));

            return this.Create(nativeID, organizationalUnitID, accountName, name, fullName, displayName, email,
                                title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase);
        }

        public void Delete(string userID)
        {            
            Indigox.Common.ADAccessor.Accessor.DeleteUser(userID);
        }

        public void Disable(string userID)
        {
            /*
             * 修改时间：2018-08-23
             * 修改人：曾勇
             * 修改内容：禁用用户时候，将用户移动到禁用用户OU下
             */
            Indigox.Common.ADAccessor.Accessor.DisableUser(userID);
            string leaveOuID = Indigox.Common.ADAccessor.Accessor.GetLeaveOU();
            Indigox.Common.ADAccessor.Accessor.MoveTo(userID, leaveOuID);
        }

        public void Enable(string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            Indigox.Common.ADAccessor.Accessor.EnableUser(userID);
        }

        public void ChangeProperty(string userID, PropertyChangeCollection propertyChanges)
        {
            ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);

            user.Name = Convert.ToString(propertyChanges.Get("Name"));
            NameService nameService = new NameService(user.Name);
            user.GivenName = nameService.GetGivenName();
            user.FamilyName = nameService.GetLastName();
            user.DisplayName = Convert.ToString(propertyChanges.Get("DisplayName"));
            user.Account = Convert.ToString(propertyChanges.Get("AccountName"));
            user.Mail = Convert.ToString(propertyChanges.Get("Email"));
            user.Mobile = Convert.ToString(propertyChanges.Get("Mobile"));
            user.Phone = Convert.ToString(propertyChanges.Get("Telephone"));
            user.Fax = Convert.ToString(propertyChanges.Get("Fax"));
            user.Title = Convert.ToString(propertyChanges.Get("Title"));
            user.Portrait = Convert.ToString(propertyChanges.Get("Profile"));

            Indigox.Common.ADAccessor.Accessor.UpdateUser(user);
        }
    }
}