using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Factory;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.Group
{
    public class CreateGroupCommand : GroupCommand
    {
        public override void Execute()
        {
            DateTime now = DateTime.Now;
            IMutableGroup item = new GroupFactory()
            {
                Name = this.Name,
                DisplayName = this.DisplayName,
                Email = this.Email,
                Description = this.Description,
                OrderNum = this.OrderNum
            }.Create(this.ID);

            this.SetMembers(item);

            RepositoryFactory.Instance.CreateRepository<IGroup>().Add(item);

            OperationLogService.LogOperation("创建群组： " + item.Name, item.GetDescription());

        }
    }
}