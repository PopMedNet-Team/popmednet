/// <reference path="../../../../js/requests/details.ts" />
var Controls;
(function (Controls) {
    var WFFileUpload;
    (function (WFFileUpload) {
        var ItemTypes;
        (function (ItemTypes) {
            ItemTypes[ItemTypes["Folder"] = 0] = "Folder";
            ItemTypes[ItemTypes["File"] = 1] = "File";
        })(ItemTypes = WFFileUpload.ItemTypes || (WFFileUpload.ItemTypes = {}));
        var sFtpFileResults = /** @class */ (function () {
            function sFtpFileResults() {
                this.Results = ko.observableArray();
            }
            sFtpFileResults.prototype.RemoveFile = function (data, event) {
            };
            return sFtpFileResults;
        }());
        WFFileUpload.sFtpFileResults = sFtpFileResults;
        var sFtpResult = /** @class */ (function () {
            function sFtpResult(path, status) {
                this.Path = ko.observable(path);
                this.Status = ko.observable(status);
            }
            return sFtpResult;
        }());
        WFFileUpload.sFtpResult = sFtpResult;
        var sFtpItem = /** @class */ (function () {
            function sFtpItem(name, path, type, length) {
                var _this = this;
                this.Name = ko.observable(name);
                this.Path = ko.observable(path);
                this.Type = ko.observable(type);
                this.Length = ko.observable(length);
                this.Selected = ko.observable(false);
                this.Loaded = ko.observable(false);
                this.Items = ko.observableArray();
                this.Files = ko.computed(function () {
                    if (_this.Items == null || _this.Items().length == 0)
                        return [];
                    var arr = ko.utils.arrayFilter(_this.Items(), function (item) {
                        return item.Type() == ItemTypes.File;
                    });
                    return arr;
                });
                this.Folders = ko.computed(function () {
                    if (_this.Items == null || _this.Items().length == 0)
                        return [];
                    var arr = [];
                    _this.Items().forEach(function (item) {
                        if (item.Type() == ItemTypes.Folder)
                            arr.push(item);
                    });
                    return arr;
                });
                this.LengthFormatted = ko.computed(function () {
                    if (_this.Length() == null)
                        return '';
                    return Global.Helpers.formatFileSize(_this.Length());
                });
            }
            return sFtpItem;
        }());
        WFFileUpload.sFtpItem = sFtpItem;
    })(WFFileUpload = Controls.WFFileUpload || (Controls.WFFileUpload = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=Common.js.map