using System;
using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.Principal
{
    public class PrincipalSummaryQuery : Indigox.Web.CQRS.GenericSingleQuery<PrincipalSummaryDTO>
    {
        public string Identity { get; set; }

        public override PrincipalSummaryDTO Single()
        {
            if ( string.IsNullOrEmpty( Identity ) )
            {
                return null;
            }

            IPrincipal principal = Indigox.Common.Membership.Principal.GetPrincipalByID( Identity );

            PrincipalSummaryDTO dto;

            if ( principal is IRole )
            {
                IRole role = principal as IRole;

                dto = new PrincipalSummaryDTO()
                {
                    Summary = GetNames( role.GetAllUsers() )
                };
            }
            else if ( principal is IOrganizationalRole )
            {
                IOrganizationalRole organizationalRole = principal as IOrganizationalRole;

                dto = new PrincipalSummaryDTO()
                {
                    Summary = GetNames( organizationalRole.GetAllUsers() )
                };
            }
            else if ( principal is IOrganizationalUnit )
            {
                IOrganizationalUnit organizationalUnit = principal as IOrganizationalUnit;

                dto = new PrincipalSummaryDTO()
                {
                    Summary = organizationalUnit.FullName
                };
            }
            else if ( principal is IOrganizationalPerson )
            {
                IOrganizationalPerson organizationalPerson = principal as IOrganizationalPerson;

                string fullname = organizationalPerson.Name;
                if ( organizationalPerson.Organization != null )
                {
                    fullname = organizationalPerson.Organization.FullName + "_" + fullname;
                }

                dto = new PrincipalSummaryDTO()
                {
                    Summary = fullname
                };
            }
            else if ( principal is IGroup )
            {
                IGroup group = principal as IGroup;

                dto = new PrincipalSummaryDTO()
                {
                    Summary = GetNames( group.GetAllUsers() )
                };
            }
            else
            {
                dto = new PrincipalSummaryDTO()
                {
                    Summary = principal.Name
                };
            }

            return dto;
        }

        private string GetNames( IList<IUser> users )
        {
            List<string> names = new List<string>();
            foreach ( IUser user in users )
            {
                names.Add( user.Name );
            }

            return string.Join( ",", names.ToArray() );
        }
    }
}