using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.SysConfiguration
{
    public class SysConfigurationStateListQuery : Indigox.Web.CQRS.GenericListQuery<SysConfigurationDTO>
    {
        public override IList<SysConfigurationDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var list = repository.Find(
                new Query().FindByCondition(
                    Specification.Equal("Enabled", true)
                )
            );
                        
            IList<SysConfigurationDTO> dtos = new List<SysConfigurationDTO>();
            foreach (var config in list)
            {
                dtos.Add(SysConfigurationDTO.ConvertSysConfigurationToDTOWithState(config));
            }
            return dtos;
        }
    }
}
