define("/UUM/Widgets/Common/TreeUtil", function () {
    var exports = {
        getPath: function (datalist, value, idField, parentidField) {
            var path = "";
            var parent = null;
            var i = null,
            length = null;
            for (i = 0, length = datalist.length; i < length; i++) {
                var data = datalist[i];
                if (data[idField] == value) {
                    parent = data[parentidField];
                    break;
                }
            }
            if (parent != null) {
                path = this.getPath(datalist, parent, idField, parentidField) + "/" + value;
            }
            else {
                path = value;
            }
            return path;
        }
    };

    return exports;
});