using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.Role
{
    public class RoleForUnitListQuery : Indigox.Web.CQRS.GenericListQuery<RoleDTO>
    {
        public string OrganizationalUnitID { get; set; }
        public string OrganizationalRoleID { get; set; }

        public override IList<RoleDTO> List()
        {
            IList<RoleDTO> dtoList = new List<RoleDTO>();

            IOrganizationalUnit organizationalUnit = null;
            if (!string.IsNullOrEmpty(this.OrganizationalUnitID))
            {
                var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
                organizationalUnit = repository.Get(this.OrganizationalUnitID);
            }
            else if (!string.IsNullOrEmpty(this.OrganizationalRoleID))
            {
                var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
                organizationalUnit = repository.Get(this.OrganizationalRoleID).Organization;
            }

            if (organizationalUnit != null)
            {
                int level = 0;
                ISpecification spec = null;
                if (organizationalUnit is Corporation)
                {
                    level = 101;
                }
                else if (organizationalUnit is Company)
                {
                    level = 102;
                }
                else if (organizationalUnit is Department)
                {
                    level = 103;
                }
                else if (organizationalUnit is Section)
                {
                    level = 104;
                }

                spec = Specification.Equal("Level", level);

                var repository = RepositoryFactory.Instance.CreateRepository<IRole>();

                IList<IRole> roleList = repository.Find(Query.NewQuery.FindByCondition(spec));

                dtoList = RoleDTO.ConvertToDTOs(roleList);

            }

            return dtoList;
        }
    }
}
