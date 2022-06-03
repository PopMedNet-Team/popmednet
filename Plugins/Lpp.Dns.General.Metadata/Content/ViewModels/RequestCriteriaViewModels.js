var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../models/requestcriteriamodels.ts" />
/// <reference path="../models/datacheckermodels.ts" />
/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../Lpp.Dns.Api/Scripts/Lpp.Dns.Interfaces.ts" />
/// <reference path="datacheckerviewmodels.ts" />
/// <reference path="../../../../lpp.dns.portal/scripts/page/page.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var Term = /** @class */ (function (_super) {
        __extends(Term, _super);
        function Term(termType) {
            var _this = _super.call(this) || this;
            _this.TermType = termType;
            _this.TemplateName = ko.computed(function () {
                if (_this.TermType == null)
                    throw "Term Type is Null";
                return RequestCriteriaModels.TermTypes[_this.TermType];
            });
            return _this;
        }
        Term.TermFactory = function (term) {
            switch (term.TermType) {
                case RequestCriteriaModels.TermTypes.AgeRangeTerm:
                    return new AgeRangeTerm(term);
                case RequestCriteriaModels.TermTypes.AgeStratifierTerm:
                    return new RequestCriteriaViewModels.AgeStratifierTerm(term);
                case RequestCriteriaModels.TermTypes.ClinicalSettingTerm:
                    return new RequestCriteriaViewModels.ClinicalSettingTerm(term);
                case RequestCriteriaModels.TermTypes.CodesTerm:
                    return new RequestCriteriaViewModels.CodesTerm(term);
                case RequestCriteriaModels.TermTypes.DataPartnerTerm:
                    return new RequestCriteriaViewModels.DataPartnerTerm(term);
                case RequestCriteriaModels.TermTypes.DateRangeTerm:
                    return new RequestCriteriaViewModels.DateRangeTerm(term);
                case RequestCriteriaModels.TermTypes.EthnicityTerm:
                    return new DataCheckerViewModels.EthnicityTerm(term);
                case RequestCriteriaModels.TermTypes.MetricTerm:
                    return new DataCheckerViewModels.MetricTerm(term);
                case RequestCriteriaModels.TermTypes.ProjectTerm:
                    return new RequestCriteriaViewModels.ProjectTerm(term);
                case RequestCriteriaModels.TermTypes.RaceTerm:
                    return new DataCheckerViewModels.RaceTerm(term);
                case RequestCriteriaModels.TermTypes.RequestStatusTerm:
                    return new RequestCriteriaViewModels.RequestStatusTerm(term);
                case RequestCriteriaModels.TermTypes.SexTerm:
                    return new RequestCriteriaViewModels.SexTerm(term);
                case RequestCriteriaModels.TermTypes.WorkplanTypeTerm:
                    return new RequestCriteriaViewModels.WorkplanTypeTerm(term);
                case RequestCriteriaModels.TermTypes.RequesterCenterTerm:
                    return new RequestCriteriaViewModels.RequesterCenterTerm(term);
                case RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm:
                    return new RequestCriteriaViewModels.ReportAggregationLevelTerm(term);
                case RequestCriteriaModels.TermTypes.PDXTerm:
                    return new DataCheckerViewModels.PDXTerm(term);
                case RequestCriteriaModels.TermTypes.RxAmtTerm:
                    return new DataCheckerViewModels.RxAmtTerm(term);
                case RequestCriteriaModels.TermTypes.RxSupTerm:
                    return new DataCheckerViewModels.RxSupTerm(term);
                case RequestCriteriaModels.TermTypes.EncounterTypeTerm:
                    return new DataCheckerViewModels.EncounterTerm(term);
                case RequestCriteriaModels.TermTypes.MetaDataTableTerm:
                    return new DataCheckerViewModels.MetaDataTableTerm(term);
                default:
                    throw "RequestCriteriaViewModels.Term.TermFactory needs to construct term: " + term.TermType;
            }
        };
        Term.prototype.toData = function () {
            var term = {
                TermType: this.TermType
            };
            return term;
        };
        return Term;
    }(Dns.ChildViewModel));
    RequestCriteriaViewModels.Term = Term;
    var RequestCriteria = /** @class */ (function (_super) {
        __extends(RequestCriteria, _super);
        function RequestCriteria(requestCriteriaData, requesterCenters, workplanTypes, reportAggregationLevels) {
            var _this = _super.call(this) || this;
            // this gets initialized in the loop with AddCriteria
            _this.Criterias = ko.observableArray();
            if (requestCriteriaData) {
                requestCriteriaData.Criterias.forEach(function (criteria) {
                    _this.AddCriteria(new RequestCriteriaViewModels.Criteria(criteria));
                });
            }
            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList = [];
            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (workplanTypes != null) {
                workplanTypes.forEach(function (wt) {
                    RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData(wt.Key, wt.Value));
                });
            }
            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList = [];
            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (requesterCenters != null) {
                requesterCenters.forEach(function (rc) {
                    RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData(rc.Key, rc.Value));
                });
            }
            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList = [];
            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (reportAggregationLevels != null) {
                reportAggregationLevels.forEach(function (ral) {
                    RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData(ral.Key, ral.Value));
                });
            }
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RequestCriteria.prototype.AddCriteria = function (criteria) {
            //console.log( 'Adding CG: ' + JSON.stringify( criteria ) );
            if (criteria.IsPrimary &&
                (this.Criterias().filter(function (cg, index, groups) { return cg.IsPrimary(); }).length > 0))
                throw 'Only one primary criteria group is allowed';
            else
                this.Criterias().push(criteria);
        };
        RequestCriteria.prototype.RemoveCriteria = function (criteria) {
            //console.log( 'Removing CG: ' + JSON.stringify( criteria ) );
            var index = this.Criterias().indexOf(criteria);
            if (index > -1)
                this.Criterias().splice(index, 1);
        };
        RequestCriteria.prototype.toData = function () {
            var requestCriteria = {
                Criterias: this.Criterias().map(function (cg, position) {
                    return cg.toData();
                })
            };
            //console.log( 'Request Criteria: ' + JSON.stringify( requestCriteria ) );
            return requestCriteria;
        };
        return RequestCriteria;
    }(Dns.ChildViewModel));
    RequestCriteriaViewModels.RequestCriteria = RequestCriteria;
    var Criteria = /** @class */ (function (_super) {
        __extends(Criteria, _super);
        function Criteria(criteriaData, name, isPrimary, isExclusion, terms, requestTerms) {
            if (name === void 0) { name = 'Primary'; }
            if (isPrimary === void 0) { isPrimary = true; }
            if (isExclusion === void 0) { isExclusion = false; }
            if (terms === void 0) { terms = []; }
            if (requestTerms === void 0) { requestTerms = []; }
            var _this = _super.call(this) || this;
            criteriaData = criteriaData || {
                Name: name,
                IsExclusion: isExclusion,
                IsPrimary: isPrimary,
                Terms: terms,
                RequestTerms: [],
                HeaderTerms: []
            };
            _this.IsExclusion = ko.observable(criteriaData.IsExclusion);
            _this.IsPrimary = ko.observable(criteriaData.IsPrimary);
            _this.Name = ko.observable(criteriaData.Name);
            // this gets initialized in the loop with AddTerm
            //this.Terms = ko.observableArray<RequestCriteriaModels.ITerm>();
            _this.HeaderTerms = ko.observableArray();
            _this.RequestTerms = ko.observableArray();
            // Add each Term supplied and separate into Header and Body Terms.
            criteriaData.Terms.forEach(function (term) {
                var thisTerm = RequestCriteriaViewModels.Term.TermFactory(term);
                _this.AddTerm(thisTerm);
                // if the term supports the Added method, call it...
                if (thisTerm.Added)
                    thisTerm.Added(_this);
            });
            _this.HeaderTerms.subscribe(function (newValue) {
                // initialize the controls
                //$(".DatePicker").kendoDatePicker({
                //    changeMonth: true,
                //    changeYear: true,
                //    dateFormat: 'mm/dd/yy',
                //    defaultDate: +0,
                //    maxDate: '12/31/2299',
                //    minDate: '01/01/1900',
                //    showButtonPanel: true
                //});
                $(".DatePicker").kendoDatePicker({
                    format: 'MM/dd/yy',
                    max: new Date(2299, 12, 31),
                    min: new Date(1900, 1, 1)
                });
            });
            _this.RequestTerms.subscribe(function (newValue) {
                // initialize the controls
                //$(".DatePicker").kendoDatePicker({
                //    changeMonth: true,
                //    changeYear: true,
                //    dateFormat: 'mm/dd/yy',
                //    defaultDate: +0,
                //    maxDate: '12/31/2299',
                //    minDate: '01/01/1900',
                //    showButtonPanel: true
                //});
                $(".DatePicker").kendoDatePicker({
                    format: 'MM/dd/yy',
                    max: new Date(2299, 12, 31),
                    min: new Date(1900, 1, 1)
                });
            });
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        Criteria.prototype.AddTerm = function (term) {
            if (term instanceof ProjectTerm || term instanceof RequestStatusTerm || (term instanceof DateRangeTerm && term.Title() == 'Submit Date Range')
                || term instanceof RequesterCenterTerm || term instanceof WorkplanTypeTerm || term instanceof ReportAggregationLevelTerm) {
                this.HeaderTerms.push(term);
            }
            else {
                this.RequestTerms.push(term);
            }
            // to support chaining, return the new term
            return term;
        };
        Criteria.prototype.RemoveTerm = function (term) {
            var index = this.HeaderTerms.indexOf(term);
            if (index > -1) {
                this.HeaderTerms.splice(index, 1);
                return;
            }
            index = this.RequestTerms.indexOf(term);
            if (index > -1) {
                this.RequestTerms.splice(index, 1);
                return;
            }
        };
        Criteria.prototype.ReplaceTerm = function (targetTerm, newTerm) {
            var index = this.RequestTerms.indexOf(targetTerm);
            if (index > -1)
                this.RequestTerms.splice(index, 1, newTerm);
            index = this.HeaderTerms.indexOf(targetTerm);
            if (index > -1)
                this.HeaderTerms.splice(index, 1, newTerm);
            return newTerm;
        };
        Criteria.prototype.toData = function () {
            var criteria = {
                Name: this.Name(),
                IsExclusion: this.IsExclusion(),
                IsPrimary: this.IsPrimary(),
                Terms: [],
                RequestTerms: this.RequestTerms().map(function (term, postion) {
                    return term.toData();
                }),
                HeaderTerms: this.HeaderTerms().map(function (term, postion) {
                    return term.toData();
                })
            };
            return criteria;
        };
        return Criteria;
    }(Dns.ChildViewModel));
    RequestCriteriaViewModels.Criteria = Criteria;
    var TaskActivities = /** @class */ (function () {
        function TaskActivities(activityData) {
            this.ProjectID = ko.observable();
            this.AllActivities = activityData; //<= flat list of activities available
            this.dsTaskOrders = new kendo.data.DataSource({ data: [] });
            this.dsActivities = new kendo.data.DataSource({ data: [] });
            this.dsActivityProjects = new kendo.data.DataSource({ data: [] });
            this.dsTaskOrders.data(ko.utils.arrayFilter(this.AllActivities, function (a) { return a.TaskLevel == 1; }));
            this.dsActivities.data(ko.utils.arrayFilter(this.AllActivities, function (a) { return a.TaskLevel == 2; }));
            this.dsActivityProjects.data(ko.utils.arrayFilter(this.AllActivities, function (a) { return a.TaskLevel == 3; }));
            var self = this;
            this.SelectProject = function (projectID) {
                self.ProjectID(projectID);
                if (projectID) {
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities, function (a) { return a.TaskLevel == 1 && a.ProjectID.toLowerCase() == projectID.toLowerCase(); }));
                }
                else {
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities, function (a) { return a.TaskLevel == 1; }));
                }
                alert("Project Changed");
            };
        }
        TaskActivities.prototype.SelectActivity = function (level, activityID) {
            //this should all be handled by the datasources and kendo dropdowns automatically. Not removing at this time since can't determine if will break other things.
        };
        return TaskActivities;
    }());
    RequestCriteriaViewModels.TaskActivities = TaskActivities;
    var AgeRangeTerm = /** @class */ (function (_super) {
        __extends(AgeRangeTerm, _super);
        function AgeRangeTerm(ageRangeData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.AgeRangeTerm) || this;
            _this.MinAge = ko.observable(ageRangeData ? ageRangeData.MinAge : 0);
            _this.MaxAge = ko.observable(ageRangeData ? ageRangeData.MaxAge : 0);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        AgeRangeTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var data = {
                TermType: superdata.TermType,
                MinAge: this.MinAge(),
                MaxAge: this.MaxAge()
            };
            return data;
        };
        return AgeRangeTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.AgeRangeTerm = AgeRangeTerm;
    var AgeStratifierTerm = /** @class */ (function (_super) {
        __extends(AgeStratifierTerm, _super);
        function AgeStratifierTerm(stratifierData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.AgeStratifierTerm) || this;
            _this.AgeStratifier = ko.observable(stratifierData ? stratifierData.AgeStratifier : RequestCriteriaModels.AgeStratifierTypes.NotSpecified);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        AgeStratifierTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var data = {
                TermType: superdata.TermType,
                AgeStratifier: this.AgeStratifier()
            };
            //console.log('Stratifier Term: ' + JSON.stringify(data));
            return data;
        };
        AgeStratifierTerm.AgeStratifiersList = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.AgeStratifierTypes.NotSpecified),
            new Dns.KeyValuePairData('No Stratification', RequestCriteriaModels.AgeStratifierTypes.None),
            new Dns.KeyValuePairData('10 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Ten),
            new Dns.KeyValuePairData('7 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Seven),
            new Dns.KeyValuePairData('4 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Four),
            new Dns.KeyValuePairData('2 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Two)
        ];
        return AgeStratifierTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.AgeStratifierTerm = AgeStratifierTerm;
    var ClinicalSettingTerm = /** @class */ (function (_super) {
        __extends(ClinicalSettingTerm, _super);
        function ClinicalSettingTerm(clinicalSettingData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.ClinicalSettingTerm) || this;
            _this.ClinicalSetting = ko.observable(clinicalSettingData ? clinicalSettingData.ClinicalSetting : RequestCriteriaModels.ClinicalSettingTypes.NotSpecified);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        ClinicalSettingTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var clinicalSettingData = {
                TermType: superdata.TermType,
                ClinicalSetting: this.ClinicalSetting()
            };
            //console.log('Clinical Setting: ' + JSON.stringify(clinicalSettingData));
            return clinicalSettingData;
        };
        ClinicalSettingTerm.ClinicalSettingsList = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.ClinicalSettingTypes.NotSpecified),
            new Dns.KeyValuePairData('Any', RequestCriteriaModels.ClinicalSettingTypes.Any),
            new Dns.KeyValuePairData('In-patient', RequestCriteriaModels.ClinicalSettingTypes.InPatient),
            new Dns.KeyValuePairData('Out-patient', RequestCriteriaModels.ClinicalSettingTypes.OutPatient),
            new Dns.KeyValuePairData('Emergency', RequestCriteriaModels.ClinicalSettingTypes.Emergency),
        ];
        return ClinicalSettingTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.ClinicalSettingTerm = ClinicalSettingTerm;
    var CodesTerm = /** @class */ (function (_super) {
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
    }(Term));
    RequestCriteriaViewModels.CodesTerm = CodesTerm;
    var DataPartnerTerm = /** @class */ (function (_super) {
        __extends(DataPartnerTerm, _super);
        function DataPartnerTerm(dataPartnersData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.DataPartnerTerm) || this;
            var dummy = [];
            _this.DataPartners = ko.observableArray(dataPartnersData ? dataPartnersData.DataPartners : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        DataPartnerTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var dataPartnersData = {
                TermType: superdata.TermType,
                DataPartners: this.DataPartners()
            };
            //console.log('Data Partners: ' + JSON.stringify(dataPartnersData));
            return dataPartnersData;
        };
        return DataPartnerTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.DataPartnerTerm = DataPartnerTerm;
    var DateRangeTerm = /** @class */ (function (_super) {
        __extends(DateRangeTerm, _super);
        function DateRangeTerm(dateRangeData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.DateRangeTerm) || this;
            var start = null;
            if (dateRangeData.StartDate && dateRangeData.StartDate != null) {
                start = moment.utc(dateRangeData.StartDate).local().toDate();
            }
            var end = null;
            if (dateRangeData.EndDate && dateRangeData.EndDate != null) {
                end = moment.utc(dateRangeData.EndDate).local().toDate();
            }
            _this.Title = ko.observable(dateRangeData.Title);
            _this.StartDate = ko.observable(start);
            _this.EndDate = ko.observable(end);
            _this.DateRangeTermType = ko.observable(dateRangeData.DateRangeTermType);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        DateRangeTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var data = {
                TermType: superdata.TermType,
                Title: this.Title(),
                StartDate: this.StartDate(),
                EndDate: this.EndDate(),
                DateRangeTermType: this.DateRangeTermType()
            };
            return data;
        };
        DateRangeTerm.ObservationPeriod = function () {
            return new DateRangeTerm({
                Title: "Observation Period",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.ObservationPeriod
            });
        };
        DateRangeTerm.SubmitDateRange = function () {
            return new DateRangeTerm({
                Title: "Submit Date Range",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.SubmitDateRange
            });
        };
        return DateRangeTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.DateRangeTerm = DateRangeTerm;
    var ProjectTerm = /** @class */ (function (_super) {
        __extends(ProjectTerm, _super);
        function ProjectTerm(projectData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.ProjectTerm) || this;
            _this.Project = ko.observable(projectData ? projectData.Project : '{00000000-0000-0000-0000-000000000000}');
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        ProjectTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var projectData = {
                TermType: superdata.TermType,
                Project: this.Project()
            };
            //console.log('Project: ' + JSON.stringify(projectData));
            return projectData;
        };
        return ProjectTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.ProjectTerm = ProjectTerm;
    var ReportAggregationLevelTerm = /** @class */ (function (_super) {
        __extends(ReportAggregationLevelTerm, _super);
        function ReportAggregationLevelTerm(reportAggregationLevelData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm) || this;
            _this.ReportAggregationLevel = ko.observable(reportAggregationLevelData == undefined ? "00000000-0000-0000-0000-000000000000" : reportAggregationLevelData.ReportAggregationLevelID);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        ReportAggregationLevelTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var reportAggregationLevelData = {
                TermType: superdata.TermType,
                ReportAggregationLevelID: this.ReportAggregationLevel()
            };
            return reportAggregationLevelData;
        };
        return ReportAggregationLevelTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.ReportAggregationLevelTerm = ReportAggregationLevelTerm;
    var RequesterCenterTerm = /** @class */ (function (_super) {
        __extends(RequesterCenterTerm, _super);
        function RequesterCenterTerm(requesterCenterData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RequesterCenterTerm) || this;
            _this.RequesterCenter = ko.observable(requesterCenterData == undefined ? "00000000-0000-0000-0000-000000000000" : requesterCenterData.RequesterCenterID);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RequesterCenterTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var requesterCenterData = {
                TermType: superdata.TermType,
                RequesterCenterID: this.RequesterCenter()
            };
            return requesterCenterData;
        };
        return RequesterCenterTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.RequesterCenterTerm = RequesterCenterTerm;
    var RequestStatusTerm = /** @class */ (function (_super) {
        __extends(RequestStatusTerm, _super);
        function RequestStatusTerm(requestStatusData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RequestStatusTerm) || this;
            _this.RequestStatus = ko.observable(requestStatusData ? requestStatusData.RequestStatus : null);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RequestStatusTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var requestStatusData = {
                TermType: superdata.TermType,
                RequestStatus: this.RequestStatus()
            };
            //console.log('Request Status: ' + JSON.stringify(requestStatusData));
            return requestStatusData;
        };
        return RequestStatusTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.RequestStatusTerm = RequestStatusTerm;
    var SexTerm = /** @class */ (function (_super) {
        __extends(SexTerm, _super);
        function SexTerm(sexData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.SexTerm) || this;
            _this.Sex = ko.observable(sexData ? sexData.Sex : RequestCriteriaModels.SexTypes.NotSpecified);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        SexTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var sexData = {
                TermType: superdata.TermType,
                Sex: this.Sex()
            };
            //console.log('Sex: ' + JSON.stringify(sexData));
            return sexData;
        };
        SexTerm.SexesList = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.SexTypes.NotSpecified),
            new Dns.KeyValuePairData('Male', RequestCriteriaModels.SexTypes.Male),
            new Dns.KeyValuePairData('Female', RequestCriteriaModels.SexTypes.Female),
            new Dns.KeyValuePairData('Both', RequestCriteriaModels.SexTypes.Both),
            new Dns.KeyValuePairData('Both Aggregated', RequestCriteriaModels.SexTypes.Aggregated)
        ];
        return SexTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.SexTerm = SexTerm;
    var WorkplanTypeTerm = /** @class */ (function (_super) {
        __extends(WorkplanTypeTerm, _super);
        function WorkplanTypeTerm(workplanTypeData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.WorkplanTypeTerm) || this;
            _this.WorkplanType = ko.observable(workplanTypeData == undefined ? "00000000-0000-0000-0000-000000000000" : workplanTypeData.WorkplanTypeID);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        WorkplanTypeTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var workplanTypeData = {
                TermType: superdata.TermType,
                WorkplanTypeID: this.WorkplanType()
            };
            return workplanTypeData;
        };
        return WorkplanTypeTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.WorkplanTypeTerm = WorkplanTypeTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
