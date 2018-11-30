interface KnockoutBindingHandlers {
    dateRangeTermValidator: KnockoutBindingHandler;
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
}