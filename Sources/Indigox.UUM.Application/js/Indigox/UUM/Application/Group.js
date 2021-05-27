define("Indigox/UUM/Application/Group",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('Group', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'FullName', text: '全名', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'Description', text: '描述', type: String },
            { name: 'OrderNum', text: '排序值', type: String },
            { name: 'IsEnabled', text: '是否激活', type: String },
            { name: 'IsDeleted', text: '是否删除', type: String },
            { name: 'Members', text: '成员', type: Array },
            { name: 'DisplayName', text: '显示名', type: String }
        ],
        primaryKey: ['ID']
    });
});