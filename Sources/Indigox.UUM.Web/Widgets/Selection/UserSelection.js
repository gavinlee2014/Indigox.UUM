define("/UUM/Widgets/Selection/UserSelection",
    [
        "jquery",
        "Indigox.Web.JsLib.Utils.DelayedTask",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.HierarchyController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.Tip.RowTip",
        "/UUM/Widgets/Selection/DisplaySelectedPlugin",
        "Indigox/UUM/Application/Principal",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
    function (
        jQuery,
        DelayedTask,
        UrlUtil,
        StringUtil,
        AutoBatch,
        ArrayProxy,
        InstructionProxy,
        RecordManager,
        HierarchyController,
        ListController,
        ItemMode,
        RowTip,
        DisplaySelectedPlugin
    ) {
        var exports = function (widget) {
            var args = window.dialogArguments || {};

            var value = args.value || JSON.parse(Page().getUrlParam('value'));
            var mode = args.mode || Page().getUrlParam('mode');
            var multi = args.multi || Page().getUrlParam('multi');

            $(widget).DataList("optionUsers").first().on("itemAdded", function (source, index, item) {
                var tooltip = $(widget).Tooltip("principalToolTip").first();
                var tip = new RowTip();
                tip.setTipControl(tooltip);
                $(item).setTip(tip);
            });

            initializeOptionUsers(widget, mode, multi);

            initializeSearchPanel(widget);

            DisplaySelectedPlugin.setControl(
                $(widget).DataList('optionUsers').first(),
                $(widget).DataList('selectedUsers').first(),
                multi
            );

            bindTree(widget);

            var recordSet = RecordManager.getInstance().createRecordSet('Principal');
            recordSet.loadRecords(value);
            var records = recordSet.getRecords();

            $(widget).DataList('optionUsers').first().setValue(records);
        };

        function initializeOptionUsers(widget, mode, multi) {
            var limit = 50;

            var typeFilters = getTypeFilters(mode);

            var arrayController = new ListController({
                params: {
                    OrganizationalUnitID: 'OR1000000000',
                    IncludeOrganizationalUnit: true,
                    QueryString: "",
                    TypeFilters: typeFilters,
                    FetchSize: limit
                },
                model: RecordManager.getInstance().createRecordSet('Principal', {
                    proxy: new InstructionProxy({
                        query: "PrincipalListQuery"
                    }),
                    addRecords: false
                })
            });

            $(widget).DataList('optionUsers').first().configure({
                controller: arrayController
            });

            //TODO: 不能放到 setController 之前设置 selMode
            $(widget).DataList('optionUsers').first().setSelMode(new ItemMode({
                allowDeselect: false,
                mode: "MULTI",
                returnValueType: "ARRAY"
            }));
        }

        function getTypeFilters(mode) {
            /*
            *  mode:
            *    1 = 选择部门
            *    2 = 选择用户
            *    3 = 选择群组
            *    4 = 选择组织角色
            *    5 = 选择角色
            */
            var orgMode = '1', userMode = '2', groupMode = '3', orgRoleMode = '4', roleMode = '5';

            var typeFilters = [];

            mode = mode || '12345';

            if (mode.indexOf(orgMode) >= 0) {
                typeFilters.push('OrganizationalUnit');
            }
            if (mode.indexOf(userMode) >= 0) {
                typeFilters.push('User');
            }
            if (mode.indexOf(orgRoleMode) >= 0) {
                typeFilters.push('OrganizationalRole');
            }

            return typeFilters;
        }

        function initializeSearchPanel(widget) {
            $(widget).Content('userSearchPanel').Button('btnSearch').first().on('clicked', function () {
                //var queryString = $(widget).Content('userSearchPanel').TextBox('txtKeyword').first().getValue();
                var queryString = jQuery(".txt-user-keyword")[0].value;

                var controller = $(widget).DataList('optionUsers').first().getController();
                controller.setParam('QueryString', queryString);
                controller.setParam('OrganizationalUnitID', '');
                controller.load();
            });
        }

        function bindTree(widget) {
            $(widget).Tree("orgTree").first().configure({
                mode: "SINGLE",
                allowDeselect: false,
                treeNodeType: "linktreenode",
                valueField: "ID",
                expandLevel: 2,
                controller: new HierarchyController({
                    model: RecordManager.getInstance().createRecordSet('OrganizationalUnit', {
                        addRecords: true,
                        proxy: new ArrayProxy()
                    }),
                    rootValue: null,
                    nodeOptions: {
                        binding: {
                            mapping: {
                                value: "ID",
                                text: "Name"
                            }
                        },
                        events: {
                            clicked: function (source, e) {
                                var orgId = source.getRecord().get('ID');
                                resetOptionUsers(widget, orgId);
                            }
                        }
                    }
                })
            });

            var batch = AutoBatch.getCurrentBatch();
            batch.list({
                name: 'OrganizationalUnitListQuery',
                properties: {
                },
                callback: function (data) {
                    var tree = $(widget).Tree("orgTree").first();
                    tree.getController().getModel().getProxy().setArray(data);
                    tree.getController().load();
                }
            });
        }

        function resetOptionUsers(widget, orgId) {
            var controller = $(widget).DataList('optionUsers').first().getController();
            controller.setParam('QueryString', '');
            controller.setParam('OrganizationalUnitID', orgId);
            controller.load();
        }

        return exports;
    });