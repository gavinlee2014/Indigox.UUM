define("/UUM/Widgets/OperationLog/LogSearchSpecification", function () {

    var LogSearchSpecification = {
        controls: [
        {
            controlType: "fieldset",
            items: [{
                controlType: "fieldcontainer",
                text: "搜索",
                children: [{
                    controlType: "textbox",
                    name: "SearchContent"
                }]
            },
        {
            controlType: "fieldcontainer",
            text: "时间段",
            children: [{
                controlType: "datepicker",
                editHMS: false,
                name: "startDate",
                width: "30%"
            }, {
                controlType: "label",
                value: "至",
                width: "20%"
            }, {
                controlType: "datepicker",
                editHMS: false,
                name: "endDate",
                width: "40%"
            }]
        }
           ]
        }]
    };

    return LogSearchSpecification;
});