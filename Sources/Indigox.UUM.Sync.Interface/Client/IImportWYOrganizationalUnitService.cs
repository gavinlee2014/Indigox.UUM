using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Indigox.UUM.Sync.Interface.Client
{
    [WebServiceBinding( Name = "ImportWYOrganizationalUnitService", Namespace = Consts.Namespace, ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public interface IImportWYOrganizationalUnitService
    {
        [WebMethod( Description = "同步组织" )]
        [SoapDocumentMethod( Consts.Namespace + "SyncOrganizationalUnit", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name);

        [WebMethod( Description = "创建组织" )]
        [SoapDocumentMethod( Consts.Namespace + "Create", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        string Create(string nativeID, string parentOrganizationalUnitID, string name);

        [WebMethod( Description = "删除组织" )]
        [SoapDocumentMethod( Consts.Namespace + "Delete", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void Delete( string organizationalUnitID );

        [WebMethod( Description = "修改组织属性" )]
        [SoapDocumentMethod( Consts.Namespace + "ChangeProperty", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped )]
        void ChangeProperty( string organizationalUnitID, string parentOrganizationalUnitID, string name);

        [WebMethod(Description = "人工同步到UUM")]
        [SoapDocumentMethod(Consts.Namespace + "Synchronization", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void Synchronization(int orgLevel, int syncNumber);

        [WebMethod(Description = "同步Exchange错误任务")]
        [SoapDocumentMethod(Consts.Namespace + "SyncError", RequestNamespace = Consts.Namespace, ResponseNamespace = Consts.Namespace, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        void SyncError();
    }
}