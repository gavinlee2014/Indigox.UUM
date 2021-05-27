using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding( Name = "ImportWYUserService", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public interface IImportEHRUserService
    {
        [WebMethod( Description = "同步用户" )]
        [SoapDocumentMethod( Consts.Namespace + "SyncUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string SyncUser(string nativeID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait);

        [WebMethod( Description = "创建用户" )]
        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string Create(string nativeID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait);

        [WebMethod( Description = "删除用户" )]
        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Delete( string userID );

        [WebMethod( Description = "禁用用户" )]
        [SoapDocumentMethod( Consts.Namespace + "Disable", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Disable( string userID,DateTime quitDate);

        [WebMethod( Description = "启用用户" )]
        [SoapDocumentMethod( Consts.Namespace + "Enable", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Enable( string userID);

        [WebMethod( Description = "修改用户属性" )]
        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void ChangeProperty( string userID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait);
    }
}