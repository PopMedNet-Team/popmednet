/// <reference path="../../Scripts/page/Page.ts" />
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
var Dialog;
(function (Dialog) {
    var CodeSelector;
    (function (CodeSelector) {
        var vm;
        var CodeSelectorViewModel = /** @class */ (function (_super) {
            __extends(CodeSelectorViewModel, _super);
            function CodeSelectorViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.queryTimer = -1;
                var self = _this;
                _this.dsSelected = new kendo.data.DataSource({
                    data: [],
                    sort: {
                        field: 'ItemCode',
                        dir: 'asc'
                    }
                });
                _this.Changed = ko.observable(false);
                _this.dsCategories = new kendo.data.DataSource({
                    data: []
                });
                var existingCodes = [];
                if (_this.Parameters.Codes.length != 0) {
                    for (var v in _this.Parameters.Codes) {
                        var code = _this.Parameters.Codes[v] || '';
                        if (code.length == 0)
                            continue;
                        if (typeof code === 'object') {
                            code.Name.replace('&#44;', ',');
                            code.Code.replace('&#44;', ',');
                            //existingCodes.push(code.replace('&#44;', ','));
                            existingCodes.push(code);
                        }
                        else {
                            existingCodes.push(code.replace('&#44;', ','));
                            existingCodes.push(code);
                        }
                    }
                }
                if (existingCodes.length > 0) {
                    if (typeof _this.Parameters.Codes[0] === 'object') {
                        var strCodes = _this.Parameters.Codes.map(function (item) {
                            var codeMap = {
                                ListId: _this.Parameters.ListID,
                                CategoryId: null,
                                ItemName: item.Name,
                                ItemCode: item.Code,
                                ItemCodeWithNoPeriod: null,
                                ExpireDate: item.ExpireDate,
                                ID: null,
                            };
                            return codeMap;
                        });
                        self.dsSelected.data(strCodes);
                    }
                    else {
                        Dns.WebApi.LookupListValue.GetCodeDetailsByCode({
                            ListID: _this.Parameters.ListID,
                            Codes: existingCodes
                        }).done(function (results) {
                            self.dsSelected.data(results);
                        });
                    }
                }
                Dns.WebApi.LookupListCategory.GetList(self.Parameters.ListID).done(function (categories) {
                    if (categories == null || categories.length == 0)
                        return;
                    categories.unshift({ ListId: -1, CategoryId: -1, CategoryName: "" });
                    categories[0].CategoryName = "";
                    _this.dsCategories.data(categories);
                });
                _this.dsResults = new kendo.data.DataSource({
                    data: [],
                    sort: {
                        field: 'ItemCode',
                        dir: 'asc'
                    }
                });
                _this.ShowCategoryDropdown = ko.observable(_this.Parameters.ShowCategoryDropdown == null ? true : _this.Parameters.ShowCategoryDropdown);
                _this.Query = ko.observable("");
                _this.Query.subscribe(function (value) {
                    if (!value) {
                        vm.dsResults.data([]);
                        return;
                    }
                    else if (value.length < 2) {
                        return;
                    }
                    _this.CategoriesValue(-1);
                    if (_this.queryTimer > -1)
                        clearTimeout(_this.queryTimer);
                    //Set a timer that gets cancelled so that we can time it out.
                    _this.queryTimer = setTimeout(function () {
                        var grid = $("#gResults").data("kendoGrid");
                        var lookup = _this.Query();
                        if (!lookup) {
                            vm.dsResults.data([]);
                            grid.refresh();
                            return;
                        }
                        Dns.WebApi.LookupListValue.LookupList(self.Parameters.ListID, lookup).done(function (data) {
                            data.forEach(function (item) {
                                item["Selected"] = false;
                            });
                            _this.dsResults.data(data);
                            grid.refresh();
                        });
                    }, 250);
                });
                _this.CategoriesValue = ko.observable(null);
                _this.CategoriesValue.subscribe(function (categoryValue) {
                    var grid = $("#gResults").data("kendoGrid");
                    vm.dsResults.data([]);
                    if (!categoryValue) {
                        grid.refresh();
                        return;
                    }
                    Dns.WebApi.LookupListValue.GetList("CategoryId eq " + categoryValue + " and ListId eq Lpp.Dns.DTO.Enums.Lists'" + self.Parameters.ListID + "'").done(function (data) {
                        data.forEach(function (item) {
                            item["Selected"] = false;
                        });
                        _this.dsResults.data(data);
                        grid.refresh();
                    });
                    ;
                });
                return _this;
            }
            CodeSelectorViewModel.prototype.Save = function (data, event) {
                //var codes = this.dsSelected.data().map((item: any) => {
                //    return item.ItemCode.trim();
                //});
                //this.Close(codes);
                var codes = this.dsSelected.data().map(function (item) {
                    //var val: Dns.ViewModels.CodeSelectorValueViewModel;
                    //val = new Dns.ViewModels.CodeSelectorValueViewModel();
                    //val.Code(item.ItemCode.trim());
                    //val.Name(item.ItemName.trim());
                    //val.ExpireDate = item.ExpireDate;
                    var val = { Code: item.ItemCode.trim(), Name: item.ItemName.trim(), ExpireDate: null };
                    return val;
                });
                this.Close(codes);
            };
            CodeSelectorViewModel.prototype.Cancel = function (data, event) {
                this.Close();
            };
            CodeSelectorViewModel.prototype.AddCode = function (arg) {
                $.each(arg.sender.select(), function (count, item) {
                    var dataItem = arg.sender.dataItem(item);
                    vm.dsSelected.data().push(dataItem);
                    $.each(vm.dsResults.data(), function (count, data) {
                        if (data == null)
                            return;
                        if (data.ID == dataItem.ID) {
                            vm.dsResults.data().splice(count, 1);
                            return;
                        }
                    });
                });
                vm.Changed(true);
            };
            CodeSelectorViewModel.prototype.RemoveCode = function (arg) {
                $.each(arg.sender.select(), function (count, item) {
                    var dataItem = arg.sender.dataItem(item);
                    vm.dsResults.data().push(dataItem);
                    $.each(vm.dsSelected.data(), function (count, data) {
                        if (data == null)
                            return;
                        if (data.ID == dataItem.ID) {
                            vm.dsSelected.data().splice(count, 1);
                            return;
                        }
                    });
                });
                vm.Changed(true);
            };
            return CodeSelectorViewModel;
        }(Global.DialogViewModel));
        CodeSelector.CodeSelectorViewModel = CodeSelectorViewModel;
        function init() {
            //In this case we do all of the data stuff in the view model because it has the parameters.
            $(function () {
                var bindingControl = $("body");
                vm = new CodeSelectorViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(CodeSelector = Dialog.CodeSelector || (Dialog.CodeSelector = {}));
})(Dialog || (Dialog = {}));
