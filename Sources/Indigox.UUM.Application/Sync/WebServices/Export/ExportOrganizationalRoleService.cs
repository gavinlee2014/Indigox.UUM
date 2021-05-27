using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interface.Server;

using C_OrganizationalRole = Indigox.Common.Membership.OrganizationalRole;
using DTO_OrganizationalPerson = Indigox.UUM.Sync.Interface.OrganizationalPerson;

namespace Indigox.UUM.Application.Sync.WebServices.Export
{
    [WebService(Name = "ExportOrganizationalRoleService", Namespace = Consts.Namespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportOrganizationalRoleService : WebService, IExportOrganizationalRoleService
    {
        public DTO_OrganizationalPerson[] GetUsers(string organizationalRoleID)
        {
            List<DTO_OrganizationalPerson> list = new List<DTO_OrganizationalPerson>();

            IOrganizationalRole m_organizationalRole = C_OrganizationalRole.GetOrganizationalRoleByID(organizationalRoleID);
            foreach (IPrincipal m_member in m_organizationalRole.Members)
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
