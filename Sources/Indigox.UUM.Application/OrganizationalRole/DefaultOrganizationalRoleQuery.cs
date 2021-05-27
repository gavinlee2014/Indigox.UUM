using System;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Factory;

namespace Indigox.UUM.Application.OrganizationalRole
{
    public class DefaultOrganizationalRoleQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalRoleDTO>
    {
        public override OrganizationalRoleDTO Single()
        {
            IOrganizationalRole item = new OrganizationalRoleFactory().Create();
            return OrganizationalRoleDTO.ConvertToDTO( item );
        }
    }
}