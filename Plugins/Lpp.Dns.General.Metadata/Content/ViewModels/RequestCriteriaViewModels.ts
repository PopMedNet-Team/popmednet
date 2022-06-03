/// <reference path="../models/requestcriteriamodels.ts" />
/// <reference path="../models/datacheckermodels.ts" />
/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../Lpp.Dns.Api/Scripts/Lpp.Dns.Interfaces.ts" />
/// <reference path="datacheckerviewmodels.ts" />
/// <reference path="../../../../lpp.dns.portal/scripts/page/page.ts" />
module RequestCriteriaViewModels {

    export class Term extends Dns.ChildViewModel implements RequestCriteriaModels.ITerm {
        public TermType: RequestCriteriaModels.TermTypes;
        public TemplateName: KnockoutComputed<string>;

        constructor(termType: RequestCriteriaModels.TermTypes) {
            super();

            this.TermType = termType;

            this.TemplateName = ko.computed(() => {
                if (this.TermType == null)
                    throw "Term Type is Null";

                return RequestCriteriaModels.TermTypes[this.TermType];
            });
        }

        public static TermFactory(term: RequestCriteriaModels.ITermData): Term {
            switch (term.TermType) {
                case RequestCriteriaModels.TermTypes.AgeRangeTerm:
                    return <Term>new AgeRangeTerm(<RequestCriteriaModels.IAgeRangeTermData>term);

                case RequestCriteriaModels.TermTypes.AgeStratifierTerm:
                    return <Term>new RequestCriteriaViewModels.AgeStratifierTerm(<RequestCriteriaModels.IAgeStratifierTermData>term);

                case RequestCriteriaModels.TermTypes.ClinicalSettingTerm:
                    return <Term>new RequestCriteriaViewModels.ClinicalSettingTerm(<RequestCriteriaModels.IClinicalSettingsTermData>term);

                case RequestCriteriaModels.TermTypes.CodesTerm:
                    return <Term>new RequestCriteriaViewModels.CodesTerm(<RequestCriteriaModels.ICodesTermData>term);

                case RequestCriteriaModels.TermTypes.DataPartnerTerm:
                    return <Term>new RequestCriteriaViewModels.DataPartnerTerm(<RequestCriteriaModels.IDataPartnerTermData>term);

                case RequestCriteriaModels.TermTypes.DateRangeTerm:
                    return <Term>new RequestCriteriaViewModels.DateRangeTerm(<RequestCriteriaModels.IDateRangeTermData>term);

                case RequestCriteriaModels.TermTypes.EthnicityTerm:
                    return <Term>new DataCheckerViewModels.EthnicityTerm(<DataCheckerModels.IEthnicityTermData>term);

                case RequestCriteriaModels.TermTypes.MetricTerm:
                    return <Term>new DataCheckerViewModels.MetricTerm(<DataCheckerModels.IMetricsTermData>term);

                case RequestCriteriaModels.TermTypes.ProjectTerm:
                    return <Term>new RequestCriteriaViewModels.ProjectTerm(<RequestCriteriaModels.IProjectTermData>term);

                case RequestCriteriaModels.TermTypes.RaceTerm:
                    return <Term>new DataCheckerViewModels.RaceTerm(<DataCheckerModels.IRaceTermData>term);

                case RequestCriteriaModels.TermTypes.RequestStatusTerm:
                    return <Term>new RequestCriteriaViewModels.RequestStatusTerm(<RequestCriteriaModels.IRequestStatusTermData>term);

                case RequestCriteriaModels.TermTypes.SexTerm:
                    return <Term>new RequestCriteriaViewModels.SexTerm(<RequestCriteriaModels.ISexTermData>term);

                case RequestCriteriaModels.TermTypes.WorkplanTypeTerm:
                    return <Term>new RequestCriteriaViewModels.WorkplanTypeTerm(<RequestCriteriaModels.IWorkplanTypeTermData>term);

                case RequestCriteriaModels.TermTypes.RequesterCenterTerm:
                    return <Term>new RequestCriteriaViewModels.RequesterCenterTerm(<RequestCriteriaModels.IRequesterCenterTermData>term);

                case RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm:
                    return <Term>new RequestCriteriaViewModels.ReportAggregationLevelTerm(<RequestCriteriaModels.IReportAggregationLevelTermData>term);

                case RequestCriteriaModels.TermTypes.PDXTerm:
                    return <Term>new DataCheckerViewModels.PDXTerm(<DataCheckerModels.IPDXTermData>term);
                case RequestCriteriaModels.TermTypes.RxAmtTerm:
                    return <Term>new DataCheckerViewModels.RxAmtTerm(<DataCheckerModels.IRxAmtTermData>term);
                case RequestCriteriaModels.TermTypes.RxSupTerm:
                    return <Term>new DataCheckerViewModels.RxSupTerm(<DataCheckerModels.IRxSupTermData>term);
                case RequestCriteriaModels.TermTypes.EncounterTypeTerm:
                    return <Term>new DataCheckerViewModels.EncounterTerm(<DataCheckerModels.IEncounterTermData>term);
                case RequestCriteriaModels.TermTypes.MetaDataTableTerm:
                    return <Term>new DataCheckerViewModels.MetaDataTableTerm(<DataCheckerModels.IMetaDataTableTermData>term);
                default:
                    throw "RequestCriteriaViewModels.Term.TermFactory needs to construct term: " + term.TermType;
            }
        }

        public toData(): RequestCriteriaModels.ITermData {
            var term: RequestCriteriaModels.ITermData = {
                TermType: this.TermType
            };

            return term;
        }
    }

    export class RequestCriteria extends Dns.ChildViewModel {
        public Criterias: KnockoutObservableArray<RequestCriteriaViewModels.Criteria>;
        public static WorkplanTypeList: Dns.KeyValuePairData<string, string>[];
        public static RequesterCenterList: Dns.KeyValuePairData<string, string>[];
        public static ReportAggregationLevelList: Dns.KeyValuePairData<string, string>[];

        constructor(requestCriteriaData?: RequestCriteriaModels.IRequestCriteriaData, requesterCenters?: RequestCriteriaModels.IRequesterCenter[], workplanTypes?: RequestCriteriaModels.IWorkplanType[], reportAggregationLevels?: RequestCriteriaModels.IReportAggregationLevel[]) {
            super();

            // this gets initialized in the loop with AddCriteria
            this.Criterias = ko.observableArray<RequestCriteriaViewModels.Criteria>();

            if (requestCriteriaData) {
                requestCriteriaData.Criterias.forEach((criteria) => {
                    this.AddCriteria(new RequestCriteriaViewModels.Criteria(criteria));
                });
            }

            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList = [];
            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (workplanTypes != null) {
                workplanTypes.forEach(wt => {
                    RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData(wt.Key, wt.Value));
                });
            }

            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList = [];
            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (requesterCenters != null) {
                requesterCenters.forEach(rc => {
                    RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData(rc.Key, rc.Value));
                });
            }

            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList = [];
            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (reportAggregationLevels != null) {
                reportAggregationLevels.forEach(ral => {
                    RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData(ral.Key, ral.Value));
                });
            }

            super.subscribeObservables();
        }

        public AddCriteria(criteria: RequestCriteriaViewModels.Criteria): void {
            //console.log( 'Adding CG: ' + JSON.stringify( criteria ) );

            if (criteria.IsPrimary &&
                (this.Criterias().filter((cg, index, groups) => { return cg.IsPrimary() }).length > 0))
                throw 'Only one primary criteria group is allowed';
            else
                this.Criterias().push(criteria);
        }

        public RemoveCriteria(criteria: RequestCriteriaViewModels.Criteria): void {
            //console.log( 'Removing CG: ' + JSON.stringify( criteria ) );

            var index = this.Criterias().indexOf(criteria);

            if (index > -1)
                this.Criterias().splice(index, 1);
        }

        public toData(): RequestCriteriaModels.IRequestCriteriaData {
            var requestCriteria: RequestCriteriaModels.IRequestCriteriaData = {
                Criterias: this.Criterias().map((cg, position) => {
                    return cg.toData();
                })
            };

            //console.log( 'Request Criteria: ' + JSON.stringify( requestCriteria ) );

            return requestCriteria;
        }
    }

    export class Criteria extends Dns.ChildViewModel {
        public IsExclusion: KnockoutObservable<boolean>;
        public IsPrimary: KnockoutObservable<boolean>;
        public Name: KnockoutObservable<string>;
        //public Terms: KnockoutObservableArray<RequestCriteriaModels.ITerm>;
        public HeaderTerms: KnockoutObservableArray<RequestCriteriaModels.ITerm>;
        public RequestTerms: KnockoutObservableArray<RequestCriteriaModels.ITerm>;

        constructor(criteriaData?: RequestCriteriaModels.ICriteriaData, name: string = 'Primary', isPrimary: boolean = true, isExclusion: boolean = false, terms: RequestCriteriaModels.ITermData[] = [], requestTerms: RequestCriteriaModels.ITermData[] = []) {
            super();

            criteriaData = criteriaData || {
                Name: name,
                IsExclusion: isExclusion,
                IsPrimary: isPrimary,
                Terms: terms,
                RequestTerms: [],
                HeaderTerms: []
            };

            this.IsExclusion = ko.observable(criteriaData.IsExclusion);
            this.IsPrimary = ko.observable(criteriaData.IsPrimary);
            this.Name = ko.observable(criteriaData.Name);
            // this gets initialized in the loop with AddTerm
            //this.Terms = ko.observableArray<RequestCriteriaModels.ITerm>();
            this.HeaderTerms = ko.observableArray<RequestCriteriaModels.ITerm>();
            this.RequestTerms = ko.observableArray<RequestCriteriaModels.ITerm>();

            // Add each Term supplied and separate into Header and Body Terms.
            criteriaData.Terms.forEach((term) => {
                var thisTerm = RequestCriteriaViewModels.Term.TermFactory(term);
                this.AddTerm(thisTerm);

                // if the term supports the Added method, call it...
                if ((<any>thisTerm).Added)
                    (<any>thisTerm).Added(this);
            });

            this.HeaderTerms.subscribe((newValue: RequestCriteriaModels.ITerm[]) => {
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

            this.RequestTerms.subscribe((newValue: RequestCriteriaModels.ITerm[]) => {
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

            super.subscribeObservables();
        }

        public AddTerm(term: RequestCriteriaViewModels.Term): RequestCriteriaViewModels.Term {

            if (term instanceof ProjectTerm || term instanceof RequestStatusTerm || (term instanceof DateRangeTerm && (<DateRangeTerm>term).Title() == 'Submit Date Range')
                || term instanceof RequesterCenterTerm || term instanceof WorkplanTypeTerm || term instanceof ReportAggregationLevelTerm) {
                this.HeaderTerms.push(term);
            }
            else {
                this.RequestTerms.push(term);
            }

            // to support chaining, return the new term
            return term;
        }

        public RemoveTerm(term: RequestCriteriaViewModels.Term): void {
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
        }

        public ReplaceTerm(targetTerm: RequestCriteriaViewModels.Term, newTerm: RequestCriteriaViewModels.Term): RequestCriteriaViewModels.Term {
            var index = this.RequestTerms.indexOf(targetTerm);

            if (index > -1)
                this.RequestTerms.splice(index, 1, newTerm);

            index = this.HeaderTerms.indexOf(targetTerm);

            if (index > -1)
                this.HeaderTerms.splice(index, 1, newTerm);

            return newTerm;
        }

        public toData(): RequestCriteriaModels.ICriteriaData {
            var criteria: RequestCriteriaModels.ICriteriaData = {
                Name: this.Name(),
                IsExclusion: this.IsExclusion(),
                IsPrimary: this.IsPrimary(),
                Terms: [],
                RequestTerms: this.RequestTerms().map((term, postion) => {
                    return term.toData();
                }),
                HeaderTerms: this.HeaderTerms().map((term, postion) => {
                    return term.toData();
                })
            };

            return criteria;
        }
    }

    export class TaskActivities {
        private AllActivities: RequestCriteriaModels.ITaskActivity[];

        public dsTaskOrders: kendo.data.DataSource;
        public dsActivities: kendo.data.DataSource;
        public dsActivityProjects: kendo.data.DataSource;

        public SelectProject: (projectID: any) => void;

        constructor(activityData: RequestCriteriaModels.ITaskActivity[]) {

            this.AllActivities = activityData;//<= flat list of activities available

            this.dsTaskOrders = new kendo.data.DataSource({ data: [] });
            this.dsActivities = new kendo.data.DataSource({ data: [] });
            this.dsActivityProjects = new kendo.data.DataSource({ data: [] });

            this.dsTaskOrders.data(ko.utils.arrayFilter(this.AllActivities, (a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 1));
            this.dsActivities.data(ko.utils.arrayFilter(this.AllActivities, (a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 2));
            this.dsActivityProjects.data(ko.utils.arrayFilter(this.AllActivities, (a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 3));

            var self = this;
            this.SelectProject = (projectID: any) => {
                self.ProjectID(projectID);
                if (projectID) {

                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities, (a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 1 && a.ProjectID.toLowerCase() == projectID.toLowerCase()));
                } else {

                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities, (a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 1));
                }

                alert("Project Changed");
            };
        }

        public SelectActivity(level: any, activityID: any) {

            //this should all be handled by the datasources and kendo dropdowns automatically. Not removing at this time since can't determine if will break other things.
        }

        private ProjectID: KnockoutObservable<any> = ko.observable();
    }

    export class AgeRangeTerm extends RequestCriteriaViewModels.Term {
        public MinAge: KnockoutObservable<number>;
        public MaxAge: KnockoutObservable<number>;

        constructor(ageRangeData?: RequestCriteriaModels.IAgeRangeTermData) {
            super(RequestCriteriaModels.TermTypes.AgeRangeTerm);

            this.MinAge = ko.observable(ageRangeData ? ageRangeData.MinAge : 0);
            this.MaxAge = ko.observable(ageRangeData ? ageRangeData.MaxAge : 0);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.ITermData {
            var superdata = super.toData();

            var data: RequestCriteriaModels.IAgeRangeTermData = {
                TermType: superdata.TermType,
                MinAge: this.MinAge(),
                MaxAge: this.MaxAge()
            };

            return data;
        }
    }

    export class AgeStratifierTerm extends RequestCriteriaViewModels.Term {
        public AgeStratifier: KnockoutObservable<RequestCriteriaModels.AgeStratifierTypes>;

        constructor(stratifierData?: RequestCriteriaModels.IAgeStratifierTermData) {
            super(RequestCriteriaModels.TermTypes.AgeStratifierTerm);

            this.AgeStratifier = ko.observable(stratifierData ? stratifierData.AgeStratifier : RequestCriteriaModels.AgeStratifierTypes.NotSpecified);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IAgeStratifierTermData {
            var superdata = super.toData();

            var data: RequestCriteriaModels.IAgeStratifierTermData = {
                TermType: superdata.TermType,
                AgeStratifier: this.AgeStratifier()
            };

            //console.log('Stratifier Term: ' + JSON.stringify(data));

            return data;
        }

        public static AgeStratifiersList: Dns.KeyValuePairData<string, RequestCriteriaModels.AgeStratifierTypes>[] = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.AgeStratifierTypes.NotSpecified),
            new Dns.KeyValuePairData('No Stratification', RequestCriteriaModels.AgeStratifierTypes.None),
            new Dns.KeyValuePairData('10 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Ten),
            new Dns.KeyValuePairData('7 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Seven),
            new Dns.KeyValuePairData('4 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Four),
            new Dns.KeyValuePairData('2 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Two)
        ];
    }

    export class ClinicalSettingTerm extends RequestCriteriaViewModels.Term {
        public ClinicalSetting: KnockoutObservable<RequestCriteriaModels.ClinicalSettingTypes>;

        constructor(clinicalSettingData?: RequestCriteriaModels.IClinicalSettingsTermData) {
            super(RequestCriteriaModels.TermTypes.ClinicalSettingTerm);

            this.ClinicalSetting = ko.observable(clinicalSettingData ? clinicalSettingData.ClinicalSetting : RequestCriteriaModels.ClinicalSettingTypes.NotSpecified);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IClinicalSettingsTermData {
            var superdata = super.toData();

            var clinicalSettingData: RequestCriteriaModels.IClinicalSettingsTermData = {
                TermType: superdata.TermType,
                ClinicalSetting: this.ClinicalSetting()
            };

            //console.log('Clinical Setting: ' + JSON.stringify(clinicalSettingData));

            return clinicalSettingData;
        }

        public static ClinicalSettingsList: Dns.KeyValuePairData<string, RequestCriteriaModels.ClinicalSettingTypes>[] = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.ClinicalSettingTypes.NotSpecified),
            new Dns.KeyValuePairData('Any', RequestCriteriaModels.ClinicalSettingTypes.Any),
            new Dns.KeyValuePairData('In-patient', RequestCriteriaModels.ClinicalSettingTypes.InPatient),
            new Dns.KeyValuePairData('Out-patient', RequestCriteriaModels.ClinicalSettingTypes.OutPatient),
            new Dns.KeyValuePairData('Emergency', RequestCriteriaModels.ClinicalSettingTypes.Emergency),
        ];
    }

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

    export class DataPartnerTerm extends RequestCriteriaViewModels.Term {
        public DataPartners: KnockoutObservableArray<string>;

        constructor(dataPartnersData?: RequestCriteriaModels.IDataPartnerTermData) {
            super(RequestCriteriaModels.TermTypes.DataPartnerTerm);

            var dummy: string[] = [];
            this.DataPartners = ko.observableArray<string>(dataPartnersData ? dataPartnersData.DataPartners : dummy);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IDataPartnerTermData {
            var superdata = super.toData();

            var dataPartnersData: RequestCriteriaModels.IDataPartnerTermData = {
                TermType: superdata.TermType,
                DataPartners: this.DataPartners()
            };

            //console.log('Data Partners: ' + JSON.stringify(dataPartnersData));

            return dataPartnersData;
        }
    }

    export class DateRangeTerm extends RequestCriteriaViewModels.Term {
        public Title: KnockoutObservable<string>;
        public StartDate: KnockoutObservable<Date>;
        public EndDate: KnockoutObservable<Date>;
        public DateRangeTermType: KnockoutObservable<RequestCriteriaModels.DateRangeTermTypes>;

        constructor(dateRangeData: RequestCriteriaModels.IDateRangeTermData) {
            super(RequestCriteriaModels.TermTypes.DateRangeTerm);

            var start = null;
            if (dateRangeData.StartDate && dateRangeData.StartDate != null) {
                start = moment.utc(dateRangeData.StartDate).local().toDate();
            }
            var end = null;
            if (dateRangeData.EndDate && dateRangeData.EndDate != null) {
                end = moment.utc(dateRangeData.EndDate).local().toDate();
            }

            this.Title = ko.observable(dateRangeData.Title);
            this.StartDate = ko.observable(start);
            this.EndDate = ko.observable(end);
            this.DateRangeTermType = ko.observable(dateRangeData.DateRangeTermType);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.ITermData {
            var superdata = super.toData();

            var data: RequestCriteriaModels.IDateRangeTermData = {
                TermType: superdata.TermType,
                Title: this.Title(),
                StartDate: this.StartDate(),
                EndDate: this.EndDate(),
                DateRangeTermType: this.DateRangeTermType()
            };

            return data;
        }

        public static ObservationPeriod(): DateRangeTerm {
            return new DateRangeTerm({
                Title: "Observation Period",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.ObservationPeriod
            });
        }

        public static SubmitDateRange(): DateRangeTerm {
            return new DateRangeTerm({
                Title: "Submit Date Range",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.SubmitDateRange
            });
        }
    }

    export class ProjectTerm extends RequestCriteriaViewModels.Term {
        public Project: KnockoutObservable<string>;

        constructor(projectData?: RequestCriteriaModels.IProjectTermData) {
            super(RequestCriteriaModels.TermTypes.ProjectTerm);

            this.Project = ko.observable(projectData ? projectData.Project : '{00000000-0000-0000-0000-000000000000}');

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IProjectTermData {
            var superdata = super.toData();

            var projectData: RequestCriteriaModels.IProjectTermData = {
                TermType: superdata.TermType,
                Project: this.Project()
            };

            //console.log('Project: ' + JSON.stringify(projectData));

            return projectData;
        }
    }

    export class ReportAggregationLevelTerm extends RequestCriteriaViewModels.Term {
        public ReportAggregationLevel: KnockoutObservable<string>;

        constructor(reportAggregationLevelData?: RequestCriteriaModels.IReportAggregationLevelTermData) {
            super(RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm);

            this.ReportAggregationLevel = ko.observable(reportAggregationLevelData == undefined ? "00000000-0000-0000-0000-000000000000" : reportAggregationLevelData.ReportAggregationLevelID);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IReportAggregationLevelTermData {
            var superdata = super.toData();

            var reportAggregationLevelData: RequestCriteriaModels.IReportAggregationLevelTermData = {
                TermType: superdata.TermType,
                ReportAggregationLevelID: this.ReportAggregationLevel()
            };

            return reportAggregationLevelData;
        }

    }

    export class RequesterCenterTerm extends RequestCriteriaViewModels.Term {
        public RequesterCenter: KnockoutObservable<string>;

        constructor(requesterCenterData?: RequestCriteriaModels.IRequesterCenterTermData) {
            super(RequestCriteriaModels.TermTypes.RequesterCenterTerm);

            this.RequesterCenter = ko.observable(requesterCenterData == undefined ? "00000000-0000-0000-0000-000000000000" : requesterCenterData.RequesterCenterID);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IRequesterCenterTermData {
            var superdata = super.toData();

            var requesterCenterData: RequestCriteriaModels.IRequesterCenterTermData = {
                TermType: superdata.TermType,
                RequesterCenterID: this.RequesterCenter()
            };

            return requesterCenterData;
        }
    }

    export class RequestStatusTerm extends RequestCriteriaViewModels.Term {
        public RequestStatus: KnockoutObservable<Dns.Enums.RequestStatuses>;

        constructor(requestStatusData?: RequestCriteriaModels.IRequestStatusTermData) {
            super(RequestCriteriaModels.TermTypes.RequestStatusTerm);

            this.RequestStatus = ko.observable(requestStatusData ? requestStatusData.RequestStatus : null);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IRequestStatusTermData {
            var superdata = super.toData();

            var requestStatusData: RequestCriteriaModels.IRequestStatusTermData = {
                TermType: superdata.TermType,
                RequestStatus: this.RequestStatus()
            };

            //console.log('Request Status: ' + JSON.stringify(requestStatusData));

            return requestStatusData;
        }

    }

    export class SexTerm extends RequestCriteriaViewModels.Term {
        public Sex: KnockoutObservable<RequestCriteriaModels.SexTypes>;

        constructor(sexData?: RequestCriteriaModels.ISexTermData) {
            super(RequestCriteriaModels.TermTypes.SexTerm);

            this.Sex = ko.observable(sexData ? sexData.Sex : RequestCriteriaModels.SexTypes.NotSpecified);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.ISexTermData {
            var superdata = super.toData();

            var sexData: RequestCriteriaModels.ISexTermData = {
                TermType: superdata.TermType,
                Sex: this.Sex()
            };

            //console.log('Sex: ' + JSON.stringify(sexData));

            return sexData;
        }

        public static SexesList: Dns.KeyValuePairData<string, RequestCriteriaModels.SexTypes>[] = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.SexTypes.NotSpecified),
            new Dns.KeyValuePairData('Male', RequestCriteriaModels.SexTypes.Male),
            new Dns.KeyValuePairData('Female', RequestCriteriaModels.SexTypes.Female),
            new Dns.KeyValuePairData('Both', RequestCriteriaModels.SexTypes.Both),
            new Dns.KeyValuePairData('Both Aggregated', RequestCriteriaModels.SexTypes.Aggregated)
        ];
    }

    export class WorkplanTypeTerm extends RequestCriteriaViewModels.Term {
        public WorkplanType: KnockoutObservable<string>;

        constructor(workplanTypeData?: RequestCriteriaModels.IWorkplanTypeTermData) {
            super(RequestCriteriaModels.TermTypes.WorkplanTypeTerm);

            this.WorkplanType = ko.observable(workplanTypeData == undefined ? "00000000-0000-0000-0000-000000000000" : workplanTypeData.WorkplanTypeID);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IWorkplanTypeTermData {
            var superdata = super.toData();

            var workplanTypeData: RequestCriteriaModels.IWorkplanTypeTermData = {
                TermType: superdata.TermType,
                WorkplanTypeID: this.WorkplanType()
            };

            return workplanTypeData;
        }

    }





}