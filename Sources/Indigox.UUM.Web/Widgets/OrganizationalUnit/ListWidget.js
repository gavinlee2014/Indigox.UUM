define("/UUM/Widgets/OrganizationalUnit/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
function (
        StringUtil,
        UrlUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        PageUrlMonitor
) {
    var exports = function (widget) {
        var organizationalUnitID = Page().getUrlParam("OrganizationalUnitID");
        var limit = 10;

        $(widget).DataList("OrganizationalUnitList").on("itemAdded", function (source, index, item) {
            $(item).Button("btnEditOrganizationalUnit").on("clicked", editHandler);
        });

        new PageUrlMonitor({ paramFilters: ["OrganizationalUnitID"], container: widget })
            .onUrlParamChanged(function () {
                reloadOrganizationalUnitList(widget);
            });

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('OrganizationalUnit', {
                proxy: new InstructionProxy({
                    query: "OrganizationalUnitListQuery"
                })
            }),
            params: {
                "OrganizationalUnitID": organizationalUnitID,
                "FetchSize": limit
            }
        });

        $(widget).DataList("OrganizationalUnitList").configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });
    };

    var editHandler = function (source) {
        UrlUtil.goTo("#/OrganizationalUnit/Edit.htm", {
            OrganizationalUnitID: source.parent.getRecord().get("ID")
        });
    };

    var reloadOrganizationalUnitList = function (widget) {
        var organizationalUnitID = Page().getUrlParam('OrganizationalUnitID');
        var listControl = $(widget).DataList("OrganizationalUnitList").first();
        var controller = listControl.getController();
        controller.setParam("OrganizationalUnitID", organizationalUnitID);
        controller.load();
    };

    return exports;
});