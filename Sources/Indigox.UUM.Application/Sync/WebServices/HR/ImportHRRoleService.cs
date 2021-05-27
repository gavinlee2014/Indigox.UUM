using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.UUM.HR.Model;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Application.Sync.WebServices.HR
{
    [WebServiceBinding(Name = "ImportRoleService", Namespace = Consts.Namespace_HR)]
    public class ImportHRRoleService : WebService, IImportRoleService
    {
        private bool RoleExist(String nativeID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", nativeID);
            var employee = repository.First(query);
            return employee != null;
        }
        void IImportRoleService.AddOrganizationalRole(string roleID, string organizationalRoleID)
        {
            throw new NotImplementedException();
        }

        void IImportRoleService.ChangeProperty(string roleID, UUM.Sync.Interface.PropertyChangeCollection propertyChanges)
        {
            throw new NotImplementedException();
        }

        string IImportRoleService.Create(string nativeID, string name, string email, string description, double orderNum)
        {
            throw new NotImplementedException();
        }

        void IImportRoleService.Delete(string roleID)
        {
            throw new NotImplementedException();
        }

        void IImportRoleService.RemoveOrganizationalRole(string roleID, string organizationalRoleID)
        {
            throw new NotImplementedException();
        }

        string IImportRoleService.SyncRole(string nativeID, string name, string email, string description, double orderNum)
        {
            throw new NotImplementedException();
        }
    }
}
