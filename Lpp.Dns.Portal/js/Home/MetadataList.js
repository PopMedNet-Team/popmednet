var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Home;
(function (Home) {
    var MetadataList;
    (function (MetadataList) {
        MetadataList.RawModel = null;
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                _this.metadata = ko.observableArray([{ DataMart: 'Sample DM 1', Name: 'Sample Metadata Name', Type: 'Sample Type' }]);
                return _this;
            }
            return ViewModel;
        }(Global.PageViewModel));
        MetadataList.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(MetadataList = Home.MetadataList || (Home.MetadataList = {}));
})(Home || (Home = {}));
