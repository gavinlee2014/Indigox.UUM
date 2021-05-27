using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.HR.Model;
using Indigox.UUM.HR.Service;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.HR
{
    public class HREmployeeQuery : Indigox.Web.CQRS.GenericSingleQuery<HREmployee>
    {
        public string ID { get; set; }

        public override HREmployee Single()
        {
            HREmployee emplyee = RepositoryFactory.Instance.CreateRepository<HREmployee>().Get(this.ID);
            if (!string.IsNullOrEmpty(emplyee.ParentID))
            {
                HROrganizationalService service = new HROrganizationalService();
                IOrganizationalUnit org = service.GetMappedPrincipal(emplyee.ParentID);
                if (org != null)
                {
                    emplyee.ParentName = org.DisplayName;
                    HREmployeeService employeeService = new HREmployeeService();
                    string principalID = employeeService.GetMappedPrincipal(this.ID);

                    if (string.IsNullOrEmpty(principalID))
                    {
                        if(org.DisplayName.IndexOf('.')!= -1) {
                            string prefix = org.DisplayName.Substring(0, org.DisplayName.IndexOf('.'));
                            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
                            int orderNum = repository.GetTotalCount(Query.NewQuery.FindByCondition(
                                Specification.And(
                                    Specification.Equal("Organization", org),
                                    Specification.Equal("Enabled", true),
                                    Specification.Equal("Deleted", false)
                                    )
                                )
                            );
                            emplyee.DisplayName = prefix + (orderNum +1) + "." + emplyee.Name;
                        }                        
                    }
                }
            }
            return emplyee;
        }
    }
}
