define("/UUM/Widgets/AddressBook/ViewWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.Browser",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/AddressBook"
    ],
    function (
        UrlUtil,
        Browser,
        RecordManager,
        InstructionProxy,
        FormController
    ) {
        var exports = function (widget) {
            var dialog = $(widget).Dialog("AddressBookViewDialog").first();
            var formControl = $(widget).Content("AddressBookViewForm").first();
            $(formControl).Literal("Profile").configure({
                mode: 0,
                binding: {
                    mapping: {
                        value: function (record) {
                            var profile = record.get("Profile");
                            if (!isNullOrUndefined(profile) && profile != "") {
                                return '<div style="float: right;"><img src="' + profile  + '" /></div>';
                            }
                            return null;
                        }
                    }
                }
            });

            var proxy = new InstructionProxy({
                query: "AddressBookQuery"
            });

            $(formControl).Button("btnClose").first().on('clicked', function () {
                dialog.close();
            });



            dialog.on("opened", function (source) {
                var controller = new FormController();
                controller.configure({
                    model: RecordManager.getInstance().createRecordSet('AddressBook', {
                        proxy: proxy
                    }),
                    params: {
                        "PrincipalID": dialog.getParam("PrincipalID")
                    }
                });

                formControl.setController(controller);
            });
        };

        return exports;
    });