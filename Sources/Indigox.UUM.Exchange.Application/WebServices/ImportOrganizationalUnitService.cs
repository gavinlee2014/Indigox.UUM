using System;
using System.Web.Services;
using Indigox.Common.ExchangeManager;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using ExchangeAddressList = Indigox.Common.ExchangeManager.AddressList;
using ExchangeGroup = Indigox.Common.ExchangeManager.Group;
using Indigox.Common.Logging;
using System.Collections.Generic;
using System.Configuration;

namespace Indigox.UUM.Exchange.Application.WebServices
{
    [WebService( Name = "ExchangeImportOrganizationalUnitService", Namespace = Consts.Namespace )]
    public class ImportOrganizationalUnitService : IImportOrganizationalUnitService
    {
        private static List<string> CreateAddressListRequiredType = new List<string>();

        private string GetAdressListName(string name,string displayName)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z]+\.");
            string prefix = reg.Match(displayName).Value;
            return prefix + name;
        }
        private string GetAddressListDisplayName(string name, string displayName)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z]+\.");
            string prefix = reg.Match(displayName).Value;
            return prefix + name;
        }

        static ImportOrganizationalUnitService()
        {
            string createAddressListRequiredType = ConfigurationManager.AppSettings["CreateAddressListRequiredType"];
            if (!String.IsNullOrEmpty(createAddressListRequiredType))
            {
                CreateAddressListRequiredType.AddRange(createAddressListRequiredType.Split(','));
            }
        }

        private bool IsCreateAddressListRequired(string organizationalUnitType)
        {
            return CreateAddressListRequiredType.Contains(organizationalUnitType);
        }

        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType)
        {
            string externalID = "";
            Log.Debug("exchange Create:" + email + ",type:" + organizationalUnitType);
            string account = email.Substring(0, email.IndexOf("@"));
            Log.Debug("exchange Create:" + account);
            Log.Debug("exchange parentOrganizationalUnitID:" + parentOrganizationalUnitID);
            string parentAccount = null;
            if (!String.IsNullOrEmpty(parentOrganizationalUnitID))
            {
                parentAccount = parentOrganizationalUnitID.Substring(0, parentOrganizationalUnitID.IndexOf("@"));
                Log.Debug("exchange parentAccount:" + parentAccount);
            }

            ExchangeManagerService service = new ExchangeManagerService();

            service.EnableDistributionGroup(account);

            externalID = email;

            if (IsCreateAddressListRequired(organizationalUnitType))
            {
                string addressListName = GetAdressListName(name,displayName) ;
                string addressListDisplayName = GetAddressListDisplayName(name,displayName);

                ExchangeGroup group = service.GetDistributionGroup(account);
                group.Name = addressListName;
                group.DisplayName = addressListName;

                ExchangeAddressList addressList = service.GetAddressList(addressListName);

                if (addressList == null)
                {
                    string[] ids=parentOrganizationalUnitID.Split(',');
                    ExchangeAddressList parent=null;
                    if(ids.Length==2)
                    {
                        string parentAddressListGuid = ids[1];
                        parent = service.GetAddressList(parentAddressListGuid);
                    }

                    addressList = service.CreateAdressListWithFilter (group, parent);
                }

                externalID = externalID + "," + addressList.ID;
            }

            return externalID;
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType)
        {
            return Create(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType);
        }

        public void Delete( string organizationalUnitID )
        {
            string addressListID = null;
            if (organizationalUnitID.IndexOf(",") > 0)
            {
                addressListID = organizationalUnitID.Substring(organizationalUnitID.IndexOf(",") + 1);
            }
            string account = organizationalUnitID.Substring(0, organizationalUnitID.IndexOf("@"));

            ExchangeManagerService service = new ExchangeManagerService();

            if (!String.IsNullOrEmpty(addressListID))
            {
                ExchangeAddressList addressList = service.GetAddressList(addressListID);
                if (addressList != null)
                {
                    service.DeleteAddressList(addressList);
                }
            }

            //service.DisableDistributionGroup(account);
        }

        public void ChangeProperty( string organizationalUnitID, PropertyChangeCollection propertyChanges )
        {
            Log.Debug("change property of:" + organizationalUnitID);
            string addressListID = null;
            if (organizationalUnitID.IndexOf(",") > 0)
            {
                addressListID = organizationalUnitID.Substring(organizationalUnitID.IndexOf(",") + 1);
            }
            string account = organizationalUnitID.Substring(0, organizationalUnitID.IndexOf("@"));

            ExchangeManagerService service = new ExchangeManagerService();

            //ExchangeGroup group = service.GetDistributionGroup(account);
            if (!String.IsNullOrEmpty(addressListID))
            {
                ExchangeAddressList addressList = service.GetAddressList(addressListID);
                if (addressList != null)
                {
                    string name = Convert.ToString(propertyChanges.Get("Name"));
                    string displayName = Convert.ToString(propertyChanges.Get("DisplayName"));
                    string addressListName = GetAdressListName(name, displayName);
                    string addressListDisplayName = GetAddressListDisplayName(name, displayName);

                    if (addressListName.EndsWith("全体"))
                    {
                        addressListName = addressListName.Substring(0, addressListName.Length - 2);
                    }
                    if (addressListDisplayName.EndsWith("全体"))
                    {
                        addressListDisplayName = addressListDisplayName.Substring(0, addressListDisplayName.Length - 2);
                    }

                    addressList.Name = addressListName;
                    addressList.DisplayName = addressListDisplayName;

                    service.UpdateAddressListWithFilter(addressList);
                }
            }
        }

        public void AddOrganizationalUnit( string parentOrganizationalUnitID, string organizationalUnitID )
        {
        }

        public void RemoveOrganizationalUnit( string parentOrganizationalUnitID, string organizationalUnitID )
        {
        }

        public void AddOrganizationalRole( string organizationalUnitID, string organizationalRoleID )
        {
        }

        public void RemoveOrganizationalRole( string organizationalUnitID, string organizationalRoleID )
        {
        }

        public void AddUser( string organizationalUnitID, string userID )
        {
        }

        public void RemoveUser( string organizationalUnitID, string userID )
        {
        }
    }
}