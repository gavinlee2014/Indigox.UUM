define("Indigox/UUM/Application/SyncTaskSummary",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('SyncTaskSummary', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'ErrorMessage', text: '错误信息', type: String }
        ],
        primaryKey: ['ID']
    });
});