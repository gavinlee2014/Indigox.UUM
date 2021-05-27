define("/UUM/Widgets/Selection/UserSelectionToolbarWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Controls.Html.Menu"
    ],
function (
        UrlUtil,
        ErrorHandler,
        Batch,
        Menu
) {
    var exports = function (widget) {
        $(widget).Menu("toolbar").first().configure({
            menuItemType: "buttonmenuitem",
            orientation: Menu.ORIENTATION_HORIZONTAL,
            staticDisplayLevels: 1,
            childNodes: [{
                name: "btnOK",
                value: "确定",
                events: {
                    clicked: function () {
                        var selected = $.DataList("selectedUsers").first().getItems();
                        var returnValue = [];

                        for (var i = 0, length = selected.length; i < length; i++) {
                            if (selected[i]) {
                                var record = selected[i].getRecord();
                                returnValue.push({
                                    UserID: record.get("ID"),
                                    UserName: record.get("Name")
                                });
                            }
                        }

                        window.returnValue = returnValue;
                        window.close();
                    }
                }
            }, {
                name: "btnCancel",
                value: "取消",
                events: {
                    clicked: function () {
                        window.returnValue = null;
                        window.close();
                    }
                }
            }, {
                name: "btnClear",
                value: "清除",
                events: {
                    clicked: function () {
                        $.DataList("selectedUsers").first().getController().getModel().clearRecord();
                    }
                }
            }]
        });
    }

    return exports;
});