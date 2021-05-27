using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public class OrganizationalUnitQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalUnitDTO>
    {
        public string OrganizationalUnitID { get; set; }

        public override OrganizationalUnitDTO Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IOrganizationalUnit item = repository.Get( this.OrganizationalUnitID );
            OrganizationalUnitDTO dto = null;
            if ( item != null )
            {
                dto = OrganizationalUnitDTO.ConvertToDTO( item );
            }
            return dto;
        }
    }
}