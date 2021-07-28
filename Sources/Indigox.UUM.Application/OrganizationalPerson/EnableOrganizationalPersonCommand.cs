using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class EnableOrganizationalPersonCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public string ID { get; set; }
        public string OrganizationalUnitID { get; set; }

        public void Execute()
        {
            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            IOrganizationalPerson item = repository.Get( this.ID );

            IOrganizationalUnit target = item.Organization;
            if ( !string.IsNullOrEmpty( OrganizationalUnitID ) )
            {
                target = GetOrganizationalUnit( OrganizationalUnitID );
            }

            PrincipalService service = new PrincipalService();
            service.Enable( item, target );
            OperationLogService.LogOperation("启用用户： " + item.Name, item.GetDescription());
        }

        private IOrganizationalUnit GetOrganizationalUnit( string id )
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            return repository.Get( id );
        }
    }
}