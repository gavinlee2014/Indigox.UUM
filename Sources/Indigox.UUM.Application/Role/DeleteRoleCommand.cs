using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.Role
{
    public class DeleteRoleCommand : RoleCommand
    {
        public override void Execute()
        {
            IRepository<IRole> repository = RepositoryFactory.Instance.CreateRepository<IRole>();
            IRole item = repository.Get( this.ID );

            PrincipalService service = new PrincipalService();
            service.Delete( item );

            OperationLogService.LogOperation("删除角色： " + item.Name, item.GetDescription());
        }
    }
}