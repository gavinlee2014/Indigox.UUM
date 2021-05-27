using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface.Server
{
    [WebServiceBinding( Name = "ExportUserServiceSoap", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    [XmlInclude( typeof( Principal ) )]
    public interface IExportUserService
    {
        [WebMethod( Description = "获取所有用户" )]
        [SoapDocumentMethod( Consts.Namespace + "GetAllUsers", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalPerson[] GetAllUsers();

        [WebMethod(Description = "获取所有用户包括禁用用户")]
        [SoapDocumentMethod(Consts.Namespace + "GetAllUsersWithDisabled", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        OrganizationalPerson[] GetAllUsersWithDisabled();

        [WebMethod(Description = "获取一部分用户")]
        [SoapDocumentMethod(Consts.Namespace + "GetUsers", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        OrganizationalPerson[] GetUsers(int startIndex, int limit);

        [WebMethod(Description = "获取用户组HR系统中的编码")]
        [SoapDocumentMethod(Consts.Namespace + "GetHREmployeeCode", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        string GetHREmployeeCode(string id);

        [WebMethod(Description = "根据ID获取用户")]
        [SoapDocumentMethod(Consts.Namespace + "GetUserByID", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        OrganizationalPerson GetUserByID(string userID);

    }
}