define("/UUM/Widgets/Principal/ChangeOrganizationWidget",
    [
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Controllers.HierarchyController",
        "Indigox.Web.JsLib.Utils.AsyncUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
function (
        RecordManager,
        UrlUtil,
        ArrayProxy,
        HierarchyController,
        AsyncUtil,
        AutoBatch,
        Batch
) {
    var exports = function (widget) {
        var dialog = $(widget).Dialog("ChangeOrganizationDialog").first();
        var orgTree = $(dialog).Tree("orgTree").first();

        //orgTree.setValue([]);
        bindTree(orgTree);

        $(dialog).Button("btnCancelChange").on('clicked', function () {
            dialog.close();
        });

        $(dialog).Button("btnConfirmChange").on('clicked', function () {
            Page().mask();

            var targetCategories = orgTree.getValue();

            var principals = dialog.getParam("Principals");

            if ((!targetCategories) || (targetCategories.length > 0)) {
                var batch = Batch.beginBatch();
                batch.execute({
                    name: 'ChangeOrganizationCommand',
                    properties: {
                        PrincipalList: principals,
                        TargetOrganization: targetCategories[0]
                    },
                    callback: function (data) {
                        dialog.close();
                        Page().unmask();
                        $.DataList("PrincipalList").setValue(null);
                        $.DataList("PrincipalList").first().getController().load();
                    },
                    errorCallbak: function (error) {
                        alert("移动部门失败!");
                        debug.error(error);
                        Page().unmask();
                    }
                });
                batch.commit();
            }
            else {
                alert("请选择新的部门");
                Page().unmask();
            }
        });
    };

    function bindTree(orgTree) {
        orgTree.configure({
            mode: "SINGLE",
            allowDeselect: false,
            selectable: true,
            treeNodeType: "checktreenode",
            valueField: "ID"
        });

        var batch = AutoBatch.getCurrentBatch();
        batch.list({
            name: 'OrganizationalUnitListQuery',
            properties: {
                ContainsLeaf: false,
                RelativeFrom: "",
                LevelLimit: 0
            },
            callback: function (orgTreeData) {
                orgTree.configure({
                    controller: new HierarchyController({
                        model: RecordManager.getInstance().createRecordSet('OrganizationalUnit', {
                            addRecords: true,
                            proxy: new ArrayProxy({
                                array: orgTreeData
                            })
                        }),
                        rootValue: null,
                        nodeOptions: {
                            binding: {
                                mapping: {
                                    text: "Name",
                                    value: "ID"
                                }
                            }
                        }
                    })
                });
            }
        });
    }

    return exports;
});