define("/UUM/Widgets/AddressBook/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/UUM/Application/AddressBook"
    ],
    function (
        StringUtil,
        UrlUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        ItemMode,
        PageUrlMonitor
    ) {
        var exports = function (widget) {
            var limit = 10;

            var listControl = $(widget).DataList("AddressBookList").first();
            configurateItemTemplate(listControl);

            new PageUrlMonitor({ paramFilters: ["OrganizationalUnitID"], container: widget })
                .onUrlParamChanged(function () {
                    reloadPrincipalList(widget);
                });

            var arrayController = new ListController({
                model: RecordManager.getInstance().createRecordSet('AddressBook', {
                    proxy: new InstructionProxy({
                        query: "AddressBookListQuery"
                    })
                }),
                params: {
                    "FetchSize": limit,
                    "OrganizationalUnitID":Page().getUrlParam('OrganizationalUnitID')
                }
            });

            $(widget).Button("btnExportCSV").on("clicked", function () {
                var queryString = $(widget).TextBox("searchKey").first().getValue();
                var organizationalUnitID = Page().getUrlParam('OrganizationalUnitID');
                var href = "/UUM/AddressBook/Export.ashx";

                UrlUtil.open(href, {
                    QueryString: queryString,
                    OrganizationalUnit: organizationalUnitID
                });
            });
            $(widget).Paging("Paging").first().configure({
                pageSize: limit,
                arrayController: arrayController
            });

            $(widget).Button("searchButton").first().on("clicked", function (source, e) {
                search();
            });

            //            $(widget).Button("searchKey").first().on("clicked", function (source, e) {
            //                search();
            //            });

            function search() {
                var queryString = $(widget).TextBox("searchKey").first().getValue();
                var paging = $(widget).Paging("Paging").first();
                paging.getArrayController().setParam("QueryString", queryString);
                paging.reset();
            }

            listControl.setController(arrayController);

            listControl.setSelMode(new ItemMode({
                allowDeselect: false,
                returnValueType: "ARRAY",
                mode: "SINGLE"
            }));

            listControl.on("itemAdded", function (source, index, item) {
                $(item).on("clicked", function (src) {
                    var principalID = item.getRecord().get("UserID");
                    $.Dialog("AddressBookViewDialog").first().open({ "PrincipalID": principalID });
                });
            });
        };

        var reloadPrincipalList = function (widget) {
            var paging = $(widget).Paging("Paging").first();
            paging.getArrayController().setParam("QueryString", "");
            $(widget).TextBox("searchKey").first().setValue("");

            var organizationalUnitID = Page().getUrlParam('OrganizationalUnitID');
            var listControl = $(widget).DataList("AddressBookList").first();
            var controller = listControl.getController();
            controller.setParam("OrganizationalUnitID", organizationalUnitID);
            paging.reset();
        };

        function configurateItemTemplate(listControl) {
            var listTemplate = listControl.getItemTemplate();
            /*listTemplate.getChild("UserOrgAndTitle").configure({
            binding: {
            mapping: {
            value: function (record) {
            var sections = [];
            if (!StringUtil.isNullOrEmpty(record.get("UserOrg"))) {
            sections.push(record.get("UserOrg"));
            }
            if (!StringUtil.isNullOrEmpty(record.get("Title"))) {
            sections.push(record.get("Title"));
            }
            return sections.join("/");
            }
            }
            }
            });*/
            listTemplate.getChild("UserName").configure({
                binding: {
                    mapping: {
                        value: function (record) {
                            return record.get("DisplayName") == null ? record.get("UserName") : record.get("DisplayName");
                        }
                    }
                }
            });
        }

        return exports;
    });