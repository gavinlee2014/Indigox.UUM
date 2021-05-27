using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface.Server
{
    [WebServiceBinding( Name = "ExportOrganizationalUnitServiceSoap", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    [XmlInclude( typeof( Principal ) )]
    public interface IExportOrganizationalUnitService
    {
        [WebMethod( Description = "获取集团部门" )]
        [SoapDocumentMethod( Consts.Namespace + "GetCorporation", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalUnit GetCorporation();

        [WebMethod( Description = "获取子部门" )]
        [SoapDocumentMethod( Consts.Namespace + "GetChildOrganizations", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalUnit[] GetChildOrganizations( string organizationalID );

        [WebMethod( Description = "获取部门包含的用户" )]
        [SoapDocumentMethod( Consts.Namespace + "GetUsers", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalPerson[] GetUsers( string organizationalID );

        [WebMethod( Description = "获取部门包含的组织角色" )]
        [SoapDocumentMethod( Consts.Namespace + "GetOrganizationalRoles", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        OrganizationalRole[] GetOrganizationalRoles( string organizationID );

        [WebMethod(Description = "获取所有部门")]
        [SoapDocumentMethod(Consts.Namespace + "GetAllOrganizations", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        OrganizationalUnit[] GetAllOrganizations();
    }
}