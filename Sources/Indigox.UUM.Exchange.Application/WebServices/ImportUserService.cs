using System.Web.Services;
using Indigox.Common.ExchangeManager;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using Indigox.Common.Logging;

namespace Indigox.UUM.Exchange.Application.WebServices
{
    [WebService(Name = "ExchangeImportUserService", Namespace = Consts.Namespace)]
    public class ImportUserService : IImportUserService
    {
        public string Create(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            Log.Debug("ExchangeImportUserService:" + accountName);

            Log.Debug("MailDatabase : " + mailDatabase);

            ExchangeManagerService service = new ExchangeManagerService();
            if (string.IsNullOrEmpty(mailDatabase))
            {
                service.EnableMailBox(accountName);
            }
            else
            {
                service.EnableMailBox(accountName, mailDatabase);
            }
            return email;
        }

        public string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            return Create(nativeID, organizationalUnitID, accountName, name, fullName, displayName, email, title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase);
        }

        public void Delete(string userID)
        {
            //new ExchangeManagerService().DisableMailBox(userID);
        }

        public void Disable(string userID)
        {
            new ExchangeManagerService().DisableMailBox(userID);            
        }

        public void Enable(string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            new ExchangeManagerService().ConnectMailBox(userID, mailDatabase);
        }

        public void ChangeProperty(string userID, PropertyChangeCollection propertyChanges)
        {
        }
    }
}