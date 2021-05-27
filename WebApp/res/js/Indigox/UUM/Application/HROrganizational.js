define("Indigox/UUM/Application/HROrganizational",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('HROrganizational', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'ParentID', text: '部门编号', type: String },
            { name: 'ParentName', text: '部门名称', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'DisplayName', text: '显示名称', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'Type', text: '类型', type: String },
            { name: 'State', text: '状态', type: String },
            { name: 'Description', text: '描述', type: String },
            { name: 'Synchronized', text: '是否已同步', type: String }
        ],
        primaryKey: ['ID']
    });
});