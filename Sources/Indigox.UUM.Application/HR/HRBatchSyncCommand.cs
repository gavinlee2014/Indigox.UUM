using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.HR.Service;
using Indigox.Web.CQRS.Interface;

namespace Indigox.UUM.Application.HR
{
    public class HRBatchSyncCommand : ICommand
    {
        public IList<string> Ids { get; set; }

        public void Execute()
        {
            HREmployeeService employeeService = new HREmployeeService();
            HROrganizationalService orgService = new HROrganizationalService();
            HROrganizationalRoleService roleService = new HROrganizationalRoleService();

            foreach (string id in Ids)
            {
                string[] arr = id.Split(',');
                string principalType = arr[0];
                if (principalType.Equals("0"))
                {
                    employeeService.Sync(arr[1]);
                }
                else if (principalType.Equals("1"))
                {
                    orgService.Sync(arr[1]);
                }
                else
                {
                    roleService.Sync(arr[1]);
                }
            }
        }
    }
}
