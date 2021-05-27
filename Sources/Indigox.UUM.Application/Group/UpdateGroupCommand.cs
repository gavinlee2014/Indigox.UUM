using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.Group
{
    public class UpdateGroupCommand : GroupCommand
    {
        public override void Execute()
        {
            IRepository<IGroup> repository = RepositoryFactory.Instance.CreateRepository<IGroup>();
            IGroup item = repository.Get( this.ID );
            this.FillPropery( item );

            GroupService service = new GroupService();
            service.Update( item );

            OperationLogService.LogOperation("编辑群组" + item.Name, item.GetDescription());
            //? call repos.Update(...) in service.Update(...)
            repository.Update( item );
        }
    }
}