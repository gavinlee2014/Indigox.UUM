using System;
using Indigox.Settings;
using Indigox.Settings.Service;

namespace Indigox.UUM.HR.Setting
{
    [SettingItem("HR.ProfilePrefixSetting", "", Title = "Employee头像url前缀设置", Description = "Employee头像url前缀设置", CategoryName = "HR同步配置")]
    public class HREmployeeProfilePrefixSetting
    {
        public string ProfilePrefix
        {
            get
            {
                SettingService settingService = new SettingService();
                string suffix = settingService.GetValue("HR.ProfilePrefixSetting");
                return suffix;
            }
        }
    }
}
