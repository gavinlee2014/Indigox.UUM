define("/UUM/Widgets/OrganizationalPerson/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/OrganizationalPerson"
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
        var organizationalPersonID = Page().getUrlParam("OrganizationalPersonID");
        var limit = 10;

        $(widget).DataList("OrganizationalPersonList").on("itemAdded", function (source, index, item) {
            $(item).Button("btnEditOrganizationalPerson").on("clicked", editHandler);
        });

        Page().listenUrlParamChanged(["OrganizationalPersonID"], { container: widget }, function () {
            reloadOrganizationalPersonList(widget);
        });

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                proxy: new InstructionProxy({
                    query: "OrganizationalPersonListQuery"
                })
            }),
            params: {
                "OrganizationalPersonID": organizationalPersonID,
                "FetchSize": limit
            }
        });

        $(widget).DataList("OrganizationalPersonList").configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });
    };

    var editHandler = function (source) {
        UrlUtil.goTo("#/OrganizationalPerson/Edit.htm", {
            OrganizationalPersonID: source.parent.getRecord().get("ID")
        });
    };

    var reloadOrganizationalPersonList = function (widget) {
        var organizationalPersonID = Page().getUrlParam('OrganizationalPersonID');
        var listControl = $(widget).DataList("OrganizationalPersonList").first();
        var controller = listControl.getController();
        controller.setParam("OrganizationalPersonID", organizationalPersonID);
        controller.load();
    };

    return exports;
});