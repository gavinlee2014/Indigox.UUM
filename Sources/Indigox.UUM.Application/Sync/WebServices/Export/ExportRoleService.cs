using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interface.Server;

using C_Role = Indigox.Common.Membership.Role;

using DTO_OrganizationalRole = Indigox.UUM.Sync.Interface.OrganizationalRole;
using DTO_Role = Indigox.UUM.Sync.Interface.Role;

namespace Indigox.UUM.Application.Sync.WebServices.Export
{
    [WebService(Name = "ExportRoleService", Namespace = Consts.Namespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportRoleService : WebService, IExportRoleService
    {
        public DTO_Role[] GetAllRoles()
        {
            List<DTO_Role> list = new List<DTO_Role>();

            IRepository<IRole> repository = RepositoryFactory.Instance.CreateRepository<IRole>();
            IList<IRole> models = repository.Find(Query.NewQuery);
            foreach (IRole model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }

        public DTO_OrganizationalRole[] GetOrganizationalRoles(string roleID)
        {
            List<DTO_OrganizationalRole> list = new List<DTO_OrganizationalRole>();

            IRole m_role = C_Role.GetRoleByID(roleID);
            foreach (IPrincipal m_member in m_role.Members)
            {
                if (m_member is IOrganizationalRole)
                {
                    list.Add(DTOConvertor.ConvertToDto((IOrganizationalRole)m_member));
                }
            }

            return list.ToArray();
        }
    }
}
