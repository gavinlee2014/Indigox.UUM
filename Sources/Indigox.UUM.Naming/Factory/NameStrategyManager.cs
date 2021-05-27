using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.UUM.Naming.Model;

namespace Indigox.UUM.Naming.Factory
{
    public class NameStrategyManager
    {
        private static NameStrategyManager instance = new NameStrategyManager();

        public static NameStrategyManager Instance
        {
            get
            {
                return instance;
            }
        }

        private NameStrategyManager()
        {
        }

        /// <summary>
        /// only used for tests.
        /// </summary>
        public NameStrategyManager(INameStrategy[] nameStrategies)
        {
            this.nameStrategies = new List<INameStrategy>(nameStrategies);
        }

        private List<INameStrategy> nameStrategies;

        private IList<NameStrategyDescriptor> GetNameStrategyDescriptors()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<NameStrategyDescriptor>();
            var condition = new Query();
            ISpecification spec = Specification.And(Specification.Equal("Enabled", true));
            condition.FindByCondition(spec).OrderByAsc("Priority");
            var list = repository.Find(condition);
            return list;
        }

        public List<INameStrategy> GetNameStrategies()
        {
            if (nameStrategies == null)
            {
                nameStrategies = new List<INameStrategy>();
                IList<NameStrategyDescriptor> nameStrategyDescriptors = GetNameStrategyDescriptors();
                foreach (NameStrategyDescriptor ns in nameStrategyDescriptors)
                {
                    nameStrategies.Add(ns.ConvertToNamestrategy());
                }
            }
            return nameStrategies;
        }
    }
}
