var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var SPAN;
(function (SPAN) {
    var Create;
    (function (Create) {
        Create.RawModel = null;
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(rawModel, bindingControl, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.ParentContext = ko.observable(true);
                _this.IndexVariable = ko.observable(rawModel.IndexVariable);
                _this.Codes = ko.observable("");
                _this.AgeOperator = ko.observable(">");
                _this.Age = ko.observable("0");
                _this.AsOfDate = ko.observable("");
                _this.StartDate = ko.observable("");
                _this.EndDate = ko.observable("");
                _this.BMI = ko.observable("");
                _this.Option = ko.observable("opt1");
                return _this;
            }
            ViewModel.prototype.SelectCode = function (listID) {
                var _this = this;
                var codes = this.Codes().split(", ");
                Global.Helpers.ShowDialog(this.IndexVariable(), "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                    ListID: listID,
                    Codes: codes
                }).done(function (results) {
                    if (!results)
                        return;
                    _this.Codes(results.map(function (i) { return i.Code; }).join(", "));
                    $("form").formChanged(true);
                });
            };
            ViewModel.prototype.save = function () {
                return this.store("");
            };
            ViewModel.IndexVariableList = [
                new Dns.KeyValuePairData('dx', 'Diagnosis Code (Dx)'),
                new Dns.KeyValuePairData('px', 'Procedure Code (Px)'),
                new Dns.KeyValuePairData('rx', 'Drug Code (Rx)'),
                new Dns.KeyValuePairData('age', 'Age'),
                new Dns.KeyValuePairData('bmi', 'BMI')
            ];
            ViewModel.BMIList = [
                new Dns.KeyValuePairData('>= 25', '>= 25'),
                new Dns.KeyValuePairData('>= 30', '>= 25'),
                new Dns.KeyValuePairData('>= 35', '>= 25'),
                new Dns.KeyValuePairData('>= 40', '>= 25'),
                new Dns.KeyValuePairData('>= 45', '>= 25'),
                new Dns.KeyValuePairData('>= 50', '>= 25')
            ];
            ViewModel.ParentContextList = [
                new Dns.KeyValuePairData('And', 'All'),
                new Dns.KeyValuePairData('Or', 'Any')
            ];
            ViewModel.AgeOperatorList = [
                new Dns.KeyValuePairData('>', '>'),
                new Dns.KeyValuePairData('>=', '>='),
                new Dns.KeyValuePairData('=', '='),
                new Dns.KeyValuePairData('<', '<'),
                new Dns.KeyValuePairData('<=', '<=')
            ];
            return ViewModel;
        }(Dns.PageViewModel));
        Create.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#fsCriteria");
                var hiddenDataControl = $("#hiddenDataControl");
                vm = new SPAN.Create.ViewModel(Create.RawModel, bindingControl, hiddenDataControl);
                ko.applyBindings(vm, bindingControl[0]);
                bindingControl.fadeIn(100);
            });
        }
        init();
    })(Create = SPAN.Create || (SPAN.Create = {}));
})(SPAN || (SPAN = {}));
