define("/UUM/Widgets/Profile/EditWidget",
    [
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/OrganizationalPerson"
    ],
function (
        InstructionProxy,
        RecordManager,
        FormController
) {
    function exports(widget) {

        var formControl = $(widget).Content("EditArea").first();
        $(formControl).FileUploadList("Profile").first().configure({
            multi: false
        });

        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                    proxy: new InstructionProxy({
                        query: "ProfileQuery",
                        updateCommand: "UpdateOrganizationalPersonCommand"
                    })
                })
            })
        });
    }

    return exports;
});