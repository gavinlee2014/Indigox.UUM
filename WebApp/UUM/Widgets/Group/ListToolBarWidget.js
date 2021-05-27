define("/UUM/Widgets/Group/ListToolBarWidget",
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
            name: "btnCreateGroup",
            value: "创建群组",
            events: {
                clicked: function (src, e) {
                    UrlUtil.goTo("#/Group/Edit.htm");
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