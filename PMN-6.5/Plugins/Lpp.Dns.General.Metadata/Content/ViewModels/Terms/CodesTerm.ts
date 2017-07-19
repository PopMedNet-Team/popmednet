/// <reference path="../../../../../lpp.dns.portal/js/_rootlayout.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class CodesTerm extends Term {
        public Codes: KnockoutObservable<string>;
        public CodeType: KnockoutObservable<string>;
        public CodesTermType: KnockoutObservable<RequestCriteriaModels.CodesTermTypes>;
        public SearchMethodType: KnockoutObservable<RequestCriteriaModels.SearchMethodTypes>;

        constructor(codesData: RequestCriteriaModels.ICodesTermData) {
            super(RequestCriteriaModels.TermTypes.CodesTerm);

            this.Codes = ko.observable(codesData.Codes);
            this.CodeType = ko.observable(codesData.CodeType);
            this.CodesTermType = ko.observable(codesData.CodesTermType);
            this.SearchMethodType = ko.observable(codesData.SearchMethodType);
            
            // when the type changes, clear the codes
            this.CodesTermType.subscribe((newValue: RequestCriteriaModels.CodesTermTypes) => {
                this.Codes('');
            });

            //this.Codes.subscribe((newValue: string) => {
            //    console.log("new value of codes is " + newValue);
            //});

            super.subscribeObservables();
        }

        public SelectCode() {
            var listID: number;
            var termType: number = (<any>this.CodesTermType()).substr != null ? parseInt(<any>this.CodesTermType()) : this.CodesTermType();
            switch (termType) {
                case RequestCriteriaModels.CodesTermTypes.Diagnosis_ICD9Term:
                    listID = Dns.Enums.Lists.ICD9Diagnosis;
                    break;
                case RequestCriteriaModels.CodesTermTypes.Drug_ICD9Term:
                    listID = Dns.Enums.Lists.DrugCode;
                    break;
                case RequestCriteriaModels.CodesTermTypes.DrugClassTerm:
                    listID = Dns.Enums.Lists.DrugClass;
                    break;
                case RequestCriteriaModels.CodesTermTypes.GenericDrugTerm:
                    listID = Dns.Enums.Lists.GenericName;
                    break;
                case RequestCriteriaModels.CodesTermTypes.HCPCSTerm:
                    listID = Dns.Enums.Lists.HCPCSProcedures;
                    break;
                case RequestCriteriaModels.CodesTermTypes.NDCTerm:
                    listID = Dns.Enums.Lists.SPANProcedure;
                    break;
                case RequestCriteriaModels.CodesTermTypes.Procedure_ICD9Term:
                    listID = Dns.Enums.Lists.ICD9Procedures;
                    break;
            }
            var codes = this.Codes().split(", ");
            Global.Helpers.ShowDialog(Global.Helpers.GetEnumString(Dns.Enums.ListsTranslation, listID), "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: listID,
                Codes: codes
            }).done((results: string[]) => {
                if (!results)
                    return; //User clicked cancel

                this.Codes(results.map((i: any) => i.Code).join(", "));

            });
        }

        public toData(): RequestCriteriaModels.ICodesTermData {
            var data: RequestCriteriaModels.ICodesTermData = {
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: this.Codes(),                
                CodesTermType: this.CodesTermType(),
                SearchMethodType: this.SearchMethodType(),
                CodeType: this.CodeType()
            };

            //console.log('Code Term: ' + JSON.stringify(data));

            return data;
        }
        
        public static Diagnosis_ICD9Term(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.Diagnosis_ICD9Term,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }

        public static Drug_ICD9Term(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.Drug_ICD9Term,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }

        public static DrugClassTerm(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.DrugClassTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }

        public static GenericDrugTerm(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.GenericDrugTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }

        public static HCPCSTerm(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.HCPCSTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }

        public static NDCTerm(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.NDCTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }

        public static Procedure_ICD9Term(): CodesTerm {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.Procedure_ICD9Term,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        }
    }
}