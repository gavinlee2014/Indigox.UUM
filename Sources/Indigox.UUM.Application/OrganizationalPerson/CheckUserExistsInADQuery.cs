using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Application.DTO;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class CheckUserExistsInADQuery : Indigox.Web.CQRS.GenericListQuery<OrganizationalPersonDTO>
    {
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string PrincipalID { get; set; }

        private T GetValue<T>(ResultPropertyCollection properties, string name)
        {
            T value = default(T);
            if (properties.Contains(name))
            {
                value = (T)properties[name][0];
            }
            return value;
        }

        public override IList<OrganizationalPersonDTO> List()
        {
            IList<OrganizationalPersonDTO> list = new List<OrganizationalPersonDTO>();

            var repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IList<IOrganizationalPerson> users = repository.Find(
                Query.NewQuery.FindByCondition(
                        Specification.Equal("ID", PrincipalID)
                    )
                );
            if (users.Count > 0 && users[0].AccountName == AccountName && users[0].Email == Email){
                return list;
            }

            Forest forest = Forest.GetCurrentForest();
            GlobalCatalog gc = forest.FindGlobalCatalog();

            using (DirectorySearcher searcher = gc.GetDirectorySearcher())
            {
                if (String.IsNullOrEmpty(Email))
                {
                    searcher.Filter = String.Format("(|(sAMAccountName={0}))", AccountName);
                }
                else
                {
                    searcher.Filter = String.Format("(|(sAMAccountName={0})(mail={1}))", AccountName, Email);
                }
                
                SearchResultCollection results = searcher.FindAll();

                OrganizationalPersonDTO dto;
                foreach (SearchResult result in results)
                {
                    DirectoryEntry entry = result.GetDirectoryEntry();
                    dto = new OrganizationalPersonDTO();
                    dto.AccountName = GetValue<string>(result.Properties, "sAMAccountName");
                    dto.Description = GetValue<string>(result.Properties, "description");
                    dto.DisplayName = GetValue<string>(result.Properties, "displayName");
                    dto.Email = GetValue<string>(result.Properties, "mail");
                    dto.Fax = GetValue<string>(result.Properties, "facsimileTelephoneNumber");
                    dto.FullName = GetValue<string>(result.Properties, "name");
                    dto.Mobile = GetValue<string>(result.Properties, "mobile");
                    dto.Name = GetValue<string>(result.Properties, "name");
                    dto.OrganizationName = GetValue<string>(result.Properties, "department");
                    dto.Telephone = GetValue<string>(result.Properties, "telephoneNumber");
                    dto.Title = GetValue<string>(result.Properties, "title");
                    list.Add(dto);
                }
            }
            return list;
        }
    }
}
