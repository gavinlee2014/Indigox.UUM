using System;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Factory;

namespace Indigox.UUM.Application.Role
{
    public class DefaultRoleQuery : Indigox.Web.CQRS.GenericSingleQuery<RoleDTO>
    {
        public override RoleDTO Single()
        {
            IRole item = new RoleFactory().Create();
            return RoleDTO.ConvertToDTO( item );
        }
    }
}