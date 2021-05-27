using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class DeleteOrganizationalPersonCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public string ID { get; set; }

        public void Execute()
        {
            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IOrganizationalPerson item = repository.Get( this.ID );

            PrincipalService service = new PrincipalService();
            service.Delete( item );

            OperationLogService.LogOperation("删除用户： "+ item.Name, "");
        }
    }
}