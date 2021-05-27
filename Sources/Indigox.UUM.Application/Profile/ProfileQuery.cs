using System;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.StateContainer;
using Indigox.UUM.Application.DTO;

namespace Indigox.UUM.Application.Profile
{
    public class ProfileQuery : Indigox.Web.CQRS.GenericSingleQuery<OrganizationalPersonDTO>
    {
        public override OrganizationalPersonDTO Single()
        {
            IOrganizationalPerson user = StateContext.Current.Session.User;

            // the user in session is a copy of cached data.
            user = Indigox.Common.Membership.OrganizationalPerson.GetOrganizationalPersonByID( user.ID );

            OrganizationalPersonDTO dto = null;
            if ( user != null )
            {
                dto = OrganizationalPersonDTO.ConvertToDTO( user );
            }
            return dto;
        }
    }
}