using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface.Server
{
    [WebServiceBinding( Name = "ExportRoleServiceSoap", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    [XmlInclude( typeof( Principal ) )]
    public interface IExportRoleService
    {
        [WebMethod( Description = "获取所有角色" )]
        [SoapDocumentMethod( Consts.Namespace + "GetAllRoles", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        Role[] GetAllRoles();

        [WebMethod( Description = "获取角色包含的组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "GetOrganizationalRoles", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalRole[] GetOrganizationalRoles( string roleID );
    }
}