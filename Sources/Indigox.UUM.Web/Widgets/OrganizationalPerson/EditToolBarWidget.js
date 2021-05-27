define("/UUM/Widgets/OrganizationalPerson/EditToolBarWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.PluginController",
        "Indigox.Web.JsLib.Controls.Plugins.PermissionPlugin",
        "Indigox.Web.JsLib.Controls.Html.Menu",
        "/CMS/Widgets/Content/EffectivePermission"
    ],
function (
        UrlUtil,
        Batch,
        InstructionProxy,
        RecordManager,
        PluginController,
        PermissionPlugin,
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
        var principalID = Page().getUrlParam("OrganizationalPersonID");
        var parentOrganizationID = Page().getUrlParam("ParentOrganizationalUnitID");

        var getID = function () {
            return principalID || parentOrganizationID;
        }
        var getSavePermissionType = function () {
            if (principalID) {
                return PermissionPlugin.WRITE;
            }
            return PermissionPlugin.CREATE;
        }
        var pluginController = new PluginController({
            params: {
                PrincipalID: getID()
            },
            model: RecordManager.getInstance().createRecordSet('EffectivePermission', {
                proxy: new InstructionProxy({
                    query: "PrincipalPermissionQuery"
                })
            })
        });

        var menuData = [{
            name: "btnSubmit",
            value: "保存",
            events: {
                clicked: function (src, e) {
                    Page().mask();
                    var profile = $.FileUploadList("Profile").first().getValue();
                    if (profile && profile.length > 0) {
                        var pic = profile[0].fileUrl;
                        var reg = /(\.jpg|\.png|\.gif)$/;
                        if (!reg.test(pic)) {
                            alert("头像格式不对，只能上传.jpg/.png/.gif格式的图片");
                            Page().unmask();
                            return;
                        }
                    }
                    var editWidget = $.Widget("OrganizationalPersonEditWidget").first();
                    var formControl = $(editWidget).Content("EditArea").first();
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

                    var batch = Batch.beginBatch();
                    batch.list({
                        name: 'CheckUserExistsQuery',
                        properties: {
                            AccountName: $.TextBox("AccountName").first().getValue(),
                            Email: $.TextBox("Email").first().getValue(),
                            PrincipalID: principalID
                        },
                        callback: function (data) {
                            if (data.length != 0) {
                                alert("该账号或者Email已经存在！");
                                Page().unmask();
                                return;
                            }
                            else {
                                formControl.submit();
                            }
                        },
                        errorCallback: function (error) {
                            ErrorHandler.logAlert(error);
                        }
                    });
                    batch.commit();
                }
            },
            plugins: [{
                name: "permission",
                config: {
                    controller: pluginController,
                    permission: getSavePermissionType()
                }
            }]
        }, {
            name: "btnClose",
            value: "返回",
            events: {
                clicked: function (src, e) {
                    UrlUtil.goBack();
                }
            }
        }];

        if (principalID) {
            menuData.splice(1, 0, {
                name: "btnDisable",
                value: "禁用",
                events: {
                    clicked: function (src, e) {
                        Page().mask();
                        if (!confirm("您确认要禁用此用户吗?")) {
                            Page().unmask();
                            return false;
                        }
                        var editWidget = $.Widget("OrganizationalPersonEditWidget").first();

                        var content = $(editWidget).Content("EditArea").first();
                        content.getController().removeRecord(0);

                        content.on("submit", function (source, successed) {
                            if (successed) {
                                alert("禁用用户成功!");
                                Page().unmask();
                                UrlUtil.goBack();
                            }
                            else {
                                alert("禁用用户失败!");
                                Page().unmask();
                            }
                            this.un('submit', arguments.callee);
                        });

                        content.submit();
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.DELETE
                    }
                }]
            }, {
                name: "btnUnlockUser",
                value: "解锁用户",
                events: {
                    clicked: function (src, e) {
                        Page().mask();
                        var batch = Batch.beginBatch();
                        batch.execute({
                            name: 'UnlockOrganizationalPersonCommand',
                            properties: {
                                ID: principalID
                            },
                            callback: function (data) {
                                alert("解锁成功!");
                                Page().unmask();
                            },
                            errorCallback: function (error) {
                                alert("解锁成功!");
                                Page().unmask();
                            }
                        });
                        batch.commit();
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.WRITE
                    }
                }]
            },{
                name: "btnSetPassword",
                value: "设置密码",
                events: {
                    clicked: function (src, e) {
                        var editWidget = $.Widget("OrganizationalPersonEditWidget").first();
                        var content = $(editWidget).Content("EditArea").first();
                        var record = content.getController().getModel().getRecord(0);
                        $.Dialog("SetPasswordDialog").open({ data: record });
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.WRITE
                    }
                }]
            });
        }

        pluginController.load();

        $(widget).Menu("toolbar").first().configure({
            menuItemType: "buttonmenuitem",
            orientation: Menu.ORIENTATION_HORIZONTAL,
            staticDisplayLevels: 1,
            childNodes: menuData
        });
    };

    return exports;
});