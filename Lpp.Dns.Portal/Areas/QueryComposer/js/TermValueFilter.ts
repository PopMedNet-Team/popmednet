module Plugins.Requests.QueryBuilder.MDQ {
    export class Terms {
        public static get DataCheckerQueryTypeID(): any {
            return '1F065B02-5BF3-4340-A412-84465C9B164C';
        }
        public static get AgeRangeID(): any {
            return 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C';
        }
        public static get DrugClassID(): any {
            return '75290001-0E78-490C-9635-A3CA01550704';
        }
        public static get DrugNameID(): any {
            return '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84';
        }
        public static get HCPCSProcedureCodesID(): any {
            return '096A0001-73B4-405D-B45F-A3CA014C6E7D';
        }
        public static get ICD9Diagnosis3digitID(): any {
            return '5E5020DC-C0E4-487F-ADF2-45431C2B7695';
        }
        public static get ICD9Diagnosis4digitID(): any {
            return 'D0800001-2810-48ED-96B9-A3D40146BAAE';
        }
        public static get ICD9Diagnosis5digitID(): any {
            return '80750001-6C3B-4C2D-90EC-A3D40146C26D';
        }
        public static get ICD9Procedure3digitID(): any {
            return 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94';
        }
        public static get ICD9Procedure4digitID(): any {
            return '9E870001-1D48-4AA3-8889-A3D40146CCB3';
        }
        public static get ZipCodeID(): any {
            return '8B5FAA77-4A4B-4AC7-B817-69F1297E24C5';
        }
        public static get CombinedDiagnosisCodesID(): any {
            return '86110001-4BAB-4183-B0EA-A4BC0125A6A7';
        }
        public static get ESPCombinedDiagnosisCodesID(): any {
            return 'A21E9775-39A4-4ECC-848B-1DC881E13689';
        }
        public static get ConditionID(): any {
            return 'EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF';
        }
        public static get VisitsID(): any {
            return 'F01BE0A4-7D8E-4288-AE33-C65166AF8335';
        }
        public static get SexID(): any {
            return '71B4545C-345B-48B2-AF5E-F84DC18E4E1A';
        }
        public static get CodeMetricID(): any {
            return 'E39D0001-07A1-4DFD-9299-A3CB00F2474B';
        }
        public static get CoverageID(): any {
            return 'DC880001-23B2-4F36-94E2-A3CB00DA8248';
        }
        public static get CriteriaID(): any {
            return '17540001-8185-41BB-BE64-A3CB00F27EC2';
        }
        public static get DispensingMetricID(): any {
            return '16ED0001-7E2D-4B27-B943-A3CB0162C262';
        }
        public static get EthnicityID(): any {
            return '702CE918-9A59-4082-A8C7-A9234536FE79';
        }
        public static get FacilityID(): any {
            return 'A257DA68-9557-4D6A-AEBD-541AA9BDD145';
        }
        public static get HeightID(): any {
            return '8BC627CA-5729-4E7A-9702-0000A45A0018';
        }
        public static get HispanicID(): any {
            return 'D26FE166-49A2-47F8-87E2-4F42A945D4D5';
        }
        public static get ObservationPeriodID(): any {
            return '98A78326-35D2-461A-B858-5B69E0FED28A';
        }
        public static get QuarterYearID(): any {
            return 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7';
        }
        public static get RaceID(): any {
            return '834F0001-FA03-4ECD-BE28-A3CD00EC02E2';
        }
        public static get SettingID(): any {
            return '2DE50001-7882-4EDE-AC4F-A3CB00D9051A';
        }
        public static get TobaccoUseID(): any {
            return '342C354B-9ECC-479B-BE61-1770E4B44675';
        }
        public static get WeightID(): any {
            return '3B0ED310-DDE9-4836-9857-0000A45A0018';
        }
        public static get YearID(): any {
            return '781A0001-1FF0-41AB-9E67-A3CB014C37F2';
        }
        public static get VitalsMeasureDateID(): any {
            return 'F9920001-AEB1-425C-A929-A4BB01515850';
        }

        public static get ProcedureCodesID(): any {
            return 'F81AE5DE-7B35-4D7A-B398-A72200CE7419';
        }


        public static Compare(a: any, b: any): boolean {
            if ((a == null && b != null) || (a != null && b == null))
                return false;

            if (a == null && b == null)
                return true;

            return a.toLowerCase() === b.toLowerCase();
        }
    }

    export class TermValueFilter {       

        public static get PCORnetModelID(): any {
            return '85ee982e-f017-4bc4-9acd-ee6ee55d2446';
        }
        public static get SummaryTablesModelID(): any {
            return 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb';
        }
        public static get ESPModelID(): any {
            return '7c69584a-5602-4fc0-9f3f-a27f329b1113';
        }
        public static get DataCheckerModelID(): any {
            return '321adaa1-a350-4dd0-93de-5de658a507df';
        }
        public static get ModularProgramModelID(): any {
            return '1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154';
        }

        public static ContainsModel(models: any[], id: any): boolean {
            return ko.utils.arrayFirst(models, (i) => i.toLowerCase() == id) != null;
        }

        private _models: any[];
        private _containsPCORnet: KnockoutComputed<boolean>;
        private _containsSummaryTables: KnockoutComputed<boolean>;
        private _containsESP: KnockoutComputed<boolean>;
        private _containsDataChecker: KnockoutComputed<boolean>;
        private _containsModularProgram: KnockoutComputed<boolean>;

        constructor(models: any[]) {
            this._models = models || [];

            this._containsPCORnet = ko.pureComputed(() => TermValueFilter.ContainsModel(this._models, TermValueFilter.PCORnetModelID));
            this._containsSummaryTables = ko.pureComputed(() => TermValueFilter.ContainsModel(this._models, TermValueFilter.SummaryTablesModelID));
            this._containsESP = ko.pureComputed(() => TermValueFilter.ContainsModel(this._models, TermValueFilter.ESPModelID));
            this._containsDataChecker = ko.pureComputed(() => TermValueFilter.ContainsModel(this._models, TermValueFilter.DataCheckerModelID));
            this._containsModularProgram = ko.pureComputed(() => TermValueFilter.ContainsModel(this._models, TermValueFilter.ModularProgramModelID));
        }

        static GetTranslations<T>(translations: Dns.Structures.KeyValuePair[], values: T[]): Dns.Structures.KeyValuePair[] {
            var selected: Dns.Structures.KeyValuePair[] = [];
            
            for (var i = 0; i < translations.length; i++) {
                var item = translations[i];
                if (values.indexOf(item.value) >= 0) {
                    selected.push(item);
                }
                if (selected.length == values.length)
                    break;
            }

            return selected;
        }

        /*Indicates if the models set for the helper contain the specified model.*/
        public HasModel(modelID: any): boolean {
            return TermValueFilter.ContainsModel(this._models, modelID);
        }

        /* Returns the valid values based on the models specified in initiation */
        public SexValues(): Dns.Structures.KeyValuePair[] {
            var sexTranslations = [];
            //Dont include MaleAndFemaleAggregated in the list of Sex Values
            Dns.Enums.SexStratificationsTranslation.forEach((s) => {
                if (s.value != Dns.Enums.SexStratifications.MaleAndFemaleAggregated)
                    sexTranslations.push(s);
            });

            if (this._models.length == 0)
                return sexTranslations;

            if (this._containsSummaryTables() == true)
                return TermValueFilter.GetTranslations<Dns.Enums.SexStratifications>(Dns.Enums.SexStratificationsTranslation, [Dns.Enums.SexStratifications.FemaleOnly, Dns.Enums.SexStratifications.MaleOnly, Dns.Enums.SexStratifications.MaleAndFemale, Dns.Enums.SexStratifications.MaleAndFemaleAggregated, Dns.Enums.SexStratifications.Unknown]);

            if (this._containsPCORnet() == false) {
                //models collection does not contain PCORnet, return subset of values. They are the same for Summary Tables and ESP
                return TermValueFilter.GetTranslations<Dns.Enums.SexStratifications>(Dns.Enums.SexStratificationsTranslation, [Dns.Enums.SexStratifications.FemaleOnly, Dns.Enums.SexStratifications.MaleOnly, Dns.Enums.SexStratifications.MaleAndFemale]);
            }

            return sexTranslations;
        }

        /* Returns the valid values based on the models specified in initiation */
        public SettingsValues(): Dns.Structures.KeyValuePair[] {
            if (this._models.length == 0)
                return Dns.Enums.SettingsTranslation;

            var translations = [];
            if (this._containsSummaryTables() || this._containsPCORnet()) {
                TermValueFilter.GetTranslations<Dns.Enums.Settings>(Dns.Enums.SettingsTranslation, [Dns.Enums.Settings.IP, Dns.Enums.Settings.AV, Dns.Enums.Settings.ED]).forEach((i) => translations.push(i));
            }

            if (this._containsSummaryTables()) {
                TermValueFilter.GetTranslations<Dns.Enums.Settings>(Dns.Enums.SettingsTranslation, [Dns.Enums.Settings.AN]).forEach((i) => translations.push(i));
            }

            if (this._containsPCORnet()) {
                TermValueFilter.GetTranslations<Dns.Enums.Settings>(Dns.Enums.SettingsTranslation, [Dns.Enums.Settings.EI, Dns.Enums.Settings.IS, Dns.Enums.Settings.OS, Dns.Enums.Settings.IC, Dns.Enums.Settings.OA, Dns.Enums.Settings.NI, Dns.Enums.Settings.UN, Dns.Enums.Settings.OT]).forEach((i) => translations.push(i));
            }

            return translations;
        }

        /* Returns the valid values based on the models specified in initiation */
        public RaceValues(): Dns.Structures.KeyValuePair[] {
            if (this._models.length == 0 || this._containsPCORnet())
                return Dns.Enums.RaceTranslation;

            if (this._containsSummaryTables()) {
                return TermValueFilter.GetTranslations<Dns.Enums.Race>(Dns.Enums.RaceTranslation, [Dns.Enums.Race.Native, Dns.Enums.Race.Asian, Dns.Enums.Race.Black, Dns.Enums.Race.Pacific, Dns.Enums.Race.White, Dns.Enums.Race.Unknown]);
            }

            return [];           
        }

        /* Returns the valid values based on the models specified in initiation */
        public RaceEthnicityValues(): Dns.Structures.KeyValuePair[] {
            if (this._models.length == 0)
                return Dns.Enums.EthnicitiesTranslation;

            if (this._containsESP()) {
                return TermValueFilter.GetTranslations<Dns.Enums.Ethnicities>(Dns.Enums.EthnicitiesTranslation, [Dns.Enums.Ethnicities.Unknown, Dns.Enums.Ethnicities.Native, Dns.Enums.Ethnicities.Asian, Dns.Enums.Ethnicities.Black, Dns.Enums.Ethnicities.White, Dns.Enums.Ethnicities.Hispanic]);
            }

            return [];
        }

        /* Returns the valid values based on the models specified in initiation */
        public AgeRangeStratifications(): Dns.Structures.KeyValuePair[] {
            
            if (this._models.length == 0)
                return Dns.Enums.AgeStratificationsTranslation;

            var translations = [];
            if (this._containsSummaryTables()) {
                TermValueFilter.GetTranslations<Dns.Enums.AgeStratifications>(Dns.Enums.AgeStratificationsTranslation, [Dns.Enums.AgeStratifications.Ten, Dns.Enums.AgeStratifications.Seven, Dns.Enums.AgeStratifications.Four, Dns.Enums.AgeStratifications.Two, Dns.Enums.AgeStratifications.None]).forEach((i) => translations.push(i));
            }

            if (this._containsESP() || this._containsPCORnet()) {
                TermValueFilter.GetTranslations<Dns.Enums.AgeStratifications>(Dns.Enums.AgeStratificationsTranslation, [Dns.Enums.AgeStratifications.FiveYearGrouping, Dns.Enums.AgeStratifications.TenYearGrouping]).forEach((i) => translations.push(i));
            }
            
            if (this._containsPCORnet()) {
                TermValueFilter.GetTranslations<Dns.Enums.AgeStratifications>(Dns.Enums.AgeStratificationsTranslation, [Dns.Enums.AgeStratifications.None]).forEach((i) => translations.push(i));
            }

            return translations;
        }

        /* Confirm all properties exist that have been addd to a template that may not exist on the request json*/
        public ConfirmTemplateProperties(request: Dns.Interfaces.IQueryComposerRequestDTO, termTemplates: IVisualTerm[]) {
            var flattenedTerms = this.FlattenTermTemplates(termTemplates, []);
            //loop through all the terms on all the criteria and make sure each term is up to date
            for (var i = 0; i < request.Where.Criteria.length; i++) {
                var criteria = request.Where.Criteria[i];
                this.ConfirmCriteria(criteria, flattenedTerms);

                if (criteria.Terms && criteria.Terms.length > 0) {
                    for (var j = 0; j < criteria.Terms.length; j++) {
                        var term = criteria.Terms[i];
                        var termTemplate = ko.utils.arrayFirst(flattenedTerms, (template: IVisualTerm) => { return template.TermID == term.Type; });
                        this.ConfirmTermValues(criteria.Terms[j], termTemplate);
                    }
                }

            }
        }

        private FlattenTermTemplates(termTemplates: IVisualTerm[], flattened: IVisualTerm[]) {
            for (var i = 0; i < termTemplates.length; i++) {

                if (termTemplates[i].TermID) {
                    flattened.push(termTemplates[i]);
                }

                if (termTemplates[i].Terms) {
                    flattened = this.FlattenTermTemplates(termTemplates[i].Terms, flattened);
                }
            }
            return flattened;
        }

        private ConfirmCriteria(criteria: Dns.Interfaces.IQueryComposerCriteriaDTO, termTemplates: IVisualTerm[]) {
            if (criteria.Terms) {
                var term;
                var termTemplate;
                for (var i = 0; i < criteria.Terms.length; i++) {
                    term = criteria.Terms[i];
                    termTemplate = ko.utils.arrayFirst(termTemplates, (template: IVisualTerm) => { return template.TermID == term.Type; });
                    this.ConfirmTermValues(term, termTemplate);
                }
            }

            if (criteria.Criteria && criteria.Criteria.length > 0) {
                for (var j = 0; j < criteria.Criteria.length; j++) {
                    this.ConfirmCriteria(criteria.Criteria[j], termTemplates);
                }
            }
        }

        private ConfirmTermValues(term: Dns.Interfaces.IQueryComposerTermDTO, termTemplate: IVisualTerm) {
            var termProperties: string[] = Object.keys(term.Values);
            var tvalProperties: string[] = Object.keys(termTemplate.ValueTemplate);
            var propertyName;
            for (var i = 0; i < tvalProperties.length; i++) {
                propertyName = tvalProperties[i];
                if (termProperties.indexOf(propertyName) < 0) {                    
                    term.Values[propertyName] = termTemplate.ValueTemplate[propertyName];
                }
            }

        }

    }

}