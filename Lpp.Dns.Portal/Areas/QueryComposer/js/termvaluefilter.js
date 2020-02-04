var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var MDQ;
            (function (MDQ) {
                var Terms = /** @class */ (function () {
                    function Terms() {
                    }
                    Object.defineProperty(Terms, "DataCheckerQueryTypeID", {
                        get: function () {
                            return '1F065B02-5BF3-4340-A412-84465C9B164C';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "AgeRangeID", {
                        get: function () {
                            return 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "DrugClassID", {
                        get: function () {
                            return '75290001-0E78-490C-9635-A3CA01550704';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "DrugNameID", {
                        get: function () {
                            return '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "HCPCSProcedureCodesID", {
                        get: function () {
                            return '096A0001-73B4-405D-B45F-A3CA014C6E7D';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ICD9Diagnosis3digitID", {
                        get: function () {
                            return '5E5020DC-C0E4-487F-ADF2-45431C2B7695';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ICD9Diagnosis4digitID", {
                        get: function () {
                            return 'D0800001-2810-48ED-96B9-A3D40146BAAE';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ICD9Diagnosis5digitID", {
                        get: function () {
                            return '80750001-6C3B-4C2D-90EC-A3D40146C26D';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ICD9Procedure3digitID", {
                        get: function () {
                            return 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ICD9Procedure4digitID", {
                        get: function () {
                            return '9E870001-1D48-4AA3-8889-A3D40146CCB3';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ZipCodeID", {
                        get: function () {
                            return '8B5FAA77-4A4B-4AC7-B817-69F1297E24C5';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "CombinedDiagnosisCodesID", {
                        get: function () {
                            return '86110001-4BAB-4183-B0EA-A4BC0125A6A7';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ESPCombinedDiagnosisCodesID", {
                        get: function () {
                            return 'A21E9775-39A4-4ECC-848B-1DC881E13689';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "MetadataRefreshID", {
                        get: function () {
                            return '421BAC16-CAAC-4918-8760-A10FF76CC87B';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ConditionID", {
                        get: function () {
                            return 'EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "TrialID", {
                        get: function () {
                            return '56A38F6D-993A-4953-BCB9-11BDD809C0B4';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "PatientReportedOutcomeID", {
                        get: function () {
                            return 'AE87C785-BB74-4708-B0CD-FA91D584615C';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "PatientReportedOutcomeEncounterID", {
                        get: function () {
                            return 'A11DCC97-4C8D-4B80-AE61-58ECB2F66C3D';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "VisitsID", {
                        get: function () {
                            return 'F01BE0A4-7D8E-4288-AE33-C65166AF8335';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "SexID", {
                        get: function () {
                            return '71B4545C-345B-48B2-AF5E-F84DC18E4E1A';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "CodeMetricID", {
                        get: function () {
                            return 'E39D0001-07A1-4DFD-9299-A3CB00F2474B';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "CoverageID", {
                        get: function () {
                            return 'DC880001-23B2-4F36-94E2-A3CB00DA8248';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "CriteriaID", {
                        get: function () {
                            return '17540001-8185-41BB-BE64-A3CB00F27EC2';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "DispensingMetricID", {
                        get: function () {
                            return '16ED0001-7E2D-4B27-B943-A3CB0162C262';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "EthnicityID", {
                        get: function () {
                            return '702CE918-9A59-4082-A8C7-A9234536FE79';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "FacilityID", {
                        get: function () {
                            return 'A257DA68-9557-4D6A-AEBD-541AA9BDD145';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "HeightID", {
                        get: function () {
                            return '8BC627CA-5729-4E7A-9702-0000A45A0018';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "HispanicID", {
                        get: function () {
                            return 'D26FE166-49A2-47F8-87E2-4F42A945D4D5';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ObservationPeriodID", {
                        get: function () {
                            return '98A78326-35D2-461A-B858-5B69E0FED28A';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "QuarterYearID", {
                        get: function () {
                            return 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "RaceID", {
                        get: function () {
                            return '834F0001-FA03-4ECD-BE28-A3CD00EC02E2';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "SettingID", {
                        get: function () {
                            return '2DE50001-7882-4EDE-AC4F-A3CB00D9051A';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "TobaccoUseID", {
                        get: function () {
                            return '342C354B-9ECC-479B-BE61-1770E4B44675';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "WeightID", {
                        get: function () {
                            return '3B0ED310-DDE9-4836-9857-0000A45A0018';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "YearID", {
                        get: function () {
                            return '781A0001-1FF0-41AB-9E67-A3CB014C37F2';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "VitalsMeasureDateID", {
                        get: function () {
                            return 'F9920001-AEB1-425C-A929-A4BB01515850';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(Terms, "ProcedureCodesID", {
                        get: function () {
                            return 'F81AE5DE-7B35-4D7A-B398-A72200CE7419';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Terms.Compare = function (a, b) {
                        if ((a == null && b != null) || (a != null && b == null))
                            return false;
                        if (a == null && b == null)
                            return true;
                        return a.toLowerCase() === b.toLowerCase();
                    };
                    return Terms;
                }());
                MDQ.Terms = Terms;
                var TermValueFilter = /** @class */ (function () {
                    function TermValueFilter(models) {
                        var _this = this;
                        this._models = models || [];
                        this._containsPCORnet = ko.pureComputed(function () { return TermValueFilter.ContainsModel(_this._models, TermValueFilter.PCORnetModelID); });
                        this._containsSummaryTables = ko.pureComputed(function () { return TermValueFilter.ContainsModel(_this._models, TermValueFilter.SummaryTablesModelID); });
                        this._containsESP = ko.pureComputed(function () { return TermValueFilter.ContainsModel(_this._models, TermValueFilter.ESPModelID); });
                        this._containsDataChecker = ko.pureComputed(function () { return TermValueFilter.ContainsModel(_this._models, TermValueFilter.DataCheckerModelID); });
                        this._containsModularProgram = ko.pureComputed(function () { return TermValueFilter.ContainsModel(_this._models, TermValueFilter.ModularProgramModelID); });
                    }
                    Object.defineProperty(TermValueFilter, "PCORnetModelID", {
                        get: function () {
                            return '85ee982e-f017-4bc4-9acd-ee6ee55d2446';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(TermValueFilter, "SummaryTablesModelID", {
                        get: function () {
                            return 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(TermValueFilter, "ESPModelID", {
                        get: function () {
                            return '7c69584a-5602-4fc0-9f3f-a27f329b1113';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(TermValueFilter, "DataCheckerModelID", {
                        get: function () {
                            return '321adaa1-a350-4dd0-93de-5de658a507df';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    Object.defineProperty(TermValueFilter, "ModularProgramModelID", {
                        get: function () {
                            return '1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154';
                        },
                        enumerable: true,
                        configurable: true
                    });
                    TermValueFilter.ContainsModel = function (models, id) {
                        return ko.utils.arrayFirst(models, function (i) { return i.toLowerCase() == id; }) != null;
                    };
                    TermValueFilter.GetTranslations = function (translations, values) {
                        var selected = [];
                        for (var i = 0; i < translations.length; i++) {
                            var item = translations[i];
                            if (values.indexOf(item.value) >= 0) {
                                selected.push(item);
                            }
                            if (selected.length == values.length)
                                break;
                        }
                        return selected;
                    };
                    /*Indicates if the models set for the helper contain the specified model.*/
                    TermValueFilter.prototype.HasModel = function (modelID) {
                        return TermValueFilter.ContainsModel(this._models, modelID);
                    };
                    /* Returns the valid values based on the models specified in initiation */
                    TermValueFilter.prototype.SexValues = function () {
                        var sexTranslations = [];
                        //Dont include MaleAndFemaleAggregated in the list of Sex Values
                        Dns.Enums.SexStratificationsTranslation.forEach(function (s) {
                            if (s.value != Dns.Enums.SexStratifications.MaleAndFemaleAggregated)
                                sexTranslations.push(s);
                        });
                        if (this._models.length == 0)
                            return sexTranslations;
                        if (this._containsSummaryTables() == true)
                            return TermValueFilter.GetTranslations(Dns.Enums.SexStratificationsTranslation, [Dns.Enums.SexStratifications.FemaleOnly, Dns.Enums.SexStratifications.MaleOnly, Dns.Enums.SexStratifications.MaleAndFemale, Dns.Enums.SexStratifications.MaleAndFemaleAggregated, Dns.Enums.SexStratifications.Unknown]);
                        if (this._containsPCORnet() == false) {
                            //models collection does not contain PCORnet, return subset of values. They are the same for Summary Tables and ESP
                            return TermValueFilter.GetTranslations(Dns.Enums.SexStratificationsTranslation, [Dns.Enums.SexStratifications.FemaleOnly, Dns.Enums.SexStratifications.MaleOnly, Dns.Enums.SexStratifications.MaleAndFemale]);
                        }
                        return sexTranslations;
                    };
                    /* Returns the valid values based on the models specified in initiation */
                    TermValueFilter.prototype.SettingsValues = function () {
                        if (this._models.length == 0)
                            return Dns.Enums.SettingsTranslation;
                        var translations = [];
                        if (this._containsSummaryTables() || this._containsPCORnet()) {
                            TermValueFilter.GetTranslations(Dns.Enums.SettingsTranslation, [Dns.Enums.Settings.IP, Dns.Enums.Settings.AV, Dns.Enums.Settings.ED]).forEach(function (i) { return translations.push(i); });
                        }
                        if (this._containsSummaryTables()) {
                            TermValueFilter.GetTranslations(Dns.Enums.SettingsTranslation, [Dns.Enums.Settings.AN]).forEach(function (i) { return translations.push(i); });
                        }
                        if (this._containsPCORnet()) {
                            TermValueFilter.GetTranslations(Dns.Enums.SettingsTranslation, [Dns.Enums.Settings.EI, Dns.Enums.Settings.IS, Dns.Enums.Settings.OS, Dns.Enums.Settings.IC, Dns.Enums.Settings.OA, Dns.Enums.Settings.NI, Dns.Enums.Settings.UN, Dns.Enums.Settings.OT]).forEach(function (i) { return translations.push(i); });
                        }
                        return translations;
                    };
                    /* Returns the valid values based on the models specified in initiation */
                    TermValueFilter.prototype.RaceValues = function () {
                        if (this._models.length == 0 || this._containsPCORnet())
                            return Dns.Enums.RaceTranslation;
                        if (this._containsSummaryTables()) {
                            return TermValueFilter.GetTranslations(Dns.Enums.RaceTranslation, [Dns.Enums.Race.Native, Dns.Enums.Race.Asian, Dns.Enums.Race.Black, Dns.Enums.Race.Pacific, Dns.Enums.Race.White, Dns.Enums.Race.Unknown]);
                        }
                        return [];
                    };
                    /* Returns the valid values based on the models specified in initiation */
                    TermValueFilter.prototype.RaceEthnicityValues = function () {
                        if (this._models.length == 0)
                            return Dns.Enums.EthnicitiesTranslation;
                        if (this._containsESP()) {
                            return TermValueFilter.GetTranslations(Dns.Enums.EthnicitiesTranslation, [Dns.Enums.Ethnicities.Unknown, Dns.Enums.Ethnicities.Native, Dns.Enums.Ethnicities.Asian, Dns.Enums.Ethnicities.Black, Dns.Enums.Ethnicities.White, Dns.Enums.Ethnicities.Hispanic]);
                        }
                        return [];
                    };
                    /* Returns the valid values based on the models specified in initiation */
                    TermValueFilter.prototype.AgeRangeStratifications = function () {
                        if (this._models.length == 0)
                            return Dns.Enums.AgeStratificationsTranslation;
                        var translations = [];
                        if (this._containsSummaryTables()) {
                            TermValueFilter.GetTranslations(Dns.Enums.AgeStratificationsTranslation, [Dns.Enums.AgeStratifications.Ten, Dns.Enums.AgeStratifications.Seven, Dns.Enums.AgeStratifications.Four, Dns.Enums.AgeStratifications.Two, Dns.Enums.AgeStratifications.None]).forEach(function (i) { return translations.push(i); });
                        }
                        if (this._containsESP() || this._containsPCORnet()) {
                            TermValueFilter.GetTranslations(Dns.Enums.AgeStratificationsTranslation, [Dns.Enums.AgeStratifications.FiveYearGrouping, Dns.Enums.AgeStratifications.TenYearGrouping]).forEach(function (i) { return translations.push(i); });
                        }
                        if (this._containsPCORnet()) {
                            TermValueFilter.GetTranslations(Dns.Enums.AgeStratificationsTranslation, [Dns.Enums.AgeStratifications.None]).forEach(function (i) { return translations.push(i); });
                        }
                        return translations;
                    };
                    /* Confirm all properties exist that have been addd to a template that may not exist on the request json*/
                    TermValueFilter.prototype.ConfirmTemplateProperties = function (request, termTemplates) {
                        var flattenedTerms = this.FlattenTermTemplates(termTemplates, []);
                        //loop through all the terms on all the criteria and make sure each term is up to date
                        for (var i = 0; i < request.Where.Criteria.length; i++) {
                            var criteria = request.Where.Criteria[i];
                            this.ConfirmCriteria(criteria, flattenedTerms);
                            if (criteria.Terms && criteria.Terms.length > 0) {
                                for (var j = 0; j < criteria.Terms.length; j++) {
                                    var term = criteria.Terms[i];
                                    var termTemplate = ko.utils.arrayFirst(flattenedTerms, function (template) { return template.TermID == term.Type; });
                                    this.ConfirmTermValues(criteria.Terms[j], termTemplate);
                                }
                            }
                        }
                    };
                    TermValueFilter.prototype.FlattenTermTemplates = function (termTemplates, flattened) {
                        for (var i = 0; i < termTemplates.length; i++) {
                            if (termTemplates[i].TermID) {
                                flattened.push(termTemplates[i]);
                            }
                            if (termTemplates[i].Terms) {
                                flattened = this.FlattenTermTemplates(termTemplates[i].Terms, flattened);
                            }
                        }
                        return flattened;
                    };
                    TermValueFilter.prototype.ConfirmCriteria = function (criteria, termTemplates) {
                        if (criteria.Terms) {
                            var term;
                            var termTemplate;
                            for (var i = 0; i < criteria.Terms.length; i++) {
                                term = criteria.Terms[i];
                                termTemplate = ko.utils.arrayFirst(termTemplates, function (template) { return template.TermID == term.Type; });
                                this.ConfirmTermValues(term, termTemplate);
                            }
                        }
                        if (criteria.Criteria && criteria.Criteria.length > 0) {
                            for (var j = 0; j < criteria.Criteria.length; j++) {
                                this.ConfirmCriteria(criteria.Criteria[j], termTemplates);
                            }
                        }
                    };
                    TermValueFilter.prototype.ConfirmTermValues = function (term, termTemplate) {
                        var termProperties = Object.keys(term.Values);
                        var tvalProperties = Object.keys(termTemplate.ValueTemplate);
                        var propertyName;
                        for (var i = 0; i < tvalProperties.length; i++) {
                            propertyName = tvalProperties[i];
                            if (termProperties.indexOf(propertyName) < 0) {
                                term.Values[propertyName] = termTemplate.ValueTemplate[propertyName];
                            }
                        }
                    };
                    return TermValueFilter;
                }());
                MDQ.TermValueFilter = TermValueFilter;
            })(MDQ = QueryBuilder.MDQ || (QueryBuilder.MDQ = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
