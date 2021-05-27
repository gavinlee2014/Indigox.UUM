using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalRole
{
    public class DeleteOrganizationalRoleCommand : OrganizationalRoleCommand
    {
        public override void Execute()
        {
            IRepository<IOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            IOrganizationalRole item = repository.Get(this.ID);

            PrincipalService service = new PrincipalService();
            service.Delete(item);
            OperationLogService.LogOperation("删除组织角色： " + item.Organization.Name + "_" + item.Name, item.GetDescription());
        }
    }
}