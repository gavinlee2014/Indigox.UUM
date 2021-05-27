using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Factory;
using Indigox.Common.DomainModels.Factory;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class DefaultOrganizationalPersonQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalPersonDTO>
    {
        public string OrganizationalUnitID { get; set; }

        private static readonly string DefaultOrganizationID = "OR1000000000";


        public override OrganizationalPersonDTO Single()
        {
            if (string.IsNullOrEmpty(this.OrganizationalUnitID))
            {
                this.OrganizationalUnitID = DefaultOrganizationID;
            }
            IOrganizationalUnit organizationalUnit = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(this.OrganizationalUnitID);

            IOrganizationalPerson item = new OrganizationalPersonFactory().Create();

            OrganizationalPersonDTO dto = OrganizationalPersonDTO.ConvertToDTO(item);
            dto.Organization = organizationalUnit.ID;
            dto.OrganizationName = organizationalUnit.Name;
            dto.OrganizationCode = organizationalUnit.DisplayName.IndexOf(".") == -1 ? "" : organizationalUnit.DisplayName.Substring(0, organizationalUnit.DisplayName.IndexOf("."));

            return dto;
        }
    }
}