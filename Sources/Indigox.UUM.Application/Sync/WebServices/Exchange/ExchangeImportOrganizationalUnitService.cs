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

namespace Indigox.UUM.Application.Sync.WebServices.Exchange
{
    [WebService( Name = "ExchangeImportOrganizationalUnitService", Namespace = Consts.Namespace )]
    public class ExchangeImportOrganizationalUnitService : IImportOrganizationalUnitService
    {
        private static List<string> CreateAddressListRequiredType = new List<string>();

        static ExchangeImportOrganizationalUnitService()
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

        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, PropertyChangeCollection extendProperties)
        {
            string externalID = "";
            Log.Debug("exchange Create:" + email + ",type:" + organizationalUnitType);
            string account = email.Substring(0, email.IndexOf("@"));
            Log.Debug("exchange Create:" + account);
            Log.Debug("exchange parentOrganizationalUnitID:" + parentOrganizationalUnitID);
            string parentAccount = null;
            string parentAddressListId = null;
            if (!String.IsNullOrEmpty(parentOrganizationalUnitID))
            {
                parentAccount = parentOrganizationalUnitID.Substring(0, parentOrganizationalUnitID.IndexOf("@"));
                parentAddressListId = parentOrganizationalUnitID.Substring(parentOrganizationalUnitID.IndexOf(",") + 1);
                Log.Debug("exchange parentAccount:" + parentAccount);
            }

            ExchangeManagerService service = new ExchangeManagerService();

            service.EnableDistributionGroup(account);

            externalID = email;

            if (IsCreateAddressListRequired(organizationalUnitType))
            {
                string addressListName = name;
                string addressListDisplayName = displayName;

                ExchangeGroup group = service.GetDistributionGroup(account);
                group.Name = addressListName;
                group.DisplayName = addressListDisplayName;

                ExchangeGroup parentGroup = null;
                ExchangeAddressList parent = null;

                /*
                 * 修改时间：2018-08-26
                 * 修改人：曾勇
                 * 修改逻辑：原来是根据部门名称直接去查找已经存在的地址簿，这样是在全体里查找。
                 * 所以能够查到，导致未新建地址簿，修改在父地址簿下根据名称去查找地址簿。
                 */
                if (!String.IsNullOrEmpty(parentAccount))
                {
                    parentGroup = service.GetDistributionGroup(parentAccount);
                    if (parentGroup != null)
                    {
                        string parentName = parentGroup.Name;
                        if (parentName.EndsWith("全体"))
                        {
                            parentName = parentName.Substring(0, parentName.Length - 2);
                        }
                        parent = service.GetAddressList(parentAddressListId);
                        Log.Debug("parent group: " + parentGroup.Name);
                        Log.Debug("parent address list is NULL: " + (parent == null)
                            + "\r\ngroup is NULL: " + (group == null));
                        if(parent != null)
                        {
                            Log.Debug("parent address list: " + parent.Identity +
                            "\r\ngroup name: " + group.Name + "group display name: " + group.DisplayName + "group organizational unit : " + group.OrganizationalUnit);
                        }
                        
                    }
                }

                
                ExchangeAddressList addressList = service.GetAddressList(addressListName, parent);

                if (addressList == null)
                {
                    addressList = service.CreateAddressList(group, parent);
                }

                externalID = externalID + "," + addressList.ID;
            }

            return externalID;
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, PropertyChangeCollection extendProperties)
        {
            return Create(nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties);
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
                    string addressListName = Convert.ToString(propertyChanges.Get("Name"));
                    string addressListDisplayName = Convert.ToString(propertyChanges.Get("DisplayName"));
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

                    service.UpdateAddressList(addressList);
                }
            }
        }

        public void AddOrganizationalUnit( string parentOrganizationalUnitID, string organizationalUnitID )
        {
            string parentAddressListID = null, addressListID = null;
            if (organizationalUnitID.IndexOf(",") > 0)
            {
                addressListID = organizationalUnitID.Substring(organizationalUnitID.IndexOf(",") + 1);
            }

            if (organizationalUnitID.IndexOf(",") > 0)
            {
                parentAddressListID = parentOrganizationalUnitID.Substring(parentOrganizationalUnitID.IndexOf(",") + 1);
            }

            ExchangeManagerService service = new ExchangeManagerService();

            if (!String.IsNullOrEmpty(addressListID))
            {
                ExchangeAddressList addressList = service.GetAddressList(addressListID);
                ExchangeAddressList parentAddressList = service.GetAddressList(parentAddressListID);
                if ((addressList != null) && (parentAddressList != null))
                {
                    service.MoveAddressList(addressList, parentAddressList);
                }
            }
        }

        public void RemoveOrganizationalUnit( string parentOrganizationalUnitID, string organizationalUnitID )
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
                ExchangeAddressList rootAddressList = new ExchangeAddressList();
                rootAddressList.Identity = "\\";
                ExchangeAddressList addressList = service.GetAddressList(addressListID);
                if (addressList != null)
                {
                    service.MoveAddressList(addressList, rootAddressList);
                }
            }
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