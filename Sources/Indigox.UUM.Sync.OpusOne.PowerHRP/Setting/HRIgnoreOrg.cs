using System;
using Indigox.Settings;
using Indigox.Settings.Service;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Setting
{
    [SettingItem("HR.HRIgnoreOrg", "", Title = "不做处理的Organization", Description = "不做处理的Organization", CategoryName = "HR同步配置")]
    public class HRIgnoreOrg
    {
        public string IgnoreOrg
        {
            get
            {
                SettingService settingService = new SettingService();
                string ignoreOrg = settingService.GetValue("HR.HRIgnoreOrg");
                return ignoreOrg;
            }
        }
    }
}
