define("Indigox/UUM/Application/HREmployee",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('HREmployee', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'ParentID', text: '部门编号', type: String },
            { name: 'ParentName', text: '部门名称', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'AccountName', text: '帐号', type: String },
            { name: 'DisplayName', text: '显示名称', type: String },
            { name: 'Tel', text: '电话', type: String },
            { name: 'Mobile', text: '手机', type: String },
            { name: 'IdCard', text: '身份证号', type: String },
            { name: 'Fax', text: '传真', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'Title', text: '头衔', type: String },
            { name: 'Enabled', text: '是否启用', type: String },
            { name: 'State', text: '状态', type: String },
            { name: 'Synchronized', text: '是否已同步', type: String },
            { name: 'Description', text: '描述', type: String },
            { name: 'MailDatabase', text: '邮箱数据库', type: String },
            { name: 'HasPolyphone', text: '含义多音字', type: String },
            { name: 'QuitDate', text: '离职日期', type: String }
        ],
        primaryKey: ['ID']
    });
});