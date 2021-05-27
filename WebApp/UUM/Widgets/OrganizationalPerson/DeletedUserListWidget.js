define("/UUM/Widgets/OrganizationalPerson/DeletedUserListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox/UUM/Application/OrganizationalPerson"
    ],
function (
        UrlUtil,
        StringUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController,
        ItemMode
) {
    var exports = function (widget) {
        var limit = 10;

        $(widget).DataList("OrganizationalPersonList").on("itemAdded", function (source, index, item) {
            $(item).Button("btnEditOrganizationalPerson").on("clicked", editHandler);
        });

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                proxy: new InstructionProxy({
                    query: "DeletedUserListQuery"
                })
            }),
            params: {
                "FetchSize": limit
            }
        });

        $(widget).DataList("OrganizationalPersonList").first().configure({
            controller: controller
        });

        $(widget).Paging("Paging").first().configure({
            pageSize: limit,
            arrayController: controller
        });

        //TODO: 不能放到 setController 之前设置 selMode
        $(widget).DataList("OrganizationalPersonList").first().setSelMode(new ItemMode({
            allowDeselect: true,
            mode: "MULTI",
            returnValueType: "ARRAY"
        }));
    };

    var editHandler = function (source) {
        var record = source.parent.getRecord();
        var id = record.get("ID");

        alert('not implemented.');
    };

    return exports;
});