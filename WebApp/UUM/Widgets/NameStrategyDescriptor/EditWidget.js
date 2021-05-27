define("/UUM/Widgets/NameStrategyDescriptor/EditWidget",
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
                    Page().mask();
                    formControl.submit();
                }
            }
        });
        $(widget).Button("btnDelete").first().configure({
            events: {
                clicked: function(source, e) {
                    Page().mask();

                    AutoBatch.getCurrentBatch().execute({
                        name: "DeleteNameStrategyDescriptorCommand",
                        properties: {
                            ID: Page().getUrlParam("ID")
                        }
                    })
                    .done(function (data) {
                        alert('删除成功。');
                        UrlUtil.goBack();
                    })
                    .fail(function (data) {
                        debug.error('删除失败。');
                    })
                    .always(function (data) {
                        Page().unmask();
                    });
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

        var ID = Page().getUrlParam("ID");

        var controller = new FormController({
            model: RecordManager.getInstance().createRecordSet('NameStrategyDescriptor', {
                proxy: new InstructionProxy({
                    query: "NameStrategyDescriptorSingleQuery",
                    updateCommand: "UpdateNameStrategyDescriptorCommand"
                })
            }),
            params: {
                "ID": ID
            }
        });

        formControl.configure({
            controller: controller
        });

    };

    return exports;
});