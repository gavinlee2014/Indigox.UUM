using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Factory;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;
using System.Collections.Generic;

namespace Indigox.UUM.Application.OrganizationalRole
{
    public class CreateOrganizationalRoleCommand : OrganizationalRoleCommand
    {
        public override void Execute()
        {
            DateTime now = DateTime.Now;

            IRole role = null;
            if (!string.IsNullOrEmpty(this.Role))
            {
                role = RepositoryFactory.Instance.CreateRepository<IRole>().Get(this.Role);
            }

            IOrganizationalUnit parent = null;
            if (!String.IsNullOrEmpty(this.Organization))
            {
                parent = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(this.Organization);
            }
            else
            {
                throw new ArgumentException("Organization '" + this.Email + "' is undefined", "Organization");
            }

            var extendProperties = new Dictionary<string, string>();

            extendProperties.Add("Level", this.Level);
            extendProperties.Add("RoleLevel", this.RoleLevel);

            IMutableOrganizationalRole item = new OrganizationalRoleFactory()
            {
                OrganizationalUnit = parent,
                Role = role,
                Name = this.Name,
                FullName = parent.FullName + "_" + this.Name,
                Email = this.Email,
                Description = this.Description,
                OrderNum = this.OrderNum,
                DisplayName=this.DisplayName,
                ExtendProperties = extendProperties
            }.Create(this.ID);

            this.SetMembers(item);

            RepositoryFactory.Instance.CreateRepository<IOrganizationalRole>().Add(item);

            OperationLogService.LogOperation("创建组织角色： " + item.Organization.Name + "_" + item.Name, item.GetDescription());

        }
    }
}