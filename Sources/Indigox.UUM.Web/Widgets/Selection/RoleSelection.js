define("/UUM/Widgets/Selection/RoleSelection",
    [
        "jquery",
        "Indigox.Web.JsLib.Utils.DelayedTask",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.Tip.RowTip",
        "/UUM/Widgets/Selection/DisplaySelectedPlugin",
        "Indigox/UUM/Application/Principal"
    ],
function (
        jQuery,
        DelayedTask,
        UrlUtil,
        StringUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        ItemMode,
        RowTip,
        DisplaySelectedPlugin
) {
    var exports = function (widget) {
        var args = window.dialogArguments || {};

        var value = args.value || JSON.parse(Page().getUrlParam('value'));
        var mode = args.mode || Page().getUrlParam('mode');
        var multi = args.multi || Page().getUrlParam('multi');

        var limit = 50;

        $(widget).DataList("optionRoles").first().on("itemAdded", function (source, index, item) {
            var tooltip = $(widget).Tooltip("principalToolTip").first();
            var tip = new RowTip();
            tip.setTipControl(tooltip);
            $(item).setTip(tip);
        });

        $(widget).DataList('optionRoles').first().configure({
            controller: new ListController({
                params: {
                    QueryString: "",
                    FetchSize: limit
                },
                model: RecordManager.getInstance().createRecordSet('Principal', {
                    proxy: new InstructionProxy({
                        query: "RoleListQuery"
                    }),
                    addRecords: false
                })
            })
        });

        //TODO: 不能放到 setController 之前设置 selMode
        $(widget).DataList('optionRoles').first().setSelMode(new ItemMode({
            allowDeselect: false,
            mode: (multi ? "MULTI" : "SINGLE"),
            returnValueType: "ARRAY"
        }));

        $(widget).Content('roleSearchPanel').Button('btnSearch').first().on('clicked', function () {
            //var queryString = $(widget).Content('roleSearchPanel').TextBox('txtKeyword').first().getValue();
            var queryString = jQuery(".txt-role-keyword")[0].value;

            var controller = $(widget).DataList('optionRoles').first().getController();
            controller.setParam('QueryString', queryString);
            controller.load();
        });

        DisplaySelectedPlugin.setControl($(widget).DataList('optionRoles').first(), $(widget).DataList('selectedUsers').first(), multi);
    }

    return exports;
});