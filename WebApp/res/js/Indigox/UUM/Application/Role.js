define("Indigox/UUM/Application/Role",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('Role', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'FullName', text: '全名', type: String },
            { name: 'DisplayName', text: '显示名称', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'Description', text: '描述', type: String },
            { name: 'OrderNum', text: '排序值', type: String },
            { name: 'IsEnabled', text: '是否激活', type: String },
            { name: 'IsDeleted', text: '是否删除', type: String },
            { name: 'Level', text: '绑定级别', type: String },
            { name: 'Members', text: '成员', type: Array }
        ],
        primaryKey: ['ID']
    });
});