using System.Web.Services;
using Indigox.Common.ExchangeManager;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using Indigox.Common.Logging;
using System.Collections.Generic;
using Indigox.UUM.HR.Setting;

namespace Indigox.UUM.Application.Sync.WebServices.Exchange
{
    [WebService( Name = "ExchangeImportUserService", Namespace = Consts.Namespace )]
    public class ExchangeImportUserService : IImportUserService
    {
        public string Create(string nativeID, string organizationalUnitID, string accountName,
            string name, string fullName, string displayName, string email, string title, string mobile,
            string telephone, string fax, double orderNum, string description, string otherContact,
            string portrait, string mailDatabase, PropertyChangeCollection extendProperties)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "";
            }
            /*
             * 修改时间：2018-08-23
             * 修改人：曾勇
             * 修改内容：启用邮箱时，传入邮箱数据库参数
             */
            if (string.IsNullOrEmpty(mailDatabase))
            {
                new ExchangeManagerService().EnableMailBox(accountName);
            }
            else
            {
                new ExchangeManagerService().EnableMailBox(accountName, mailDatabase);
            }            

            return email;   
        }

        public string SyncUser(string nativeID, string organizationalUnitID, string accountName,
            string name, string fullName, string displayName, string email, string title, string mobile,
            string telephone, string fax, double orderNum, string description, string otherContact,
            string portrait, string mailDatabase, PropertyChangeCollection extendProperties)
        {
            return Create(nativeID, organizationalUnitID, accountName, name, fullName, displayName, email, title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase, extendProperties);
        }

        public void Delete( string userID )
        {
            //new ExchangeManagerService().DisableMailBox(userID);
        }

        public void Disable( string userID )
        {
            if (!string.IsNullOrEmpty(userID))
            {
                string account = userID;
                if (userID.IndexOf("@") > -1)
                {
                    account = userID.Substring(0, userID.IndexOf("@"));
                }
                ExchangeManagerService service = new ExchangeManagerService();
                service.LimitMailBox(account);
                service.HideMailBox(account);
            }
        }

        public void Enable( string userID ,string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                string account = userID;
                if (userID.IndexOf("@") > -1)
                {
                    account = userID.Substring(0, userID.IndexOf("@"));
                }
                ExchangeManagerService service = new ExchangeManagerService();
                service.UnlimitMailBox(account);
                service.ShowMailBox(account);
            }
            //new ExchangeManagerService().ConnectMailBox(userID,mailDatabase);
        }

        public void ChangeProperty( string userID, PropertyChangeCollection propertyChanges )
        {
            string email = string.Empty, accountName = string.Empty, mailDatabase = string.Empty;
            foreach (var item in propertyChanges.PropertyChanges)
            {
                if(item.Name.Equals("AccountName"))
                {
                    accountName = (string)item.Value;
                }
                if (item.Name.Equals("Email"))
                {
                    email = (string)item.Value;
                }
                if (item.Name.Equals("MailDatabase"))
                {
                    mailDatabase = (string)item.Value;
                }
            }
            if (string.IsNullOrEmpty(email))
            {
                return;
            }
            if (string.IsNullOrEmpty(mailDatabase))
            {
                new ExchangeManagerService().EnableMailBox(accountName);
            }
            else
            {
                new ExchangeManagerService().EnableMailBox(accountName, mailDatabase);
            }

            return;
        }
    }
}