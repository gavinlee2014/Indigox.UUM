define("/UUM/Widgets/Role/EditWidget",
    [
        "Indigox.Web.JsLib.Utils.ArrayUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Validation.Rules.NotBlankRule",
        "Indigox.Web.JsLib.Controls.Validation.Rules.MailAddressRule",
        "Indigox/UUM/Application/Role"
    ],
function (
        ArrayUtil,
        UrlUtil,
        Batch,
        InstructionProxy,
        RecordManager,
        FormController,
        ListController,
        NotBlankRule,
        MailAddressRule
) {
    function exports(widget) {
        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                autoLoad: false,
                model: RecordManager.getInstance().createRecordSet('Role', {
                    proxy: new InstructionProxy({
                        createCommand: "CreateRoleCommand",
                        updateCommand: "UpdateRoleCommand",
                        deleteCommand: "DeleteRoleCommand"
                    })
                })
            })
        });

        $(widget).Content("EditArea").UserSelect("Members").first().configure({
            mode: "4",
            multi: true
        });

        $(widget).Content("EditArea").DropDownList("Level").setReturnValueType("String");
        $(widget).Content("EditArea").DropDownList("Level").first().configure({
            allowBlank: true,
            items: [
                    { text: "集团", value: "101" },
                    { text: "公司", value: "102" },
                    { text: "一级部门", value: "103" },
                    { text: "二级部门", value: "104" }
                ]
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

        $(formControl).TextBox("Email").first().configure({
            "validateRules": [
                {
                    "type": "mail",
                    "errorMessage": "邮箱格式不正确"
                }
            ]
        });


        $(widget).UserSelect("Members").setMulti(true);
        $(formControl).RichTextBox("Description").first().configure({
            "mode": "plaintext"
        });

        var roleID = Page().getUrlParam("RoleID");
        var role = null;

        if (roleID) {
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("RoleQuery");
            form.getController().setParam("RoleID", roleID);
            form.getController().load();
        }
        else {
            var batch = Batch.beginBatch();
            batch.single({
                name: "DefaultRoleQuery",
                callback: function (data) {
                    role = data;
                    roleID = role.ID;

                    var controller = $(widget).Content("EditArea").first().getController();
                    controller.getModel().addRecord(role);
                }
            });
            batch.syncCommit();
        }

        Page().listenUrlParamChanged(["RoleID"], { container: widget }, function () {
            var roleID = Page().getUrlParam("RoleID");
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("RoleQuery");
            form.getController().setParam("RoleID", roleID);
            form.getController().load();
        });
    }

    return exports;
});