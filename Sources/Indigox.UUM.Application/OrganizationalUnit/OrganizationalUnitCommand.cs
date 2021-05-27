using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.HR.Service;
using Indigox.UUM.HR.Setting;
using Indigox.UUM.Naming.Service;
using Indigox.Web.CQRS.Interface;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public abstract class OrganizationalUnitCommand : ICommand
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        public string BusinessType { get; set; }
        public IList<SimplePrincipalDTO> Members { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public IList<SimplePrincipalDTO> Manager { get; set; }
        public IList<SimplePrincipalDTO> Director { get; set; }

        public abstract void Execute();

        protected void FillPropery( IOrganizationalUnit item )
        {
            if (String.IsNullOrEmpty(this.Email))
            {
                var nameService = new NameService();
                var emailSuffix = EmailSettingService.Instance.GetSuffix(Organization);
                var accountName = nameService.Naming(this.Name);
                this.Email = string.Format("{0}@{1}", accountName, emailSuffix);
            }

            IMutableOrganizationalUnit mutableItem = (IMutableOrganizationalUnit)item;
            mutableItem.Name = this.Name;
            mutableItem.FullName = this.Name;
            mutableItem.Email = this.Email;
            mutableItem.Description = this.Description;
            mutableItem.OrderNum = this.OrderNum;
            mutableItem.DisplayName = this.DisplayName;
            mutableItem.ExtendProperties = new Dictionary<string, string>();
            mutableItem.ExtendProperties.Add("BusinessType", this.BusinessType);

            SetMembers(mutableItem);
        }

        protected void SetMembers(IMutableOrganizationalUnit mutableItem)
        {
            
            if ((this.Manager != null) && (this.Manager.Count > 0))
            {
                mutableItem.Manager = RepositoryFactory.Instance.CreateRepository<IPrincipal>().Get(this.Manager[0].UserID);
            }

            IMutableDepartment dept = mutableItem as IMutableDepartment;
            if (dept != null)
            {
                if ((this.Director != null) && (this.Director.Count > 0))
                {
                    dept.Director = RepositoryFactory.Instance.CreateRepository<IPrincipal>().Get(this.Director[0].UserID);
                }
            }
        }
    }
}