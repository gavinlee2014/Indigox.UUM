using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.DTO
{
    public class OrganizationalPersonDTO : PrincipalDTO
    {
        public OrganizationalPersonDTO()
        {
            this.MemberOfGroups = new List<SimplePrincipalDTO>();
            this.MemberOfOrganizationalRoles = new List<SimplePrincipalDTO>();
        }

        public string AccountName { get; set; }
        public string IdCard { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string OtherContact { get; set; }
        public string Organization { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationCode { get; set; }
        public string Level { get; set; }
        public string StaffNo { get; set; }
        public string StaffType { get; set; }
        public string Gender { get; set; }
        public string ContractCompanyTexNo { get; set; }
        public string ContractCompanyName { get; set; }
        public string LastJoinDate { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public string NeedClockOn { get; set; }

        public IList<SimplePrincipalDTO> MemberOfOrganizationalRoles { get; set; }
        public IList<SimplePrincipalDTO> MemberOfGroups { get; set; }
        public IList<ProfileDTO> Profile { get; set; }

        internal static OrganizationalPersonDTO ConvertToDTO( IOrganizationalPerson item )
        {
            Log.Error("查询用户 ： " + item.ID + " | IdCard : " + item.IdCard + " | AccountName : " + item.AccountName);
            OrganizationalPersonDTO dto = new OrganizationalPersonDTO();
            FillBaseProperties( item, dto );
            dto.AccountName = item.AccountName;
            dto.Title = item.Title;
            dto.IdCard = item.IdCard;
            dto.Mobile = item.Mobile;
            dto.Telephone = item.Telephone;
            dto.Fax = item.Fax;
            dto.OtherContact = item.OtherContact;
            dto.DisplayName = item.DisplayName;
            dto.Profile =ProfileDTO.ConvertToDTOs(item);
            dto.MailDatabase = item.MailDatabase;
            dto.Level = item.Level == 0 ? "" : item.Level.ToString();
            dto.StaffNo = item.ExtendProperties.ContainsKey("StaffNo") ? item.ExtendProperties["StaffNo"] : String.Empty;
            dto.StaffType = item.ExtendProperties.ContainsKey("StaffType") ? item.ExtendProperties["StaffType"] : String.Empty;
            dto.Gender = item.ExtendProperties.ContainsKey("Gender") ? item.ExtendProperties["Gender"] : String.Empty;
            dto.NeedClockOn = item.ExtendProperties.ContainsKey("NeedClockOn") ? item.ExtendProperties["NeedClockOn"] : String.Empty;
            dto.LastJoinDate = item.ExtendProperties.ContainsKey("LastJoinDate") ? item.ExtendProperties["LastJoinDate"] : String.Empty;
            dto.ContractStartDate = item.ExtendProperties.ContainsKey("ContractStartDate") ? item.ExtendProperties["ContractStartDate"] : String.Empty;
            dto.ContractEndDate = item.ExtendProperties.ContainsKey("ContractEndDate") ? item.ExtendProperties["ContractEndDate"] : String.Empty;
            dto.ContractCompanyTexNo = item.ExtendProperties.ContainsKey("ContractCompanyTexNo") ? item.ExtendProperties["ContractCompanyTexNo"] : String.Empty;
            dto.ContractCompanyName = item.ExtendProperties.ContainsKey("ContractCompanyName") ? item.ExtendProperties["ContractCompanyName"] : String.Empty;


            if ( item.Organization != null )
            {
                dto.Organization = item.Organization.ID;
                dto.OrganizationName = item.Organization.Name;
                dto.OrganizationCode = item.Organization.DisplayName.IndexOf(".") == -1 ? "" : item.Organization.DisplayName.Substring(0, item.Organization.DisplayName.IndexOf("."));
            }
            if (item.MemberOf != null)
            {
                foreach (IContainer container in item.MemberOf)
                {
                    SimplePrincipalDTO containerDTO = SimplePrincipalDTO.ConvertToDTO(container);

                    IOrganizationalRole role = container as IOrganizationalRole;
                    if (role != null)
                    {
                        dto.MemberOfOrganizationalRoles.Add(containerDTO);
                    }

                    IGroup group = container as IGroup;
                    if (group != null)
                    {
                        dto.MemberOfGroups.Add(containerDTO);
                    }

                }
            }
            return dto;
        }

        internal static IList<OrganizationalPersonDTO> ConvertToDTOs( IList<IOrganizationalPerson> items )
        {
            List<OrganizationalPersonDTO> dtoList = new List<OrganizationalPersonDTO>();
            foreach ( IOrganizationalPerson item in items )
            {
                dtoList.Add( OrganizationalPersonDTO.ConvertToDTO( item ) );
            }
            return dtoList;
        }
    }
}