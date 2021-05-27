define("Indigox/UUM/Application/SysList",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('SysList', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'SysName', text: '系统名称', type: String }
        ],
        primaryKey: ['ID']
    });

});