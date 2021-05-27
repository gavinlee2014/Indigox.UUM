using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class DisableOrganizationalPersonCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public string ID { get; set; }

        public void Execute()
        {
            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IOrganizationalPerson item = repository.Get( this.ID );

            PrincipalService service = new PrincipalService();
            service.Disable( item );
            OperationLogService.LogOperation("禁用用户： " + item.Name, "");
        }
    }
}