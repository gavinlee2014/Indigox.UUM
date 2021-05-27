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
    [WebServiceBinding( Name = "ImportOrganizationalRoleService", Namespace = Consts.Namespace )]
    [XmlInclude( typeof( DTO_Principal ) )]
    public class ImportOrganizationalRoleServiceClient : SoapHttpClientProtocol, IImportOrganizationalRoleService
    {
        public ImportOrganizationalRoleServiceClient( string url )
        {
            this.Url = url;
        }

        [SoapDocumentMethod( Consts.Namespace + "SyncOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string SyncOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, PropertyChangeCollection extendProperties)
        {
            object[] results = this.Invoke("SyncOrganizationalRole", new object[] { nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum, extendProperties });
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string Create(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, PropertyChangeCollection extendProperties)
        {
            object[] results = this.Invoke("Create", new object[] { nativeID, organizationalUnitID, name, fullName, displayName, email, description, orderNum, extendProperties });
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void Delete( string organizationalRoleID )
        {
            this.Invoke( "Delete", new object[] { organizationalRoleID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void ChangeProperty( string organizationalRoleID, PropertyChangeCollection propertyChanges )
        {
            this.Invoke( "ChangeProperty", new object[] { organizationalRoleID, propertyChanges } );
        }

        [SoapDocumentMethod( Consts.Namespace + "AddUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void AddUser( string organizationalRoleID, string userID )
        {
            this.Invoke( "AddUser", new object[] { organizationalRoleID, userID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "RemoveUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void RemoveUser( string organizationalRoleID, string userID )
        {
            this.Invoke( "RemoveUser", new object[] { organizationalRoleID, userID } );
        }
    }
}