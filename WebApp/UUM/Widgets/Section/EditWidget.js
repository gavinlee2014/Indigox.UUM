define("/UUM/Widgets/Section/EditWidget",
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
        "Indigox/UUM/Application/OrganizationalUnit"
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
        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                autoLoad: false,
                model: RecordManager.getInstance().createRecordSet('OrganizationalUnit', {
                    proxy: new InstructionProxy({
                        createCommand: "CreateOrganizationalUnitCommand",
                        updateCommand: "UpdateOrganizationalUnitCommand",
                        deleteCommand: "DeleteOrganizationalUnitCommand"
                    })
                })
            })
        });

        var formControl = $(widget).Content("EditArea").first();
        $(formControl).TextBox("Name").first().configure({
            "validateRules": [
                    {
                        "type": "notblank",
                        "errorMessage": "部门名称不允许为空"
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

        $(formControl).RichTextBox("Description").first().configure({
            "mode": "plaintext"
        });

        var organizationalUnitID = Page().getUrlParam("SectionID");
        var organizationalUnit = null;

        if (organizationalUnitID) {
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("OrganizationalUnitQuery");
            form.getController().setParam("OrganizationalUnitID", organizationalUnitID);
            form.getController().load();
        }
        else {
            var parentOrganizationalUnitID = Page().getUrlParam("ParentOrganizationalUnitID");

            var batch = Batch.beginBatch();
            batch.single({
                name: "DefaultOrganizationalUnitQuery",
                callback: function (data) {
                    organizationalUnit = data;
                    organizationalUnit.Organization = parentOrganizationalUnitID;
                    organizationalUnit.Type = "Section";
                    organizationalUnitID = organizationalUnit.ID;

                    var controller = $(widget).Content("EditArea").first().getController();
                    controller.getModel().addRecord(organizationalUnit);
                }
            });
            batch.syncCommit();
        }

        Page().listenUrlParamChanged(["SectionID"], { container: widget }, function () {
            var organizationalUnitID = Page().getUrlParam("SectionID");
            var form = $(widget).Content("EditArea").first();
            form.getController().getModel().getProxy().setQuery("OrganizationalUnitQuery");
            form.getController().setParam("OrganizationalUnitID", organizationalUnitID);
            form.getController().load();
        });
    }

    return exports;
});