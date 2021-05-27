using System;
using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interface.Server;

using C_Corporation = Indigox.Common.Membership.Corporation;
using C_Group = Indigox.Common.Membership.Group;
using C_OrganizationalRole = Indigox.Common.Membership.OrganizationalRole;
using C_OrganizationalUnit = Indigox.Common.Membership.OrganizationalUnit;
using C_Role = Indigox.Common.Membership.Role;

using DTO_Group = Indigox.UUM.Sync.Interface.Group;
using DTO_OrganizationalRole = Indigox.UUM.Sync.Interface.OrganizationalRole;
using DTO_OrganizationalUnit = Indigox.UUM.Sync.Interface.OrganizationalUnit;
using DTO_OrganizationalPerson = Indigox.UUM.Sync.Interface.OrganizationalPerson;
using DTO_Role = Indigox.UUM.Sync.Interface.Role;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;
using Indigox.UUM.Sync.Interface;

namespace Indigox.UUM.Application.Sync.WebServices.Export
{
    [WebService(Name = "ExportUserService", Namespace = Consts.Namespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportUserService : WebService, IExportUserService
    {
        public DTO_OrganizationalPerson[] GetAllUsers()
        {
            List<DTO_OrganizationalPerson> list = new List<DTO_OrganizationalPerson>();

            IRepository<IUser> repository = RepositoryFactory.Instance.CreateRepository<IUser>();
            IList<IUser> models = repository.Find(
                Query.NewQuery.FindByCondition(
                    Specification.And(
                        Specification.NotEqual("Organization",null),
                        Specification.Equal("Deleted", false)
                    )
                    /*Specification.And(
                        Specification.Equal("Enabled",true),
                        Specification.Equal("Deleted",false)
                    )*/
                )
            );

            foreach (IUser model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }
        public DTO_OrganizationalPerson[] GetAllUsersWithDisabled()
        {
            List<DTO_OrganizationalPerson> list = new List<DTO_OrganizationalPerson>();

            IRepository<IUser> repository = RepositoryFactory.Instance.CreateRepository<IUser>();
            IList<IUser> models = repository.Find(
                Query.NewQuery.FindByCondition(
                    Specification.Equal("Deleted", false)
                )
            );

            foreach (IUser model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }

        public DTO_OrganizationalPerson[] GetUsers(int startIndex, int limit)
        {
            List<DTO_OrganizationalPerson> list = new List<DTO_OrganizationalPerson>();

            IRepository<IUser> repository = RepositoryFactory.Instance.CreateRepository<IUser>();
            IList<IUser> models = repository.Find(Query.NewQuery.StartFrom(startIndex).LimitTo(limit));
            foreach (IUser model in models)
            {
                list.Add(DTOConvertor.ConvertToDto(model));
            }

            return list.ToArray();
        }

        public string GetHREmployeeCode(string id)
        {
            string employeeCode = "";
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText(string.Format(@"SELECT HRObjectID From HRMapping where PrincipalID = '{0}'",id));
            if (recordset.Records.Count > 0)
            {
                employeeCode = recordset.Records[0].GetString("HRObjectID");
            }

            return employeeCode;
        }

        public DTO_OrganizationalPerson GetUserByID(string userID)
        {
            IRepository<IUser> repository = RepositoryFactory.Instance.CreateRepository<IUser>();
            IUser user = repository.Get(userID);

            return DTOConvertor.ConvertToDtoWithIdCard(user);
        }

        
    }
}
