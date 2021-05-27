using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.Utils;

namespace Indigox.UUM.Application.SysConfiguration
{
    public class DeleteSysConfigurationCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }

        public void Execute()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var descriptor = repository.Get( ID );

            repository.Remove( descriptor );
        }
    }
}