define("/UUM/Widgets/OrganizationalPerson/EditWidget",
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
        "/UUM/Widgets/Common/MailDatabaseData",
        "Indigox/UUM/Application/OrganizationalPerson"
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
        MailAddressRule,
        MailDatabaseData
) {
    function exports(widget) {
        var mailDatabse = $(widget).Content("EditArea").DropDownList("MailDatabase").first();

        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                autoLoad: false,
                model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                    proxy: new InstructionProxy({
                        createCommand: "CreateOrganizationalPersonCommand",
                        updateCommand: "UpdateOrganizationalPersonCommand",
                        deleteCommand: "DisableOrganizationalPersonCommand"
                    })
                })
            })
        });

        mailDatabse.configure({
            items: MailDatabaseData,
            ReturnValueType: "STRING",
            allowBlank: true,
        });

        var formControl = $(widget).Content("EditArea").first();
        $(formControl).TextBox("Name").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "用户名称不允许为空"
                }
            ]
        });

        $(formControl).TextBox("AccountName").first().configure({
            "validateRules": [
                {
                    "type": "notblank",
                    "errorMessage": "帐号不允许为空"
                }
            ]
        });

        $(formControl).FileUploadList("Profile").first().configure({
            multi: false
        });

        $(formControl).TextBox("Email").first().configure({
            "validateRules": [
                {
                    "type": "mail",
                    "errorMessage": "邮箱格式不正确"
                }
            ]
        });

        $(formControl).RichTextBox("OtherContact").first().configure({
            "mode": "plaintext"
        });

        $(formControl).UserSelect("MemberOfOrganizationalRoles").first().configure({
            mode: "4",
            multi: true
        });

        $(formControl).UserSelect("MemberOfGroups").first().configure({
            mode: "3",
            multi: true
        });

        var organizationalPersonID = Page().getUrlParam("OrganizationalPersonID");
        var organizationalPerson = null;

        if (organizationalPersonID) {
            mailDatabse.setReadonly(true);
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("OrganizationalPersonQuery");
            form.getController().setParam("OrganizationalPersonID", organizationalPersonID);
            form.getController().load();
        }
        else {
            var parentOrganizationalUnitID = Page().getUrlParam("ParentOrganizationalUnitID");            

            var batch = Batch.beginBatch();
            batch.single({
                name: "DefaultOrganizationalPersonQuery",
                properties: {
                    OrganizationalUnitID: parentOrganizationalUnitID
                },
                callback: function (data) {
                    organizationalPerson = data;
                    organizationalPerson.Organization = parentOrganizationalUnitID;
                    organizationalPersonID = organizationalPerson.ID;

                    var controller = $(widget).Content("EditArea").first().getController();
                    controller.getModel().addRecord(organizationalPerson);
                }
            });
            batch.syncCommit();
        }

        Page().listenUrlParamChanged(["OrganizationalPersonID"], { container: widget }, function () {
            var organizationalPersonID = Page().getUrlParam("OrganizationalPersonID");
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("OrganizationalPersonQuery");
            form.getController().setParam("OrganizationalPersonID", organizationalPersonID);
            form.getController().load();
        });
    }

    return exports;
});