using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using DTO_Principal = Indigox.UUM.Sync.Interface.Principal;

namespace Indigox.UUM.Sync.WebServiceClients
{
    [WebServiceBinding( Name = "ImportUserService", Namespace = Consts.Namespace )]
    [XmlInclude( typeof( DTO_Principal ) )]
    public class ImportUserServiceClient : SoapHttpClientProtocol, IImportUserService
    {
        public ImportUserServiceClient( string url )
        {
            this.Url = url;
        }

        /*
         * 修改时间：2018-08-27
         * 修改人：曾勇
         * 修改Bug：调用SyncUser和Create方法时，未将参数mailDatabase传入
         */

        [SoapDocumentMethod(Consts.Namespace + "SyncUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, PropertyChangeCollection extendProperties)
        {
            object[] results = this.Invoke("SyncUser", new object[] { nativeID, organizationalUnitID, accountName, name, fullName, displayName, email, title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase, extendProperties });
            return (string)results[0];
        }

        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public string Create(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, PropertyChangeCollection extendProperties)
        {
            object[] results = this.Invoke( "Create", new object[] { nativeID, organizationalUnitID, accountName, name, fullName, displayName, email, title, mobile, telephone, fax, orderNum, description, otherContact ,portrait, mailDatabase, extendProperties } );
            return (string)results[ 0 ];
        }

        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void Delete( string userID )
        {
            this.Invoke( "Delete", new object[] { userID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "Disable", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void Disable( string userID )
        {
            this.Invoke( "Disable", new object[] { userID } );
        }

        [SoapDocumentMethod( Consts.Namespace + "Enable", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void Enable( string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            this.Invoke( "Enable", new object[] {  userID,  organizationalUnitID,  accountName,  name,  fullName,  displayName,  email,  title,  mobile,  telephone,  fax,  orderNum,  description,  otherContact,  portrait,  mailDatabase} );
        }

        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        public void ChangeProperty( string userID, PropertyChangeCollection propertyChanges )
        {
            this.Invoke( "ChangeProperty", new object[] { userID, propertyChanges } );
        }
    }
}