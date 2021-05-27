define("/UUM/Widgets/OrganizationalPerson/DisabledUserListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox/UUM/Application/OrganizationalPerson"
    ],
    function (
        UrlUtil,
        Batch,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController,
        ItemMode
    ) {
        var exports = function (widget) {
            var limit = 10;

            $(widget).DataList("OrganizationalPersonList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnEnableOrganizationalPerson").on("clicked", function (source) {
                    var record = source.parent.getRecord();
                    var id = record.get("ID");
                    var organization = record.get("Organization");
                    var dialog = $.Dialog("EnableToDialog").first();

                    dialog.on("closed", function (succeed) {
                        reloadOrganizationalPersonList(widget);
                    });

                    dialog.open({
                        ID: id,
                        Organization: [organization]
                    });
                });

                $(item).Button("btnDeleteOrganizationalPerson").on("clicked", function (source) {
                    var record = source.parent.getRecord();
                    var id = record.get("ID");

                    if (confirm("确定要删除用户[" + record.get("Name") + "]吗？")) {
                        var batch = Batch.beginBatch();
                        batch.execute({
                            name: "DeleteOrganizationalPersonCommand",
                            properties: {
                                ID: id
                            }
                        })
                        .done(function (data) {
                            alert('成功删除用户。');
                            reloadOrganizationalPersonList(widget);
                        })
                        .fail(function (data) {
                            alert('删除用户失败！');
                        });
                        batch.commit();
                    }
                });
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                    proxy: new InstructionProxy({
                        query: "DisabledUserListQuery"
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
            // $(widget).DataList("OrganizationalPersonList").first().setSelMode(new ItemMode({
            //     allowDeselect: true,
            //     mode: "MULTI",
            //     returnValueType: "ARRAY"
            // }));

            $(widget).Button("searchButton").first().on("clicked", function (source, e) {
                //debug.log("searchButton clicked...");

                var queryString = $(widget).TextBox("searchKey").first().getValue();
                var paging = $(widget).Paging("Paging").first();
                paging.getArrayController().setParam("QueryString", queryString);
                paging.reset();
            });
        };

        var reloadOrganizationalPersonList = function (widget) {
            var listControl = $(widget).DataList("OrganizationalPersonList").first();
            var controller = listControl.getController();
            controller.load();
        };

        return exports;
    });