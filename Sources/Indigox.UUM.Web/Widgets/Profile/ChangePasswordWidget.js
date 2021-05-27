define("/UUM/Widgets/Profile/ChangePasswordWidget",
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
        $(widget).Content("EditArea").first().configure({
            controller: new FormController({
                model: RecordManager.getInstance().createRecordSet('OrganizationalPerson', {
                    proxy: new InstructionProxy({
                        query: "ProfileQuery"
                    })
                })
            })
        });
    }

    return exports;
});