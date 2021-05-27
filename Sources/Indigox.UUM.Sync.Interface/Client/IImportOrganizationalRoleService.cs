using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding( Name = "ImportOrganizationalRoleService", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public interface IImportOrganizationalRoleService
    {
        [WebMethod( Description = "同步组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "SyncOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string SyncOrganizationalRole(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, PropertyChangeCollection extendProperties);

        [WebMethod( Description = "创建组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string Create(string nativeID, string organizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, PropertyChangeCollection extendProperties);

        [WebMethod( Description = "删除组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Delete( string organizationalRoleID );

        [WebMethod( Description = "修改组织角色属性" )]
        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void ChangeProperty( string organizationalRoleID, PropertyChangeCollection propertyChanges );

        [WebMethod( Description = "添加用户到组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "AddUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void AddUser( string organizationalRoleID, string userID );

        [WebMethod( Description = "从组织角色中移除用户" )]
        [SoapDocumentMethod( Consts.Namespace + "RemoveUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void RemoveUser( string organizationalRoleID, string userID );
    }
}