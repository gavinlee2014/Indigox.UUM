using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Sync.Mail
{
    public class MailService
    {
        private const string SUBJECT = "UUM同步邮件";

        public void SendMail(string sendTo,ISyncTask task)
        {
            if (string.IsNullOrEmpty(sendTo)) return;
            SendMail(new string[] { sendTo }, SUBJECT, task.Description);
        }

        public void SendMail(IList<string> sendToList, string subject, string body)
        {
            if (sendToList.Count > 0)
            {
                Indigox.Common.Message.Service.SendMail sender = new Indigox.Common.Message.Service.SendMail();
                SetSenderProperties(sender);
                sender.SendAsync(sendToList, subject, body);
            }
            else
            {
                throw new ArgumentException("Send mail action Error : 收件人为空。\r\n Email body is : \r\n" + body);
            }
        }

        private void SetSenderProperties(Indigox.Common.Message.Service.BaseSend sender)
        {
            SyncMailSender setting = new SyncMailSender();
            sender.Account = setting.Account;
            sender.Password = setting.Password;
            sender.MailAddress = setting.Address;
            sender.MailHost = setting.Host;
            sender.MailPort = setting.Port;
            sender.EnableSSL = setting.EnableSSL;
        }
    }
}
