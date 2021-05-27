define("/UUM/Widgets/OrganizationalPerson/EnableToDialogWidget",
    [
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.HierarchyController",
        "Indigox.Web.JsLib.Controls.Selection.TreeMode",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
    function (
        AutoBatch,
        Batch,
        ArrayProxy,
        RecordManager,
        HierarchyController,
        TreeMode
    ) {
        var exports = function (widget) {
            var dialog = $(widget).Dialog("EnableToDialog").first();
            var orgTree = $(dialog).Tree("orgTree").first();

            orgTree.configure({
                mode: "SINGLE",
                allowDeselect: false,
                selectable: true,
                treeNodeType: "checktreenode",
                valueField: "ID"
            });

            var id = ''; // dialogArgs
            var organization = ''

            dialog.on("opened", function (source, e) {
                id = this.getParam("ID");
                organization = this.getParam("Organization");

                bindTree(orgTree, organization);
            });

            $(dialog).Button("btnCancel").on('clicked', function () {
                dialog.close();
            });

            $(dialog).Button("btnOK").on('clicked', function () {
                Page().mask();

                var targetCategories = orgTree.getValue();

                var principals = dialog.getParam("Principals");

                if (targetCategories && targetCategories[0]) {
                    var batch = Batch.beginBatch();
                    batch.execute({
                        name: 'EnableOrganizationalPersonCommand',
                        properties: {
                            ID: id,
                            OrganizationalUnitID: targetCategories[0]
                        }
                    })
                    .done(function (data) {
                        alert('启用用户成功。');
                        dialog.close();
                    })
                    .fail(function (error) {
                        alert('启用用户失败！');
                        debug.error(error);
                    })
                    .always(function () {
                        Page().unmask();
                    });
                    batch.commit();
                }
                else {
                    alert("请选择部门！");
                    Page().unmask();
                }
            });
        };

        function bindTree(orgTree, organization) {
            AutoBatch.getCurrentBatch().list({
                name: 'OrganizationalUnitListQuery',
                properties: {
                    ContainsLeaf: false,
                    RelativeFrom: "",
                    LevelLimit: 0
                }
            })
            .done(function (orgTreeData) {
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


                var path = getPath(orgTreeData, organization[0], "ID", "Organization");
                orgTree.expandPath(path);

                orgTree.setValue(organization);
            });
        }

        function getPath(datalist, value, idField, parentidField) {
            var path = "";
            var parent = null;
            var i = null,
            length = null;
            for (i = 0, length = datalist.length; i < length; i++) {
                var data = datalist[i];
                if (data[idField] == value) {
                    parent = data[parentidField];
                    break;
                }
            }
            if (parent != null) {
                path = getPath(datalist, parent, idField, parentidField) + "/" + value;
            }
            else {
                path = value;
            }
            return path;
        }

        return exports;
    });