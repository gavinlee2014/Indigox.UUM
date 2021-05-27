using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Specifications;
using Indigox.UUM.Sync.Interfaces;
using Indigox.Common.DomainModels.Interface.Specifications;

namespace Indigox.UUM.Application.SyncTask
{
    public class SyncTaskListQuery : Indigox.Web.CQRS.GenericListQuery<SyncTaskDTO>
    {
        public string Tag { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public string CreateTimeBegin { get; set; }
        public string CreateTimeEnd { get; set; }

        public override IList<SyncTaskDTO> List()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Tasks.SyncTask>();
            var query = InitQuery();
            query.OrderByDesc("ID");
            query.StartFrom(FirstResult).LimitTo(FetchSize);
            var list = repository.Find(query);

            IList<SyncTaskDTO> dtoList = new List<SyncTaskDTO>();
            foreach (var v in list)
            {
                dtoList.Add(SyncTaskDTO.ConvertToDTO(v));
            }
            return dtoList;
        }

        public override int Size()
        {
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Tasks.SyncTask>();
            var query = InitQuery();
            return repository.GetTotalCount(query);
        }

        private Query InitQuery()
        {
            var query = new Query();

            ISpecification spec = Specification.NotEqual("Tag", null);

            if (!string.IsNullOrEmpty(Tag))
            {
                spec = Specification.And(spec, Specification.Like("Tag", "%" + Tag + "%"));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                spec = Specification.And(spec, Specification.Like("Description", "%" + Description + "%"));
            }
            if (State >= 0)
            {
                spec = Specification.And(spec, Specification.Equal("State", ConvertToState(State)));
            }
            if (!string.IsNullOrEmpty(CreateTimeBegin))
            {
                spec = Specification.And(spec, Specification.GreaterOrEqual("CreateTime", DateTime.Parse(CreateTimeBegin)));
            }
            if (!string.IsNullOrEmpty(CreateTimeEnd))
            {
                spec = Specification.And(spec, Specification.LessOrEqual("CreateTime", DateTime.Parse(CreateTimeEnd)));
            }

            query.Specifications = spec;

            return query;
        }

        private SyncTaskState ConvertToState(int s)
        {
            switch(s){
                case 0:
                    return SyncTaskState.Initiated;
                case 1:
                    return SyncTaskState.Successed;
                case 2:
                    return SyncTaskState.Failed;
            }
            return default(SyncTaskState);
        }
    }
}
