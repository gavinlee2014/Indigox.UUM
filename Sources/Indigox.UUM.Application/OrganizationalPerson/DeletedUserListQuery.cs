using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class DeletedUserListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string OrganizationalPersonID { get; set; }

        public override IList<OrganizationalPersonDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();

            IList<IOrganizationalPerson> users = repository.Find(
                Query.NewQuery
                    .FindByCondition( Specification.Equal( "Deleted", true ) )
                    .StartFrom( this.FirstResult )
                    .LimitTo( this.FetchSize )
                    .OrderByDesc( "ModifyTime" ) );

            IList<OrganizationalPersonDTO> list = OrganizationalPersonDTO.ConvertToDTOs( users );

            return list;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();

            int count = repository.GetTotalCount(
                Query.NewQuery.FindByCondition(
                    Specification.Equal( "Deleted", true ) ) );

            return count;
        }
    }
}