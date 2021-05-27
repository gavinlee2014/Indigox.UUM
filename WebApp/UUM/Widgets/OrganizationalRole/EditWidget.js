define("/UUM/Widgets/OrganizationalRole/EditWidget",
    [
        "Indigox.Web.JsLib.Utils.ArrayUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Validation.Rules.NotBlankRule",
        "Indigox.Web.JsLib.Controls.Validation.Rules.MailAddressRule",
        "Indigox/UUM/Application/OrganizationalRole",
        "Indigox/UUM/Application/Role"
    ],
function (
        ArrayUtil,
        UrlUtil,
        Batch,
        InstructionProxy,
        RecordManager,
        FormController,
        ListController,
        NotBlankRule,
        MailAddressRule
) {
    function exports(widget) {
        var organizationalRoleID = Page().getUrlParam("OrganizationalRoleID");
        var parentOrganizationalUnitID = Page().getUrlParam("ParentOrganizationalUnitID");
        var organizationalRole = null;

        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                autoLoad: false,
                model: RecordManager.getInstance().createRecordSet('OrganizationalRole', {
                    proxy: new InstructionProxy({
                        createCommand: "CreateOrganizationalRoleCommand",
                        updateCommand: "UpdateOrganizationalRoleCommand",
                        deleteCommand: "DeleteOrganizationalRoleCommand"
                    })
                })
            })
        });

        $(widget).Content("EditArea").UserSelect("Members").first().configure({
            mode: "2",
            multi: true
        });

        var formControl = $(widget).Content("EditArea").first();
        $(formControl).TextBox("Name").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "组织角色名称不允许为空"
                }
            ]
        });

        $(formControl).TextBox("Email").first().configure({
            "validateRules": [
                {
                    "type": "mail",
                    "errorMessage": "邮箱格式不正确"
                }
            ]
        });

        $(widget).UserSelect("Members").setMulti(true);
        $(formControl).RichTextBox("Description").first().configure({
            "mode": "plaintext"
        });

        $(widget).Content("EditArea").DropDownList("Role").first().configure({
            allowBlank: true,
            binding: {
                mapping: {
                    value: function (record) {
                        var role = record.get("Role");
                        return isNullOrUndefined(role) ? "" : record.get("Role").UserID;
                    },

                    writeValue: function (record, control) {
                        record.set("Role", control.getValue());
                    }
                }
            },
            ReturnValueType: "STRING",
            controller: new ListController({
                itemOptions: {
                    binding: {
                        mapping: {
                            text: "Name",
                            value: "ID"
                        }
                    }
                },
                model: RecordManager.getInstance().createRecordSet('Role', {
                    proxy: new InstructionProxy({
                        query: "RoleForUnitListQuery"
                    })
                }),
                params: {
                    OrganizationalRoleID: organizationalRoleID,
                    OrganizationalUnitID: parentOrganizationalUnitID
                }
            })
        });

        if (organizationalRoleID) {
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("OrganizationalRoleQuery");
            form.getController().setParam("OrganizationalRoleID", organizationalRoleID);
        }
        else {

            var batch = Batch.beginBatch();
            batch.single({
                name: "DefaultOrganizationalRoleQuery",
                callback: function (data) {
                    organizationalRole = data;
                    organizationalRole.Organization = parentOrganizationalUnitID;
                    organizationalRoleID = organizationalRole.ID;

                    var controller = $(widget).Content("EditArea").first().getController();
                    controller.getModel().addRecord(organizationalRole);
                }
            });
            batch.syncCommit();
        }

        Page().listenUrlParamChanged(["OrganizationalRoleID"], { container: widget }, function () {
            var organizationalRoleID = Page().getUrlParam("OrganizationalRoleID");
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("OrganizationalRoleQuery");
            form.getController().setParam("OrganizationalRoleID", organizationalRoleID);
            form.getController().load();
        });
    }

    return exports;
});