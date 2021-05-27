define("/UUM/Widgets/OperationLog/OperationLogWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controls.Tip.RowTip",
        "Indigox.Web.JsLib.Controls.Html.Literal",
        "Indigox/UUM/Application/OperationLog"
    ],
    function (
        UrlUtil,
        Formatters,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController,
        RowTip,
        Literal
    ) {
        var limit = 12;

        var exports = function (widget) {

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('OperationLog', {
                    proxy: new InstructionProxy({
                        query: "OperationLogListQuery"
                    })
                }),
                params: {
                    "FetchSize": limit
                }
            });

            $(widget).DataList("OperationLogList").first().configure({
                controller: controller
            });

            $(widget).Paging("Paging").first().configure({
                pageSize: limit,
                arrayController: controller
            });

            $(widget).Content("ToolTipContent").first().configure({
                controller: new FormController({
                    model: RecordManager.getInstance().createRecordSet("OperationLog", {})
                })
            });

            $(widget).DataList("OperationLogList").first().on("itemAdded", function (source, index, item) {
                var tooltip = $.Tooltip("ToolTip").first();
                var tip = new RowTip();
                tip.setTipControl(tooltip);
                tip.setIsDynamic(false);
                $(item).setTip(tip);
            });
        };
        return exports;
    });