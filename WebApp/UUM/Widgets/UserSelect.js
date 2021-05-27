define("/UUM/Widgets/UserSelect",
    [
        "Indigox.Web.JsLib.Utils.DelayedTask",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.Html.Dialog",
        "Indigox/UUM/Application/AddressBook"
    ],
    function (
        DelayedTask,
        UrlUtil,
        StringUtil,
        InstructionProxy,
        RecordManager,
        ListController,
        ItemMode,
        Dialog
    ) {
        var exports = function (widget) {
            var dialog = $(widget).Dialog("userSelectDialog").first();
            var searchPanel = $(widget).Content('searchPanel').first();
            var optionUsers = $(widget).DataList('optionUsers').first();
            var optionUsersPaging = $(widget).Paging('optionUsersPaging').first();
            var selectedUsers = $(widget).DataList('selectedUsers').first();
            var toolbar = $(widget).Content('toolbar').first();

            dialog.on('opened', function (source, e) {
                var value = this.getParam("value");
                var mode = this.getParam("mode");
                var multi = this.getParam("multi");

                initializeToolbar(toolbar, dialog, optionUsers);

                initializeOptionUsers(optionUsers, optionUsersPaging, mode, multi);

                initializeSearchPanel(searchPanel, optionUsersPaging, optionUsers);

                initializeSelectedUsers(selectedUsers);

                optionUsers.on('itemSelectedChanged', (function () {
                    var delayedTask = new DelayedTask(function (optionUsers, selectedUsers) {
                        var value = optionUsers.getSelMode().selected.elements;
                        configureSelectedUsers(selectedUsers, value);
                    }, source, [optionUsers, selectedUsers]);

                    return function (source, e) {
                        //debug.log('itemSelectedChanged: ', optionUsers.getValue());
                        delayedTask.delay(100);
                    };
                } ()));

                selectedUsers.on("itemAdded", function (source, index, item) {
                    var task = new DelayedTask(function () {
                        //debug.log(item, item.record);
                        if (!multi) {
                            optionUsers.setValue([]);
                        }
                        else {
                            optionUsers.deselectItem(item.record);
                        }
                    }, item, []);

                    var onclicked = function (source, e) {
                        task.delay(100);
                    };

                    item.on("clicked", onclicked);
                });

                dialog.un('opened', arguments.callee);
            });

            dialog.on('opened', function (source) {
                //debug.log("arguments:", arguments);

                var value = source.getParam("value"),
                    mode = source.getParam("mode"),
                    multi = source.getParam("multi");

                //debug.log("set multi mode...");
                try {
                    optionUsers.setAllowDeselect((multi ? true : false));
                    optionUsers.setMode(multi ? "MULTI" : "SINGLE");
                } catch (e) {
                }

                var recordSet = RecordManager.getInstance().createRecordSet('AddressBook');
                recordSet.loadRecords(value);
                var records = recordSet.getRecords();

                optionUsers.setValue(records);
            });
        };

        function initializeToolbar(toolbar, dialog, optionUsers) {
            toolbar.find('btnOK')[0].on('clicked', function () {
                //debug.log('selectedUsers: ', optionUsers.getValue());

                var selected = optionUsers.getSelMode().selected.elements;
                var returnValue = [];

                for (var i = 0, length = selected.length; i < length; i++) {
                    if (selected[i]) {
                        returnValue.push(selected[i].data || selected[i]);
                    }
                }

                dialog.dialogResult = Dialog.DIALOG_RESULT_OK;
                dialog.returnValue = returnValue;
                dialog.close();
            });

            toolbar.find('btnCancel')[0].on('clicked', function () {
                //debug.log('canceled.');
                dialog.dialogResult = Dialog.DIALOG_RESULT_CANCEL;
                dialog.close();
            });

            toolbar.find('btnClear')[0].on('clicked', function () {
                optionUsers.setValue([]);
            });
        }

        function initializeOptionUsers(listControl, pagingControl, mode, multi) {
            //debug.log('initializeOptionUsers...');

            var limit = 10;

            var proxy = new InstructionProxy({
                query: "AddressBookListQuery"
            });

            var arrayController = new ListController({
                params: {
                    QueryString: "",
                    TypeFilters: [],
                    FetchSize: limit
                },
                model: RecordManager.getInstance().createRecordSet('AddressBook', {
                    proxy: proxy,
                    addRecords: true
                })
            });

            listControl.setController(arrayController);

            if (pagingControl) {
                pagingControl.configure({
                    pageSize: limit,
                    arrayController: arrayController
                });
            }

            //debug.log("set selmode...");
            listControl.setSelMode(new ItemMode({
                allowDeselect: (multi ? true : false),
                returnValueType: "ARRAY",
                mode: (multi ? "MULTI" : "SINGLE")
            }));
        }

        function initializeSearchPanel(searchPanel, pagingControl, optionUsers) {
            var txtKeyword = searchPanel.find('txtKeyword')[0];
            var btnSearch = searchPanel.find('btnSearch')[0];

            btnSearch.on('clicked', function () {
                //debug.log('search...');
                //debug.log('keyword: ' + txtKeyword.getValue());

                var queryString = txtKeyword.getValue();

                optionUsers.getController().setParam('QueryString', queryString);

                pagingControl.reset();
            });
        }

        function initializeSelectedUsers(listControl, userRecords) {
            var recordSet = RecordManager.getInstance().createRecordSet('AddressBook');

            var arrayController = new ListController({
                model: recordSet
            });

            listControl.setController(arrayController);
        }

        function configureSelectedUsers(listControl, value) {
            var recordSet = RecordManager.getInstance().createRecordSet('AddressBook');

            var arrayController = new ListController({
                model: recordSet
            });

            listControl.setController(arrayController);

            if (value) {
                for (var i = 0, length = value.length; i < length; i++) {
                    recordSet.addRecord(value[i]);
                }
            }
        }

        return exports;
    });