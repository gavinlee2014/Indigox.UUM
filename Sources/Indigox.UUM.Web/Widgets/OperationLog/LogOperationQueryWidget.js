define("/UUM/Widgets/OperationLog/LogOperationQueryWidget",
    [
        "Indigox.Web.JsLib.UI.UIManager",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "/UUM/Widgets/OperationLog/LogSearchSpecification",
        "/UUM/Widgets/OperationLog/LogSearchSpecificationSchema",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "jquery"
    ],
    function (
        UIManager,
        AutoBatch,
        LogSearchSpecification,
        LogSearchSpecificationSchema,
        FormController,
        RecordManager,
        ArrayProxy,
        jQuery
    ) {
        var exports = function (widget) {
            initSearchPanel(widget);


            $(widget).Button('btnSearch').on('clicked', function () {
                var startDate = $.Widget('LogQueryWidget').DatePicker('startDate').first().getValue();
                var endDate = $.Widget('LogQueryWidget').DatePicker('endDate').first().getValue();
                var searchContent = $.Widget('LogQueryWidget').TextBox('SearchContent').first().getValue();
                var controller = $.Widget('OperationLogWidget').DataList('OperationLogList').first().getController();
                controller.setParam('LogTimeBegin', startDate);
                controller.setParam('LogTimeEnd', endDate);
                controller.setParam('SearchContent', searchContent);
                controller.load();
            });
        }

        function initSearchPanel(widget) {
            var contentType = LogSearchSpecification;
            var searchPanel = $(widget).Content("SearchPanel").first();
            searchPanel.setControls(contentType.controls);

            searchPanel.configure({
                controller: new FormController({
                    model: RecordManager.getInstance().createRecordSet("LogSearchSpecification", {
                        proxy: new ArrayProxy({
                            array: [{}]
                        })
                    })
                })
            });

        }

        return exports;
    });