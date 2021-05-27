define("/UUM/Widgets/SysConfiguration/StateListWidget",
    [
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox/UUM/Application/SysConfiguration"
    ],
    function (
        ErrorHandler,
        InstructionProxy,
        RecordManager,
        ListController,
        Formatters,
        AutoBatch
    ) {
        var exports = function (widget) {
            var limit = 10;

            $(widget).DataList("SysConfigurationList").first().getItemTemplate().configureChildren({
                "SyncType": {
                    binding: {
                        mapping: {
                            value: new Formatters.ValueTextFormatter({
                                mapping: [{
                                    value: 1, text: "提供数据"
                                }, {
                                    value: 2, text: "接收数据"
                                }]
                            })
                        }
                    }
                },
                "SyncState": {
                    binding: {
                        mapping: {
                            value: new Formatters.ValueTextFormatter({
                                mapping: [{
                                    value: 0, text: "同步中"
                                }, {
                                    value: 1, text: "同步出错"
                                }, {
                                    value: 2, text: "同步完成"
                                }]
                            })
                        }
                    }
                },
                "btnRetry": {
                    binding: {
                        mapping: {
                            visible: function (record) {
                                var syncState = record.get('SyncState');
                                return (syncState == 1);
                            }
                        }
                    }
                }
            });

            $(widget).DataList("SysConfigurationList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnRetry").on("clicked", syncHandler);
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('SysConfiguration', {
                    proxy: new InstructionProxy({
                        query: "SysConfigurationStateListQuery"
                    })
                })
            });

            $(widget).DataList("SysConfigurationList").first().configure({
                controller: controller
            });
        };

        var syncHandler = function (source) {

            Page().mask();
            var record = source.getParent().getRecord();
            var syncTaskID = record.get("SyncTaskID");
            AutoBatch.getCurrentBatch().execute({
                name: "RetrySyncTaskCommand",
                properties: {
                    ID: syncTaskID
                }
            })
            .done(function (data) {
                alert("重试成功！");
                if ($.DataList("SyncTaskList").first()) {
                    $.DataList("SyncTaskList").first().getController().load();
                }
            })
            .fail(function (data) {
                alert("重试失败！");
                debug.error('重试失败:' + data.Message);
            })
            .always(function (data) {
                Page().unmask();
            });
        };

        return exports;
    });