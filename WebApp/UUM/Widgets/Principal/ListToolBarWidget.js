define("/UUM/Widgets/Principal/ListToolBarWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.PluginController",
        "Indigox.Web.JsLib.Controls.Plugins.PermissionPlugin",
        "Indigox.Web.JsLib.Controls.Html.Menu",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "/CMS/Widgets/Content/EffectivePermission"
    ],
    function (
        UrlUtil,
        StringUtil,
        Batch,
        InstructionProxy,
        RecordManager,
        PluginController,
        PermissionPlugin,
        Menu,
        PageUrlMonitor
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

            var pluginController = new PluginController({
                params: {
                    PrincipalID: principalID
                },
                model: RecordManager.getInstance().createRecordSet('EffectivePermission', {
                    proxy: new InstructionProxy({
                        query: "PrincipalPermissionQuery"
                    })
                })
            });

            new PageUrlMonitor({ paramFilters: ["OrganizationalUnitID"], container: widget })
                .onUrlParamChanged(function () {
                    reLoadPluginController();
                });

            var menuData = [{
                name: "btnCreateOrganizationalUnit",
                value: "创建部门",
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.CREATE
                    }
                }],
                childNodes: [{
                    name: "btnCreateCompany",
                    value: "创建公司",
                    events: {
                        clicked: function (src, e) {
                            UrlUtil.goTo("#/Company/Edit.htm", {
                                ParentOrganizationalUnitID: Page().getUrlParam("OrganizationalUnitID")
                            });
                        }
                    }
                }, {
                    name: "btnCreateDepartment",
                    value: "创建一级部门",
                    events: {
                        clicked: function (src, e) {
                            UrlUtil.goTo("#/Department/Edit.htm", {
                                ParentOrganizationalUnitID: Page().getUrlParam("OrganizationalUnitID")
                            });
                        }
                    }
                }, {
                    name: "btnCreateSection",
                    value: "创建二级部门",
                    events: {
                        clicked: function (src, e) {
                            UrlUtil.goTo("#/Section/Edit.htm", {
                                ParentOrganizationalUnitID: Page().getUrlParam("OrganizationalUnitID")
                            });
                        }
                    }
                }]
            },
            {
                name: "btnCreateOrganizationalPerson",
                value: "创建人员",
                events: {
                    clicked: function (src, e) {
                        UrlUtil.goTo("#/OrganizationalPerson/Edit.htm", {
                            ParentOrganizationalUnitID: Page().getUrlParam("OrganizationalUnitID")
                        });
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.CREATE
                    }
                }]
            },
            {
                name: "btnCreateOrganizationalRole",
                value: "创建组织角色",
                events: {
                    clicked: function (src, e) {
                        UrlUtil.goTo("#/OrganizationalRole/Edit.htm", {
                            ParentOrganizationalUnitID: Page().getUrlParam("OrganizationalUnitID")
                        });
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.CREATE
                    }
                }]
            },
            {
                name: "btnChangeOrganization",
                value: "移动",
                events: {
                    clicked: function (src, e) {
                        var records = $.DataList("PrincipalList").getValue();
                        var selectedPrincipal = [];
                        var i, length;
                        for (i = 0, length = records.length; i < length; i++) {
                            selectedPrincipal.push(records[i].data);
                        }
                        $.Dialog("ChangeOrganizationDialog").first().open({ Principals: selectedPrincipal });
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.WRITE
                    }
                }]
            },
            {
                name: "btnPermission",
                value: "权限",
                events: {
                    clicked: function (src, e) {
                        var principalID = Page().getUrlParam("OrganizationalUnitID");
                        if (StringUtil.isNullOrEmpty(principalID)) {
                            principalID = "OR1000000000";
                        }
                        UrlUtil.goTo("#/Admin/Permission/List.htm", {
                            Identifier: principalID,
                            Type: "Indigox.Common.Membership.Interfaces.IPrincipal"
                        });
                    }
                },
                plugins: [{
                    name: "permission",
                    config: {
                        controller: pluginController,
                        permission: PermissionPlugin.ADMINISTRATION
                    }
                }]
            }];

            var reLoadPluginController = function () {
                var orgID = Page().getUrlParam("OrganizationalUnitID");
                pluginController.setParam("PrincipalID", orgID);
                pluginController.clearModel();
                pluginController.load();
            }
            pluginController.load();

            $(widget).Menu("toolbar").first().configure({
                menuItemType: "dropdownmenuitem",
                orientation: Menu.ORIENTATION_HORIZONTAL,
                staticDisplayLevels: 1,
                childNodes: menuData
            });
        };

        return exports;
    });