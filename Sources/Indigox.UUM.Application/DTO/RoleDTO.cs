using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class RoleDTO : ContainerDTO
    {
        public int Level { get; set; }

        internal static RoleDTO ConvertToDTO(IRole item)
        {
            RoleDTO dto = new RoleDTO();
            FillBaseProperties(item, dto);
            dto.Level = (int)item.Level;
            dto.Members = SimplePrincipalDTO.ConvertToDTOs(item.Members);
            return dto;
        }

        internal static IList<RoleDTO> ConvertToDTOs(IList<IRole> items)
        {
            List<RoleDTO> dtoList = new List<RoleDTO>();
            foreach (IRole item in items)
            {
                RoleDTO dto = new RoleDTO();
                FillBaseProperties(item, dto);
                dto.Level = (int)item.Level;
                dtoList.Add(dto);
            }
            return dtoList;
        }

    }
}
