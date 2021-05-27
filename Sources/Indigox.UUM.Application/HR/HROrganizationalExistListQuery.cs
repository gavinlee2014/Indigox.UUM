using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.UUM.HR.Model;
using Indigox.Common.DomainModels.Specifications;
using Indigox.UUM.HR.Service;

namespace Indigox.UUM.Application.HR
{
    public class HROrganizationalExistListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalUnitDTO>
    {
        public string ID { get; set; }

        public override IList<OrganizationalUnitDTO> List()
        {
            IList<OrganizationalUnitDTO> dtoList = new List<OrganizationalUnitDTO>();

            Query condition = Query.NewQuery;

            condition.OrderByAsc("OrderNum").OrderByAsc("Name");

            condition.FindByCondition(GetSpecification());

            IList<IOrganizationalUnit> list = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Find(condition);
            dtoList = OrganizationalUnitDTO.ConvertToDTOs(list);

            return dtoList;
        }

        private ISpecification GetSpecification()
        {
            HROrganizational organizational = RepositoryFactory.Instance.CreateRepository<HROrganizational>().Get(this.ID);

            ISpecification spec = Specification.And(
                Specification.Equal("Enabled", true),
                Specification.Equal("Deleted", false),
                Specification.Like("Name", "%" + organizational.Name + "%")
            );

            IOrganizationalUnit parentOrganizationalUnit = new HROrganizationalService().GetMappedPrincipal(organizational.ParentID);

            if (parentOrganizationalUnit != null)
            {
                spec = Specification.And(Specification.Equal("Organization", parentOrganizationalUnit), spec);
            }

            return spec;
        }

    }
}
