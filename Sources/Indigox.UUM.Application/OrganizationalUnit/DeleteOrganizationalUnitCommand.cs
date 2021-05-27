using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Service;
using Indigox.UUM.Application.Principal;
using Indigox.UUM.Extend;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.Application.OrganizationalUnit
{
    public class DeleteOrganizationalUnitCommand : OrganizationalUnitCommand
    {
        public override void Execute()
        {
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IOrganizationalUnit item = repository.Get(this.ID);
            

            PrincipalService service = new PrincipalService();
            service.Delete(item);
            OperationLogService.LogOperation("删除部门： " + item.Name + "_" + item.Name, item.GetDescription());

            /***
             * 修改时间：2018-11-25
             * 修改人：曾勇
             * 修改内容：删除部门后，将Organization为部门ID的人员的Organization属性设为null
             **/
            IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
            db.ExecuteText("update Principal set Organization = null where Type = '201' and Organization = '" + this.ID + "'");
        }
    }
}