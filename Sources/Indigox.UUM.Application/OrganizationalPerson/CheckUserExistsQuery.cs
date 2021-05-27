using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.DomainModels.Interface.Specifications;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class CheckUserExistsQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string PrincipalID { get; set; }

        public override IList<OrganizationalPersonDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IList<IOrganizationalPerson> users = repository.Find(
                Query.NewQuery.FindByCondition(
                        Specification.And(
                            Specification.NotEqual("ID", PrincipalID),
                            Specification.Or(
                                Specification.Equal("AccountName", this.AccountName),
                                Specification.Equal("Email", this.Email)
                            )
                        )
                    )
                );

            IList<OrganizationalPersonDTO> list = OrganizationalPersonDTO.ConvertToDTOs(users);

            return list;
        }
    }
}
