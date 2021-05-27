define("/UUM/Widgets/Selection/TabContainer",
    [
        "Indigox.Web.JsLib.Utils.DelayedTask"
    ],
function (
        DelayedTask
) {
    var exports = function (widget) {
        $(widget).Button("tabButton1").first().on("clicked", function () {
            //debug.log('tabButton1 actived.');
            active('tab1');
        });

        $(widget).Button("tabButton2").first().on("clicked", function () {
            //debug.log('tabButton2 actived.');
            active('tab2');
        });

        $(widget).Button("tabButton3").first().on("clicked", function () {
            //debug.log('tabButton3 actived.');
            active('tab3');
        });

        var args = window.dialogArguments || {};

        var value = args.value || JSON.parse(Page().getUrlParam('value'));
        var mode = args.mode || Page().getUrlParam('mode');
        var multi = args.multi || Page().getUrlParam('multi');

        widget.on("loaded", function () {
            setTabContainer(widget, mode);
        });
    }

    function setTabContainer(widget, mode) {
        /*
        *  mode:
        *    1 = 选择部门
        *    2 = 选择用户
        *    3 = 选择群组
        *    4 = 选择组织角色
        *    5 = 选择角色
        */
        var orgMode = '1', userMode = '2', groupMode = '3', orgRoleMode = '4', roleMode = '5';

        var activeTab = null;

        mode = mode || '12345';

        if (mode.indexOf(orgMode) >= 0 || mode.indexOf(userMode) >= 0 || mode.indexOf(orgRoleMode) >= 0) {
            $(widget).Button('tabButton1').setVisible(true);
            activeTab = activeTab || 'tab1';
        }
        if (mode.indexOf(roleMode) >= 0) {
            $(widget).Button('tabButton2').setVisible(true);
            activeTab = activeTab || 'tab2';
        }
        if (mode.indexOf(groupMode) >= 0) {
            $(widget).Button('tabButton3').setVisible(true);
            activeTab = activeTab || 'tab3';
        }

        active(activeTab);
    }

    var lastTabName = null;

    function active(tabName) {
        if (tabName && tabName != lastTabName) {
            if (lastTabName) {
                $(lastTabName).first().setVisible(false);
            }
            $(tabName).first().setVisible(true);
            lastTabName = tabName;
        }
    }

    return exports;
});