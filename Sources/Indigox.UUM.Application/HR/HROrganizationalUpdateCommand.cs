using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Web.CQRS.Interface;
using Indigox.UUM.HR.Model;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;

namespace Indigox.UUM.Application.HR
{
    public class HROrganizationalUpdateCommand : ICommand
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public void Execute()
        {
            IRepository<HROrganizational> repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            HROrganizational item = repository.Get(this.ID);
            item.Email = this.Email;
            item.DisplayName = this.DisplayName;

            repository.Update(item);
        }
    }
}
