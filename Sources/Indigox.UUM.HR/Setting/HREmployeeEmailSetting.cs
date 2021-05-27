using System;
using Indigox.Settings;
using Indigox.Settings.Service;

namespace Indigox.UUM.HR.Setting
{
    [SettingItem("HR.EmployeeEmailSetting", "", Title = "Employee邮箱后缀设置", Description = "Employee邮箱后缀设置", CategoryName = "HR同步配置")]
    public class HREmployeeEmailSetting
    {
        public string EmailSuffix
        {
            get
            {
                SettingService settingService = new SettingService();
                string suffix = settingService.GetValue("HR.EmployeeEmailSetting");
                return suffix;
            }
        }

        public string DefaultEmailSuffix
        {
            get
            {
                SettingService settingService = new SettingService();
                string suffix = settingService.GetValue("HR.DefaultEmailSuffix");
                return suffix;
            }
        }
    }
}
