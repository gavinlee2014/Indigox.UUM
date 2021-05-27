define("/UUM/Widgets/Principal/ListWidget",
    [
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
            var organizationalUnitID = Page().getUrlParam("OrganizationalUnitID");
            var limit = 10;

            $(widget).DataList("PrincipalList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnEditPrincipal").on("clicked", editHandler);
                //$(item).Button("btnEditPermission").on("clicked", editPermissionHandler);
            });

            new PageUrlMonitor({ paramFilters: ["OrganizationalUnitID"], container: widget })
                .onUrlParamChanged(function () {
                    reloadPrincipalList(widget);
                });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                    proxy: new InstructionProxy({
                        query: "GeneralListQuery"
                    })
                }),
                params: {
                    "OrganizationalUnitID": organizationalUnitID,
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

            //TODO: 不能放到 setController 之前设置 selMode
            listControl.setSelMode(new ItemMode({
                allowDeselect: true,
                mode: "MULTI",
                returnValueType: "ARRAY"
            }));

            $(widget).Content('searchPanel').CheckBoxList('typeFilters').first().configure({
                items: [
                    { value: "OrganizationalUnit", text: "部门", selected: true },
                    { value: "OrganizationalPerson", text: "用户", selected: true },
                    { value: "OrganizationalRole", text: "组织角色", selected: true }
                ]
            });

            $(widget).Content('searchPanel').CheckBoxList('typeFilters').first().on('valueChanged', function () {
                var typeFilters = $(widget).Content('searchPanel').CheckBoxList('typeFilters').first().getValue();
                //debug.log(typeFilters);

                var controller = $(widget).DataList('PrincipalList').first().getController();
                controller.setParam('TypeFilters', typeFilters);
                controller.load();
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

        var reloadPrincipalList = function (widget) {
            var organizationalUnitID = Page().getUrlParam('OrganizationalUnitID');
            var listControl = $(widget).DataList("PrincipalList").first();
            var controller = listControl.getController();
            controller.setParam("OrganizationalUnitID", organizationalUnitID);
            //controller.load();
            $(widget).Paging("Paging").first().reset();
        };

        return exports;
    });