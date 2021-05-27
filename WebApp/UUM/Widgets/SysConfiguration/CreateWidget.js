define("/UUM/Widgets/SysConfiguration/CreateWidget",
    [
        "Indigox.Web.JsLib.Utils.ArrayUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Validation.Rules.NotBlankRule",
        "Indigox/UUM/Application/SysConfiguration",
        "Indigox/UUM/Application/SysList"
    ],
function (
        ArrayUtil,
        UrlUtil,
        AutoBatch,
        InstructionProxy,
        RecordManager,
        FormController,
        ListController,
        NotBlankRule
) {
    function exports(widget) {

        function validate(widget, formControl) {
            var validator = $(widget).Validator("Validator").first();

            if (!isNullOrUndefined(validator)) {
                var result = validator.validate(formControl);
                if (false === result.matched) {
                    return false;
                }
            }

            return true;
        };

        var formControl = $(widget).Content("EditArea").first();

        $(formControl).TextBox("ClientName").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "系统名不允许为空"
                }
            ]
        });

        formControl.on('submit', function (successed) {
            if (successed) {
                alert('保存成功。');
                UrlUtil.goBack();
            }
            else {
                debug.error('保存失败。');
            }
            Page().unmask();
        });

        $(widget).Button("btnSave").first().configure({
            events: {
                clicked: function (src, e) {
                    if (!validate(widget, formControl)) {
                        Page().unmask();
                        return;
                    }
                    Page().mask();
                    formControl.submit();
                }
            }
        });

        $(widget).Button("btnReturn").first().configure({
            events: {
                clicked: function (src, e) {
                    UrlUtil.goBack();
                }
            }
        });

        var ID = Page().getUrlParam("ID");

        var controller = new FormController({
            model: RecordManager.getInstance().createRecordSet('SysConfiguration', {
                proxy: new InstructionProxy({
                    createCommand: "CreateSysConfigurationCommand"
                })
            })
        });

        formControl.configure({
            controller: controller
        });

        var sysListController = new ListController({
            itemOptions: {
                controlType: "checkboxitem",
                binding: {
                    mapping: {
                        text: "SysName",
                        value: "ID"
                    }
                }
            },
            model: RecordManager.getInstance().createRecordSet('SysList', {
                proxy: new InstructionProxy({
                    query: "SysListQuery"
                })
            }),
            params: {
                "ID": 0
            }
        });

        $(widget).CheckBoxList("Dependencies").first().configure({
            controller: sysListController
        });
        controller.insertRecord(0);
    };

    return exports;
});