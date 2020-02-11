/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="termvaluefilter.ts" />
/// <reference path="../../../js/requests/details.ts" />
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
//ko.components.register('MDQ', {
//    viewModel: { require: Plugins.Requests.QueryBuilder.MDQ.vm },
//        template: { require: '<span>...</span>' }
//});
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var Edit;
            (function (Edit) {
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl, rawRequestData, projectID, requestID) {
                        var _this = _super.call(this, bindingControl) || this;
                        _this.MDQ = ko.observable();
                        _this.UploadViewModel = null;
                        var self = _this;
                        _this.IsTemplateEdit = ko.observable(false);
                        _this.fileUpload = ko.observable(false);
                        _this.MDQ = ko.observable(false);
                        _this.rawRequestData = rawRequestData;
                        _this.projectID = projectID;
                        return _this;
                    }
                    ViewModel.prototype.UpdateRoutings = function (updates) {
                        Plugins.Requests.QueryBuilder.DataMartRouting.vm.UpdateRoutings(updates);
                    };
                    ViewModel.prototype.fileUploadDMLoad = function () {
                        Plugins.Requests.QueryBuilder.DataMartRouting.vm.LoadDataMarts(this.projectID, JSON.stringify((new Dns.ViewModels.QueryComposerRequestViewModel(this.rawRequestData)).toData()));
                    };
                    return ViewModel;
                }(Global.PageViewModel));
                Edit.ViewModel = ViewModel;
                function init(rawRequestData, fieldOptions, defaultPriority, defaultDueDate, additionalInstructions, existingRequestDataMarts, requestTypeID, visualTerms, isTemplateEdit, projectID, templateID, requestID) {
                    if (isTemplateEdit === void 0) { isTemplateEdit = false; }
                    if (projectID === void 0) { projectID = Global.GetQueryParam("projectID"); }
                    var promise = $.Deferred();
                    $.when(requestTypeID == null ? null : Dns.WebApi.Templates.GetByRequestType(requestTypeID)).done(function (requestTypeTemplates) {
                        var bindingControl = $('#ComposerControl');
                        Edit.vm = new ViewModel(bindingControl, rawRequestData, projectID, requestID);
                        ko.applyBindings(Edit.vm, bindingControl[0]);
                        Plugins.Requests.QueryBuilder.DataMartRouting.init($('#DataMartsControl'), fieldOptions, existingRequestDataMarts, defaultDueDate, defaultPriority, additionalInstructions);
                        if (isTemplateEdit) {
                            Edit.vm.MDQ(true);
                            Edit.vm.fileUpload(false);
                            Plugins.Requests.QueryBuilder.MDQ.init(rawRequestData, fieldOptions, defaultPriority, defaultDueDate, additionalInstructions, existingRequestDataMarts, requestTypeID, visualTerms, isTemplateEdit, projectID, templateID).done(function (viewModel) {
                                promise.resolve(viewModel);
                            });
                        }
                        else {
                            if (requestTypeTemplates != null && requestTypeTemplates.length > 0) {
                                if (requestTypeTemplates[0].ComposerInterface == Dns.Enums.QueryComposerInterface.FileDistribution) {
                                    Edit.vm.MDQ(false);
                                    Edit.vm.fileUpload(true);
                                    var fileUploadID = "2F60504D-9B2F-4DB1-A961-6390117D3CAC";
                                    Edit.vm.UploadViewModel = Controls.WFFileUpload.Index.init($('#FileUploadControl'), rawRequestData, fileUploadID);
                                    //vm.UploadViewModel = Controls.WFFileUpload.ForTask.init($("#FileUploadControl"), tasks);
                                    Edit.vm.fileUploadDMLoad();
                                    promise.resolve();
                                }
                                else {
                                    Edit.vm.MDQ(true);
                                    Plugins.Requests.QueryBuilder.MDQ.init(rawRequestData, fieldOptions, defaultPriority, defaultDueDate, additionalInstructions, existingRequestDataMarts, requestTypeID, visualTerms, isTemplateEdit, projectID, templateID).done(function (viewModel) {
                                        promise.resolve(viewModel);
                                    });
                                }
                            }
                        }
                        Edit.vm.IsTemplateEdit(isTemplateEdit);
                    });
                    return promise;
                }
                Edit.init = init;
            })(Edit = QueryBuilder.Edit || (QueryBuilder.Edit = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
