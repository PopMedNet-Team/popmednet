/// <reference path="../../../../../lpp.dns.portal/js/_rootlayout.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
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
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var CodesTerm = (function (_super) {
        __extends(CodesTerm, _super);
        function CodesTerm(codesData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.CodesTerm) || this;
            _this.Codes = ko.observable(codesData.Codes);
            _this.CodeType = ko.observable(codesData.CodeType);
            _this.CodesTermType = ko.observable(codesData.CodesTermType);
            _this.SearchMethodType = ko.observable(codesData.SearchMethodType);
            // when the type changes, clear the codes
            _this.CodesTermType.subscribe(function (newValue) {
                _this.Codes('');
            });
            //this.Codes.subscribe((newValue: string) => {
            //    console.log("new value of codes is " + newValue);
            //});
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        CodesTerm.prototype.SelectCode = function () {
            var _this = this;
            var listID;
            var termType = this.CodesTermType().substr != null ? parseInt(this.CodesTermType()) : this.CodesTermType();
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
            }).done(function (results) {
                if (!results)
                    return; //User clicked cancel
                _this.Codes(results.map(function (i) { return i.Code; }).join(", "));
            });
        };
        CodesTerm.prototype.toData = function () {
            var data = {
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: this.Codes(),
                CodesTermType: this.CodesTermType(),
                SearchMethodType: this.SearchMethodType(),
                CodeType: this.CodeType()
            };
            //console.log('Code Term: ' + JSON.stringify(data));
            return data;
        };
        CodesTerm.Diagnosis_ICD9Term = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.Diagnosis_ICD9Term,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        CodesTerm.Drug_ICD9Term = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.Drug_ICD9Term,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        CodesTerm.DrugClassTerm = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.DrugClassTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        CodesTerm.GenericDrugTerm = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.GenericDrugTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        CodesTerm.HCPCSTerm = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.HCPCSTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        CodesTerm.NDCTerm = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.NDCTerm,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        CodesTerm.Procedure_ICD9Term = function () {
            return new CodesTerm({
                TermType: RequestCriteriaModels.TermTypes.CodesTerm,
                Codes: '',
                CodeType: '',
                CodesTermType: RequestCriteriaModels.CodesTermTypes.Procedure_ICD9Term,
                SearchMethodType: RequestCriteriaModels.SearchMethodTypes.ExactMatch
            });
        };
        return CodesTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.CodesTerm = CodesTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
