using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class GroupDTO : ContainerDTO
    {

        internal static GroupDTO ConvertToDTO(IGroup item)
        {
            GroupDTO dto = new GroupDTO();
            FillBaseProperties(item, dto);
            dto.Members = SimplePrincipalDTO.ConvertToDTOs(item.Members);
            return dto;
        }

        internal static IList<GroupDTO> ConvertToDTOs(IList<IGroup> items)
        {
            List<GroupDTO> dtoList = new List<GroupDTO>();
            foreach (IGroup item in items)
            {
                GroupDTO dto = new GroupDTO();
                FillBaseProperties(item, dto);
                dtoList.Add(dto);
            }
            return dtoList;
        }

    }
}
