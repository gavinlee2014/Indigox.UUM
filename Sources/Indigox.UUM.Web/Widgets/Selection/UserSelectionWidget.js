define("/UUM/Widgets/Selection/UserSelectionWidget",
    [
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Tip.RowTip",
        "Indigox/UUM/Application/Principal",
        "Indigox/UUM/Application/PrincipalSummary"
    ],
function (
        InstructionProxy,
        RecordManager,
        FormController,
        ListController,
        RowTip
) {
    var exports = function (widget) {
        var args = window.dialogArguments || {};

        var value = args.value || JSON.parse(Page().getUrlParam('value'));
        var mode = args.mode || Page().getUrlParam('mode');
        var multi = args.multi || Page().getUrlParam('multi');

        var recordSet = RecordManager.getInstance().createRecordSet('SelectedPrincipal');

        // set recordSet
        $(widget).DataList('selectedUsers').first().configure({
            controller: new ListController({
                model: recordSet
            })
        });

        // set unselect click event
        $(widget).DataList('selectedUsers').first().on("itemAdded", function (source, index, item) {
            item.on("clicked", function (source, e) {
                //debug.log('remove...', item.record);
                var recordSet = $(widget).DataList('selectedUsers').first().getController().getModel();
                recordSet.removeRecord(item.record);
            });
        });

        // bind tooltip to datalist
        $(widget).DataList("selectedUsers").first().on("itemAdded", function (source, index, item) {
            var tooltip = $(widget).Tooltip("principalToolTip").first();
            var tip = new RowTip();
            tip.setTipControl(tooltip);
            $(item).setTip(tip);
        });

        // set tooltip controller
        $(widget).Content("principalToolTipContent").first().configure({
            controller: new FormController({
                model: RecordManager.getInstance().createRecordSet("PrincipalSummary", {
                    proxy: new InstructionProxy({
                        query: "PrincipalSummaryQuery"
                    })
                })
            })
        });

        Page().on("loaded", function () {
            jQuery(".txt-user-keyword").keydown(function (e) {
                if (e.keyCode == 13) {
                    $(widget).Content('userSearchPanel').Button('btnSearch').first().click();
                }
            });
            jQuery(".txt-role-keyword").keydown(function (e) {
                if (e.keyCode == 13) {
                    $(widget).Content('roleSearchPanel').Button('btnSearch').first().click();
                }
            });
            jQuery(".txt-group-keyword").keydown(function (e) {
                if (e.keyCode == 13) {
                    $(widget).Content('groupSearchPanel').Button('btnSearch').first().click();
                }
            });

        })
        // set selected value
        if (value) {
            for (var i = 0, length = value.length; i < length; i++) {
                var v = value[i];
                recordSet.addRecord({ ID: v.UserID, Name: v.UserName });
            }
        }
    };

    return exports;
});