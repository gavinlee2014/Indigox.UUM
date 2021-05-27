using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
using System.Web.Services.Description;

namespace Indigox.UUM.Sync.Interface.Server
{
    [WebServiceBinding(Name = "ExportUserServiceSoap", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1)]
    [XmlInclude(typeof(Principal))]
    public interface IExportUserService
    {
        [WebMethod(Description = "获取所有用户")]
        [SoapDocumentMethod(Consts.Namespace + "GetAllUsers", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        OrganizationalUser[] GetAllUsers();
    }
}
