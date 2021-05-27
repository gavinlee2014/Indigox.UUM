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
    public class HREmployeeUpdateCommand : ICommand
    {
        public string ID { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string MailDatabase { get; set; }

        public void Execute()
        {
            IRepository<HREmployee> repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            HREmployee item = repository.Get(this.ID);
            item.AccountName = this.AccountName;
            item.DisplayName = this.DisplayName;
            item.Email = this.Email;
            item.MailDatabase = this.MailDatabase;

            repository.Update(item);
        }
    }
}
