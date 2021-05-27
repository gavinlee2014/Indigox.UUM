define("/UUM/Widgets/SyncTask/ListWidget",
    [
        "/UUM/Widgets/SyncTask/SearchSyncTaskContentType",
        "/UUM/Widgets/SyncTask/SearchSyncTaskSchema",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.Util",
        "Indigox.Web.JsLib.Utils.DelayedTask",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/UUM/Application/SyncTask"
    ],
    function (
        SearchSyncTaskContentType,
        SearchSyncTaskSchema,
        StringUtil,
        UrlUtil,
        Util,
        DelayedTask,
        Formatters,
        AutoBatch,
        ArrayProxy,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController,
        ItemMode,
        PageUrlMonitor
    ) {
        var exports = function (widget) {
            var limit = 10;

            initSearchPanel(widget);

            $(widget).DataList("SyncTaskList").first().getItemTemplate().configureChildren({
                "State": {
                    binding: {
                        mapping: {
                            value: new Formatters.ValueTextFormatter({
                                mapping: [{
                                    value: 0, text: "未执行"
                                }, {
                                    value: 1, text: "成功"
                                }, {
                                    value: 2, text: "失败"
                                }, {
                                    value: 3, text: "忽略"
                                }]
                            })
                        }
                    }
                },
                "btnIgnore": {
                    binding: {
                        mapping: {
                            visible: function (record) {
                                var state = record.get("State");
                                return state == 2 ? true : false;
                            }
                        }
                    }
                }
            });

            $(widget).DataList("SyncTaskList").first().on("itemAdded", function (source, index, item) {
                $(item).Button("btnIgnore").on("clicked", function (source) {
                    Page().mask();
                    var record = source.getParent().getRecord();
                    var syncTaskID = record.get("ID");
                    AutoBatch.getCurrentBatch().execute({
                        name: "IgnoreTaskCommand",
                        properties: {
                            ID: syncTaskID
                        }
                    })
                    .done(function (data) {
                        source.setVisible(false);
                        alert("忽略任务成功！");
                        var delayedTask = new DelayedTask(function () {
                            if ($.DataList("SyncTaskList").first()) {
                                $.DataList("SyncTaskList").first().getController().load();
                            }
                        }, this, []);
                        delayedTask.delay(1000);
                    })
                    .fail(function (data) {
                        alert("忽略任务失败！");
                        debug.error('忽略任务失败:' + data.Message);
                    })
                    .always(function (data) {
                        Page().unmask();
                    });
                });
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('SyncTask', {
                    proxy: new InstructionProxy({
                        query: "SyncTaskListQuery"
                    })
                }),
                params: {
                    Tag: '',
                    Description: '',
                    State: -1,
                    CreateTimeBegin: '',
                    CreateTimeEnd: '',
                    FetchSize: limit
                }
            });

            var listControl = $(widget).DataList("SyncTaskList").first();

            listControl.configure({
                controller: controller
            });

            $(widget).Paging("Paging").configure({
                pageSize: limit,
                arrayController: controller
            });

            $(widget).Button("btnSearch").first().on("clicked", function () {
                var searchPanel = $(widget).Content("SearchPanel").first();
                var controller = searchPanel.getController();
                controller.updateRecord();
                var specification = Util.copy({}, controller.getModel().getRecord(0).data);
                var paging = $(widget).Paging("Paging").first();
                SetSearchParam(paging.getArrayController(), specification);
                paging.reset();
            });

        };


        function initSearchPanel(widget) {
            var contentType = SearchSyncTaskContentType;
            var searchPanel = $(widget).Content("SearchPanel").first();
            searchPanel.setControls(contentType.controls);

            searchPanel.configure({
                controller: new FormController({
                    model: RecordManager.getInstance().createRecordSet("SearchSyncTask", {
                        proxy: new ArrayProxy({
                            array: [{}]
                        })
                    })
                })
            });
        }

        function SetSearchParam(arrController, specification) {
            for (var p in specification) {
                if (p) {
                    var v = specification[p];

                    switch (p) {
                        case 'CreateTimeBegin':
                            arrController.setParam("CreateTimeBegin", v);
                            break;

                        case 'Tag':
                            arrController.setParam("Tag", v);
                            break;

                        case 'Description':
                            arrController.setParam("Description", v);
                            break;

                        case 'State':
                            arrController.setParam("State", v[0]);
                            break;

                        case 'CreateTimeEnd':
                            arrController.setParam("CreateTimeEnd", v);
                            break;
                    }
                }

            }
        }
        return exports;
    });