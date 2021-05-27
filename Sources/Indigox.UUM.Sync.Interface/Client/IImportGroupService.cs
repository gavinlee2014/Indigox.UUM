using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding( Name = "ImportGroupService", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public interface IImportGroupService
    {
        [WebMethod( Description = "同步群组" )]
        [SoapDocumentMethod( Consts.Namespace + "SyncGroup", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string SyncGroup( string nativeID, string name, string email, string description, double orderNum );

        [WebMethod( Description = "创建群组" )]
        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string Create( string nativeID, string name, string email, string description, double orderNum );

        [WebMethod( Description = "删除群组" )]
        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Delete( string groupID );

        [WebMethod( Description = "修改群组属性" )]
        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void ChangeProperty( string groupID, PropertyChangeCollection propertyChanges );

        [WebMethod( Description = "将组织添加到群组" )]
        [SoapDocumentMethod( Consts.Namespace + "AddOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void AddOrganizationalUnit( string groupID, string organizationalUnitID );

        [WebMethod( Description = "从群组移除组织" )]
        [SoapDocumentMethod( Consts.Namespace + "RemoveOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void RemoveOrganizationalUnit( string groupID, string organizationalUnitID );

        [WebMethod( Description = "添加到组织角色到群组" )]
        [SoapDocumentMethod( Consts.Namespace + "AddOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void AddOrganizationalRole( string groupID, string organizationalRoleID );

        [WebMethod( Description = "从群组中移除组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "RemoveOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void RemoveOrganizationalRole( string groupID, string organizationalRoleID );

        [WebMethod( Description = "添加用户到群组" )]
        [SoapDocumentMethod( Consts.Namespace + "AddUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void AddUser( string groupID, string userID );

        [WebMethod( Description = "从群组中移除用户" )]
        [SoapDocumentMethod( Consts.Namespace + "RemoveUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void RemoveUser( string groupID, string userID );
    }
}