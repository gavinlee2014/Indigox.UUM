using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Web.CQRS.Interface;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Service;
using Indigox.Common.ADAccessor;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class UnlockOrganizationalPersonCommand : ICommand
    {
        public string ID { get; set; }

        public void Execute()
        {
            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IOrganizationalPerson item = repository.Get(this.ID);

            Accessor.UnlockUser(item.AccountName);
            OperationLogService.LogOperation("解锁用户： " + item.Name, "");
        }
    }
}
