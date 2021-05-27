using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.Application.DTO
{
    public class AddressBookDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }

        public string AccountName { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string OtherContact { get; set; }
        public string Organization { get; set; }
        public string OrganizationName { get; set; }
        public string Profile { get; set; }

    }
}
