define("Indigox/UUM/Application/AddressBook",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('AddressBook', {
        columns: [
            { name: 'UserID', text: '编号', type: String },
            { name: 'UserName', text: '名称', type: String },
            { name: 'FullName', text: '全名', type: String },
            { name: 'DisplayName', text: '显示名', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'IsEnabled', text: '是否激活', type: String },
            { name: 'IsDeleted', text: '是否删除', type: String },
            { name: 'Organization', text: '所属部门ID', type: String },
            { name: 'OrganizationName', text: '所属部门', type: String },
            { name: 'AccountName', text: '帐号', type: String },
            { name: 'Title', text: '职位', type: String },
            { name: 'Mobile', text: '手机号码', type: String },
            { name: 'Telephone', text: '办公电话', type: String },
            { name: 'Fax', text: '传真', type: String },
            { name: 'Profile', text: '头像', type: String },
            { name: 'OtherContact', text: '其他', type: String }
        ],
        primaryKey: ['UserID']
    });
});