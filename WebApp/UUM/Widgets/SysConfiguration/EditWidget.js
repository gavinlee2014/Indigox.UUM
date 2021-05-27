define("/UUM/Widgets/SysConfiguration/EditWidget",
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
                "ID": ID
            }
        });

        $(widget).CheckBoxList("Dependencies").first().configure({
            controller: sysListController
        });

        $(widget).RadioBoxList("SyncType").first().configure({
            ReturnValueType: "string",
            items: [
                { value: 1, text: '提供数据' },
                { value: 2, text: '接收数据' }
            ]
        });

        var formControl = $(widget).Content("EditArea").first();
        var ID = Page().getUrlParam("ID");

        var controller = new FormController({
            model: RecordManager.getInstance().createRecordSet('SysConfiguration', {
                proxy: new InstructionProxy({
                    query: "SysConfigurationSingleQuery",
                    updateCommand: "UpdateSysConfigurationCommand"
                })
            }),
            params: {
                "ID": ID
            }
        });

        formControl.configure({
            controller: controller
        });

        $(widget).Button("btnDelete").first().configure({
            events: {
                clicked: function (source, e) {
                    Page().mask();

                    AutoBatch.getCurrentBatch().execute({
                        name: "DeleteSysConfigurationCommand",
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
                    Page().mask();
                    formControl.submit();
                }
            }
        });
    };

    return exports;
});