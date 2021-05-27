define("/UUM/Widgets/OrganizationalRole/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/OrganizationalRole"
    ],
function (
        UrlUtil,
        StringUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController
) {
    var exports = function (widget) {
        var organizationalPersonID = Page().getUrlParam("OrganizationalRoleID");
        var limit = 10;

        $(widget).DataList("OrganizationalRoleList").on("itemAdded", function (source, index, item) {
            $(item).Button("btnEditOrganizationalRole").on("clicked", editHandler);
        });

        Page().listenUrlParamChanged(["OrganizationalRoleID"], { container: widget }, function () {
            reloadOrganizationalRoleList(widget);
        });

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('OrganizationalRole', {
                proxy: new InstructionProxy({
                    query: "OrganizationalRoleListQuery"
                })
            }),
            params: {
                "OrganizationalRoleID": organizationalPersonID,
                "FetchSize": limit
            }
        });

        $(widget).DataList("OrganizationalRoleList").configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });
    };

    var editHandler = function (source) {
        UrlUtil.goTo("#/OrganizationalRole/Edit.htm", {
            OrganizationalRoleID: source.parent.getRecord().get("ID")
        });
    };

    var reloadOrganizationalRoleList = function (widget) {
        var organizationalPersonID = Page().getUrlParam('OrganizationalRoleID');
        var listControl = $(widget).DataList("OrganizationalRoleList").first();
        var controller = listControl.getController();
        controller.setParam("OrganizationalRoleID", organizationalPersonID);
        controller.load();
    };

    return exports;
});