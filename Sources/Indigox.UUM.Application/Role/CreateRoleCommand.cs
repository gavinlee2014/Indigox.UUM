using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Factory;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.Role
{
    public class CreateRoleCommand : RoleCommand
    {
        public override void Execute()
        {
            DateTime now = DateTime.Now;
            IMutableRole item = new RoleFactory()
            {
                Name = this.Name,
                Email = this.Email,
                Description = this.Description,
                OrderNum = this.OrderNum,
                Level = (RoleLevel)this.Level,
            }.Create(this.ID);

            this.SetMembers(item);

            RepositoryFactory.Instance.CreateRepository<IRole>().Add(item);

            OperationLogService.LogOperation("创建角色： " +item.Name, item.GetDescription());

        }
    }
}