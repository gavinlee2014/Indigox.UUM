using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Web.CQRS.Interface;
using Indigox.Common.ADAccessor;
using Indigox.Settings;
using Indigox.Settings.Service;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.Application.Profile
{
    
    [SettingItem("UUM.User.PasswordSource", "DataBase", Title = "密码获取源", Description = "AD/DataBase", CategoryName = "统一用户管理", AllowEmpty = true)]    
    public class ChangePasswordCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public string AccountName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        private SettingService settingService = new SettingService();
        public void Execute()
        {
            string passwordSource = settingService.GetValue("UUM.User.PasswordSource");
            if (passwordSource == "AD")
            {
                Accessor.ChangePassword(this.AccountName, this.OldPassword, this.NewPassword);
            }
            else if (passwordSource == "DataBase")
            {
                ChangeDBPassword changeDbPassword = new ChangeDBPassword();
                changeDbPassword.ChangePassword(AccountName, OldPassword, NewPassword);
            }
            else {
                throw new Exception("未配置密码源");
             }
        }
    }
}
