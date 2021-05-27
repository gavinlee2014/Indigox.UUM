using System;
using System.Collections.Generic;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.HR.Model;
using NHibernate;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.UUM.Application.HR
{
    public class HREmployeeListQuery : Indigox.Web.CQRS.GenericListQuery<HREmployee>
    {
        public override IList<HREmployee> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            var query = new Query();
            query.StartFrom(FirstResult).LimitTo(FetchSize).OrderByAsc("Synchronized");
            var list = repository.Find(query);
            return list;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            var query = new Query();
            return repository.GetTotalCount(query);
        }
    }
}
