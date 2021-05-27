define("/UUM/Widgets/OrganizationalUnit/EditToolBarWidget",
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
        }

        var exports = function (widget) {
            var principalID = Page().getUrlParam("OrganizationalUnitID");
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
                name: "submit",
                value: "保存",
                events: {
                    clicked: function (src, e) {
                        Page().mask();

                        var editWidget = $.Widget("OrganizationalUnitEditWidget").first();
                        var content = $(editWidget).Content("EditArea").first();

                        if (!validate(editWidget, content)) {
                            Page().unmask();
                            return;
                        }

                        content.on("submit", function (source, successed) {
                            if (successed) {
                                alert("部门保存成功!");
                                Page().unmask();
                                UrlUtil.goBack();
                            }
                            else {
                                alert("部门保存失败!");
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
                        permission: getSavePermissionType()
                    }
                }]
            }, {
                name: "close",
                value: "返回",
                events: {
                    clicked: function (src, e) {
                        UrlUtil.goBack();
                    }
                }
            }];

            if (principalID) {
                menuData.splice(1, 0, {
                    name: "delete",
                    value: "删除",
                    events: {
                        clicked: function (src, e) {
                            Page().mask();
                            if (!confirm("您确认要删除此部门吗?")) {
                                Page().unmask();
                                return false;
                            }

                            var editWidget = $.Widget("OrganizationalUnitEditWidget").first();
                            var content = $(editWidget).Content("EditArea").first();

                            content.on("submit", function (source, successed) {
                                if (successed) {
                                    alert("删除部门成功!");
                                    Page().unmask();
                                    UrlUtil.goBack();
                                }
                                else {
                                    alert("删除部门失败!");
                                    Page().unmask();
                                }
                                this.un('submit', arguments.callee);
                            });

                            content.getController().removeRecord(0);
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