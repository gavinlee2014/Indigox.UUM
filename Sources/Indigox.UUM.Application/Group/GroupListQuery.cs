using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.Group
{
    public class GroupListQuery : Indigox.Web.CQRS.GenericListQuery<GroupDTO>
    {
        public string QueryString { get; set; }

        public override IList<GroupDTO> List()
        {
            IList<GroupDTO> dtoList = new List<GroupDTO>();

            var repository = RepositoryFactory.Instance.CreateRepository<IGroup>();

            Query condition = Query.NewQuery;

            if ( FetchSize > 0 )
            {
                condition.StartFrom( FirstResult ).LimitTo( FetchSize );
            }

            condition.OrderByAsc( "OrderNum" ).OrderByAsc( "Name" );

            ISpecification spec = Specification.And(
                    Specification.Equal( "Enabled", true ), Specification.Equal( "Deleted", false ) );

            if ( !string.IsNullOrEmpty( QueryString ) )
            {
                spec = Specification.And( spec, Specification.Like( "Name", "%" + QueryString + "%" ) );
            }

            condition.FindByCondition( spec );

            IList<IGroup> list = RepositoryFactory.Instance.CreateRepository<IGroup>().Find( condition );
            dtoList = GroupDTO.ConvertToDTOs( list );

            return dtoList;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IGroup>();

            Query condition = Query.NewQuery;

            ISpecification spec = Specification.And(
                Specification.Equal( "Enabled", true ), Specification.Equal( "Deleted", false ) );

            if ( !string.IsNullOrEmpty( QueryString ) )
            {
                spec = Specification.And( spec, Specification.Like( "Name", "%" + QueryString + "%" ) );
            }

            condition.FindByCondition( spec );

            return RepositoryFactory.Instance.CreateRepository<IGroup>().GetTotalCount( condition );
        }
    }
}