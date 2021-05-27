using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding( Name = "ImportHRUserService", Namespace = Consts.Namespace_HR+"user/", ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public interface IImportHRUserService
    {
        [WebMethod( Description = "同步用户" )]
        [SoapDocumentMethod( Consts.Namespace + "SyncUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string idCard, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, HRPropertyChangeCollection extendProperties);

        [WebMethod( Description = "创建用户" )]
        [SoapDocumentMethod( Consts.Namespace + "CreateUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string Create(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string idCard, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, HRPropertyChangeCollection extendProperties);

        [WebMethod( Description = "删除用户" )]
        [SoapDocumentMethod( Consts.Namespace + "DeleteUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Delete( string userID );

        [WebMethod( Description = "禁用用户" )]
        [SoapDocumentMethod( Consts.Namespace + "DisableUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Disable( string userID, string quitDate );

        [WebMethod(Description = "测试")]
        [SoapDocumentMethod(Consts.Namespace + "TestSync", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void TestSync(int arg1, int arg2);

        [WebMethod( Description = "启用用户" )]
        [SoapDocumentMethod( Consts.Namespace + "EnableUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Enable( string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase);

        [WebMethod( Description = "修改用户属性" )]
        [SoapDocumentMethod( Consts.Namespace + "ChangeUserProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void ChangeProperty( string userID, HRPropertyChangeCollection propertyChanges );
    }
}