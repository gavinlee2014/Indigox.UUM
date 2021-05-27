using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.SyncTask
{
    public class SyncTaskSummaryQuery : Indigox.Web.CQRS.GenericSingleQuery<SyncTaskSummaryDTO>
    {
         public string Identity { get; set; }

         public override SyncTaskSummaryDTO Single()
         {
             if (string.IsNullOrEmpty(Identity))
             {
                 return null;
             }
             var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Tasks.SyncTask>();
             var syncTask = repository.Get(Identity);
             var summaryDTO = new SyncTaskSummaryDTO();
             summaryDTO.ID = syncTask.ID;
             summaryDTO.ErrorMessege = syncTask.ErrorMessage;
             return summaryDTO;
         }
    }
}
