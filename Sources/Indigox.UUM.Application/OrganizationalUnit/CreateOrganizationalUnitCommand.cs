using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using System.Collections.Generic;
using Indigox.UUM.Factory;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public class CreateOrganizationalUnitCommand : OrganizationalUnitCommand
    {
        public override void Execute()
        {
            DateTime now = DateTime.Now;
            IOrganizationalUnit parent = null;
            string fullName = this.Name;
            IRepository<IOrganizationalUnit> repos = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            AssertEmailNotUsed(repos);
            if (!String.IsNullOrEmpty(this.Organization))
            {
                parent = repos.Get(this.Organization);
                if (!(parent is ICorporation))
                {
                    fullName = parent.FullName + "_" + this.Name;
                }
            }
            else
            {
                throw new ArgumentException("Organization '" + this.Email + "' is undefined", "Organization");
            }

            var extendProperties = new Dictionary<string, string>();

            extendProperties.Add("BusinessType", this.BusinessType);

            IMutableOrganizationalUnit item = new OrganizationalUnitFactory()
            {
                ParentOrganizationalUnit = parent,
                Name = this.Name,
                FullName = fullName,
                Email = this.Email,
                Description = this.Description,
                OrderNum = this.OrderNum,
                DisplayName = this.DisplayName,
                ExtendProperties = extendProperties
            }.Create(this.ID, this.Type);

            this.SetMembers(item);
            repos.Add(item);

            OperationLogService.LogOperation("创建部门： " + item.Organization.Name + "_" + item.Name, item.GetDescription());

        }


        private void AssertEmailNotUsed(IRepository<IOrganizationalUnit> repos)
        {
            if (!String.IsNullOrEmpty(this.Email))
            {
                IList<IOrganizationalUnit> emailExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("Email", this.Email)));

                if (emailExists.Count > 0)
                {
                    throw new ArgumentException("Email '" + this.Email + "' already exist", "Email");
                }
            }
        }
    }
}