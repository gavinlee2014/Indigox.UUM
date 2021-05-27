using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ADGroup = Indigox.Common.ADAccessor.ObjectModel.Group;
using ADOrganizationalUnit = Indigox.Common.ADAccessor.ObjectModel.OrganizationalUnit;

namespace Indigox.UUM.AD.Application.WebServices
{
    [WebService( Name = "ImportOrganizationalRoleService", Namespace = Consts.Namespace )]
    public class ImportOrganizationalRoleService : IImportOrganizationalRoleService
    {
        public string Create(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum)
        {
            ADGroup parentOrgGroup = null;
            if ( String.IsNullOrEmpty( organizationalUnitID ) )
            {
                parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
            }
            else
            {
                parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByID( organizationalUnitID );
            }

            ADOrganizationalUnit parentOrgOU = Indigox.Common.ADAccessor.Accessor.GetOrganizationByByID( parentOrgGroup.Parent.ToString() );

            string containerID = null;
            if ( parentOrgOU != null )
            {
                containerID = parentOrgOU.ID.ToString();
            }

            string groupID = null;
            if ( parentOrgGroup != null )
            {
                groupID = parentOrgGroup.ID.ToString();
            }

            ADGroup group = new ADGroup()
            {
                Name = name,
                DisplayName = displayName.Trim(),
                Mail = email
            };
            ADGroup createdGroup = Indigox.Common.ADAccessor.Accessor.CreateGroup( containerID, group );
            //Indigox.Common.ADAccessor.Accessor.AddToGroup( createdGroup.ID.ToString(), groupID );

            return createdGroup.ID.ToString();
        }

        public string SyncOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum)
        {
            ADGroup group = Indigox.Common.ADAccessor.Accessor.GetGroupByAccount( email.Substring( 0, email.IndexOf( "@" ) ) );
            if ( group != null )
            {
                ADGroup parentOrgGroup = null;
                if ( String.IsNullOrEmpty( organizationalUnitID ) )
                {
                    parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
                }
                else
                {
                    parentOrgGroup = Indigox.Common.ADAccessor.Accessor.GetGroupByID( organizationalUnitID );
                }
                Indigox.Common.ADAccessor.Accessor.MoveTo( group.ID.ToString(), parentOrgGroup.Parent.ToString() );
                //Indigox.Common.ADAccessor.Accessor.AddToGroup( group.ID.ToString(), parentOrgGroup.ID.ToString() );

                return group.ID.ToString();
            }
            return this.Create(nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum);
        }

        public void Delete( string organizationalRoleID )
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalRoleID);
            Indigox.Common.ADAccessor.Accessor.DeleteGroup( organizationalRoleID );
        }

        public void ChangeProperty( string organizationalRoleID, PropertyChangeCollection propertyChanges )
        {
            ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>( organizationalRoleID );

            group.Name = Convert.ToString( propertyChanges.Get( "FullName" ) );
            group.DisplayName = Convert.ToString(propertyChanges.Get("DisplayName"));
            group.Mail = Convert.ToString(propertyChanges.Get("Email"));
            Indigox.Common.ADAccessor.Accessor.UpdateGroup( group );
        }

        public void AddUser( string organizationalRoleID, string userID )
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalRoleID);
            //ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);

            Indigox.Common.ADAccessor.Accessor.AddToGroup( userID, organizationalRoleID );
        }

        public void RemoveUser( string organizationalRoleID, string userID )
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(organizationalRoleID);
            //ADUser user = ADAccessorUtil.GetADObjectByID<ADUser>(userID);

            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup( userID, organizationalRoleID );
        }
    }
}