using System;
using Indigox.Settings;
using Indigox.Settings.Service;

namespace Indigox.UUM.HR.Setting
{
    [SettingItem("HR.SyncModel", "", Title = "是否自动同步", Description = "HR系统同步方式设置", CategoryName = "HR同步配置")]
    public class HRSyncMode
    {
        public bool IsAutomaticSync{
            get {
                SettingService settingService = new SettingService();
                bool isAutomatic=Convert.ToBoolean(settingService.GetValue("HR.SyncModel"));
                return isAutomatic;
            }
        }
    }
}
