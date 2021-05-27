using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalRole
{
    public class OrganizationalRoleQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalRoleDTO>
    {
        public string OrganizationalRoleID { get; set; }

        public override OrganizationalRoleDTO Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            IOrganizationalRole item = repository.Get( this.OrganizationalRoleID );
            OrganizationalRoleDTO dto = null;
            if ( item != null )
            {
                dto = OrganizationalRoleDTO.ConvertToDTO( item );
            }
            return dto;
        }
    }
}