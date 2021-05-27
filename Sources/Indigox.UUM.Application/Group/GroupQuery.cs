using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.Group
{
    public class GroupQuery : Indigox.Web.CQRS.GenericSingleQuery<GroupDTO>
    {
        public string GroupID { get; set; }

        public override GroupDTO Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IGroup>();
            IGroup item = repository.Get( this.GroupID );
            GroupDTO dto = null;
            if ( item != null )
            {
                dto = GroupDTO.ConvertToDTO( item );
            }
            return dto;
        }
    }
}