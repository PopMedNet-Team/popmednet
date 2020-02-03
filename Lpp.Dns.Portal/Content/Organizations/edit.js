/// <reference path="../../Scripts/Common.ts" />
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
var Organizations;
(function (Organizations) {
    var Edit;
    (function (Edit) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(organizationID, details, ehrs) {
                var _this = _super.call(this, null) || this;
                _this.OrganizationID = organizationID;
                _this.Registries = ko.observableArray();
                details.map(function (item) {
                    _this.Registries.push(new RegistryViewModel(item));
                });
                _this.Registries.subscribe(_this.raiseChange);
                _this.EHRs = ko.observableArray();
                ehrs.map(function (item) {
                    _this.EHRs.push(new EHRViewModel(item));
                });
                _this.EHRs.subscribe(_this.raiseChange);
                return _this;
            }
            ViewModel.prototype.AddRegistry = function (data, event) {
                //$(".RegistriesForSelection").dialog({
                //    title: "Choose Registry or Research Data Set To Add", width: 600, modal: true,
                //    buttons: { Cancel: function () { $(".RegistriesForSelection").dialog("close"); } }
                //});
            };
            ViewModel.prototype.AddEHR = function (data, event) {
                this.EHRs.push(new EHRViewModel({
                    Id: 0,
                    EndYear: null,
                    OrganizationID: this.OrganizationID,
                    Other: null,
                    StartYear: null,
                    System: 0,
                    Type: 1
                }));
            };
            ViewModel.prototype.RemoveRegistry = function (data, event) {
                this.Registries.remove(function (item) {
                    return item.Selected();
                });
            };
            ViewModel.prototype.NewRegistry = function (data, event) {
                $("#RegistryCreateLink").click();
            };
            ViewModel.prototype.AddNewRegistryFromDialog = function (id, name, type) {
                this.Registries.push(new RegistryViewModel({
                    Description: null,
                    Name: name,
                    RegistryID: id,
                    Type: type
                }));
            };
            ViewModel.prototype.save = function () {
                var registries = ko.mapping.toJSON(Edit.vm.Registries);
                var ehrs = ko.mapping.toJSON(Edit.vm.EHRs);
                $("#hRegistries").val(registries).trigger("change");
                $("#hEHRs").val(ehrs).trigger("change");
                return true;
            };
            ViewModel.prototype.EHRRemove = function (data, event) {
                Edit.vm.EHRs.remove(data);
                this.raiseChange();
            };
            return ViewModel;
        }(Dns.PageViewModel));
        Edit.ViewModel = ViewModel;
        var RegistryViewModel = (function () {
            function RegistryViewModel(registry) {
                this.RegistryID = ko.observable(registry.RegistryID);
                this.Name = ko.observable(registry.Name);
                this.Type = ko.observable(registry.Type);
                this.Description = ko.observable(registry.Description);
                this.Selected = ko.observable(false);
                this.Description.subscribe(function (value) {
                    if (Edit.vm)
                        Edit.vm.raiseChange();
                });
            }
            return RegistryViewModel;
        }());
        Edit.RegistryViewModel = RegistryViewModel;
        var EHRViewModel = (function () {
            function EHRViewModel(ehr) {
                this.Id = ko.observable(ehr.Id);
                this.OrganizationID = ko.observable(ehr.OrganizationID);
                this.Type = ko.observable(ehr.Type);
                this.System = ko.observable(ehr.System);
                this.Other = ko.observable(ehr.Other);
                this.StartYear = ko.observable(ehr.StartYear);
                this.EndYear = ko.observable(ehr.EndYear);
                this.Type.subscribe(this.change);
                this.System.subscribe(this.change);
                this.Other.subscribe(this.change);
                this.StartYear.subscribe(this.change);
                this.EndYear.subscribe(this.change);
            }
            EHRViewModel.prototype.change = function (value) {
                if (Edit.vm)
                    Edit.vm.raiseChange();
            };
            return EHRViewModel;
        }());
        Edit.EHRViewModel = EHRViewModel;
        function init(organizationID, details, ehrs, bindingControl) {
            Edit.vm = new ViewModel(organizationID, details, ehrs);
            ko.applyBindings(Edit.vm, bindingControl[0]);
        }
        Edit.init = init;
    })(Edit = Organizations.Edit || (Organizations.Edit = {}));
})(Organizations || (Organizations = {}));
//# sourceMappingURL=edit.js.map