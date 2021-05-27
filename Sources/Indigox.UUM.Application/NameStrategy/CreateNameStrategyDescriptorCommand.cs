using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Naming.Model;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.UUM.Application.NameStrategy
{
    public class CreateNameStrategyDescriptorCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public float Priority { get; set; }
        public string ClassName { get; set; }
        public string Assembly { get; set; }
        public DateTime LastModifyTime { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<NameStrategyDescriptor>();
            var descriptor = new NameStrategyDescriptor();

            descriptor.Priority = Priority;
            descriptor.ClassName = ClassName;
            descriptor.Assembly = Assembly;
            descriptor.LastModifyTime = DateTime.Now;
            descriptor.Enabled = Enabled;
            descriptor.Description = Description;

            repository.Add(descriptor);
        }
    }
}
