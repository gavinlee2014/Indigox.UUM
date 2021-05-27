﻿define("/UUM/Widgets/Role/ListToolBarWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controls.Html.Menu"
    ],
function (
        UrlUtil,
        Batch,
        RecordManager,
        Menu
) {
    function validate(widget, formControl) {
        var validator = $(widget).Validator("Validator").first();

        if (!isNullOrUndefined(validator)) {
            var result = validator.validate(formControl);
            if (false === result.matched) {
                return false;
            }
        }

        return true;
    };

    var exports = function (widget) {
        var menuData = [{
            name: "btnCreateRole",
            value: "创建角色",
            events: {
                clicked: function (src, e) {
                    UrlUtil.goTo("#/Role/Edit.htm");
                }
            }
        }];

        $(widget).Menu("toolbar").first().configure({
            menuItemType: "buttonmenuitem",
            orientation: Menu.ORIENTATION_HORIZONTAL,
            staticDisplayLevels: 1,
            childNodes: menuData
        });
    };

    return exports;
});