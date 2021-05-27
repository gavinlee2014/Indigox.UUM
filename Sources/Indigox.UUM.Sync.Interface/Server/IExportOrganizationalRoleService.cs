using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface.Server
{
    [WebServiceBinding( Name = "ExportOrganizationalRoleServiceSoap", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    [XmlInclude( typeof( Principal ) )]
    public interface IExportOrganizationalRoleService
    {
        [WebMethod( Description = "获取组织角色包含的用户" )]
        [SoapDocumentMethod( Consts.Namespace + "GetUsers", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalPerson[] GetUsers( string organizationalRoleID );
    }
}