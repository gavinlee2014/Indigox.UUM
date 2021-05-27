using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding(Name = "ImportHROrganizationalUnitService", Namespace = Consts.Namespace_HR+"organizationalunit/", ConformsTo = WsiProfiles.BasicProfile1_1)]
    public interface IImportHROrganizationalUnitService
    {
        [WebMethod(Description = "同步组织")]
        [SoapDocumentMethod(Consts.Namespace + "SyncOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, HRPropertyChangeCollection extendProperties);

        [WebMethod(Description = "创建组织")]
        [SoapDocumentMethod(Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        string Create(string nativeID, string parentOrganizationalUnitID, string name, string fullName, string displayName, string email, string description, double orderNum, string organizationalUnitType, HRPropertyChangeCollection extendProperties);

        [WebMethod(Description = "删除组织")]
        [SoapDocumentMethod(Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void Delete(string organizationalUnitID);

        [WebMethod(Description = "修改组织属性")]
        [SoapDocumentMethod(Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void ChangeProperty(string organizationalUnitID, HRPropertyChangeCollection propertyChanges);

        [WebMethod(Description = "添加下级组织")]
        [SoapDocumentMethod(Consts.Namespace + "AddOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void AddOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID);

        [WebMethod(Description = "移除下级组织")]
        [SoapDocumentMethod(Consts.Namespace + "RemoveOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void RemoveOrganizationalUnit(string parentOrganizationalUnitID, string organizationalUnitID);

        [WebMethod(Description = "添加组织角色")]
        [SoapDocumentMethod(Consts.Namespace + "AddOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void AddOrganizationalRole(string organizationalUnitID, string organizationalRoleID);

        [WebMethod(Description = "移除组织角色")]
        [SoapDocumentMethod(Consts.Namespace + "RemoveOrganizationalRole", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void RemoveOrganizationalRole(string organizationalUnitID, string organizationalRoleID);

        [WebMethod(Description = "添加用户到组织")]
        [SoapDocumentMethod(Consts.Namespace + "AddUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void AddUser(string organizationalUnitID, string userID);

        [WebMethod(Description = "从组织中移除用户")]
        [SoapDocumentMethod(Consts.Namespace + "RemoveUser", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void RemoveUser(string organizationalUnitID, string userID);
    }
}
