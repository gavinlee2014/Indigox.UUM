using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.SysConfiguration
{
    public class SysListQuery : Indigox.Web.CQRS.GenericListQuery<SysListDTO>
    {
        public int ID { get; set; }

        public override IList<SysListDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var condition = new Query();
            condition.Specifications = Specification.NotEqual("ID", ID);
            var list = repository.Find(condition);
            IList<SysListDTO> sysList = new List<SysListDTO>();
            foreach (var v in list)
            {
                sysList.Add(new SysListDTO() { ID=v.ID,SysName=v.ClientName});
            }
            return sysList;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var condition = new Query();
            condition.Specifications = Specification.NotEqual("ID", ID);
            return repository.GetTotalCount(condition);
        }
    }
}
