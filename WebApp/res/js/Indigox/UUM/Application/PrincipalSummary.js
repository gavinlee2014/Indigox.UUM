define("Indigox/UUM/Application/PrincipalSummary",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('PrincipalSummary', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'Summary', text: 'Summary', type: String }
        ],
        primaryKey: ['ID']
    });
});