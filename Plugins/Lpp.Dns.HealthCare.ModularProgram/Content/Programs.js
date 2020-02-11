/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
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
var ModularProgram;
(function (ModularProgram) {
    var Create;
    (function (Create) {
        var vm;
        var ProgramTypes;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(data, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.Programs = ko.observableArray();
                data.forEach(function (program) {
                    var p = new Program(program, true);
                    var ProgramTypeName = _this.GetProgramTypeName(program.type);
                    if (ProgramTypeName) {
                        p.programTypeName(ProgramTypeName);
                        _this.Programs.push(p);
                    }
                });
                //Now go through and back fill any new ones that have just been added.
                ProgramTypes.forEach(function (programType) {
                    if (!_this.ProgramExists(programType)) {
                        var p = new Program({
                            type: programType.Code,
                            name: programType.Name,
                            description: null,
                            scenarios: null
                        }, false);
                        var ProgramTypeName = _this.GetProgramTypeName(programType.Code);
                        if (ProgramTypeName) {
                            p.programTypeName(ProgramTypeName);
                            _this.Programs.push(p);
                        }
                    }
                });
                _this.SelectAll = ko.observable(false);
                return _this;
                //this.SelectAll.subscribe((value) => {
                //    this.Programs().forEach((program) => {
                //        program.Selected(value);
                //    });
                //});
            }
            ViewModel.prototype.save = function () {
                if (!this.isValid())
                    return;
                var data = [];
                //this.Programs().forEach((program) => {
                //    if (!program.Selected())
                //        return;
                //    data.push(program.toData());
                //});
                return _super.prototype.store.call(this, data);
            };
            ViewModel.prototype.ProgramExists = function (programType) {
                var result = false;
                this.Programs().forEach(function (program) {
                    if (program.type() == programType.Code) {
                        result = true;
                        return;
                    }
                });
                return result;
            };
            ViewModel.prototype.GetProgramTypeName = function (code) {
                var name = null;
                ProgramTypes.forEach(function (type) {
                    if (type.Code == code) {
                        name = type.Name;
                        return;
                    }
                });
                return name;
            };
            return ViewModel;
        }(Dns.PageViewModel));
        Create.ViewModel = ViewModel;
        function init(data, programTypes, bindingControl, hiddenDataControl) {
            //ProgramTypes = programTypes;
            //vm = new ViewModel(data, hiddenDataControl);
            //ko.applyBindings(vm, bindingControl[0]);
            //bindingControl.fadeIn(100);
            Dns.EnableValidation();
        }
        Create.init = init;
        var Program = /** @class */ (function (_super) {
            __extends(Program, _super);
            function Program(program, selected) {
                var _this = _super.call(this) || this;
                _this.name = ko.observable(program == null ? null : program.name); //Validation pulled from input control's HTML 5.
                _this.type = ko.observable(program == null ? null : program.type);
                _this.description = ko.observable(program == null ? null : program.description); //Validation pulled from input control's HTML 5.
                _this.scenarios = ko.observable(program == null ? null : program.scenarios); //Validation pulled from input control's HTML 5. Example of inline would be: .extend({ maxLength: 255 })
                //this.Selected = ko.observable(selected);
                //this.Selected.subscribe((value) => {
                //    if (!value) {
                //        this.description(null);
                //        this.name(this.programTypeName());
                //    }
                //});
                _super.prototype.subscribeObservables.call(_this);
                _this.programTypeName = ko.observable(null);
                return _this;
            }
            Program.prototype.toData = function () {
                var result = {
                    description: this.description(),
                    name: this.name(),
                    type: this.type(),
                    scenarios: this.scenarios()
                };
                return result;
            };
            return Program;
        }(Dns.ChildViewModel));
        Create.Program = Program;
    })(Create = ModularProgram.Create || (ModularProgram.Create = {}));
})(ModularProgram || (ModularProgram = {}));
(function (ModularProgram) {
    var Display;
    (function (Display) {
        var vm;
        var ViewModel = /** @class */ (function () {
            function ViewModel(hasResponses, data) {
                this.HasResponses = ko.observable(hasResponses);
                this.SignatureData = ko.observableArray(data);
            }
            return ViewModel;
        }());
        Display.ViewModel = ViewModel;
        function init(hasResponses, data, bindingControl) {
            vm = new ViewModel(hasResponses, data);
            ko.applyBindings(vm, bindingControl[0]);
            bindingControl.fadeIn(100);
            Dns.EnableValidation();
        }
        Display.init = init;
    })(Display = ModularProgram.Display || (ModularProgram.Display = {}));
})(ModularProgram || (ModularProgram = {}));
//# sourceMappingURL=Programs.js.map