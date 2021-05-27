using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class DisabledUserListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string QueryString { get; set; }

        public override IList<OrganizationalPersonDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();

            IList<IOrganizationalPerson> users = repository.Find(
                Query.NewQuery.FindByCondition( GetSpecification() )
                    .StartFrom( this.FirstResult )
                    .LimitTo( this.FetchSize )
                    .OrderByDesc( "ModifyTime" )
            );

            IList<OrganizationalPersonDTO> list = OrganizationalPersonDTO.ConvertToDTOs( users );

            return list;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();

            int count = repository.GetTotalCount(
                Query.NewQuery.FindByCondition( GetSpecification() )
            );

            return count;
        }

        private ISpecification GetSpecification()
        {
            ISpecification spec = Specification.And(
                Specification.Equal( "Enabled", false ),
                Specification.NotEqual( "Deleted", true )
            );

            if ( !string.IsNullOrEmpty( QueryString ) )
            {
                string likestr = "%" + QueryString + "%";
                spec = Specification.And(
                    spec,
                    Specification.Or(
                        Specification.Like( "Name", likestr ),
                        Specification.Like( "AccountName", likestr ),
                        Specification.Like( "Mobile", likestr ),
                        Specification.Like( "Telephone", likestr ),
                        Specification.Like( "Fax", likestr )
                    )
                );
            }

            return spec;
        }
    }
}