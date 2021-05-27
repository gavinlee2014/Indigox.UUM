define("/UUM/Widgets/NameStrategyDescriptor/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/UUM/Application/NameStrategyDescriptor"
    ],
    function (
        StringUtil,
        UrlUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        ItemMode,
        PageUrlMonitor
    ) {
        var exports = function (widget) {
            var limit = 10;

            $(widget).DataList("NameStrategyDescriptorList").first().getItemTemplate().configureChildren({
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
            $(widget).DataList("NameStrategyDescriptorList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnEditNameStrategyDescriptor").on("clicked", editHandler);
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('NameStrategyDescriptor', {
                    proxy: new InstructionProxy({
                        query: "NameStrategyDescriptorListQuery"
                    })
                }),
                params: {
                    "FetchSize": limit
                }
            });

            var listControl = $(widget).DataList("NameStrategyDescriptorList").first();

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
            var url = "#/NameStrategyDescriptor/Edit.htm?ID=" + id;
            UrlUtil.goTo(url);
        };

        return exports;
    });