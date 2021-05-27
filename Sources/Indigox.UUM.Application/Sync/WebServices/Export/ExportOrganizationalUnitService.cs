using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interface.Server;

using C_Corporation = Indigox.Common.Membership.Corporation;
using C_Group = Indigox.Common.Membership.Group;
using C_OrganizationalRole = Indigox.Common.Membership.OrganizationalRole;
using C_OrganizationalUnit = Indigox.Common.Membership.OrganizationalUnit;
using C_Role = Indigox.Common.Membership.Role;

using DTO_Group = Indigox.UUM.Sync.Interface.Group;
using DTO_OrganizationalRole = Indigox.UUM.Sync.Interface.OrganizationalRole;
using DTO_OrganizationalUnit = Indigox.UUM.Sync.Interface.OrganizationalUnit;
using DTO_OrganizationalPerson = Indigox.UUM.Sync.Interface.OrganizationalPerson;
using DTO_Role = Indigox.UUM.Sync.Interface.Role;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.Sync.WebServices.Export
{
    [WebService(Name = "ExportOrganizationalUnitService", Namespace = Consts.Namespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportOrganizationalUnitService : WebService, IExportOrganizationalUnitService
    {


        public DTO_OrganizationalUnit GetCorporation()
        {
            IOrganizationalUnit m_coporation = C_Corporation.GetCorporation();
            return DTOConvertor.ConvertToDto(m_coporation);
        }

        public DTO_OrganizationalUnit[] GetChildOrganizations(string organizationalID)
        {
            List<DTO_OrganizationalUnit> list = new List<DTO_OrganizationalUnit>();

            IOrganizationalUnit m_parentOrganization = C_OrganizationalUnit.GetOrganizationByID(organizationalID);
            foreach (IPrincipal m_member in m_parentOrganization.Members)
            {
                if (m_member is IOrganizationalUnit)
                {
                    list.Add(DTOConvertor.ConvertToDto((IOrganizationalUnit)m_member));
                }
            }

            return list.ToArray();
        }

        public DTO_OrganizationalPerson[] GetUsers(string organizationalID)
        {
            List<DTO_OrganizationalPerson> list = new List<DTO_OrganizationalPerson>();

            IOrganizationalUnit m_parentOrganization = C_OrganizationalUnit.GetOrganizationByID(organizationalID);
            foreach (IPrincipal m_member in m_parentOrganization.Members)
            {
                if (m_member is IUser)
                {
                    list.Add(DTOConvertor.ConvertToDto((IUser)m_member));
                }
            }

            return list.ToArray();
        }

        public DTO_OrganizationalRole[] GetOrganizationalRoles(string organizationID)
        {
            List<DTO_OrganizationalRole> list = new List<DTO_OrganizationalRole>();

            IOrganizationalUnit m_parentOrganization = C_OrganizationalUnit.GetOrganizationByID(organizationID);
            foreach (IPrincipal m_member in m_parentOrganization.Members)
            {
                if (m_member is IOrganizationalRole)
                {
                    list.Add(DTOConvertor.ConvertToDto((IOrganizationalRole)m_member));
                }
            }

            return list.ToArray();
        }

        public DTO_OrganizationalUnit[] GetAllOrganizations()
        {
            List<DTO_OrganizationalUnit> list = new List<DTO_OrganizationalUnit>();

            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IList<IOrganizationalUnit> models = repository.Find(
                Query.NewQuery.FindByCondition(
                    Specification.Equal("Deleted", false)
                )
            );

            foreach (IOrganizationalUnit model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }
    }
}
