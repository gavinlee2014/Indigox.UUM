using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interface.Server;

using C_Group = Indigox.Common.Membership.Group;

using DTO_Group = Indigox.UUM.Sync.Interface.Group;
using DTO_OrganizationalRole = Indigox.UUM.Sync.Interface.OrganizationalRole;
using DTO_OrganizationalUnit = Indigox.UUM.Sync.Interface.OrganizationalUnit;
using DTO_OrganizationalPerson = Indigox.UUM.Sync.Interface.OrganizationalPerson;

namespace Indigox.UUM.Application.Sync.WebServices.Export
{
    [WebService(Name = "ExportGroupService", Namespace = Consts.Namespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportGroupService : WebService, IExportGroupService
    {
        public DTO_Group[] GetAllGroups()
        {
            List<DTO_Group> list = new List<DTO_Group>();

            IRepository<IGroup> repository = RepositoryFactory.Instance.CreateRepository<IGroup>();
            IList<IGroup> models = repository.Find(Query.NewQuery);
            foreach (IGroup model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }

        public DTO_OrganizationalRole[] GetOrganizationalRoles(string groupID)
        {
            List<DTO_OrganizationalRole> list = new List<DTO_OrganizationalRole>();

            IRepository<IOrganizationalRole> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>();
            IList<IOrganizationalRole> models = repository.Find(Query.NewQuery);
            foreach (IOrganizationalRole model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }

        public DTO_OrganizationalUnit[] GetOrganizationalUnits(string groupID)
        {
            List<DTO_OrganizationalUnit> list = new List<DTO_OrganizationalUnit>();

            IGroup m_group = C_Group.GetGroupByID(groupID);
            foreach (IPrincipal m_member in m_group.Members)
            {
                if (m_member is IOrganizationalUnit)
                {
                    list.Add(DTOConvertor.ConvertToDto((IOrganizationalUnit)m_member));
                }
            }

            return list.ToArray();
        }

        public DTO_OrganizationalPerson[] GetUsers(string groupID)
        {
            List<DTO_OrganizationalPerson> list = new List<DTO_OrganizationalPerson>();

            IGroup m_group = C_Group.GetGroupByID(groupID);
            foreach (IPrincipal m_member in m_group.Members)
            {
                if (m_member is IUser)
                {
                    list.Add(DTOConvertor.ConvertToDto((IUser)m_member));
                }
            }

            return list.ToArray();
        }
    }
}
