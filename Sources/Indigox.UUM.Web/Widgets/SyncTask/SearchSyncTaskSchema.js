define("/UUM/Widgets/SyncTask/SearchSyncTaskSchema",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register("SearchSyncTask", {
        "columns": [{
            "name": "Tag",
            "text": "所属系统"
        }, {
            "name": "Description",
            "text": "描述"
        }, {
            "name": "State",
            "text": "状态"
        }, {
            "name": "CreateTimeBegin",
            "text": "创建时间Begin"
        }, {
            "name": "CreateTimeEnd",
            "text": "创建时间End"
        }]
    });
});