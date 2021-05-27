using System;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Factory;

namespace Indigox.UUM.Application.Group
{
    public class DefaultGroupQuery : Indigox.Web.CQRS.GenericSingleQuery<GroupDTO>
    {
        public override GroupDTO Single()
        {
            IGroup item = new GroupFactory().Create();
            return GroupDTO.ConvertToDTO( item );
        }
    }
}