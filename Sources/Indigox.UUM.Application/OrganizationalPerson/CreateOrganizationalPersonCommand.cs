using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Utilities;
using Indigox.UUM.Factory;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class CreateOrganizationalPersonCommand : OrganizationalPersonCommand
    {
        public override void Execute()
        {
            IRepository<IOrganizationalPerson> repos = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            AssertAccountNameNotUsed( repos );
            AssertEmailNotUsed( repos );
            AssertIdCardNotUsed(repos);
            AssertMobileNotUsed(repos);
            ArgumentAssert.NotEmpty( this.Organization, "Organization" );

            IOrganizationalUnit parent = GetParentOrganizationalUnit( this.Organization );

            DateTime now = DateTime.Now;
            var extendProperties = new Dictionary<string, string>();

            extendProperties.Add("StaffNo", this.StaffNo);
            extendProperties.Add("StaffType", this.StaffType);
            extendProperties.Add("Gender", this.Gender);
            extendProperties.Add("NeedClockOn", this.NeedClockOn);
            extendProperties.Add("LastJoinDate", this.LastJoinDate);
            extendProperties.Add("ContractStartDate", this.ContractStartDate);
            extendProperties.Add("ContractEndDate", this.ContractEndDate);
            extendProperties.Add("ContractCompanyTexNo", this.ContractCompanyTexNo);
            extendProperties.Add("ContractCompanyName", this.ContractCompanyName);
            IMutableOrganizationalPerson item = new OrganizationalPersonFactory()
            {
                OrganizationalUnit = parent,
                Name = this.Name,
                FullName = parent.Name + "_" + this.Name,
                Email = this.Email,
                MailDatabase = this.MailDatabase,
                AccountName = this.AccountName,
                Title = this.Title,
                Mobile = this.Mobile,
                IdCard = this.IdCard,
                Telephone = this.Telephone,
                Fax = this.Fax,
                OtherContact = this.OtherContact,
                OrderNum = this.OrderNum,
                DisplayName=this.DisplayName,
                Profile = this.Profile.Count > 0 ? this.Profile[0].GetFileUrl() : "",
                ExtendProperties = extendProperties
            }.Create( this.ID );

            this.SetMembers( item );

            repos.Add( item );

            OperationLogService.LogOperation("创建用户： "  + item.Name, item.GetDescription());
        }

        private void AssertAccountNameNotUsed( IRepository<IOrganizationalPerson> repos )
        {
            IList<IOrganizationalPerson> accountExists = repos.Find( Query.NewQuery.FindByCondition( Specification.Equal( "AccountName", this.AccountName ) ) );
            if ( accountExists.Count > 0 )
            {
                throw new ArgumentException( "AccountName '" + this.AccountName + "' already exist", "AccountName" );
            }
        }

        private void AssertMobileNotUsed(IRepository<IOrganizationalPerson> repos)
        {
            if (!string.IsNullOrEmpty(this.Mobile))
            {
                IList<IOrganizationalPerson> accountExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("Mobile", this.Mobile)));
                if (accountExists.Count > 0)
                {
                    throw new ArgumentException("Mobile '" + this.Mobile + "' already exist", "Mobile");
                }
            }
        }


        private void AssertEmailNotUsed( IRepository<IOrganizationalPerson> repos )
        {
            if ( !String.IsNullOrEmpty( this.Email ) )
            {
                IList<IOrganizationalPerson> emailExists = repos.Find( Query.NewQuery.FindByCondition( Specification.Equal( "Email", this.Email ) ) );

                if ( emailExists.Count > 0 )
                {
                    throw new ArgumentException( "Email '" + this.Email + "' already exist", "Email" );
                }
            }
        }


        private void AssertIdCardNotUsed(IRepository<IOrganizationalPerson> repos)
        {
            if (!String.IsNullOrEmpty(this.IdCard))
            {
                IList<IOrganizationalPerson> emailExists = repos.Find(Query.NewQuery.FindByCondition(Specification.Equal("IdCard", this.IdCard)));

                if (emailExists.Count > 0)
                {
                    throw new ArgumentException("IdCard '" + this.IdCard + "' already exist", "IdCard");
                }
            }
        }

        private IOrganizationalUnit GetParentOrganizationalUnit( string organizationID )
        {
            return RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get( organizationID );
        }
    }
}