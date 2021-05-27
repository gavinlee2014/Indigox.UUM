using System;
using System.Collections.Generic;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.HR.Model;
using NHibernate;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.UUM.Application.HR
{
    public class HROrganizationalListQuery : Indigox.Web.CQRS.GenericListQuery<HROrganizational>
    {
        
        public override IList<HROrganizational> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            var query = new Query();
            query.StartFrom(FirstResult).LimitTo(FetchSize);
            var list = repository.Find(query);
            return list;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            var query = new Query();
            return repository.GetTotalCount(query);
        }

    }
}
