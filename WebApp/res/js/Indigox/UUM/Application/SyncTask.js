define("Indigox/UUM/Application/SyncTask",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('SyncTask', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Description', text: '描述', type: String },
            { name: 'ErrorMessage', text: '错误信息', type: String },
            { name: 'State', text: '状态', type: String },
            { name: 'CreateTime', text: '创建时间', type: String },
            { name: 'ExecuteTime', text: '执行时间', type: String },
            { name: 'Tag', text: '所属系统', type: String }
        ],
        primaryKey: ['ID']
    });

});