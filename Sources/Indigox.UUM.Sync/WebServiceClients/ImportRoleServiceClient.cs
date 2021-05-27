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
    [WebServiceBinding( Name = "ImportRoleService", Namespace = Consts.Namespace )]
    [XmlInclude( typeof( DTO_Principal ) )]
    public class ImportRoleServiceClient : SoapHttpClientProtocol, IImportRoleService
    {
        public ImportRoleServiceClient( string url )
        {
            this.Url = url;
        }

        [SoapDocumentMethod( Consts.Namespace + "SyncRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string SyncRole( string nativeID, string name, string email, string description, double orderNum )
        {
            object[] results = this.Invoke( "SyncRole", new object[] { nativeID, name, email, description, orderNum } );
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            object[] results = this.Invoke( "Create", new object[] { nativeID, name, email, description, orderNum } );
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void Delete( string roleID )
        {
            this.Invoke( "Delete", new object[] { roleID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void ChangeProperty( string roleID, PropertyChangeCollection propertyChanges )
        {
            this.Invoke( "ChangeProperty", new object[] { roleID, propertyChanges } );
        }

        [SoapDocumentMethod( Consts.Namespace + "AddOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void AddOrganizationalRole( string roleID, string organizationalRoleID )
        {
            this.Invoke( "AddOrganizationalRole", new object[] { roleID, organizationalRoleID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "RemoveOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void RemoveOrganizationalRole( string roleID, string organizationalRoleID )
        {
            this.Invoke( "RemoveOrganizationalRole", new object[] { roleID, organizationalRoleID } );
        }
    }
}