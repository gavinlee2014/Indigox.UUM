using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using DTO_Principal = Indigox.UUM.Sync.Interface.Principal;

namespace Indigox.UUM.Sync.WebServiceClients
{
    [WebServiceBinding(Name = "ImportOrganizationalUnitService", Namespace = Consts.Namespace)]
    [XmlInclude(typeof(DTO_Principal))]
    public class ImportOrganizationalUnitServiceClient : SoapHttpClientProtocol, IImportOrganizationalUnitService
    {
        public ImportOrganizationalUnitServiceClient(string url)
        {
            this.Url = url;
        }

        [SoapDocumentMethod(Consts.Namespace + "SyncOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, PropertyChangeCollection extendProperties)
        {
            object[] results = this.Invoke("SyncOrganizationalUnit", new object[] { nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties });
            return (string)results[0];
        }

        [SoapDocumentMethod(Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, PropertyChangeCollection extendProperties)
        {
            object[] results = this.Invoke("Create", new object[] { nativeID, parentOrganizationalUnitID, name, fullName, displayName, email, description, orderNum, organizationalUnitType, extendProperties });
            return (string)results[0];
        }

        [SoapDocumentMethod(Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void Delete(string organizationalUnitID)
        {
            this.Invoke("Delete", new object[] { organizationalUnitID });
        }

        [SoapDocumentMethod(Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void ChangeProperty(string organizationalUnitID, PropertyChangeCollection propertyChanges)
        {
            this.Invoke("ChangeProperty", new object[] { organizationalUnitID, propertyChanges });
        }

        [SoapDocumentMethod(Consts.Namespace + "AddOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void AddOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            this.Invoke("AddOrganizationalUnit", new object[] { parentOrganizationalUnitID, organizationalUnitID });
        }

        [SoapDocumentMethod(Consts.Namespace + "RemoveOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void RemoveOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID)
        {
            this.Invoke("RemoveOrganizationalUnit", new object[] { parentOrganizationalUnitID, organizationalUnitID });
        }

        [SoapDocumentMethod(Consts.Namespace + "AddOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void AddOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            this.Invoke("AddOrganizationalRole", new object[] { organizationalUnitID, organizationalRoleID });
        }

        [SoapDocumentMethod(Consts.Namespace + "RemoveOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void RemoveOrganizationalRole(string organizationalUnitID, string organizationalRoleID)
        {
            this.Invoke("RemoveOrganizationalRole", new object[] { organizationalUnitID, organizationalRoleID });
        }

        [SoapDocumentMethod(Consts.Namespace + "AddUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void AddUser(string organizationalUnitID, string userID)
        {
            this.Invoke("AddUser", new object[] { organizationalUnitID, userID });
        }

        [SoapDocumentMethod(Consts.Namespace + "RemoveUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void RemoveUser(string organizationalUnitID, string userID)
        {
            this.Invoke("RemoveUser", new object[] { organizationalUnitID, userID });
        }
    }
}