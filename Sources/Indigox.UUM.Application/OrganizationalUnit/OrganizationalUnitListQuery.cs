using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public class OrganizationalUnitListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalUnitDTO>
    {
        public string OrganizationalUnitID { get; set; }

        public override IList<OrganizationalUnitDTO> List()
        {
            IList<OrganizationalUnitDTO> dtoList = new List<OrganizationalUnitDTO>();

            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();

            Query condition = Query.NewQuery;

            if ( FetchSize > 0 )
            {
                condition.StartFrom( FirstResult ).LimitTo( FetchSize );
            }

            condition.OrderByAsc("DisplayName").OrderByAsc("OrderNum");

            ISpecification spec = Specification.And(
                    Specification.Equal( "Enabled", true ), Specification.Equal( "Deleted", false ) );

            IOrganizationalUnit parentOrganizationalUnit = null;
            if ( !String.IsNullOrEmpty( this.OrganizationalUnitID ) )
            {
                parentOrganizationalUnit = repository.Get( OrganizationalUnitID );
            }
            if ( parentOrganizationalUnit != null )
            {
                spec = Specification.And( Specification.Equal( "Organization", parentOrganizationalUnit ), spec );
            }

            condition.FindByCondition( spec );

            IList<IOrganizationalUnit> list = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Find( condition );
            dtoList = OrganizationalUnitDTO.ConvertToDTOs( list );

            return dtoList;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();

            Query condition = Query.NewQuery;

            ISpecification spec = Specification.And(
                Specification.Equal( "Enabled", true ), Specification.Equal( "Deleted", false ) );

            IOrganizationalUnit parentOrganizationalUnit = null;
            if ( !String.IsNullOrEmpty( this.OrganizationalUnitID ) )
            {
                parentOrganizationalUnit = repository.Get( OrganizationalUnitID );
            }
            if ( parentOrganizationalUnit != null )
            {
                spec = Specification.And( Specification.Equal( "Organization", parentOrganizationalUnit ), spec );
            }

            condition.FindByCondition( spec );

            return RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().GetTotalCount(condition);
        }
    }
}