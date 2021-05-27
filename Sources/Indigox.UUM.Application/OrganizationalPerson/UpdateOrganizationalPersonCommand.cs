using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Extend;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class UpdateOrganizationalPersonCommand : OrganizationalPersonCommand
    {
        public override void Execute()
        {
            IRepository<IOrganizationalPerson> repos = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            AssertAccountNameNotUsed( repos );
            AssertEmailNotUsed( repos );
            AssertMobileNotUsed(repos);
            AssertIdCardNotUsed(repos);

            IOrganizationalPerson item = repos.Get( this.ID );
            this.FillPropery( item );

            OrganizationalPersonService service = new OrganizationalPersonService();
            service.Update( item );

            OperationLogService.LogOperation("编辑人员：" + item.Name, item.GetDescription());
            //? call repos.Update(...) in service.Update(...)
            repos.Update( item );
        }

        private void AssertAccountNameNotUsed( IRepository<IOrganizationalPerson> repos )
        {
            IList<IOrganizationalPerson> accountExists = repos.Find( Query.NewQuery.FindByCondition( Specification.Equal( "AccountName", this.AccountName ) ) );
            foreach ( IOrganizationalPerson accountExist in accountExists )
            {
                if ( !accountExist.ID.Equals( this.ID ) )
                {
                    throw new ArgumentException( "account '" + this.AccountName + "' used by other", "AccountName" );
                }
            }
        }

        private void AssertEmailNotUsed( IRepository<IOrganizationalPerson> repos )
        {
            if ( !String.IsNullOrEmpty( this.Email ) )
            {
                IList<IOrganizationalPerson> emailExists = repos.Find( Query.NewQuery.FindByCondition( Specification.Equal( "Email", this.Email ) ) );
                foreach ( IOrganizationalPerson emailExist in emailExists )
                {
                    if ( !emailExist.ID.Equals( this.ID ) )
                    {
                        throw new ArgumentException( "email '" + this.Email + "' used by other", "Email" );
                    }
                }
            }
        }

        private void AssertIdCardNotUsed(IRepository<IOrganizationalPerson> repos)
        {
            if (!string.IsNullOrEmpty(this.IdCard))
            {
                IList<IOrganizationalPerson> mobileExists = repos.Find(
                    Query.NewQuery.FindByCondition(
                        Specification.And(
                            Specification.Equal("IdCard", this.IdCard),
                            Specification.NotEqual("ID", this.ID)
                        )
                    )
                );
                if (mobileExists.Count > 0)
                {
                    throw new ArgumentException("更新用户失败，IdCard：'" + this.IdCard + "' 已经存在。");
                }
            }
        }

        private void AssertMobileNotUsed(IRepository<IOrganizationalPerson> repos)
        {
            if (!string.IsNullOrEmpty(this.Mobile))
            {
                IList<IOrganizationalPerson> mobileExists = repos.Find(
                    Query.NewQuery.FindByCondition(
                        Specification.And(
                            Specification.Equal("Mobile", this.Mobile),
                            Specification.NotEqual("ID", this.ID)
                        )
                    )
                );
                if (mobileExists.Count > 0)
                {
                    throw new ArgumentException("更新用户失败，Mobile：'" + this.Mobile + "' 已经存在。");
                }
            }
        }
    }
}