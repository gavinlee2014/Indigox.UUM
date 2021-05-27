define("/UUM/Widgets/Profile/EditToolBarWidget",
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
            name: "submit",
            value: "保存",
            events: {
                clicked: function (src, e) {
                    Page().mask();
                    var editWidget = $.Widget("ProfileEditWidget").first();
                    var formControl = $(editWidget).Content("EditArea").first();

                    var profile = $(formControl).FileUploadList("Profile").first().getValue();
                    if (profile && profile.length > 0) {
                        var pic = profile[0].fileUrl;
                        var reg = /(\.jpg|\.png|\.gif)$/;
                        if (!reg.test(pic)) {
                            alert("头像格式不对，只能上传.jpg/.png/.gif格式的图片");
                            Page().unmask();
                            return;
                        }
                    }

                    if (!validate(editWidget, formControl)) {
                        Page().unmask();
                        return;
                    }

                    formControl.on("submit", function (source, successed) {
                        if (successed) {
                            alert("用户保存成功!");
                            Page().unmask();
                            UrlUtil.goBack();
                        }
                        else {
                            alert("用户保存失败!");
                            Page().unmask();
                        }
                        this.un('submit', arguments.callee);
                    });
                    formControl.submit();
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