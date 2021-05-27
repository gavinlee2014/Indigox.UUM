using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class FilterUserQuery : Indigox.Web.CQRS.GenericListQuery<SimplePrincipalDTO>
    {
        public string KeyWord { get; set; }
        private IRecordSet Query()
        {
            string sql = "select u.ID,p.Name from users u left join principal p on u.id=p.id where p.name like '%'+@KeyWord+'%' or u.AccountName like '%'+@KeyWord+'%' and p.Isdeleted=0";
            var factory = new DatabaseFactory();
            var db = factory.CreateDatabase("UUM");
            var command = db.CreateTextCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.AddParameter("@KeyWord varchar", KeyWord);
            var recordSet = db.Query(command);
            return recordSet;
        }
        public override IList<SimplePrincipalDTO> List()
        {
            var recordSet = Query();
            IList<SimplePrincipalDTO> list = new List<SimplePrincipalDTO>();
            foreach (var v in recordSet.Records)
            {
                var user = new SimplePrincipalDTO();
                user.UserID = v.GetValue("ID").ToString();
                user.UserName = v.GetValue("Name").ToString();
                list.Add(user);
            }
            return list;
        }

        public override int Size()
        {
            var recordSet = Query();
            return recordSet.Records.Count;
        }
    }
}