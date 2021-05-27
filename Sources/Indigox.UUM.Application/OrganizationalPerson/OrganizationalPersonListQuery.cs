using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class OrganizationalPersonListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string OrganizationalUnitID { get; set; }

        public override IList<OrganizationalPersonDTO> List()
        {
            IList<OrganizationalPersonDTO> dtoList = new List<OrganizationalPersonDTO>();

            Query condition = Query.NewQuery;

            if ( FetchSize > 0 )
            {
                condition.StartFrom( FirstResult ).LimitTo( FetchSize );
            }

            condition.OrderByAsc( "OrderNum" ).OrderByAsc( "Name" );

            condition.FindByCondition( GetSpecification() );

            IList<IOrganizationalPerson> list = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().Find( condition );
            dtoList = OrganizationalPersonDTO.ConvertToDTOs( list );

            return dtoList;
        }

        public override int Size()
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();

            Query condition = Query.NewQuery;

            condition.FindByCondition( GetSpecification() );

            return RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().GetTotalCount(condition);
        }

        private ISpecification GetSpecification()
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();

            ISpecification spec = Specification.And(
                Specification.Equal( "Enabled", true ),
                Specification.Equal( "Deleted", false )
            );

            IOrganizationalUnit parentOrganizationalUnit = null;
            if ( !String.IsNullOrEmpty( this.OrganizationalUnitID ) )
            {
                parentOrganizationalUnit = repository.Get( OrganizationalUnitID );
            }
            if ( parentOrganizationalUnit != null )
            {
                spec = Specification.And( Specification.Equal( "Organization", parentOrganizationalUnit ), spec );
            }

            return spec;
        }
    }
}