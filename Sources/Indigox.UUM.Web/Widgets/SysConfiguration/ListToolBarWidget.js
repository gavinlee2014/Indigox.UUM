define("/UUM/Widgets/SysConfiguration/ListToolBarWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Controls.Html.Menu"
    ],
    function (
        UrlUtil,
        Menu
    ) {
        var exports = function (widget) {
            $(widget).Menu("toolbar").first().configure({
                menuItemType: "buttonmenuitem",
                orientation: Menu.ORIENTATION_HORIZONTAL,
                staticDisplayLevels: 1,
                childNodes: [{
                    name: "btnCreateSysConfiguration",
                    value: "新建同步系统",
                    events: {
                        clicked: function (src, e) {
                            var url = "#/SysConfiguration/Create.htm";
                            UrlUtil.goTo(url);
                        }
                    }
                }]
            });
        };

        return exports;
    });