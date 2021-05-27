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
    public class NameStrategyDescriptorListQuery : Indigox.Web.CQRS.GenericListQuery<NameStrategyDescriptor>
    {
        public override IList<NameStrategyDescriptor> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<NameStrategyDescriptor>();
            var condition = new Query();
            condition.OrderByDesc("Enabled");
            condition.OrderByAsc("Priority");
            condition.StartFrom(FirstResult).LimitTo(FetchSize);
            var list = repository.Find(condition);
            return list;
        }
        
        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<NameStrategyDescriptor>();
            var condition = new Query();
            return repository.GetTotalCount(condition);
        }

       
    }
    
}
