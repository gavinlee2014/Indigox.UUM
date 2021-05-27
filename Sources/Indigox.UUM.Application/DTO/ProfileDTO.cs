using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.DTO
{
    public class ProfileDTO
    {
        public string fileName { get; set; }
        public string fileUrl { get; set; }
        public int size { get; set; }

        public string GetFileUrl()
        {
            int index = fileUrl.IndexOf("//");
            if (index > 0)
            {
                fileUrl = fileUrl.Substring(index, fileUrl.Length - index);
            }
            return fileUrl;
        }

        public static IList<ProfileDTO> ConvertToDTOs(IOrganizationalPerson person)
        {
            IList<ProfileDTO> dtos = new List<ProfileDTO>();
            if (!string.IsNullOrEmpty(person.Profile))
            {
                ProfileDTO profileDTO = new ProfileDTO();
                profileDTO.fileUrl = person.Profile;
                profileDTO.fileName = person.Profile.Substring(person.Profile.LastIndexOf("/") + 1);
                dtos.Add(profileDTO);
            }
            return dtos;
        }
    }
}
