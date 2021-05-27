define("/UUM/Widgets/Group/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/Group"
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
        var groupID = Page().getUrlParam("GroupID");
        var limit = 10;

        $(widget).DataList("GroupList").on("itemAdded", function (source, index, item) {
            $(item).Button("btnEditGroup").on("clicked", editHandler);
            $(item).Button("btnEditPermission").on("clicked", editPermissionHandler);
        });

        Page().listenUrlParamChanged(["GroupID"], { container: widget }, function () {
            reloadGroupList(widget);
        });

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('Group', {
                proxy: new InstructionProxy({
                    query: "GroupListQuery"
                })
            }),
            params: {
                "GroupID": groupID,
                "FetchSize": limit
            }
        });

        $(widget).DataList("GroupList").configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });
    };

    var editHandler = function (source) {
        UrlUtil.goTo("#/Group/Edit.htm", {
            GroupID: source.parent.getRecord().get("ID")
        });
    };

    var editPermissionHandler = function (source) {
        var record = source.parent.getRecord();
        var principalID = record.get("ID");
        UrlUtil.goTo("#/Admin/Permission/List.htm", {
            Identifier: principalID,
            Type: "Indigox.Common.Membership.Interfaces.IPrincipal"
        });
    };

    var reloadGroupList = function (widget) {
        var groupID = Page().getUrlParam('GroupID');
        var listControl = $(widget).DataList("GroupList").first();
        var controller = listControl.getController();
        controller.setParam("GroupID", groupID);
        controller.load();
    };

    return exports;
});