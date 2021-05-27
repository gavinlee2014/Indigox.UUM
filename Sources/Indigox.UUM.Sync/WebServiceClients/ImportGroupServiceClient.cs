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
    [WebServiceBinding( Name = "ImportGroupService", Namespace = Consts.Namespace )]
    [XmlInclude( typeof( DTO_Principal ) )]
    public class ImportGroupServiceClient : SoapHttpClientProtocol, IImportGroupService
    {
        public ImportGroupServiceClient( string url )
        {
            this.Url = url;
        }

        [SoapDocumentMethod( Consts.Namespace + "SyncGroup", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string SyncGroup( string nativeID, string name, string email, string description, double orderNum )
        {
            object[] results = this.Invoke( "SyncGroup", new object[] { nativeID, name, email, description, orderNum } );
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string Create( string nativeID, string name, string email, string description, double orderNum )
        {
            object[] results = this.Invoke( "Create", new object[] { nativeID, name, email, description, orderNum } );
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void Delete( string groupID )
        {
            this.Invoke( "Delete", new object[] { groupID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void ChangeProperty( string groupID, PropertyChangeCollection propertyChanges )
        {
            this.Invoke( "ChangeProperty", new object[] { groupID, propertyChanges } );
        }

        [SoapDocumentMethod( Consts.Namespace + "AddOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void AddOrganizationalUnit( string groupID, string organizationalUnitID )
        {
            this.Invoke( "AddOrganizationalUnit", new object[] { groupID, organizationalUnitID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "RemoveOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void RemoveOrganizationalUnit( string groupID, string organizationalUnitID )
        {
            this.Invoke( "RemoveOrganizationalUnit", new object[] { groupID, organizationalUnitID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "AddOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void AddOrganizationalRole( string groupID, string organizationalRoleID )
        {
            this.Invoke( "AddOrganizationalRole", new object[] { groupID, organizationalRoleID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "RemoveOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void RemoveOrganizationalRole( string groupID, string organizationalRoleID )
        {
            this.Invoke( "RemoveOrganizationalRole", new object[] { groupID, organizationalRoleID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "AddUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void AddUser( string groupID, string userID )
        {
            this.Invoke( "AddUser", new object[] { groupID, userID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "RemoveUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void RemoveUser( string groupID, string userID )
        {
            this.Invoke( "RemoveUser", new object[] { groupID, userID } );
        }
    }
}