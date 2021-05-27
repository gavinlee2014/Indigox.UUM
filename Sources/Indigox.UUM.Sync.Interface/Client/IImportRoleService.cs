using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding( Name = "ImportRoleService", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public interface IImportRoleService
    {
        [WebMethod( Description = "同步角色" )]
        [SoapDocumentMethod( Consts.Namespace + "SyncRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string SyncRole( string nativeID, string name, string email, string description, double orderNum );

        [WebMethod( Description = "创建角色" )]
        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string Create( string nativeID, string name, string email, string description, double orderNum );

        [WebMethod( Description = "删除角色" )]
        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Delete( string roleID );

        [WebMethod( Description = "修改角色属性" )]
        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void ChangeProperty( string roleID, PropertyChangeCollection propertyChanges );

        [WebMethod( Description = "添加组织角色到角色" )]
        [SoapDocumentMethod( Consts.Namespace + "AddOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void AddOrganizationalRole( string roleID, string organizationalRoleID );

        [WebMethod( Description = "从角色中移除组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "RemoveOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void RemoveOrganizationalRole( string roleID, string organizationalRoleID );
    }
}