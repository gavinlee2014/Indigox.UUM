using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Settings;
using Indigox.UUM.Sync.Interfaces;
using Indigox.Settings.Service;

namespace Indigox.UUM.Sync.Mail
{
    [SettingItem("MailSender.Account", "", Title = "用户名", Description = "用户名", CategoryName = "邮件发送", AllowEmpty = true)]
    [SettingItem("MailSender.Password", "", Title = "密码", Description = "密码", CategoryName = "邮件发送", AllowEmpty = true)]
    [SettingItem("MailSender.Address", "", Title = "邮件地址", Description = "邮件地址", CategoryName = "邮件发送", AllowEmpty = true)]
    [SettingItem("MailSender.Port", "", Title = "邮件端口", Description = "邮件端口", CategoryName = "邮件发送", AllowEmpty = true)]
    [SettingItem("MailSender.Host", "", Title = "服务器地址", Description = "服务器地址", CategoryName = "邮件发送", AllowEmpty = true)]
    [SettingItem("MailSender.EnableSSL", "false", Title = "是否启用SSL", Description = "是否启用SSL", CategoryName = "邮件发送", AllowEmpty = true)]
    public class SyncMailSender
    {
        private SettingService settingService=new SettingService ();

        public string Account
        {
            get
            {
                return settingService.GetValue("MailSender.Account");
            }
        }

        public string Password
        {
            get
            {
                return settingService.GetValue("MailSender.Password");
            }
        }

        public string Address
        {
            get
            {
                return settingService.GetValue("MailSender.Address");
            }
        }

        public string Port
        {
            get
            {
                return settingService.GetValue("MailSender.Port");
            }
        }

        public string Host
        {
            get
            {
                return settingService.GetValue("MailSender.Host");
            }
        }

        public string EnableSSL
        {
            get
            {
                return settingService.GetValue("MailSender.EnableSSL");
            }
        }
    }
}
