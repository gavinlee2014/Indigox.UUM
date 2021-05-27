define("/UUM/Widgets/OrganizationalPerson/SetPasswordWidget",
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
            var dialog = $(widget).Dialog("SetPasswordDialog").first();

            dialog.on("opened", function (source, e) {
                var record = this.getParam("data");
                var form = $(widget).Content("SetPasswordPanel").first();
                form.getBinding().bind(form, record);
            });

            $(dialog).Button("btnCancel").on('clicked', function () {
                dialog.close();
            });

            $(dialog).Button("btnOK").on('clicked', function () {
                Page().mask();
                var form = $(widget).Content("SetPasswordPanel").first();
                //var record = form.getController().getModel().getRecord(0);
                var account = $(form).Label("AccountName").getValue();
                var password = $(form).TextBox("Password").getValue();

                if (password) {
                    var batch = Batch.beginBatch();
                    batch.execute({
                        name: 'SetPasswordCommand',
                        properties: {
                            AccountName: account,
                            Password: password
                        }
                    })
                    .done(function (data) {
                        alert('修改密码成功。');
                        dialog.close();
                        $(form).TextBox("Password").setValue("");
                    })
                    .fail(function (error) {
                        alert('修改密码失败！');
                        debug.error(error);
                    })
                    .always(function () {
                        Page().unmask();
                    });
                    batch.commit();
                }
                else {
                    alert("请输入密码！");
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