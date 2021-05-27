define("/UUM/Widgets/Group/EditWidget",
    [
        "Indigox.Web.JsLib.Utils.ArrayUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Validation.Rules.NotBlankRule",
        "Indigox/UUM/Application/Group"
    ],
function (
        ArrayUtil,
        UrlUtil,
        Batch,
        InstructionProxy,
        RecordManager,
        FormController,
        ListController,
        NotBlankRule
) {
    function exports(widget) {
        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                model: RecordManager.getInstance().createRecordSet('Group', {
                    proxy: new InstructionProxy({
                        createCommand: "CreateGroupCommand",
                        updateCommand: "UpdateGroupCommand",
                        deleteCommand: "DeleteGroupCommand"
                    })
                })
            })
        });

        $(widget).Content("EditArea").UserSelect("Members").first().configure({
            mode: "12345",
            multi: true
        });

        var formControl = $(widget).Content("EditArea").first();
        $(formControl).TextBox("Name").first().configure({
            "validateRules": [
                    {
                        "type": "notblank",
                        "errorMessage": "用户名称不允许为空"
                    }
                ]
        });
        $(widget).UserSelect("Members").setMulti(true);

        $(formControl).RichTextBox("Description").first().configure({
            "mode": "plaintext"
        });

        var groupID = Page().getUrlParam("GroupID");
        var group = null;

        if (groupID) {
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("GroupQuery");
            form.getController().setParam("GroupID", groupID);
            form.getController().load();
        }
        else {
            var batch = Batch.beginBatch();
            batch.single({
                name: "DefaultGroupQuery",
                callback: function (data) {
                    group = data;
                    groupID = group.ID;

                    var controller = $(widget).Content("EditArea").first().getController();
                    controller.getModel().addRecord(group);
                }
            });
            batch.syncCommit();
        }

        Page().listenUrlParamChanged(["GroupID"], { container: widget }, function () {
            var groupID = Page().getUrlParam("GroupID");
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("GroupQuery");
            form.getController().setParam("GroupID", groupID);
            form.getController().load();
        });
    }

    return exports;
});