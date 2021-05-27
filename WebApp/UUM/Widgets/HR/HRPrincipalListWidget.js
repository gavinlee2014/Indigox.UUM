define("/UUM/Widgets/HR/HRPrincipalListWidget",
    [
        "/UUM/Widgets/HR/SearchHRPrincipalContentType",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.Util",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Formatters",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox/UUM/Application/HRPrincipal"
    ],
function (
        SearchHRPrincipalContentType,
        ArrayProxy,
        UrlUtil,
        Util,
        StringUtil,
        ErrorHandler,
        Formatters,
        ItemMode,
        Batch,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController
) {
    var exports = function (widget) {
        var limit = 10;

        //initSearchPanel(widget);

        var selectAll = $(widget).CheckBox("selectAll").first();

        var controller = new ListController({
            model: RecordManager.getInstance().createRecordSet('HRPrincipal', {
                proxy: new InstructionProxy({
                    query: "HRPrincipalListQuery"
                })
            }),
            params: {
                name: '',
                state: -1,
                "FetchSize": limit
            }
        });

        RecordManager.getInstance().register("SearchHRPrincipalSchema", {
            "columns": [{
                "name": "Name",
                "text": "名称"
            }, {
                "name": "State",
                "text": "状态"
            }]
        });

        var searchFormController = new FormController({
            model: RecordManager.getInstance().createRecordSet("SearchHRPrincipalSchema", {
                proxy: new ArrayProxy({
                    array: [{}]
                })
            })
        });

        var contentType = SearchHRPrincipalContentType;
        var searchPanel = $(widget).Content("SearchPanel").first();
        searchPanel.setControls(contentType.controls);

        searchPanel.configure({
            controller: searchFormController
        });

        $(widget).Button("btnSearch").first().on("clicked", function () {
            reload();
        });

        var reload = function () {
            var searchPanel = $(widget).Content("SearchPanel").first();
            var controller = searchPanel.getController();
            controller.updateRecord();
            var specification = Util.copy({}, controller.getModel().getRecord(0).data);
            var paging = $(widget).Paging("Paging").first();
            SetSearchParam(paging.getArrayController(), specification);
            $.DataList("HRPrincipalList").first().setValue(null);
            paging.reset();
        }
        var listControl = $(widget).DataList("HRPrincipalList").first();

        function SetSearchParam(arrController, specification) {
            for (var p in specification) {
                if (p) {
                    var v = specification[p];

                    switch (p) {
                        case 'Name':
                            arrController.setParam("name", v);
                            break;

                        case 'State':
                            arrController.setParam("state", parseInt(v));
                            break;
                    }
                }
            }
        }

        listControl.getItemTemplate().configureChildren({
            "State": {
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
            },
            "btnSync": {
                binding: {
                    mapping: {
                        visible: function (record) {
                            return !record.get("Synchronized");
                        }
                    }
                }
            },
            "PrincipalType": {
                binding: {
                    mapping: {
                        value: function (record) {
                            var type = record.get("PrincipalType");
                            if (type == 0) {
                                return "人员";
                            }
                            else {
                                return "部门";
                            }
                        }
                    }
                }
            }
        });

        listControl.on("itemAdded", function (source, index, item) {
            var record = item.getRecord();
            if (!record.get("Synchronized")) {
                $(item).Button("btnSync").on("clicked", function (source) {
                    var logType = source.parent.getRecord().get("PrincipalType");
                    var dialogName = logType == 0 ? "HREmployeeSyncDialog" : "HROrganizationalSyncDialog";
                    var dialog = $.Dialog(dialogName).first();
                    if (dialog) {
                        dialog.reset();
                    }
                    dialog.open({ "ID": record.get("ID"), "State": record.get("State") });
                    dialog.on('closed', function () {
                        debug.log('dialog closed..');
                        var list = $.Widget("HRPrincipalListWidget").first().find("HRPrincipalList")[0];
                        list.getController().load();

                        dialog.un('closed', arguments.callee);
                    });
                });
            }
        });

        listControl.configure({
            controller: controller
        });

        $(widget).Paging("Paging").configure({
            pageSize: limit,
            arrayController: controller
        });

        //TODO: 不能放到 setController 之前设置 selMode
        listControl.setSelMode(new ItemMode({
            allowDeselect: true,
            mode: "MULTI",
            returnValueType: "ARRAY"
        }));

        function getSelectedIds() {
            var records = $.DataList("HRPrincipalList").getValue();
            var ids = [];
            var i;
            for (i = 0; records && i < records.length; i++) {
                var data = records[i].data;
                ids.push(data.PrincipalType + "," + data.ID);
            }
            return ids;
        }

        $(widget).Button("btnSyncAll").first().on("clicked", function () {
            var ids = getSelectedIds();

            var batch = Batch.beginBatch();
            batch.execute({
                name: "HRBatchSyncCommand",
                properties: {
                    Ids: ids
                }
            })
            .done(function (data) {
                alert('批量同步成功！');
                reload();
            })
            .fail(function (data) {
                alert('批量同步失败！');
            });
            batch.commit();
        });

        function setSelect() {
            var items = $.DataList("HRPrincipalList").getItems();
            var checkState = selectAll.getValue();
            var i;
            for (i = 0; i < items.length; i++) {
                if (checkState) {
                    if (!items[i].selected) {
                        items[i].click();
                    }
                }
                else {
                    if (items[i].selected) {
                        items[i].click();
                    }
                }
            }
        }

        selectAll.on("clicked", function () {
            setSelect();
        });
    };

    return exports;
});