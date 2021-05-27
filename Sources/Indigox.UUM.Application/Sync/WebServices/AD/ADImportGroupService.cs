using System;
using System.Collections.Generic;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ADGroup = Indigox.Common.ADAccessor.ObjectModel.Group;
using ADUser = Indigox.Common.ADAccessor.ObjectModel.User;

namespace Indigox.UUM.Application.Sync.WebServices.AD
{
    [WebService(Name = "ImportGroupService", Namespace = Consts.Namespace)]
    public class ADImportGroupService : IImportGroupService
    {
        public string Create(string nativeID, string name, string email, string description, double orderNum)
        {
            ADGroup group = new ADGroup()
            {
                Name = name,
                DisplayName = name,
                Mail = email
            };
            ADGroup createdGroup = Indigox.Common.ADAccessor.Accessor.CreateGroup(null, group);
            return createdGroup.ID.ToString();
        }

        public string SyncGroup(string nativeID, string name, string email, string description, double orderNum)
        {
            ADGroup group = Indigox.Common.ADAccessor.Accessor.GetGroupByAccount(email.Substring(0, email.IndexOf("@")));
            if (group != null)
            {
                /*
                ADGroup rootGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
                Indigox.Common.ADAccessor.Accessor.MoveTo(group.ID.ToString(), rootGroup.Parent.ToString());
                Indigox.Common.ADAccessor.Accessor.AddToGroup(group.ID.ToString(), rootGroup.ID.ToString());
                **/
                return group.ID.ToString();
                
            }
            return this.Create(nativeID,name, email, description, orderNum);
        }

        public void Delete(string groupID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(groupID);
            Indigox.Common.ADAccessor.Accessor.DeleteGroup(groupID);
        }

        public void ChangeProperty( string groupID, PropertyChangeCollection propertyChanges )
        {
            ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(groupID);

            group.Name = Convert.ToString( propertyChanges.Get( "Name" ) );
            group.DisplayName = Convert.ToString(propertyChanges.Get("DisplayName"));
            group.Mail = Convert.ToString(propertyChanges.Get("Email"));

            Indigox.Common.ADAccessor.Accessor.UpdateGroup( group );
        }

        public void AddOrganizationalUnit(string groupID, string organizationalUnitID)
        {
            Indigox.Common.ADAccessor.Accessor.AddToGroup(organizationalUnitID, groupID);
        }

        public void RemoveOrganizationalUnit(string groupID, string organizationalUnitID)
        {
            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup(organizationalUnitID, groupID);
        }

        public void AddOrganizationalRole(string groupID, string organizationalRoleID)
        {
            Indigox.Common.ADAccessor.Accessor.AddToGroup(organizationalRoleID, groupID);
        }

        public void RemoveOrganizationalRole(string groupID, string organizationalRoleID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(groupID);
            //ADGroup role = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalRoleID);
            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup(organizationalRoleID, groupID);
        }

        public void AddUser(string groupID, string userID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(groupID);
            //ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);
            Indigox.Common.ADAccessor.Accessor.AddToGroup(userID, groupID);
        }

        public void RemoveUser(string groupID, string userID)
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(groupID);
            //ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);
            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup(userID, groupID);
        }

    }
}
