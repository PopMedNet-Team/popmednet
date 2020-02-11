var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../../js/_rootlayout.ts" />
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var SaveCriteriaGroup;
            (function (SaveCriteriaGroup) {
                var vm;
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl) {
                        var _this = _super.call(this, bindingControl) || this;
                        _this.options = _this.Parameters;
                        _this.Template = new Dns.ViewModels.TemplateViewModel();
                        _this.Template.Data(JSON.stringify(_this.options.CriteriaGroup));
                        _this.Template.CreatedByID(User.ID);
                        _this.Template.CreatedOn(moment.utc().toDate());
                        _this.Template.Type(Dns.Enums.TemplateTypes.CriteriaGroup);
                        _this.Template.QueryType(_this.options.AdapterDetail);
                        return _this;
                    }
                    ViewModel.prototype.Save = function () {
                        var _this = this;
                        if (!this.Validate())
                            return;
                        var updateDetails = {
                            Name: this.Template.Name(),
                            Description: this.Template.Description(),
                            Json: JSON.stringify(this.options.CriteriaGroup),
                            AdapterDetail: this.options.AdapterDetail,
                            TemplateID: this.options.TemplateID,
                            RequestTypeID: this.options.RequestTypeID,
                            RequestID: this.options.RequestID
                        };
                        Dns.WebApi.Templates.SaveCriteriaGroup(updateDetails).done(function (results) {
                            _this.Close();
                        });
                    };
                    return ViewModel;
                }(Global.DialogViewModel));
                SaveCriteriaGroup.ViewModel = ViewModel;
                function init() {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                }
                init();
            })(SaveCriteriaGroup = QueryBuilder.SaveCriteriaGroup || (QueryBuilder.SaveCriteriaGroup = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
