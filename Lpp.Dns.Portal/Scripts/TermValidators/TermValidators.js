var PMNTermValidators;
(function (PMNTermValidators) {
    ko.bindingHandlers.dateRangeTermValidator = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var startDate = valueAccessor().values.Start;
            var endDate = valueAccessor().values.End;
            var isValid = ko.observable(true);
            var uniqueID = Constants.Guid.newGuid();
            var errorMessage = "<br><small>Please enter a properly formatted (mm/dd/yyyy) start or end date range</small>";
            var jqElement = $(element);
            jqElement.attr('id', uniqueID);
            var validate = function () {
                if (startDate() == null && endDate() == null) {
                    errorMessage = "<br><small>Please enter a properly formatted (mm/dd/yyyy) start or end date range</small>";
                    isValid(false);
                }
                else if ((startDate() != null || endDate() != null) && (startDate() <= endDate()) || endDate() == undefined) {
                    isValid(true);
                }
                else {
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
            isValid.subscribe(function (newValue) {
                if (newValue) {
                    jqElement.children().remove();
                }
                else {
                    jqElement.children().remove();
                    jqElement.append(errorMessage);
                }
            });
            valueAccessor().termValidators[uniqueID] = function () {
                if ($("#" + uniqueID).length == 0) {
                    delete valueAccessor().termValidators[uniqueID];
                    return true;
                }
                else {
                    validate();
                    return isValid();
                }
            };
            jqElement.parent().siblings(".col-delete").find("button").click(function (e) {
                delete valueAccessor().termValidators[uniqueID];
            });
        }
    };
    ko.bindingHandlers.patientReportedOutcomeEncounterTermValidator = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var daysBefore = valueAccessor().values.DaysBefore;
            var daysAfter = valueAccessor().values.DaysAfter;
            var isValid = ko.observable(true);
            var uniqueID = Constants.Guid.newGuid();
            var errorMessage = "<span role='alert' class='k-widget k-tooltip k-tooltip-validation k-invalid-msg'><span class='k-icon k-i-warning'></span><small>Both Days Before and Days After must contain a value of 0 or above</small></div>";
            var jqElement = $(element);
            jqElement.attr('id', uniqueID);
            var validate = function () {
                if (daysBefore() == null || daysAfter() == null) {
                    isValid(false);
                }
                else if (parseInt(daysBefore()) <= -1 || parseInt(daysAfter()) <= -1) {
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
            isValid.subscribe(function (newValue) {
                if (newValue) {
                    if (jqElement.hasClass("k-invalid"))
                        jqElement.removeClass("k-invalid");
                    jqElement.children().remove();
                }
                else {
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
                }
                else {
                    validate();
                    return isValid();
                }
            };
            jqElement.parent().siblings(".col-delete").find("button").click(function (e) {
                delete valueAccessor().termValidators[uniqueID];
            });
        }
    };
    ko.bindingHandlers.patientReportedOutcomeTermValidator = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var itemName = valueAccessor().values.ItemName;
            var itemResponse = valueAccessor().values.ItemResponse;
            var isValid = ko.observable(true);
            var uniqueID = Constants.Guid.newGuid();
            var errorMessage = "<span role='alert' class='k-widget k-tooltip k-tooltip-validation k-invalid-msg'><span class='k-icon k-i-warning'></span><small>Item Name or Item Response must contain a value</small></div>";
            var jqElement = $(element);
            jqElement.attr('id', uniqueID);
            var validate = function () {
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
            isValid.subscribe(function (newValue) {
                if (newValue) {
                    if (jqElement.hasClass("k-invalid"))
                        jqElement.removeClass("k-invalid");
                    jqElement.children().remove();
                }
                else {
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
                }
                else {
                    validate();
                    return isValid();
                }
            };
            jqElement.parent().siblings(".col-delete").find("button").click(function (e) {
                delete valueAccessor().termValidators[uniqueID];
            });
        }
    };
})(PMNTermValidators || (PMNTermValidators = {}));
