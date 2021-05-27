using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class PrincipalDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MailDatabase { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public IDictionary<string, string> ExtendProperties { get; set; }

        internal static void FillBaseProperties(IPrincipal item, PrincipalDTO dto)
        {
            dto.ID = item.ID;
            dto.Name = item.Name;
            dto.FullName = item.FullName;
            dto.Email = item.Email;
            dto.Description = item.Description;
            dto.OrderNum = item.OrderNum;
            dto.IsEnabled = item.Enabled;
            dto.IsDeleted = item.Deleted;
            dto.Type = item.GetType().Name;
            dto.DisplayName = item.DisplayName;
            dto.MailDatabase = item.MailDatabase;
            dto.ExtendProperties = item.ExtendProperties;
        }

        internal static PrincipalDTO ConvertToDTO(IPrincipal item)
        {
            PrincipalDTO dto = new PrincipalDTO();
            FillBaseProperties(item, dto);
            return dto;
        }

        internal static IList<PrincipalDTO> ConvertToDTOs(IList<IPrincipal> items)
        {
            List<PrincipalDTO> dtoList = new List<PrincipalDTO>();
            foreach (IPrincipal item in items)
            {
                dtoList.Add(PrincipalDTO.ConvertToDTO(item));
            }
            return dtoList;
        }
    }
}
