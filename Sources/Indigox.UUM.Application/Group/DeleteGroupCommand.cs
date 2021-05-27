using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.Group
{
    public class DeleteGroupCommand : GroupCommand
    {
        public override void Execute()
        {
            IRepository<IGroup> repository = RepositoryFactory.Instance.CreateRepository<IGroup>();
            IGroup item = repository.Get( this.ID );

            PrincipalService service = new PrincipalService();
            service.Delete( item );

            OperationLogService.LogOperation("删除群组： " + item.Name, item.GetDescription());
        }
    }
}