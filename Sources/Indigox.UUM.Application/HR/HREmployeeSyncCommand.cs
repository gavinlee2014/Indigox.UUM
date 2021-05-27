using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Web.CQRS.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.HR.Model;
using Indigox.UUM.HR.Service;

namespace Indigox.UUM.Application.HR
{
    public class HREmployeeSyncCommand : ICommand
    {
        public string ID { get; set; }

        public string PrincipalID { get; set; }

        public void Execute()
        {
            HREmployeeService service = new HREmployeeService();
            service.Sync(this.ID, this.PrincipalID);
        }
    }
}
