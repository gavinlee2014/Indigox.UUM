using System;
using System.Web.Services;
using Indigox.Common.ExchangeManager;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ExchangeAddressList = Indigox.Common.ExchangeManager.AddressList;
using ExchangeGroup = Indigox.Common.ExchangeManager.Group;

namespace Indigox.UUM.Exchange.Application.WebServices
{
    [WebService( Name = "ExchangeImportGroupService", Namespace = Consts.Namespace )]
    public class ImportGroupService : IImportGroupService
    {
        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            string account = email.Substring(0, email.IndexOf("@"));

            ExchangeManagerService service = new ExchangeManagerService();

            service.EnableDistributionGroup(account);

            //ExchangeGroup group = service.GetDistributionGroup(account);

            //service.CreateAddressList(group, null);

            return email;
        }

        public string SyncGroup( string nativeID, string name, string email, string description, double orderNum )
        {
            return Create(nativeID, name, email, description, orderNum);
        }

        public void Delete( string groupID )
        {
            //string account = groupID.Substring(0, groupID.IndexOf("@"));

            //ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);

            //service.DisableDistributionGroup(account);
        }

        public void ChangeProperty( string groupID, PropertyChangeCollection propertyChanges )
        {
            //string account = groupID.Substring(0, groupID.IndexOf("@"));

            //ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);
            //ExchangeAddressList addressList = service.GetAddressList(group.Name);
            //addressList.Name = Convert.ToString(propertyChanges.Get("Name"));

            //service.UpdateAddressList(addressList);
        }

        public void AddOrganizationalUnit( string groupID, string organizationalUnitID )
        {
        }

        public void RemoveOrganizationalUnit( string groupID, string organizationalUnitID )
        {
        }

        public void AddOrganizationalRole( string groupID, string organizationalRoleID )
        {
        }

        public void RemoveOrganizationalRole( string groupID, string organizationalRoleID )
        {
        }

        public void AddUser( string groupID, string userID )
        {
        }

        public void RemoveUser( string groupID, string userID )
        {
        }
    }
}