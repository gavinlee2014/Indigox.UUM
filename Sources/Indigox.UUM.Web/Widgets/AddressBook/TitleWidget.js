define("/UUM/Widgets/AddressBook/TitleWidget",
    [

    ],
function (

) {
    var exports = function (widget) {
        widget.on('loading', function () {
            configureTitle(widget);
        });

        Page().listenUrlParamChanged(['ID'], { container: widget }, function () {
            configureTitle(widget);
        });
    };

    function configureTitle(widget) {
        var userNameTitle = widget.find("UserName")[0];
        var listValue = $.DataList("AddressBookList").first().getValue();

        var title = "详细信息";
        if (listValue) {
            title = listValue[0].get("UserName");
        }
        userNameTitle.setValue(title);
    }

    return exports;
});