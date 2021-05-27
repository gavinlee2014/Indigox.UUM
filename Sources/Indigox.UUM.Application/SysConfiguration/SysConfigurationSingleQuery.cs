using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Sync.Model;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.SysConfiguration
{
    public class SysConfigurationSingleQuery : Indigox.Web.CQRS.GenericSingleQuery<SysConfigurationDTO>
    {
        public int ID { get; set; }

        public override SysConfigurationDTO Single()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var condition = new Query();
            var config = repository.Get(ID);
            return SysConfigurationDTO.ConvertSysConfigurationToDTO( config);
        }
    }
}
