define("/UUM/Widgets/Profile/ChangePasswordToolbarWidget",
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

                    var editWidget = $.Widget("ChangePasswordWidget").first();
                    var formControl = $(editWidget).Content("EditArea").first();
                    if (!validate(editWidget, formControl)) {
                        Page().unmask();
                        return;
                    }
                    var record = formControl.getController().getModel().getRecord(0);
                    var oldPwd = $(formControl).TextBox("OldPassword").getValue();
                    var newPwd = $(formControl).TextBox("NewPassword").getValue();
                    var confirmPwd = $(formControl).TextBox("ConfirmPassword").getValue();

                    if (newPwd != confirmPwd) {
                        alert("两次密码输入不一致");
                        Page().unmask();
                        return;
                    }
                    if (oldPwd == null||oldPwd=="") {
                        alert("原密码不允许为空");
                        Page().unmask();
                        return;
                    }
                    if (newPwd == null || newPwd == "" || confirmPwd == null || confirmPwd=="") {
                        alert("新密码不允许为空");
                        Page().unmask();
                        return;
                    }

                    var batch = Batch.beginBatch();
                    batch.execute({
                        name: 'ChangePasswordCommand',
                        properties: {
                            AccountName: record.get("AccountName"),
                            OldPassword: oldPwd,
                            NewPassword: newPwd
                        }
                    })
                    .done(function (data) {
                        alert('修改密码成功。');
                        UrlUtil.goBack();
                    })
                    .fail(function (error) {
                        alert("修改密码失败!请检查旧密码是否输入正确,密码复杂度是否符合要求");
                        debug.error(error);
                    })
                    .always(function () {
                        Page().unmask();
                    });
                    batch.commit();

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