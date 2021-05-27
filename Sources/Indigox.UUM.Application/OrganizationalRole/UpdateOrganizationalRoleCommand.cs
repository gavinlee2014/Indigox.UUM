using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalRole
{
    public class UpdateOrganizationalRoleCommand : OrganizationalRoleCommand
    {
        public override void Execute()
        {
            IRepository<IMutableOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IMutableOrganizationalRole>();
            IMutableOrganizationalRole item = repository.Get( this.ID );

            this.FillPropery( item );
            
            if (!String.IsNullOrEmpty(this.Role))
            {
                item.Role = RepositoryFactory.Instance.CreateRepository<IMutableRole>().Get(this.Role);
            }
            else
            {
                item.Role = null;
            }
            

            OrganizationalRoleService service = new OrganizationalRoleService();
            service.Update( item );

            OperationLogService.LogOperation("编辑组织角色" + item.Name, item.GetDescription());
            //? call repos.Update(...) in service.Update(...)
            repository.Update( item );
        }
    }
}