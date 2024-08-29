/// <reference path="../typings/bootstrap.d.ts" />

//Adds href binding instead of using attr: {href: ...}
(<any>ko.bindingHandlers).href = {
    update: function (element, valueAccessor) {
        (<any>ko.bindingHandlers.attr).update(element, function () {
            return { href: valueAccessor() }
        });
    }
};

//Adds src binding instead of using attr: {src: ...}
(<any>ko.bindingHandlers).src = {
    update: function (element, valueAccessor) {
        (<any>ko.bindingHandlers.attr).update(element, function () {
            return { src: valueAccessor() }
        });
    }
};

//Adds reciprical hidden binding that is the opposite of visible
(<any>ko.bindingHandlers).hidden = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        (<any>ko.bindingHandlers.visible).update(element, function () { return !value; });
    }
};

//Implements value change on keypress instead of blur
(<any>ko.bindingHandlers).instantValue = {
    init: function (element, valueAccessor, allBindings) {
        var newAllBindings = function () {
            // for backwards compatibility w/ knockout  < 3.0
            return ko.utils.extend(allBindings(), { valueUpdate: 'afterkeydown' });
        };
        (<any>newAllBindings).get = function (a) {
            return a === 'valueupdate' ? 'afterkeydown' : allBindings.get(a);
        };
        (<any>newAllBindings).has = function (a) {
            return a === 'valueupdate' || allBindings.has(a);
        };
        (<any>ko.bindingHandlers.value).init(element, valueAccessor, newAllBindings);
    },
    update: ko.bindingHandlers.value.update
};

//Toogles a value based on a click. Highly useful for expand/contracts etc.
(<any>ko.bindingHandlers).toggle = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        (<any>ko).applyBindingsToNode(element, {
            click: function () {
                value(!value());
            }
        });
    }
};

//Currency formatter
(<any>ko.bindingHandlers).currency = {
    symbol: ko.observable('$'),
    update: function (element, valueAccessor, allBindingsAccessor) {
        return (<any>ko.bindingHandlers.text).update(element, function () {
            var value = +(ko.utils.unwrapObservable(valueAccessor()) || 0),
                symbol = ko.utils.unwrapObservable(allBindingsAccessor().symbol === undefined
                    ? allBindingsAccessor().symbol
                    : (<any>ko.bindingHandlers).currency.symbol);
            return symbol + value.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
        });
    }
};

//Prevents binding a specific section so that the view model won't bind. This allows binding separately to a different view model. Use <!-- ko stopBinding: true --> and <!-- /ko --> to wrap HTML elements you don't want to be bound.
(<any>ko.bindingHandlers).stopBinding = {
    init: function () {
        return { controlsDescendantBindings: true };
    }
};
(<any>ko.virtualElements.allowedBindings).stopBinding = true;

(<any>ko.bindingHandlers).date = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        ko.utils.registerEventHandler(element, "change", function () {
            var value = valueAccessor();

            if (element.value !== null && element.value !== undefined && element.value.length > 0) {
                value(element.value);
            }
            else {
                value("");
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var pattern = allBindings.format || 'MM/dd/yyyy';
        var output = valueUnwrapped != null && valueUnwrapped != undefined ? (<any>valueUnwrapped).toString(pattern) : null;
        
        if ($(element).is("input") === true) {
            $(element).val(output);
        } else {
            $(element).text(output);
        }
    }
};

(<any>ko.bindingHandlers).fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.unwrap(value) ? $(element).fadeIn() : $(element).fadeOut();
    }
};

// Allows a checkbox to be used as tri-state.
(<any>ko.bindingHandlers).indeterminateValue = {
    update: (element, valueAccessor) => {
        let value = ko.utils.unwrapObservable(valueAccessor());
        element.indeterminate = value;
    }
};