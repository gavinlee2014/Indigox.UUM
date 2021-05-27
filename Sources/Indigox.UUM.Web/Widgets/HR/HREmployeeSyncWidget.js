define("/UUM/Widgets/HR/HREmployeeSyncWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.AsyncUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Formatters.DateTimeFormatter",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "/UUM/Widgets/Common/MailDatabaseData",
        "Indigox/UUM/Application/HREmployee",
        "Indigox/UUM/Application/OrganizationalPerson"
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
        DateTimeFormatter,
        InstructionProxy,
        MailDatabaseData
) {
    var exports = function (widget) {
        var mailDatabse = $(widget).Content("HREmployeeEditPanel").DropDownList("MailDatabase").first();

        mailDatabse.configure({
            items: MailDatabaseData,
            ReturnValueType: "STRING",
            allowBlank: true,
        });

        $(widget).Content("HREmployeeEditPanel").TextBox("Email").first().configure({
            "validateRules": [
                {
                    "type": "mail",
                    "errorMessage": "邮箱格式不正确"
                }
            ]
        });

        $(widget).Content("HREmployeeEditPanel").Literal("HasPolyphone").first().configure({
            binding: {
                mapping: {
                    value: function (record) {
                        var tip = "";
                        var hasPolyphone = record.get("HasPolyphone");
                        if(hasPolyphone == true){
                            tip = "用户名字包含多音字，请注意用户登录帐号！";
                        }
                        var quitDate = record.get("QuitDate");
                        if( quitDate != '0001-01-01 00:00:00' && record.get("State") == "4"){
                            tip += "  离职日期：" + quitDate;
                        } 
                        return tip;
                    }
                }
            }
        });

        $(widget).Content("HREmployeeEditPanel").Label("State").first().configure({
            binding: {
                mapping: {
                    value: new Formatters.ValueTextFormatter({
                        mapping: [{
                            value: 0, text: "新建"
                        }, {
                            value: 1, text: "修改"
                        }, {
                            value: 2, text: "删除"
                        }, {
                            value: 3, text: "启用"
                        }, {
                            value: 4, text: "禁用"
                        }]
                    })
                }
            }
        });

        $(widget).DropDownList('bindExistAccount').first().configure({
            allowBlank: true,
            ReturnValueType: "STRING"
        });

        $(widget).Dialog('HREmployeeSyncDialog').on('opened', function (source) {
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
                                    return record.get('Name') + "(" + record.get('AccountName') + ")";
                                },
                                value: "ID"
                            }
                        }
                    },
                    model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                        proxy: new InstructionProxy({
                            query: "HREmployeeExistListQuery"
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
            $(widget).Content("HREmployeeEditPanel").Label("Title").first().setValue("");
            $(widget).Content("HREmployeeEditPanel").Label("ParentName").first().setValue("");
            $(widget).Content("HREmployeeEditPanel").Label("Tel").first().setValue("");
            $(widget).Content("HREmployeeEditPanel").Label("Mobile").first().setValue("");
            $(widget).Content("HREmployeeEditPanel").Label("IdCard").first().setValue("");
            $(widget).Content("HREmployeeEditPanel").Label("Fax").first().setValue("");
            $(widget).Content("HREmployeeEditPanel").Label("Description").first().setValue("");
            $(widget).DropDownList('bindExistAccount').first().setValue("");

            $(widget).Content("HREmployeeEditPanel").first().reset();
            $(widget).Content("HREmployeeEditPanel").configure({
                controller: new FormController({
                    model: RecordManager.getInstance().createRecordSet('HREmployee', {
                        proxy: new InstructionProxy({
                            updateCommand: "HREmployeeUpdateCommand",
                            query: "HREmployeeQuery"
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

            var dialog = $(widget).Dialog('HREmployeeSyncDialog').first();
            var form = $(dialog).Content("HREmployeeEditPanel").first();

            var record = form.getController().getModel().getRecord(0);
            var quitDate = record.get("QuitDate");
            if( record.get("State") == "4" && new DateTimeFormatter().convertDateFromString( quitDate ) > new Date()){                
                alert("该用户离职日期还未到，不能同步！");
                Page().unmask();
                return ;
            }
            /*
             * 修改时间2018-08-23
             * 修改人：曾勇
             * 修改内容：增加判断，如果对应部门还在UUM中还未创建，不允许同步
             */
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
                        name: 'HREmployeeSyncCommand',
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
            var dialog = $(widget).Dialog('HREmployeeSyncDialog').first();
            dialog.close();
        });
    };


    return exports;
});