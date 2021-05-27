define("/UUM/Widgets/OperationLog/LogSearchSpecificationSchema",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    var firstDate = new Date();
    firstDate.setDate(1);
    var lastDate = new Date(firstDate);
    lastDate.setMonth(firstDate.getMonth() + 1);
    lastDate.setDate(0);
    RecordManager.getInstance().register("LogSearchSpecification", {
        "columns": [{
            "name": "startDate",
            "text": "开始时间",
            "controlType": "datepicker",
            "controlConfig": null,
            "readOnly": false,
            "required": false,
            "display": true
        }, {
            "name": "endDate",
            "text": "结束时间",
            "controlType": "datepicker",
            "controlConfig": null,
            "readOnly": false,
            "required": false,
            "display": true
        }]
    });
});