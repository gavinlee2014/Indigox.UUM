using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.Role
{
    public class UpdateRoleCommand : RoleCommand
    {
        public override void Execute()
        {
            IRepository<IRole> repository = RepositoryFactory.Instance.CreateRepository<IRole>();
            IRole item = repository.Get( this.ID );
            this.FillPropery( item );

            RoleService service = new RoleService();
            service.Update( item );

            OperationLogService.LogOperation("编辑角色" + item.Name, item.GetDescription());
            //? call repos.Update(...) in service.Update(...)
            repository.Update( item );
        }
    }
}