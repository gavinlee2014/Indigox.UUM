define("/UUM/Widgets/NameStrategyDescriptor/ListToolBarWidget",
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
                    name: "btnCreateNameStrategyDescriptor",
                    value: "新建命名规则",
                    events: {
                        clicked: function (src, e) {
                            var url = "#/NameStrategyDescriptor/Create.htm";
                            UrlUtil.goTo(url);
                        }
                    }
                }]
            });
        };

        return exports;
    });