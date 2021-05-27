define("Indigox/UUM/Application/NameStrategyDescriptor",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('NameStrategyDescriptor', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Priority', text: '优先级', type: String },
            { name: 'Enabled', text: '是否启用', type: String },
            { name: 'Assembly', text: '程序集', type: String },
            { name: 'ClassName', text: '类名', type: String },
            { name: 'Description', text: '描述', type: String },
            { name: 'LastModifyTime', text: '最后修改时间', type: String }
        ],
        primaryKey: ['ID']
    });

});