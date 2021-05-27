define("Indigox/UUM/Application/Principal",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('Principal', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'FullName', text: '全名', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'IsEnabled', text: '是否激活', type: String },
            { name: 'IsDeleted', text: '是否删除', type: String },
            { name: 'Type', text: '类型', type: String },
            { name: 'Organization', text: '所属部门', type: String }
        ],
        primaryKey: ['ID']
    });

    RecordManager.getInstance().register('SelectedPrincipal', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'FullName', text: '全名', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'IsEnabled', text: '是否激活', type: String },
            { name: 'IsDeleted', text: '是否删除', type: String },
            { name: 'Type', text: '类型', type: String },
            { name: 'Organization', text: '所属部门', type: String }
        ],
        primaryKey: ['ID']
    });
});