using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class OrganizationalRoleDTO : ContainerDTO
    {
        public OrganizationalRoleDTO()
        {
        }

        public SimplePrincipalDTO Role { get; set; }
        public string Organization { get; set; }
        public string Level { get; set; }
        public string RoleLevel { get; set; }

        internal static OrganizationalRoleDTO ConvertToDTO(IOrganizationalRole item)
        {
            OrganizationalRoleDTO dto = new OrganizationalRoleDTO();
            FillBaseProperties(item, dto);
            dto.Members = SimplePrincipalDTO.ConvertToDTOs(item.Members);
            dto.Level = item.ExtendProperties.ContainsKey("Level") ? item.ExtendProperties["Level"] : String.Empty;
            dto.RoleLevel = item.ExtendProperties.ContainsKey("RoleLevel") ? item.ExtendProperties["RoleLevel"] : String.Empty;

            if (item.Role != null)
            {
                dto.Role = SimplePrincipalDTO.ConvertToDTO(item.Role);
            }

            if (item.Organization != null)
            {
                dto.Organization = item.Organization.ID;
            }
            return dto;
        }

        internal static IList<OrganizationalRoleDTO> ConvertToDTOs(IList<OrganizationalRoleDTO> items)
        {
            List<OrganizationalRoleDTO> dtoList = new List<OrganizationalRoleDTO>();
            foreach (IOrganizationalRole item in items)
            {
                OrganizationalRoleDTO dto = new OrganizationalRoleDTO();
                FillBaseProperties(item, dto);
                if (item.Role != null)
                {
                    dto.Role = SimplePrincipalDTO.ConvertToDTO(item.Role);
                }
                if (item.Organization != null)
                {
                    dto.Organization = item.Organization.ID;
                }
                dtoList.Add(dto);
            }
            return dtoList;
        }

    }
}
