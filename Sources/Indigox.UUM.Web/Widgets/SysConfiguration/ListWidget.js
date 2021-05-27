define("/UUM/Widgets/SysConfiguration/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/UUM/Application/SysConfiguration"
    ],
    function (
        StringUtil,
        UrlUtil,
        ErrorHandler,
        InstructionProxy,
        RecordManager,
        ListController,
        Batch,
        ItemMode,
        PageUrlMonitor
    ) {
        var exports = function (widget) {
            var limit = 10;

            $(widget).DataList("SysConfigurationList").first().getItemTemplate().configureChildren({
                "Enabled": {
                    binding: {
                        mapping: {
                            value: function (record) {
                                var enabled = record.get("Enabled");
                                return enabled == true ? "是" : "否";
                            }
                        }
                    }
                }
            });
            $(widget).DataList("SysConfigurationList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnSysConfiguration").on("clicked", editHandler);
                $(item).Button("btnSyncAll").on("clicked", syncHandler);
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('SysConfiguration', {
                    proxy: new InstructionProxy({
                        query: "SysConfigurationListQuery"
                    })
                }),
                params: {
                    "FetchSize": limit
                }
            });

            var listControl = $(widget).DataList("SysConfigurationList").first();

            listControl.configure({
                controller: controller
            });

            $(widget).Paging("Paging").configure({
                pageSize: limit,
                arrayController: controller
            });

        };

        var editHandler = function (source) {
            var record = source.parent.getRecord();
            var id = record.get("ID");
            var url = "#/SysConfiguration/Edit.htm?ID=" + id;
            UrlUtil.goTo(url);
        };
        var syncHandler = function (source) {
            if (!confirm("是否确认执行全同步?")) {
                return;
            }
            var record = source.parent.getRecord();
            var id = record.get("ID");
            var batch = Batch.beginBatch();
            batch.execute({
                name: 'FullSyncCommand',
                properties: {
                    SystemID: id
                },
                callback: function (data) {
                    alert("同步成功!");
                    Page().unmask();
                },
                errorCallback: function (error) {
                    ErrorHandler.logAlert(error);
                    alert("同步失败!");
                    Page().unmask();
                }
            });
            Page().mask();
            batch.commit();
        };

        return exports;
    });