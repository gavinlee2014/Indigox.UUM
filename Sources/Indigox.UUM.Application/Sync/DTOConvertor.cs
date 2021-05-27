using Indigox.Common.Membership.Interfaces;

using DTO_Group = Indigox.UUM.Sync.Interface.Group;
using DTO_OrganizationalRole = Indigox.UUM.Sync.Interface.OrganizationalRole;
using DTO_OrganizationalUnit = Indigox.UUM.Sync.Interface.OrganizationalUnit;
using DTO_OrganizationalPerson = Indigox.UUM.Sync.Interface.OrganizationalPerson;
using DTO_Role = Indigox.UUM.Sync.Interface.Role;

namespace Indigox.UUM.Application.Sync
{
    public static class DTOConvertor
    {
        public static DTO_Group ConvertToDto( IGroup model )
        {
            DTO_Group dto = new DTO_Group();
            dto.ID = model.ID;
            dto.Name = model.Name;
            dto.FullName = model.FullName;
            dto.Email = model.Email;
            dto.OrderNum = model.OrderNum;
            dto.Description = model.Description;
            dto.DisplayName = model.DisplayName;

            return dto;
        }

        public static DTO_OrganizationalRole ConvertToDto( IOrganizationalRole model )
        {
            DTO_OrganizationalRole dto = new DTO_OrganizationalRole();
            dto.ID = model.ID;
            dto.Name = model.Name;
            dto.FullName = model.FullName;
            if ( model.Organization != null )
            {
                dto.OrganizationID = model.Organization.ID;
            }
            dto.Email = model.Email;
            dto.Description = model.Description;
            dto.OrderNum = model.OrderNum;
            dto.DisplayName = model.DisplayName;

            return dto;
        }

        public static DTO_OrganizationalUnit ConvertToDto( IOrganizationalUnit model )
        {
            DTO_OrganizationalUnit dto = new DTO_OrganizationalUnit();
            dto.ID = model.ID;
            dto.Name = model.Name;
            dto.FullName = model.FullName;
            if ( model.Organization != null )
            {
                dto.OrganizationID = model.Organization.ID;
            }
            dto.Email = model.Email;
            dto.Description = model.Description;
            dto.OrderNum = model.OrderNum;
            dto.DisplayName = model.DisplayName;
            if (model is ICorporation)
            {
                dto.OrganizationalType = "Corporation";
            }
            else if(model is ICompany )
            {
                dto.OrganizationalType = "Company";
            }
            else if(model is IDepartment)
            {
                dto.OrganizationalType = "Department";
            }
            else if (model is ISection)
            {
                dto.OrganizationalType = "Section";
            }

            return dto;
        }

        public static DTO_OrganizationalPerson ConvertToDto( IUser model )
        {
            IOrganizationalPerson organizationalPerson = model as IOrganizationalPerson;

            DTO_OrganizationalPerson dto = new DTO_OrganizationalPerson();
            dto.ID = model.ID;
            dto.Name = model.Name;
            dto.FullName = model.FullName;
            dto.Account = model.AccountName;
            if ( organizationalPerson != null && organizationalPerson.Organization != null )
            {
                dto.OrganizationID = organizationalPerson.Organization.ID;
            }
            dto.Email = model.Email;
            dto.Description = model.Description;
            dto.OrderNum = model.OrderNum;
            dto.Fax = model.Fax;
            dto.Mobile = model.Mobile;
            dto.Telephone = model.Telephone;
            dto.Title = model.Title;
            dto.DisplayName = model.DisplayName;
            dto.Profile = model.Profile;
            dto.Enabled = model.Enabled;

            return dto;
        }

        public static DTO_OrganizationalPerson ConvertToDtoWithIdCard(IUser model)
        {
            IOrganizationalPerson organizationalPerson = model as IOrganizationalPerson;

            DTO_OrganizationalPerson dto = new DTO_OrganizationalPerson();
            dto.ID = model.ID;
            dto.Name = model.Name;
            dto.FullName = model.FullName;
            dto.Account = model.AccountName;
            if (organizationalPerson != null && organizationalPerson.Organization != null)
            {
                dto.OrganizationID = organizationalPerson.Organization.ID;
            }
            dto.Email = model.Email;
            dto.Description = model.Description;
            dto.OrderNum = model.OrderNum;
            dto.Fax = model.Fax;
            dto.Mobile = model.Mobile;
            dto.IdCard = model.IdCard;
            dto.Telephone = model.Telephone;
            dto.Title = model.Title;
            dto.DisplayName = model.DisplayName;
            dto.Profile = model.Profile;
            dto.Enabled = model.Enabled;

            return dto;
        }

        public static DTO_Role ConvertToDto( IRole model )
        {
            DTO_Role dto = new DTO_Role();
            dto.ID = model.ID;
            dto.Name = model.Name;
            dto.FullName = model.FullName;
            dto.Email = model.Email;
            dto.Description = model.Description;
            dto.OrderNum = model.OrderNum;
            dto.DisplayName = model.DisplayName;

            return dto;
        }
    }
}