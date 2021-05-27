using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class SimplePrincipalDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }

        internal static SimplePrincipalDTO ConvertToDTO(IPrincipal item)
        {
            SimplePrincipalDTO dto = new SimplePrincipalDTO();
            dto.UserID = item.ID;
            dto.UserName = item.Name;
            return dto;
        }

        internal static IList<SimplePrincipalDTO> ConvertToDTOs(IList<IPrincipal> items)
        {
            List<SimplePrincipalDTO> dtoList = new List<SimplePrincipalDTO>();
            foreach (IPrincipal item in items)
            {
                dtoList.Add(SimplePrincipalDTO.ConvertToDTO(item));
            }
            return dtoList;
        }

    }
}
