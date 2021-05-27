using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Service;
using Indigox.Web.CQRS.Interface;

namespace Indigox.UUM.Application.Principal
{
    public class ChangeOrganizationCommand : ICommand
    {
        public IList<PrincipalDTO> PrincipalList { get; set; }
        public string TargetOrganization { get; set; }

        public void Execute()
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IOrganizationalUnit target = repository.Get( this.TargetOrganization );
            PrincipalService service = new PrincipalService();

            foreach ( PrincipalDTO dto in this.PrincipalList )
            {
                IOrganizationalObject item = Indigox.Common.Membership.Principal.GetPrincipalByID( dto.ID ) as IOrganizationalObject;
                if ( item != null )
                {
                    service.MoveTo( item, target );
                }
            }
        }
    }
}