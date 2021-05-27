define("/UUM/Widgets/Principal/UserSearchListWidget",
    [
        "jquery",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/UUM/Application/OrganizationalPerson"
    ],
    function (
        jQuery,
        StringUtil,
        UrlUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        ItemMode,
        PageUrlMonitor
    ) {
        var typeMap = {
            "100": "部门",
            "101": "集团",
            "102": "公司",
            "103": "一级部门",
            "104": "二级部门",
            "200": "人员",
            "201": "人员",
            "300": "群组",
            "400": "组织角色",
            "500": "角色"
        };

        var typeClassMap = {
            "100": "OrganizationalUnit",
            "101": "Corporation",
            "102": "Company",
            "103": "Department",
            "104": "Section",
            "200": "User",
            "201": "OrganizationalPerson",
            "300": "Group",
            "400": "OrganizationalRole",
            "500": "Role"
        };

        var exports = function (widget) {
            var limit = 10;

            var GetAllTypeFilter = function () {
                return ["OrganizationalUnit", "OrganizationalPerson", "OrganizationalRole"];
            }

            $(widget).DataList("PrincipalList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnEditPrincipal").on("clicked", editHandler);
            });

            var controller = new ListController({
                autoLoad: false,
                model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                    proxy: new InstructionProxy({
                        query: "PrincipalSearchQuery"
                    })
                }),
                params: {
                    "QueryString": "",
                    "FetchSize": limit
                }
            });

            var listControl = $(widget).DataList("PrincipalList").first();
            var template = listControl.getItemTemplate();
            template.getChild("Type").configure({
                binding: {
                    mapping: {
                        value: function (record) {
                            return typeMap[record.get("Type")] || "";
                        }
                    }
                }
            });
            template.getChild("Name").configure({
                binding: {
                    mapping: {
                        value: function (record) {
                            return record.get("DisplayName") == null ? record.get("Name") : record.get("DisplayName");
                        }
                    }
                }
            });

            listControl.configure({
                controller: controller
            });

            $(widget).Paging("Paging").configure({
                pageSize: limit,
                arrayController: controller
            });

            var reloadList = function () {
                var listControl = $(widget).DataList("PrincipalList").first();
                var controller = listControl.getController();
                var queryString = jQuery(".text-searchkey")[0].value;

                controller.setParam('QueryString', queryString);

                $(widget).Paging("Paging").first().reset();
            };

            $(widget).Content('searchPanel').Button('searchButton').first().on("clicked", reloadList);

            Page().on('loaded', function () {
                jQuery(".text-searchkey").keydown(function (e) {
                    if (e.keyCode == 13) {
                        setTimeout(reloadList, 100);
                    }
                });
            });

        };

        var editHandler = function (source) {
            var record = source.parent.getRecord();
            var id = record.get("ID");
            var type = typeClassMap[record.get("Type")];
            var url = "#/" + type + "/Edit.htm?" + type + "ID=" + id;
            /*UrlUtil.goTo("#/OrganizationalUnit/Edit.htm", {
            OrganizationalUnitID: source.parent.getRecord().get("ID")
            });*/
            UrlUtil.goTo(url);
        };

        var editPermissionHandler = function (source) {
            var record = source.parent.getRecord();
            var principalID = record.get("ID");
            UrlUtil.goTo("#/Admin/Permission/List.htm", {
                Identifier: principalID,
                Type: "Indigox.Common.Membership.Interfaces.IPrincipal"
            });
        };

        return exports;
    });