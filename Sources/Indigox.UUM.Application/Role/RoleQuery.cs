using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.Role
{
    public class RoleQuery : Indigox.Web.CQRS.GenericSingleQuery<RoleDTO>
    {
        public string RoleID { get; set; }

        public override RoleDTO Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IRole>();
            IRole item = repository.Get( this.RoleID );
            RoleDTO dto = null;
            if ( item != null )
            {
                dto = RoleDTO.ConvertToDTO( item );
            }
            return dto;
        }
    }
}