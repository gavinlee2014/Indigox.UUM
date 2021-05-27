define("/UUM/Widgets/Role/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/Role"
    ],
function (
        UrlUtil,
        StringUtil,
        Formatters,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController
) {
    var exports = function (widget) {
        var roleID = Page().getUrlParam("RoleID");
        var limit = 10;

        $(widget).DataList("RoleList").on("itemAdded", function (source, index, item) {
            $(item).Button("btnEditRole").on("clicked", editHandler);
            $(item).Button("btnEditPermission").on("clicked", editPermissionHandler);
        });

        $(widget).DataList("RoleList").first().getItemTemplate().configureChildren({
            "Level": {
                binding: {
                    mapping: {
                        value: new Formatters.ValueTextFormatter({
                            mapping: [{
                                value: 101, text: "集团"
                            }, {
                                value: 102, text: "公司"
                            }, {
                                value: 103, text: "一级部门"
                            }, {
                                value: 104, text: "二级部门"
                            }]
                        })
                    }
                }
            }
        });

        Page().listenUrlParamChanged(["RoleID"], { container: widget }, function () {
            reloadRoleList(widget);
        });

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('Role', {
                proxy: new InstructionProxy({
                    query: "RoleListQuery"
                })
            }),
            params: {
                "RoleID": roleID,
                "FetchSize": limit
            }
        });

        $(widget).DataList("RoleList").configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });
    };

    var editHandler = function (source) {
        UrlUtil.goTo("#/Role/Edit.htm", {
            RoleID: source.parent.getRecord().get("ID")
        });
    };

    var editPermissionHandler = function (source) {
        var record = source.parent.getRecord();
        var principalID = record.get("ID");
        UrlUtil.goTo("#/Admin/Permission/List.htm", {
            Identifier: principalID,
            Type: "Indigox.Common.Membership.Interfaces.IPrincipal"
        });
    };

    var reloadRoleList = function (widget) {
        var roleID = Page().getUrlParam('RoleID');
        var listControl = $(widget).DataList("RoleList").first();
        var controller = listControl.getController();
        controller.setParam("RoleID", roleID);
        controller.load();
    };

    return exports;
});