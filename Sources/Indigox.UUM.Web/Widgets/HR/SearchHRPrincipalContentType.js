define("/UUM/Widgets/HR/SearchHRPrincipalContentType", function () {

    var SearchHRPrincipalContentType = {
        controls: [{
            controlType: "fieldset",
            rowFields: 1,
            items: [
                {
                    controlType: "fieldcontainer",
                    text: "名称",
                    children: [{
                        controlType: "textbox",
                        name: "Name"
                    }]
                }
            ]},{
            controlType: "fieldset",
            rowFields: 2,
            items: [
            {
                controlType: "fieldcontainer",
                text: "类型",
                children: [{
                    controlType: "dropdownlist",
                    name: "Type",
                    items: [
                        { text: "请选择", value: "-1", selected: true },
                        { text: "人员", value: "0" },
                        { text: "部门", value: "1" }
                    ]
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
                        { text: "新建", value: "0" },
                        { text: "修改", value: "1" },
                        { text: "删除", value: "2" },
                        { text: "启用", value: "3" },
                        { text: "禁用", value: "4" }
                    ]
                }]
            }
            ]
        }]
    };

    return SearchHRPrincipalContentType;
});