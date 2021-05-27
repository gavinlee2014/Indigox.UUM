using System;
using System.Web.Services;
using Indigox.Common.ExchangeManager;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ExchangeAddressList = Indigox.Common.ExchangeManager.AddressList;
using ExchangeGroup = Indigox.Common.ExchangeManager.Group;

namespace Indigox.UUM.Exchange.Application.WebServices
{
    [WebService( Name = "ExchangeImportRoleService", Namespace = Consts.Namespace )]
    public class ImportRoleService : IImportRoleService
    {
        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            string account = email.Substring( 0, email.IndexOf( "@" ) );

            ExchangeManagerService service = new ExchangeManagerService();

            service.EnableDistributionGroup(account);

            //ExchangeGroup group = service.GetDistributionGroup(account);

            //service.CreateAddressList(group, null);

            return email;
        }

        public string SyncRole( string nativeID, string name, string email, string description, double orderNum )
        {
            return Create(nativeID, name, email, description, orderNum);
        }

        public void Delete( string roleID )
        {
            //string account = roleID.Substring(0, roleID.IndexOf("@"));

            //ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);

            //service.DisableDistributionGroup(account);
        }

        public void ChangeProperty( string roleID, PropertyChangeCollection propertyChanges )
        {
            //string account = roleID.Substring(0, roleID.IndexOf("@"));

            //ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);
            //ExchangeAddressList addressList = service.GetAddressList(group.Name);
            //addressList.Name = Convert.ToString(propertyChanges.Get("Name"));

            //service.UpdateAddressList(addressList);
        }

        public void AddOrganizationalRole( string roleID, string organizationalRoleID )
        {
        }

        public void RemoveOrganizationalRole( string roleID, string organizationalRoleID )
        {
        }
    }
}