define("/UUM/Widgets/HR/HROrganizationalSyncWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.AsyncUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox/UUM/Application/HROrganizational",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
function (
        UrlUtil,
        AsyncUtil,
        ErrorHandler,
        Formatters,
        RecordManager,
        FormController,
        ListController,
        Batch,
        InstructionProxy
) {
    var exports = function (widget) {

        $(widget).Content("HROrganizationalEditPanel").Label("State").first().configure({
            binding: {
                mapping: {
                    value: new Formatters.ValueTextFormatter({
                        mapping: [{
                            value: 0, text: "新建"
                        }, {
                            value: 1, text: "修改"
                        }, {
                            value: 2, text: "删除"
                        }]
                    })
                }
            }
        });

        $(widget).DropDownList('bindExistAccount').first().configure({
            allowBlank: true,
            ReturnValueType: "STRING"
        });

        /*
         * 修改时间2018-08-23
         * 修改人：曾勇
         * 修改内容：增加判断，如果对应上级部门还在UUM中还未创建，不允许同步
         */
        $(widget).Content("HREmployeeEditPanel").Label("ParentName").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "对应部门还未在UUM中创建"
                }
            ]
        });

        $(widget).Dialog('HROrganizationalSyncDialog').on('opened', function (source) {
            var id = this.getParam("ID");
            var state = this.getParam("State");

            if (state == 0) {
                $(widget).Content('bindExistPanel').first().setVisible(true);
            }
            else {
                $(widget).Content('bindExistPanel').first().setVisible(false);
            }

            $(widget).DropDownList('bindExistAccount').first().configure({
                controller: new ListController({
                    itemOptions: {
                        binding: {
                            mapping: {
                                text: function (record) {
                                    return record.get('DisplayName');
                                },
                                value: "ID"
                            }
                        }
                    },
                    model: RecordManager.getInstance().createRecordSet('OrganizationalUnit', {
                        proxy: new InstructionProxy({
                            query: "HROrganizationalExistListQuery"
                        })
                    }),
                    params: {
                        ID: id
                    }
                })
            });

            /*
             * 修改时间 2018-08-23
             * 修改人   曾勇
             * 修改内容：
             * 1、在Dialog打开时，重置各Label的值
             * 2、重置绑定账号的DropDownList的值
             */
            $(widget).Content("HROrganizationalEditPanel").Label("ParentName").first().setValue("");
            $(widget).Content("HROrganizationalEditPanel").Label("Description").first().setValue("");
            $(widget).DropDownList('bindExistAccount').first().setValue("");

            $(widget).Content("HROrganizationalEditPanel").first().reset();
            $(widget).Content("HROrganizationalEditPanel").configure({
                controller: new FormController({
                    model: RecordManager.getInstance().createRecordSet('HROrganizational', {
                        proxy: new InstructionProxy({
                            updateCommand: "HROrganizationalUpdateCommand",
                            query: "HROrganizationalQuery"
                        })
                    }),
                    params: {
                        "ID": id
                    }
                })
            });
        });

        $(widget).Button("btnSubmit").on("clicked", function () {
            Page().mask();

            var dialog = $(widget).Dialog('HROrganizationalSyncDialog').first();
            var form = $(dialog).Content("HROrganizationalEditPanel").first();
            /*
             * 修改时间2018-08-23
             * 修改人：曾勇
             * 修改内容：增加判断，如果对应部门还在UUM中还未创建，不允许同步
             */
            var record = form.getController().getModel().getRecord(0);
            if (record.get("ParentName") == null || record.get("ParentName") == "") {
                alert("对应部门还未在UUM中创建，不能同步！");
                Page().unmask();
                return;
            }

            form.on("submit", function (source, successed) {
                if (successed) {
                    var record = form.getController().getModel().getRecord(0);
                    var batch = Batch.beginBatch();
                    batch.execute({
                        name: 'HROrganizationalSyncCommand',
                        properties: {
                            ID: record.get("ID"),
                            PrincipalID: $(widget).DropDownList('bindExistAccount').getValue()
                        },
                        callback: function (data) {
                            record.set("Synchronized", true);
                            dialog.close();
                            alert("同步成功!");
                            Page().unmask();
                        },
                        errorCallback: function (error) {
                            ErrorHandler.logAlert(error);
                            alert("同步失败!" + error.Message);
                            Page().unmask();
                        }
                    });
                    batch.commit();
                }
                else {
                    alert("同步失败！");
                    Page().unmask();
                }
                this.un('submit', arguments.callee);
            });
            form.submit();
        });

        $(widget).Button("btnCancel").on("clicked", function () {
            var dialog = $(widget).Dialog('HROrganizationalSyncDialog').first();
            dialog.close();
        });
    };


    return exports;
});