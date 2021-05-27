using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class OrganizationalUnitDTO : ContainerDTO
    {
        public string Organization { get; set; }
        public string BusinessType { get; set; }

        public IList<SimplePrincipalDTO> Manager { get; set; }
        public IList<SimplePrincipalDTO> Director { get; set; }

        internal static OrganizationalUnitDTO ConvertToDTO(IOrganizationalUnit item)
        {
            OrganizationalUnitDTO dto = new OrganizationalUnitDTO();
            FillBaseProperties(item, dto);

            dto.Members = SimplePrincipalDTO.ConvertToDTOs(item.Members);
            dto.BusinessType = item.ExtendProperties.ContainsKey("BusinessType") ? item.ExtendProperties["BusinessType"] : String.Empty;

            if (item.Organization != null)
            {
                dto.Organization = item.Organization.ID;
            }

            if (item.Manager != null)
            {
                dto.Manager = new List<SimplePrincipalDTO>() { SimplePrincipalDTO.ConvertToDTO(item.Manager) };
            }

            IDepartment dept = item as IDepartment;
            if (dept != null)
            {
                if (dept.Director != null)
                {
                    dto.Director = new List<SimplePrincipalDTO>() { SimplePrincipalDTO.ConvertToDTO(dept.Director) };
                }
            }
            
            return dto;
        }

        internal static IList<OrganizationalUnitDTO> ConvertToDTOs(IList<IOrganizationalUnit> items)
        {
            List<OrganizationalUnitDTO> dtoList = new List<OrganizationalUnitDTO>();
            foreach (IOrganizationalUnit item in items)
            {
                OrganizationalUnitDTO dto = new OrganizationalUnitDTO();
                FillBaseProperties(item, dto);
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
