using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class OrganizationalPersonQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalPersonDTO>
    {
        public string OrganizationalPersonID { get; set; }

        public override OrganizationalPersonDTO Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IOrganizationalPerson item = repository.Get( this.OrganizationalPersonID );
            OrganizationalPersonDTO dto = null;
            if ( item != null )
            {
                dto = OrganizationalPersonDTO.ConvertToDTO( item );
            }
            return dto;
        }
    }
}