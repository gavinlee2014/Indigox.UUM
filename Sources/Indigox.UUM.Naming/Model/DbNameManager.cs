using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership;

namespace Indigox.UUM.Naming.Model
{
    public class DbNameManager : INameManager
    {
        public bool Contains( string name )
        {
            IRepository db = RepositoryFactory.Instance.CreateRepository<User>();
            ISpecification spec = Specification.And(Specification.Equal("AccountName", name));
            Query condition = new Query();
            condition.FindByCondition(spec);
            object obj=db.First(condition);
            return obj!=null;
        }
    }
}