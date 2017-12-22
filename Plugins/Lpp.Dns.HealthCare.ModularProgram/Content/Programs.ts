/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />

module ModularProgram.Create {
    var vm: ViewModel;
    var ProgramTypes: IProgramTypes[];
    export class ViewModel extends Dns.PageViewModel {
        public Programs = ko.observableArray<Program>();
        public SelectAll: KnockoutObservable<boolean>;        

        constructor(data: IProgram[], hiddenDataControl: JQuery) {
            super(hiddenDataControl);

            data.forEach((program) => {
                var p: Program = new Program(program, true);
                var ProgramTypeName = this.GetProgramTypeName(program.type);
                if (ProgramTypeName) {
                    p.programTypeName(ProgramTypeName);

                    this.Programs.push(p);
                }
            });

            //Now go through and back fill any new ones that have just been added.
            ProgramTypes.forEach((programType) => {
                if (!this.ProgramExists(programType)) {
                    var p = new Program({
                        type: programType.Code,
                        name: programType.Name,
                        description: null,
                        scenarios: null
                    }, false);

                    var ProgramTypeName = this.GetProgramTypeName(programType.Code);
                    if (ProgramTypeName) {
                        p.programTypeName(ProgramTypeName);
                        this.Programs.push(p);
                    }
                }
            });

            this.SelectAll = ko.observable(false);
            //this.SelectAll.subscribe((value) => {
            //    this.Programs().forEach((program) => {
            //        program.Selected(value);
            //    });
            //});
        }
        
        public save() {
            if (!this.isValid())
                return;

            var data: IProgram[] = [];

            //this.Programs().forEach((program) => {
            //    if (!program.Selected())
            //        return;
            //    data.push(program.toData());
            //});

            return super.store(data);
        }


        private ProgramExists(programType: IProgramTypes): boolean {
            var result = false;
            this.Programs().forEach((program) => {
                if (program.type() == programType.Code) {
                    result = true;
                    return;
                }
            });

            return result;
        }

        private GetProgramTypeName(code: string): string {
            var name: string = null;
            ProgramTypes.forEach((type) => {
                if (type.Code == code) {
                    name = type.Name;
                    return;
                }
            });

            return name;
        }
    }

    export function init(data: IProgram[], programTypes: IProgramTypes[], bindingControl: JQuery, hiddenDataControl: JQuery) {
        //ProgramTypes = programTypes;
        //vm = new ViewModel(data, hiddenDataControl);
        //ko.applyBindings(vm, bindingControl[0]);
        //bindingControl.fadeIn(100);
        Dns.EnableValidation();
    }

    export class Program extends Dns.ChildViewModel {
        public name: KnockoutObservable<string>;
        public type: KnockoutObservable<string>;
        public scenarios: KnockoutObservable<string>;
        public description: KnockoutObservable<string>;
        //public Selected: KnockoutObservable<boolean>;
        public programTypeName: KnockoutObservable<string>;

        constructor(program: IProgram, selected: boolean) {
            super();
            this.name = ko.observable(program == null ? null : program.name); //Validation pulled from input control's HTML 5.
            this.type = ko.observable(program == null ? null : program.type);
            this.description = ko.observable(program == null ? null : program.description); //Validation pulled from input control's HTML 5.
            this.scenarios = ko.observable(program == null ? null : program.scenarios); //Validation pulled from input control's HTML 5. Example of inline would be: .extend({ maxLength: 255 })
            //this.Selected = ko.observable(selected);
            //this.Selected.subscribe((value) => {
            //    if (!value) {
            //        this.description(null);
            //        this.name(this.programTypeName());
            //    }
            //});

            super.subscribeObservables();

            this.programTypeName = ko.observable(null);
        }

        public toData(): IProgram {
            var result: IProgram = {
                description: this.description(),
                name: this.name(),
                type: this.type(),
                scenarios: this.scenarios()
            };

            return result;
        }
    }

    export interface IProgram {
        name: string;
        type: string;
        description: string;
        scenarios: string;
    }

    export interface IProgramTypes {
        Code: string;
        Name: string;
    }
}

module ModularProgram.Display {
    var vm: ViewModel;

    export class ViewModel {
        public HasResponses: KnockoutObservable<boolean>;
        public SignatureData: KnockoutObservableArray<ISignatureDatum>;

        constructor(hasResponses: boolean, data: ISignatureDatum[]) {
            this.HasResponses = ko.observable(hasResponses);
            this.SignatureData = ko.observableArray(data);
        }
    }

    export function init(hasResponses: boolean, data: ISignatureDatum[], bindingControl: JQuery) {
        vm = new ViewModel(hasResponses, data);
        ko.applyBindings(vm, bindingControl[0]);
        bindingControl.fadeIn(100);
        Dns.EnableValidation();
    }

    export interface ISignatureDatum {
        name: string;
        value: string;
    }

}