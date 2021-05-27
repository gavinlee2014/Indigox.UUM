using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.Role
{
    public class RoleListQuery : Indigox.Web.CQRS.GenericListQuery<RoleDTO>
    {
        public string QueryString { get; set; }

        public override IList<RoleDTO> List()
        {
            IList<RoleDTO> dtoList = new List<RoleDTO>();

            var repository = RepositoryFactory.Instance.CreateRepository<IRole>();

            Query condition = Query.NewQuery;

            if ( FetchSize > 0 )
            {
                condition.StartFrom( FirstResult ).LimitTo( FetchSize );
            }

            condition.OrderByDesc( "CreateTime" );

            ISpecification spec = Specification.And(
                    Specification.Equal( "Enabled", true ), Specification.Equal( "Deleted", false ) );

            if ( !string.IsNullOrEmpty( QueryString ) )
            {
                spec = Specification.And( spec, Specification.Like( "Name", "%" + QueryString + "%" ) );
            }

            condition.FindByCondition( spec );

            IList<IRole> list = RepositoryFactory.Instance.CreateRepository<IRole>().Find( condition );
            dtoList = RoleDTO.ConvertToDTOs( list );

            return dtoList;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IRole>();

            Query condition = Query.NewQuery;

            ISpecification spec = Specification.And(
                Specification.Equal( "Enabled", true ), Specification.Equal( "Deleted", false ) );

            if ( !string.IsNullOrEmpty( QueryString ) )
            {
                spec = Specification.And( spec, Specification.Like( "Name", "%" + QueryString + "%" ) );
            }

            condition.FindByCondition( spec );

            return RepositoryFactory.Instance.CreateRepository<IRole>().GetTotalCount(condition);
        }
    }
}