using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface.Server
{
    [WebServiceBinding( Name = "ExportGroupServiceSoap", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    [XmlInclude( typeof( Principal ) )]
    public interface IExportGroupService
    {
        [WebMethod( Description = "获取所有群组" )]
        [SoapDocumentMethod( Consts.Namespace + "GetAllGroups", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        Group[] GetAllGroups();

        [WebMethod( Description = "获取群组包含的组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "GetOrganizationalRoles", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalRole[] GetOrganizationalRoles( string groupID );

        [WebMethod( Description = "获取群组包含的组织" )]
        [SoapDocumentMethod( Consts.Namespace + "GetOrganizationalUnits", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalUnit[] GetOrganizationalUnits( string groupID );

        [WebMethod( Description = "获取群组包含的用户" )]
        [SoapDocumentMethod( Consts.Namespace + "GetUsers", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalPerson[] GetUsers( string groupID );
    }
}