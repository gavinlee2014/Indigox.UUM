using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using Indigox.Common.StateContainer;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class CurrentUserQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public override IList<OrganizationalPersonDTO> List()
        {
            List<OrganizationalPersonDTO> list = new List<OrganizationalPersonDTO>();
            if (StateContext.Current.Session.User != null)
            {
                string currentUserID = StateContext.Current.Session.User.ID;
                var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
                IOrganizationalPerson item = repository.Get(currentUserID);
                if (item != null)
                {
                    list.Add(OrganizationalPersonDTO.ConvertToDTO(item));
                }
            }
            return list;
        }
    }
}
