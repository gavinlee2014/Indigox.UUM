define("Indigox/UUM/Application/HRPrincipal",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('HRPrincipal', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'ParentID', text: 'HR父部门', type: String },
            { name: 'State', text: '状态', type: String },
            { name: 'Synchronized', text: '是否已同步', type: String },
            { name: 'ModifyTime', text: '修改时间', type: String },
            { name: 'PrincipalType', text: '对象类型', type: String }
        ],
        primaryKey: ['ID']
    });
});