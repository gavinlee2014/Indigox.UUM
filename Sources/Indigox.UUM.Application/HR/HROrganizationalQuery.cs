using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.HR.Model;
using Indigox.UUM.HR.Service;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.HR
{
    public class HROrganizationalQuery : Indigox.Web.CQRS.GenericSingleQuery<HROrganizational>
    {
        public string ID { get; set; }

        public override HROrganizational Single()
        {
            HROrganizational organizational = RepositoryFactory.Instance.CreateRepository<HROrganizational>().Get(this.ID);
            if (!string.IsNullOrEmpty(organizational.ParentID))
            {
                HROrganizationalService service = new HROrganizationalService();
                IOrganizationalUnit org = service.GetMappedPrincipal(organizational.ParentID);
                if (org != null)
                {
                    organizational.ParentName = org.DisplayName;
                }
            }
            return organizational;
        }
    }
}
