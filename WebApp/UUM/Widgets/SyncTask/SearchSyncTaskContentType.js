define("/UUM/Widgets/SyncTask/SearchSyncTaskContentType", function () {

    var SearchSyncTaskContentType = {
        controls: [{
            controlType: "fieldset",
            rowFields: 1,
            items: [
                {
                controlType: "fieldcontainer",
                text: "描述",
                children: [{
                    controlType: "textbox",
                    name: "Description"
                }]
            }
            ]
        }, {
            controlType: "fieldset",
            rowFields: 2,
            items: [
            {
                controlType: "fieldcontainer",
                text: "所属系统",
                children: [{
                    controlType: "textbox",
                    name: "Tag"
                }]
            },
            {
                controlType: "fieldcontainer",
                text: "状态",
                children: [{
                    controlType: "dropdownlist",
                    name: "State",
                    items: [
                        { text: "请选择", value: "-1", selected: true },
                        { text: "未执行", value: "0"},
                        { text: "成功", value: "1" },
                        { text: "失败", value: "2" }
                    ]
                }]

            }
            ]
        }, {
            controlType: "fieldset",
            items: [{
                controlType: "fieldcontainer",
                text: "创建时间",
                children: [{
                    controlType: "datepicker",
                    editHMS: false,
                    name: "CreateTimeBegin",
                    width: "40%"
                }, {
                    controlType: "label",
                    value: "至",
                    width: "5%"
                }, {
                    controlType: "datepicker",
                    editHMS: false,
                    name: "CreateTimeEnd",
                    width: "40%"
                }]
            }]
        }]
    };

    return SearchSyncTaskContentType;
});