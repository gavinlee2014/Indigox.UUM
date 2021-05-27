using System;
using System.Web.Services;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ADGroup = Indigox.Common.ADAccessor.ObjectModel.Group;

namespace Indigox.UUM.Application.Sync.WebServices.AD
{
    [WebService( Name = "ImportRoleService", Namespace = Consts.Namespace )]
    public class ADImportRoleService : IImportRoleService
    {
        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            ADGroup group = new ADGroup()
            {
                Name = name,
                DisplayName = name,
                Mail = email
            };
            ADGroup createdGroup = Indigox.Common.ADAccessor.Accessor.CreateGroup( null, group );

            //ADAccessorUtil.CreateADMapping(roleID, createdGroup);
            return createdGroup.ID.ToString();
        }

        public string SyncRole( string nativeID, string name, string email, string description, double orderNum )
        {
            ADGroup group = Indigox.Common.ADAccessor.Accessor.GetGroupByAccount( email.Substring( 0, email.IndexOf( "@" ) ) );
            if ( group != null )
            {
                ADGroup rootGroup = Indigox.Common.ADAccessor.Accessor.GetDefaultGroup();
                Indigox.Common.ADAccessor.Accessor.MoveTo( group.ID.ToString(), rootGroup.Parent.ToString() );
                Indigox.Common.ADAccessor.Accessor.AddToGroup( group.ID.ToString(), rootGroup.ID.ToString() );
                return group.ID.ToString();
            }
            return this.Create( nativeID, name, email, description, orderNum );
        }

        public void Delete( string roleID )
        {
            //ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>(roleID);
            Indigox.Common.ADAccessor.Accessor.DeleteGroup( roleID );
        }

        public void ChangeProperty( string roleID, PropertyChangeCollection propertyChanges )
        {
            ADGroup group = ADAccessorUtil.GetADObjectByID<ADGroup>( roleID );

            group.Name = Convert.ToString( propertyChanges.Get( "FullName" ) );
            group.DisplayName = Convert.ToString(propertyChanges.Get("FullName"));
            group.Mail = Convert.ToString( propertyChanges.Get( "Email" ) );

            Indigox.Common.ADAccessor.Accessor.UpdateGroup( group );
        }

        public void AddOrganizationalRole( string roleID, string organizationalRoleID )
        {
            Indigox.Common.ADAccessor.Accessor.AddToGroup( organizationalRoleID, roleID );
        }

        public void RemoveOrganizationalRole( string roleID, string organizationalRoleID )
        {
            Indigox.Common.ADAccessor.Accessor.RemoveFromGroup( organizationalRoleID, roleID );
        }
    }
}