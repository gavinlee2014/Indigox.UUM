define("/UUM/Widgets/Selection/DisplaySelectedPlugin",
    [
        "Indigox.Web.JsLib.Utils.DelayedTask",
        "Indigox/UUM/Application/Principal",
        "Indigox/UUM/Application/OrganizationalUnit"
    ],
function (
        DelayedTask
) {
    var exports = {
        setControl: function (optionDataList, selectedDataList, multi) {
            var delayedTask = new DelayedTask(function () {
                var optionRecords = optionDataList.getSelMode().selected.elements;

                var selectedRecordSet = selectedDataList.getController().getModel();

                if (!multi) {
                    var selectedData = null;
                    if (optionRecords) {
                        for (var i = 0, length = optionRecords.length; i < length; i++) {
                            //TODO: contains record which id is null
                            if (optionRecords[i].data.ID) {
                                selectedData = optionRecords[i].data;
                            }
                        }
                    }

                    if (!selectedData) {
                        // selectedRecordSet.clearRecord();
                    }
                    else {
                        selectedRecordSet.addRecord(selectedData);

                        while (selectedRecordSet.size() > 1) {
                            selectedRecordSet.removeRecord(0);
                        }
                    }
                }
                else {
                    if (optionRecords) {
                        for (var i = 0, length = optionRecords.length; i < length; i++) {
                            //TODO: contains record which id is null
                            if (optionRecords[i].data.ID) {
                                selectedRecordSet.addRecord(optionRecords[i].data);
                            }
                        }
                    }
                }
            });

            optionDataList.on('itemSelectedChanged', function (source, e) {
                //debug.log('itemSelectedChanged: ', optionUsers.getValue());
                delayedTask.delay(100);
            });

            selectedDataList.on("itemRemoved", function (source, index, item) {
                //if (!optionDataList.getSelMode().getAllowDeselect()) {
                //    optionDataList.setValue([]);
                //}
                //else {
                var optionRecord = optionDataList.getController().getModel().createRecord(item.record.data);
                optionDataList.getSelMode().deselect(optionRecord);
                //}
            });
        }
    };

    return exports;
});