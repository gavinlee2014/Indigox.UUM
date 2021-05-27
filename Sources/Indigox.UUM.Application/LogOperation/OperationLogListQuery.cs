using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.NHibernateImpl.Model;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.LogOperation
{
    public class OperationLogListQuery : Indigox.Web.CQRS.GenericListQuery<OperationLogDTO>
    {
        public string SearchContent { get; set; }
        public string LogTimeBegin { get; set; }
        public string LogTimeEnd { get; set; }

        public override IList<OperationLogDTO> List()
        {
            Query condition = InitQuery();
            condition.StartFrom(FirstResult).LimitTo(FetchSize).OrderByDesc("OperationTime");
            IList<OperationLog> list = RepositoryFactory.Instance.CreateRepository<OperationLog>().Find(condition);
            return OperationLogDTO.ConvertToDTOs(list);
        }

        public override int Size()
        {
            Query query = InitQuery();
            return RepositoryFactory.Instance.CreateRepository<OperationLog>().GetTotalCount(query);
        }

        public Query InitQuery()
        {
            Query query = Query.NewQuery;
            List<ISpecification> conditions = new List<ISpecification>();
            DateTime start, end;

            if (!string.IsNullOrEmpty(LogTimeBegin) && DateTime.TryParse(LogTimeBegin, out start))
            {
                conditions.Add(Specification.GreaterOrEqual("OperationTime", start));
            }
            if (!string.IsNullOrEmpty(LogTimeEnd) && DateTime.TryParse(LogTimeEnd, out end))
            {
                conditions.Add(Specification.LessOrEqual("OperationTime", end));
            }
            if (!string.IsNullOrEmpty(SearchContent))
            {
                conditions.Add(Specification.Or(Specification.Like("Operator", "%" + SearchContent + "%"), Specification.Like("Operation", "%" + SearchContent + "%")));
            }
            query.Specifications = Specification.And(conditions.ToArray());
            return query;
        }

    }
}
