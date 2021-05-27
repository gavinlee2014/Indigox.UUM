using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.SysConfiguration
{
    public class SysConfigurationListQuery : Indigox.Web.CQRS.GenericListQuery<SysConfigurationDTO>
    {
        public override IList<SysConfigurationDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var condition = new Query();
            condition.StartFrom(FirstResult).LimitTo(FetchSize);
            var list = repository.Find(condition);
            IList<SysConfigurationDTO> dtos = new List<SysConfigurationDTO>();
            foreach (var config in list)
            {
                dtos.Add(SysConfigurationDTO.ConvertSysConfigurationToDTO(config));
            }
            return dtos;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var condition = new Query();
            return repository.GetTotalCount(condition);
        }
    }
}
