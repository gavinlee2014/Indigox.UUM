using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Naming.Model;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.UUM.Application.NameStrategy
{
    public class NameStrategyDescriptorSingleQuery : Indigox.Web.CQRS.GenericSingleQuery<NameStrategyDescriptor>
    {
        public int ID { get; set; }

        public override NameStrategyDescriptor Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<NameStrategyDescriptor>();
            var condition = new Query();
            var descriptor = repository.Get(ID);
            return descriptor;
        }
    }
}
