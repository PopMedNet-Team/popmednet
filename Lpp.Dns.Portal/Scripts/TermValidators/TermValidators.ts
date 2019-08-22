
interface KnockoutBindingHandlers {
    dateRangeTermValidator: KnockoutBindingHandler;
    patientReportedOutcomeEncounterTermValidator: KnockoutBindingHandler;
    patientReportedOutcomeTermValidator: KnockoutBindingHandler;
}

module PMNTermValidators {
    ko.bindingHandlers.dateRangeTermValidator = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var startDate: KnockoutObservable<any> = valueAccessor().values.Start;
            var endDate: KnockoutObservable<any> = valueAccessor().values.End;
            var isValid: KnockoutObservable<boolean> = ko.observable<boolean>(true);
            var uniqueID: string = Constants.Guid.newGuid();

            var errorMessage: string = "<br><small>Please enter a properly formatted (mm/dd/yyyy) start or end date range</small>";
            var jqElement: JQuery = $(element);
           
            jqElement.attr('id', uniqueID);
            var validate: Function = function () {
                if (startDate() == null && endDate() == null) {
                    errorMessage = "<br><small>Please enter a properly formatted (mm/dd/yyyy) start or end date range</small>";
                    isValid(false);
                }
                else if ((startDate() != null || endDate() != null) && (startDate() <= endDate()) || endDate() == undefined) {
                    isValid(true);
                } else {
                    if (endDate() != null && startDate() != null && endDate() < startDate()) {
                        errorMessage = "<br><small>The end date can't be before the start date</small><span class='glyphicon glyphicon-remove error-icon'></span>";
                    }
                    else {
                        errorMessage = "<br><small>Please enter a properly formatted (mm/dd/yyyy) start or end date range</small>";
                    }
                    isValid(false);
                }
            };

            startDate.subscribe(function (newValue) {
                validate();
            });

            endDate.subscribe(function (newValue) {
                validate();
            });

            isValid.subscribe(function (newValue: boolean) {
                if (newValue) {
                    jqElement.children().remove();
                } else {
                    jqElement.children().remove();
                    jqElement.append(errorMessage);
                }
            });

            valueAccessor().termValidators[uniqueID] = function () {
                if ($("#" + uniqueID).length == 0) {
                    delete valueAccessor().termValidators[uniqueID];
                    return true;
                } else {
                    validate();
                    return isValid();
                }
            };

            jqElement.parent().siblings(".col-delete").find("button").click(function (e) {
                delete valueAccessor().termValidators[uniqueID];
            });
        }
    }

    ko.bindingHandlers.patientReportedOutcomeEncounterTermValidator = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var daysBefore: KnockoutObservable<any> = valueAccessor().values.DaysBefore;
            var daysAfter: KnockoutObservable<any> = valueAccessor().values.DaysAfter;
            var isValid: KnockoutObservable<boolean> = ko.observable<boolean>(true);
            var uniqueID: string = Constants.Guid.newGuid();

            var errorMessage: string = "<span role='alert' class='k-widget k-tooltip k-tooltip-validation k-invalid-msg'><span class='k-icon k-i-warning'></span><small>Both Days Before and Days After must contain a value of 0 or above</small></div>";
            var jqElement: JQuery = $(element);

            jqElement.attr('id', uniqueID);
            var validate: Function = function () {
                if (daysBefore() == null || daysAfter() == null) {
                    isValid(false);
                }
                else if (parseInt(daysBefore()) <= -1 || parseInt(daysAfter()) <= -1) {
                    isValid(false);
                }
                else if (parseInt(daysBefore()) === 0 && parseInt(daysAfter()) === 0) {
                    errorMessage = "<span role='alert' class='k-widget k-tooltip k-tooltip-validation k-invalid-msg'><span class='k-icon k-i-warning'></span><small>Days Before or Days After must have a value greater than 0</small></div>";
                    isValid(false);
                }
                else {
                    isValid(true);
                }
            };

            daysBefore.subscribe(function (newValue) {
                validate();
            });

            daysAfter.subscribe(function (newValue) {
                validate();
            });

            isValid.subscribe(function (newValue: boolean) {
                if (newValue) {
                    if (jqElement.hasClass("k-invalid"))
                        jqElement.removeClass("k-invalid");
                    jqElement.children().remove();                    
                } else {
                    jqElement.children().remove();
                    if (!jqElement.hasClass("k-invalid"))
                        jqElement.addClass("k-invalid");
                    jqElement.append(errorMessage);
                }
            });

            valueAccessor().termValidators[uniqueID] = function () {
                if ($("#" + uniqueID).length == 0) {
                    delete valueAccessor().termValidators[uniqueID];
                    return true;
                } else {
                    validate();
                    return isValid();
                }
            };

            jqElement.parent().siblings(".col-delete").find("button").click(function (e) {
                delete valueAccessor().termValidators[uniqueID];
            });
        }
    }

    ko.bindingHandlers.patientReportedOutcomeTermValidator = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var itemName: KnockoutObservable<any> = valueAccessor().values.ItemName;
            var itemResponse: KnockoutObservable<any> = valueAccessor().values.ItemResponse;
            var isValid: KnockoutObservable<boolean> = ko.observable<boolean>(true);
            var uniqueID: string = Constants.Guid.newGuid();

            var errorMessage: string = "<span role='alert' class='k-widget k-tooltip k-tooltip-validation k-invalid-msg'><span class='k-icon k-i-warning'></span><small>Item Name or Item Response must contain a value</small></div>";
            var jqElement: JQuery = $(element);

            jqElement.attr('id', uniqueID);
            var validate: Function = function () {
                if (itemName() == null && itemResponse() == null) {
                    isValid(false);
                }
                else {
                    isValid(true);
                }
            };

            itemName.subscribe(function (newValue) {
                validate();
            });

            itemResponse.subscribe(function (newValue) {
                validate();
            });

            isValid.subscribe(function (newValue: boolean) {
                if (newValue) {
                    if (jqElement.hasClass("k-invalid"))
                        jqElement.removeClass("k-invalid");
                    jqElement.children().remove();
                } else {
                    jqElement.children().remove();
                    if (!jqElement.hasClass("k-invalid"))
                        jqElement.addClass("k-invalid");
                    jqElement.append(errorMessage);
                }
            });

            valueAccessor().termValidators[uniqueID] = function () {
                if ($("#" + uniqueID).length == 0) {
                    delete valueAccessor().termValidators[uniqueID];
                    return true;
                } else {
                    validate();
                    return isValid();
                }
            };

            jqElement.parent().siblings(".col-delete").find("button").click(function (e) {
                delete valueAccessor().termValidators[uniqueID];
            });
        }
    }
}