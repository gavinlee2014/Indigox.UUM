using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Naming.Model;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.UUM.Application.NameStrategy
{
    public class DeleteNameStrategyDescriptorCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<NameStrategyDescriptor>();
            var condition = new Query();
            var descriptor = repository.Get(ID);

            repository.Remove(descriptor);
        }
    }
}
