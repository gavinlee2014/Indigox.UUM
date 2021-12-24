using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Web.CQRS.Interface;
using Indigox.Common.ADAccessor;
using Indigox.UUM.Service;

namespace Indigox.UUM.Application.OrganizationalPerson
{
    public class SetPasswordCommand : ICommand
    {
        public string AccountName { get; set; }
        public string Password { get; set; }

        public void Execute()
        {
            Accessor.SetPassword(this.AccountName, this.Password);
            OrganizationalPersonService servcie = new OrganizationalPersonService();
            string encoded = "";
            if (!String.IsNullOrEmpty(this.Password))
            {
                encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Password));
            }
            servcie.UpdateUserPasswordByAccount(this.AccountName, encoded);
            OperationLogService.LogOperation("修改密码：" + this.AccountName, AccountName + "修改密码");
        }
    }
}
