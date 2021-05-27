define("/UUM/Widgets/Common/UserWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.HierarchyController",
        "Indigox.Web.JsLib.Controls.Html.Menu",
        "Indigox.Web.JsLib.Controls.Html.Tree",
        "Indigox.Web.JsLib.Controls.Binding.PropertyBinding",
        "/UUM/Widgets/Common/TreeUtil",
        "Indigox.Web.JsLib.WebContexts.Context",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
    function (
        UrlUtil,
        StringUtil,
        AutoBatch,
        Batch,
        ArrayProxy,
        InstructionProxy,
        RecordManager,
        HierarchyController,
        Menu,
        Tree,
        PropertyBinding,
        TreeUtil,
        Context
    ) {

        function getAccordions(MenuData, parent) {

            var TopMenu = [];

            for (var index = 0; index < MenuData.length; index++) {
                if (MenuData[index].Parent === parent) {
                    var accordionsItem = { text: MenuData[index].Title, id: MenuData[index].ID, value: MenuData[index].ID, href: MenuData[index].Url };
                    TopMenu.push(accordionsItem);
                }
            }

            return TopMenu;
        }

        function bind(accordion, data) {
            var root = 0;
            if (data.length > 0) {
                root = data[0].RootID;
            }
            var accordions = getAccordions(data, root);

            for (var i = 0, length = accordions.length; i < length; i++) {
                var nodes = getAccordions(data, accordions[i].id);

                if (accordions[i].text == "组织结构") {
                    var orgTree = new Tree();
                    bindOrgTree(orgTree);
                    accordions[i].children = [orgTree];
                }
                else if (accordions[i].text == "通讯录") {
                    var addressTree = new Tree();
                    bindAddressTree(addressTree);
                    accordions[i].children = [addressTree];
                }
                else {
                    if (nodes.length == 0) {
                        continue;
                    }
                    var menu = new Menu();
                    menu.configure({
                        orientation: 1,
                        menuItemType: "linkmenuitem",
                        staticDisplayLevels: 2,
                        childNodes: nodes
                    });
                    accordions[i].children = [menu];
                }
            }

            accordion.configure({
                items: accordions
            });

            // expand accordion by location href
            expandAccordion(accordion);

            // 1. load orgTree and addressTree
            // 2. expand orgTree or addressTree by location href
            AutoBatch.getCurrentBatch().list({
                name: 'OrganizationalUnitListQuery',
                properties: {
                }
            })
                .done(function (data) {
                    if (orgTree && orgTree.getController()) {
                        orgTree.getController().getModel().getProxy().setArray(data);
                        orgTree.getController().load();
                    }

                    if (addressTree && addressTree.getController()) {
                        addressTree.getController().getModel().getProxy().setArray(data);
                        addressTree.getController().load();
                    }

                    var organizationalUnitID = Page().getUrlParam("OrganizationalUnitID");
                    if (!StringUtil.isNullOrEmpty(organizationalUnitID)) {
                        var path = TreeUtil.getPath(data, organizationalUnitID, "ID", "Organization");
                        var url = Page().getUrl();
                        if (url.indexOf("/AddressBook/") > -1 && addressTree) {
                            addressTree.expandPath(path);
                        }
                        else if (url.indexOf("/Principal/") > -1 && orgTree) {
                            orgTree.expandPath(path);
                        }
                    }
                });

        }


        function expandAccordion(accordion) {
            var url = Page().getUrl();
            var key = "";
            if (url.indexOf("/AddressBook/") > -1) {
                key = "通讯录";
            }
            else if (url.indexOf("/Principal/") > -1) {
                key = "组织结构";
            }
            else if (url.indexOf("/HR/") > -1) {
                key = "系统同步";
            }
            else {
                key = "工作区";
            }

            var accordionItems = accordion.getItems();
            for (var i = 0, length = accordionItems.length; i < length; i++) {
                var name = accordionItems[i].getText();
                if (name === key) {
                    accordionItems[i].expand();
                }
            }
        }

        function bindOrgTree(orgTree) {
            orgTree.configure({
                mode: "SINGLE",
                allowDeselect: false,
                treeNodeType: "linktreenode",
                valueField: "ID",
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
                                text: function (record) {
                                    return record.get("DisplayName") == null ? record.get("Name") : record.get("DisplayName");
                                },
                                href: function (record) {
                                    var url = "";
                                    var organizationalUnitID = record.get("ID");
                                    url = UrlUtil.join("#/Principal/List.htm", {
                                        "OrganizationalUnitID": organizationalUnitID
                                    });
                                    return url;
                                }
                            }
                        }
                    }
                })
            });
        }

        function bindAddressTree(addressTree) {
            addressTree.configure({
                mode: "SINGLE",
                allowDeselect: false,
                treeNodeType: "linktreenode",
                valueField: "ID",
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
                                text: function (record) {
                                    return record.get("DisplayName") == null ? record.get("Name") : record.get("DisplayName");
                                },
                                href: function (record) {
                                    var url = "";
                                    var organizationalUnitID = record.get("ID");
                                    url = UrlUtil.join("#/AddressBook/List.htm", {
                                        "OrganizationalUnitID": organizationalUnitID
                                    });
                                    return url;
                                }
                            }
                        }
                    }
                })
            });
        }

        var exports = function (widget) {

            var MenuData = Context.getInstance().getNavigation("UUM") || [];
            var accordion = $(widget).Accordion("mainmenu").first();
            bind(accordion, MenuData);
            // expand(accordion);
        };

        return exports;
    });