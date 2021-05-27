using System;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Factory;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public class DefaultOrganizationalUnitQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalUnitDTO>
    {
        public override OrganizationalUnitDTO Single()
        {
            IOrganizationalUnit item = new OrganizationalUnitFactory().Create();
            return OrganizationalUnitDTO.ConvertToDTO( item );
        }
    }
}