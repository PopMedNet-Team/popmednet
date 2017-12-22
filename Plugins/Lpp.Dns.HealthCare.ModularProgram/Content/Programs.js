var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ModularProgram;
(function (ModularProgram) {
    var Create;
    (function (Create) {
        var vm;
        var ProgramTypes;
        var ViewModel = (function (_super) {
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
            }
            ViewModel.prototype.save = function () {
                if (!this.isValid())
                    return;
                var data = [];
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
            Dns.EnableValidation();
        }
        Create.init = init;
        var Program = (function (_super) {
            __extends(Program, _super);
            function Program(program, selected) {
                var _this = _super.call(this) || this;
                _this.name = ko.observable(program == null ? null : program.name);
                _this.type = ko.observable(program == null ? null : program.type);
                _this.description = ko.observable(program == null ? null : program.description);
                _this.scenarios = ko.observable(program == null ? null : program.scenarios);
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
        var ViewModel = (function () {
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
