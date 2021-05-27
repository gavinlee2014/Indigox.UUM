using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.HR.Model;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.HR.Service;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.HR
{
    public class HREmployeeExistListQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string ID { get; set; }

        public override IList<OrganizationalPersonDTO> List()
        {
            IList<OrganizationalPersonDTO> dtoList = new List<OrganizationalPersonDTO>();

            Query condition = Query.NewQuery;

            condition.OrderByAsc("OrderNum").OrderByAsc("Name");

            condition.FindByCondition(GetSpecification());

            IList<IOrganizationalPerson> list = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().Find(condition);
            dtoList = OrganizationalPersonDTO.ConvertToDTOs(list);

            return dtoList;
        }

        private ISpecification GetSpecification()
        {
            HREmployee employee = RepositoryFactory.Instance.CreateRepository<HREmployee>().Get(this.ID);

            ISpecification spec = Specification.And(
                Specification.Equal("Enabled", true),
                Specification.Equal("Deleted", false),
                Specification.Equal("Name", employee.Name)
            );

            IOrganizationalUnit parentOrganizationalUnit = new HROrganizationalService().GetMappedPrincipal(employee.ParentID);

            if (parentOrganizationalUnit != null)
            {
                spec = Specification.And(Specification.Equal("Organization", parentOrganizationalUnit), spec);
            }

            return spec;
        }

    }
}
