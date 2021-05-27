define("Indigox/UUM/Application/SysConfiguration",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('SysConfiguration', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'ClientName', text: '系统名称', type: String },
            { name: 'UserSyncWebService', text: '用户同步WebService', type: String },
            { name: 'Email', text: '邮箱', type: String },
            { name: 'RoleSyncWebService', text: '角色同步WebService', type: String },
            { name: 'OrganizationUnitSyncWebService', text: '组织机构同步WebService', type: String },
            { name: 'GroupSyncWebService', text: '群组同步WebService', type: String },
            { name: 'OrganizationRoleSyncWebService', text: '组织角色同步WebService', type: String },
            { name: 'Sequence', text: '顺序', type: String },
            { name: 'Enabled', text: '是否启用', type: String },
            { name: "Dependencies", text: '依赖系统', type: Array },
            { name: "SyncType", text: '同步类型', type: Array },
            { name: "SyncState", text: '同步状态', type: Array },
            { name: "SyncTaskID", text: '同步任务编号', type: Array }
        ],
        primaryKey: ['ID']
    });

});