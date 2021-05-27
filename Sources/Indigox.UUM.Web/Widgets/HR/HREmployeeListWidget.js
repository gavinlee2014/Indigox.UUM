define("/UUM/Widgets/HR/HREmployeeListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/HREmployee"
    ],
function (
        UrlUtil,
        StringUtil,
        ErrorHandler,
        Formatters,
        Batch,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController
) {
    var exports = function (widget) {
        var limit = 10;

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('HREmployee', {
                proxy: new InstructionProxy({
                    query: "HREmployeeListQuery"
                })
            }),
            params: {
                "FetchSize": limit
            }
        });

        var listControl = $(widget).DataList("HREmployeeList").first();

        listControl.getItemTemplate().configureChildren({
            "State": {
                binding: {
                    mapping: {
                        value: new Formatters.ValueTextFormatter({
                            mapping: [{
                                value: 0, text: "新建"
                            }, {
                                value: 1, text: "修改"
                            }, {
                                value: 2, text: "删除"
                            }]
                        })
                    }
                }
            },
            "btnSync": {
                binding: {
                    mapping: {
                        visible: function (record) {
                            return !record.get("Synchronized");
                        }
                    }
                }
            },
            "Synchronized": {
                binding: {
                    mapping: {
                        value: function (record) {
                            if (record.get("Synchronized")) {
                                return "已同步";
                            }
                            else {
                                return "未同步";
                            }
                        }
                    }
                }
            }
        });

        listControl.on("itemAdded", function (source, index, item) {
            var record = item.getRecord();
            if (!record.get("Synchronized")) {
                $(item).Button("btnSync").on("clicked", function () {
                    var dialog = $.Dialog('HREmployeeSyncDialog').first();
                    dialog.open({ "ID": record.get("ID"), "State": record.get("State") });
                });
            }
        });

        listControl.configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });
    };

    return exports;
});