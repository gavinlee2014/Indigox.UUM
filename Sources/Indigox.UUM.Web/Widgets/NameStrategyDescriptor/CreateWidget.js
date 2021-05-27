define("/UUM/Widgets/NameStrategyDescriptor/CreateWidget",
    [
        "Indigox.Web.JsLib.Utils.ArrayUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Validation.Rules.NotBlankRule",
        "Indigox/UUM/Application/NameStrategyDescriptor"
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

        $(formControl).TextBox("ClassName").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "类名不允许为空"
                }
            ]
        });
        $(formControl).TextBox("Assembly").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "程序集名称不允许为空"
                }
            ]
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
        $(formControl).RichTextBox("Description").first().configure({
            "mode": "plaintext"
        });

        var controller = new FormController({
            model: RecordManager.getInstance().createRecordSet('NameStrategyDescriptor', {
                proxy: new InstructionProxy({
                    createCommand: "CreateNameStrategyDescriptorCommand"
                })
            })
        });

        formControl.configure({
            controller: controller
        });

        controller.insertRecord(0);

    };

    return exports;
});