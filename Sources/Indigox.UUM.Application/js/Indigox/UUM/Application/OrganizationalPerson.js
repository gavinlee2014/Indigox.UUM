define("Indigox/UUM/Application/OrganizationalPerson",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('OrganizationalPerson', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Name', text: '名称', type: String },
            { name: 'Type', text: '类型', type: String },
            { name: 'FullName', text: '全名', type: String },
            { name: 'Email', text: '邮件', type: String },
            { name: 'OrderNum', text: '排序值', type: String },
            { name: 'IsEnabled', text: '是否激活', type: String },
            { name: 'IsDeleted', text: '是否删除', type: String },
            { name: 'Organization', text: '所属部门ID', type: String },
            { name: 'OrganizationName', text: '所属部门', type: String },
            { name: 'OrganizationCode', text: '部门编号', type: String },            
            { name: 'AccountName', text: '帐号', type: String },
            { name: 'Title', text: '职称', type: String },
            { name: 'Level', text: '职级', type: String },
            { name: 'Mobile', text: '手机号码', type: String },
            { name: 'IdCard', text: '身份证号', type: String },
            { name: 'Telephone', text: '办公电话', type: String },
            { name: 'Fax', text: '传真', type: String },
            { name: 'OtherContact', text: '其他联系方式', type: String },
            { name: 'MemberOfOrganizationalRoles', text: '所属组织角色', type: Array },
            { name: 'MemberOfGroups', text: '所属群组', type: Array },
            { name: 'DisplayName', text: '显示名', type: String },
            { name: 'Profile', text: '头像', type: String },
            { name: 'MailDatabase', text: '邮箱数据库', type: String }
        ],
        primaryKey: ['ID']
    });
});