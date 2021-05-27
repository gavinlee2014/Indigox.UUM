using System;
using System.Web.Services;
using Indigox.Common.ExchangeManager;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ExchangeAddressList = Indigox.Common.ExchangeManager.AddressList;
using ExchangeGroup = Indigox.Common.ExchangeManager.Group;


namespace Indigox.UUM.Exchange.Application.WebServices
{
    [WebService( Name = "ExchangeImportOrganizationalRoleService", Namespace = Consts.Namespace )]
    public class ImportOrganizationalRoleService : IImportOrganizationalRoleService
    {
        public string Create(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum)
        {
            string account = email.Substring(0, email.IndexOf("@"));
            string parentAccount = null;
            if (!String.IsNullOrEmpty(organizationalUnitID))
            {
                parentAccount = organizationalUnitID.Substring(0, email.IndexOf("@"));
            }

            ExchangeManagerService service = new ExchangeManagerService();

            service.EnableDistributionGroup(account);

            //ExchangeGroup group = service.GetDistributionGroup(account);
            //ExchangeGroup parentGroup = service.GetDistributionGroup(parentAccount);
            //ExchangeAddressList parent = service.GetAddressList(parentGroup.Name);

            //service.CreateAddressList(group, parent);

            return email;
        }

        public string SyncOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum)
        {
            return Create(nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum);
        }

        public void Delete( string organizationalRoleID )
        {
            //string account = organizationalRoleID.Substring(0, organizationalRoleID.IndexOf("@"));

            //ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);

            //service.DisableDistributionGroup(account);
        }

        public void ChangeProperty( string organizationalRoleID, PropertyChangeCollection propertyChanges )
        {
            //string account = organizationalRoleID.Substring(0, organizationalRoleID.IndexOf("@"));

            //ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);
            //ExchangeAddressList addressList = service.GetAddressList(group.Name);
            //addressList.Name = Convert.ToString(propertyChanges.Get("Name"));

            //service.UpdateAddressList(addressList);
        }

        public void AddUser( string organizationalRoleID, string userID )
        {
        }

        public void RemoveUser( string organizationalRoleID, string userID )
        {
        }
    }
}