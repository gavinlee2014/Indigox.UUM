define("Indigox/UUM/Application/OperationLog",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('OperationLog', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Operator', text: '用户', type: String },
            { name: 'Operation', text: '操作描述', type: String },
            { name: 'OperationTime', text: '操作时间', type: String },
            { name: 'DetailInformation', text: '详细信息', type: String }
        ],
        primaryKey: ['ID']
    });
});