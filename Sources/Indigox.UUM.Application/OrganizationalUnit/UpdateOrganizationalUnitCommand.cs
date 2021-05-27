using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public class UpdateOrganizationalUnitCommand : OrganizationalUnitCommand
    {
        public override void Execute()
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IOrganizationalUnit item = repository.Get( this.ID );
            this.FillPropery( item );

            OrganizationalUnitService service = new OrganizationalUnitService();
            service.Update( item );
            
            OperationLogService.LogOperation("编辑部门： "+item.Organization.Name+"_"+item.Name,item.GetDescription());
            //? call repos.Update(...) in service.Update(...)
            repository.Update( item );


        }
    }
}