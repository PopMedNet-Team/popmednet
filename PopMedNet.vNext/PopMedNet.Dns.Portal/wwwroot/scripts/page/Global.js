import * as Constants from './constants.js';
export class PageViewModel {
    ScreenPermissions;
    HasPermission;
    title;
    Title;
    ValidationMessage;
    Validator;
    _BindingControl;
    /**
     *
     * @param bindingControl The control the page viewmodel is bound to.
     * @param screenPermissions The permissions for controlling the UI the user has.
     * @param validationContainer The container to use for validation other than the root binding container. If not specified the 'bindingControl' element is used.
     */
    constructor(bindingControl, screenPermissions = null, validationContainer = null) {
        this._BindingControl = bindingControl;
        var self = this;
        this.ScreenPermissions = screenPermissions == null ? null : screenPermissions.map((sp) => {
            return sp.toLowerCase();
        });
        this.ValidationMessage = ko.observable("");
        this.Validator = (validationContainer || bindingControl).kendoValidator({
            validate: () => {
                var errors = "";
                this.Validator.errors().forEach((item) => {
                    errors += "<li>" + item + "</li>\r\n";
                });
                if (errors.length > 0)
                    errors = "<ul style=\"margin-left: 50px;\">\r\n" + errors + "</ul>";
                this.ValidationMessage(errors);
            }
        }).data("kendoValidator");
        this.title = ko.observable("");
        this.Title = ko.computed({
            read: () => {
                return this.title();
            },
            write: (value) => {
                this.title(value);
                document.title = value;
            }
        });
        this.HasPermission = (permissionID) => {
            if (self.ScreenPermissions == null)
                throw 'Screen Permissions were not passed';
            return ko.utils.arrayFirst(self.ScreenPermissions, (item) => {
                return item.toLowerCase() == permissionID.toLowerCase();
            }) != null;
        };
    }
    Validate() {
        return this.Validator.validate();
    }
    WatchTitle(observable, prefix) {
        observable.subscribe((value) => {
            document.title = prefix + (!value ? "New" : value);
        });
        observable.notifySubscribers(observable());
    }
}
export class WorkflowActivityViewModel {
    ScreenPermissions;
    HasPermission;
    _BindingControl;
    constructor(bindingControl, screenPermissions = null) {
        var self = this;
        this._BindingControl = bindingControl;
        this.ScreenPermissions = screenPermissions;
        this.HasPermission = (permissionID) => {
            if (self.ScreenPermissions == null)
                throw 'Screen Permissions were not passed';
            return ko.utils.arrayFirst(self.ScreenPermissions, (item) => {
                return item.toLowerCase() == permissionID.toLowerCase();
            }) != null;
        };
    }
    Complete(data, event) {
        let el = $(event.target);
        var resultID = data.GetResultID(el);
        this.PostComplete(resultID);
    }
    GetResultID(ctl) {
        var resultID = ctl.attr("data-ResultID");
        return resultID;
    }
    PostComplete(resultID) {
        throw "Not implemented";
    }
}
export class DialogViewModel extends PageViewModel {
    lastDocumentHeight = -1;
    Parameters;
    constructor(bindingControl) {
        super(bindingControl);
        this.Parameters = this.Window().options.parameters;
    }
    Window() {
        return Helpers.GetDialogWindow();
    }
    WindowElement() {
        return Helpers.GetDialogWindowElement();
    }
    Close(results = null) {
        this.Window().options.returnResults = results;
        this.Window().close();
    }
    Center() {
        this.Window().center();
    }
}
//this class is a temporary fix until jquery.cookie can handle % correctly.
export class Cookies {
    static get(name) {
        var result = new RegExp('(?:^|;\\s*)' + ('' + name).replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&') + '=([^;]*)').exec(document.cookie);
        if (result == null || result.length < 2)
            return null;
        return result[1];
    }
}
export class Helpers {
    static ToPercent(count, total) {
        return total <= 0 ? 0 : parseFloat((count / total * 100).toFixed(2));
    }
    //Create a copy of an object when type is not known.
    static CopyObject(object) {
        var objectCopy = {};
        for (var prop in object) {
            if (Array.isArray(object[prop])) {
                objectCopy[prop] = ko.observableArray(object[prop].slice(object[prop].length));
            }
            else {
                objectCopy[prop] = ko.observable(object[prop]);
            }
        }
        return objectCopy;
    }
    static ConvertTermObject(object) {
        var objectCopy = {};
        for (var prop in object) {
            if (Array.isArray(object[prop])) {
                objectCopy[prop] = ko.observableArray(object[prop]);
            }
            else {
                objectCopy[prop] = ko.observable(object[prop]);
            }
        }
        return objectCopy;
    }
    //Tests the passed password for strength, with 0 being blank, and 5 being very strong.
    static TestPasswordStrength(password) {
        if (password == null || password.length == 0 || password.indexOf(":") > -1 || password.indexOf(";") > -1 || password.indexOf("<") > -1)
            return 0; //Blank/invalid
        if (password.length <= 4)
            return 1; //Weak
        var score = 1;
        if (password.length >= 8)
            score++;
        if (password.length >= 12)
            score++;
        var reg = /\d/;
        if (reg.test(password))
            score++;
        reg = /^(?=.*[a-z])(?=.*[A-Z]).+$/;
        if (reg.test(password))
            score++;
        reg = /[`,!,@,#,$,%,^,&,*,?,_,~,-,�,(,)]/;
        if (reg.test(password))
            score++;
        return score;
    }
    static GetEnumString(enumArray, value) {
        var item = ko.utils.arrayFirst(enumArray, (item) => {
            return item.value == value;
        });
        if (!item)
            return "";
        return item.text;
    }
    static GetEnumValue(enumArray, text, defaultValue) {
        var item = ko.utils.arrayFirst(enumArray, (item) => {
            return item.text == text;
        });
        if (!item)
            return defaultValue;
        return item.value;
    }
    static AddNotSelected(arr, displayProperty = null, valueProperty = null) {
        displayProperty = displayProperty || "text";
        valueProperty = valueProperty || "value";
        if (arr.length == 0 || arr[0][valueProperty] != null) {
            var add = {};
            add[displayProperty] = '<Not Selected>';
            add[valueProperty] = null;
            arr.unshift(add);
        }
        return arr;
    }
    static AddClearAllFiltersMenuItem(e) {
        let popup = e.container.data('kendoPopup');
        let menu = e.container.find(".k-menu").data("kendoMenu");
        let grid = e.sender;
        menu.append({ text: "Clear All Filters", spriteCssClass: 'k-i-filter-clear' });
        menu.bind("select", function (e) {
            if ($(e.item).text() == "Clear All Filters") {
                //First Clear the Filter of the grid, then close the menu, then Popup.  Must be done in this order.  See https://www.telerik.com/forums/close-menu-on-custom-column-menu-item
                grid.dataSource.filter({});
                menu.close();
                popup.close();
            }
        });
    }
    static GetDataSourceSettings(ds) {
        var state = {
            page: ds.page(),
            pageSize: ds.pageSize(),
            sort: ds.sort(),
            group: ds.group(),
            filter: ds.filter()
        };
        return JSON.stringify(state);
    }
    static GetGridSettings(grid) {
        var ds = grid.dataSource;
        var state = {
            page: ds.page(),
            pageSize: ds.pageSize(),
            sort: ds.sort(),
            group: ds.group(),
            filter: ds.filter(),
            columns: grid.columns
        };
        return JSON.stringify(state);
    }
    static SetDataSourceFromSettings(ds, settings) {
        var s = JSON.parse(settings);
        s.page = ds.page();
        s.pageSize = ds.pageSize();
        if (s == null) {
            ds.query();
        }
        else {
            delete s["columns"];
            ds.query(s);
        }
    }
    static SetGridFromSettings(grid, settings) {
        grid.options["Updating"] = true;
        try {
            var s = JSON.parse(settings);
            this.SetDataSourceFromSettings(grid.dataSource, settings);
            if (s == null)
                return;
            var filteredMembers = [];
            var filter;
            var x = () => {
                //reset filtered members
                filteredMembers = [];
                //dataSource has been updated already so use the grid's dataSource for the filters instead of the settings variable s
                var gridFitler = grid.dataSource.filter();
                if (gridFitler != null) {
                    filter = gridFitler.filters;
                    filter.forEach((f) => {
                        filteredMembers.push(f.field);
                    });
                }
                grid.thead.find(".k-header-column-menu.k-state-active").removeClass("k-state-active");
                if (filter) {
                    grid.thead.find("th[data-field]").each(function () {
                        var cell = $(this);
                        var cellfield = cell.data("field");
                        for (var k = 0; k < filteredMembers.length; k++) {
                            if (filteredMembers[k] == cellfield) {
                                cell.find(".k-header-column-menu").addClass("k-state-active");
                                var cellheader = cell.find(".k-header-column-menu");
                            }
                        }
                    });
                }
            };
            grid.bind("dataBound", x);
            //PMNDEV-5197: We need to remove/hide the columns that are set as hidden first.
            //The previous code below was moving columns and changing/updating indexes which caused the wrong columns to appear in the UI with invalid values at times.
            s.columns.forEach((col) => {
                if (col.hidden == true) {
                    grid.hideColumn(col.field);
                }
            });
            for (var i = 0; i < s.columns.length; i++) {
                if (grid.columns[i].field == s.columns[i].field) {
                    //Do Nothing
                }
                if (grid.columns[i].field != s.columns[i].field) {
                    for (var j = 0; j < grid.columns.length; j++) {
                        if (grid.columns[j].field == s.columns[i].field) {
                            grid.columns[j].width = s.columns[i].width;
                            grid.columns[j].hidden = s.columns[i].hidden;
                            grid.reorderColumn(i, grid.columns[j]);
                        }
                    }
                }
            }
        }
        finally {
            grid.options["Updating"] = false;
        }
    }
    static SetDataSourceFromSettingsWithDates(ds, settings, colDates) {
        var s = JSON.parse(settings);
        s.page = ds.page();
        s.pageSize = ds.pageSize();
        if (s == null) {
            ds.query();
        }
        else {
            delete s["columns"];
            if (s.filter != null && s.filter.filters != null) {
                var originalFilters = s.filter;
                var filter = { logic: s.filter.logic, filters: [] };
                originalFilters.filters.forEach((item) => {
                    if (colDates.indexOf(item.field) >= 0) {
                        filter.filters.push({ field: item.field, operator: item.operator, value: new Date(item.value) });
                    }
                    else {
                        filter.filters.push(item);
                    }
                });
                s.filter = filter;
            }
            ds.query(s);
        }
    }
    static SetGridFromSettingsWithDates(grid, settings, colDates) {
        grid.options["Updating"] = true;
        try {
            var s = JSON.parse(settings);
            this.SetDataSourceFromSettingsWithDates(grid.dataSource, settings, colDates);
            if (s == null)
                return;
            var filteredMembers = [];
            var filter;
            var x = () => {
                //reset filtered members
                filteredMembers = [];
                //dataSource has been updated already so use the grid's dataSource for the filters instead of the settings variable s
                var gridFitler = grid.dataSource.filter();
                if (gridFitler != null) {
                    filter = gridFitler.filters;
                    filter.forEach((f) => {
                        filteredMembers.push(f.field);
                    });
                }
                grid.thead.find(".k-header-column-menu.k-state-active").removeClass("k-state-active");
                if (filter) {
                    grid.thead.find("th[data-field]").each(function () {
                        var cell = $(this);
                        var cellfield = cell.data("field");
                        for (var k = 0; k < filteredMembers.length; k++) {
                            if (filteredMembers[k] == cellfield) {
                                cell.find(".k-header-column-menu").addClass("k-state-active");
                                var cellheader = cell.find(".k-header-column-menu");
                            }
                        }
                    });
                }
            };
            grid.bind("dataBound", x);
            s.columns.forEach((col) => {
                if (col.hidden == true) {
                    grid.hideColumn(col.field);
                }
            });
            for (var i = 0; i < s.columns.length; i++) {
                if (grid.columns[i].field == s.columns[i].field) {
                }
                if (grid.columns[i].field != s.columns[i].field) {
                    for (var j = 0; j < grid.columns.length; j++) {
                        if (grid.columns[j].field == s.columns[i].field) {
                            grid.columns[j].width = s.columns[i].width;
                            grid.columns[j].hidden = s.columns[i].hidden;
                            grid.reorderColumn(i, grid.columns[j]);
                        }
                    }
                }
            }
        }
        finally {
            grid.options["Updating"] = false;
        }
    }
    static SetGridDefaultColumnDefaultSizes(grid, columnSizes) {
        grid.columns.forEach(function (column) {
            for (var i = 0; i < Object.getOwnPropertyNames(columnSizes).length; i++) {
                var colProp = Object.getOwnPropertyNames(columnSizes)[i];
                var colValue = columnSizes[colProp];
                if (column.title === colProp)
                    grid.resizeColumn(column, colValue);
            }
        });
    }
    static GetColumnFilterOperatorDefaults() {
        return {
            operators: {
                string: {
                    contains: "Contains",
                    doesnotcontain: "Does not contain",
                    eq: "Is equal to",
                    neq: "Is not equal to",
                    startswith: "Starts with",
                    endswith: "Ends With",
                    isnull: "Is null",
                    isnotnull: "Is not null",
                    isempty: "Is empty",
                    isnotempty: "Is not empty",
                },
                date: {
                    gt: "Is After",
                    lt: "Is Before"
                },
                number: {
                    eq: "Is equal to",
                    neq: "Is not equal to",
                    gte: "Is greater than or equal to",
                    gt: "Is greater than",
                    lte: "Is less than or equal to",
                    lt: "Is less than",
                    isnull: "Is null",
                    isnotnull: "Is not null"
                }
            },
        };
    }
    static WatchGridForChanges(grid, func) {
        grid.bind("columnResize", () => {
            if (grid.options["Updating"])
                return;
            func();
        });
        grid.bind("columnReorder", () => {
            if (grid.options["Updating"])
                return;
            func();
        });
        grid.bind("columnHide", () => {
            if (grid.options["Updating"])
                return;
            func();
        });
        grid.bind("columnShow", () => {
            if (grid.options["Updating"])
                return;
            func();
        });
        grid.bind("columnLock", () => {
            if (grid.options["Updating"])
                return;
            func();
        });
        grid.bind("columnUnlock", () => {
            if (grid.options["Updating"])
                return;
            func();
        });
        grid.dataSource.bind("change", (e) => {
            if (grid.options["Updating"])
                return;
            func();
        });
    }
    static PersistDataSourceSettingsToUrl(ds, setCurrentUrl) {
        var params = "";
        //Build up the url based on the sort etc
        if (ds.sort()) {
            ds.sort().forEach((item) => {
                if ((item.field == "SubmittedOn" && item.dir == "desc"))
                    return;
                params += "&sort=" + encodeURIComponent(item.field) + "|" + encodeURIComponent(item.dir);
            });
        }
        if (ds.filter()) {
            ds.filter().filters.forEach((item) => {
                params += "&filter=" + encodeURIComponent(item.field) + "|" + encodeURIComponent(item.operator.toString()) + "|" + encodeURIComponent(item.value);
            });
        }
        //Create the entire url
        var url = window.location.href;
        if (url.indexOf("?") == -1) {
            url = url.substr(0, url.indexOf("?") + 1);
        }
        else if (params.length > 0) {
            url += "?";
        }
        if (params.length > 0)
            url += params.substr(1);
        if (setCurrentUrl)
            history.replaceState("", document.title, url);
        return url;
    }
    static GetFiltersFromUrl() {
        var filter = GetQueryParam("filter");
        if (filter) {
            if (!$.isArray(filter))
                filter = $.makeArray(filter);
            var filters = [];
            filter.forEach((item) => {
                var args = item.split("|");
                filters.push({ field: args[0], operator: args[1], value: args[2] });
            });
            return filters;
        }
        return null;
    }
    static GetSortsFromUrl() {
        var sort = GetQueryParam("sort");
        if (sort) {
            var sorts = [];
            if (!$.isArray(sort))
                sort = $.makeArray(sort);
            sort.forEach((item) => {
                var args = item.split("|");
                sorts.push({ field: args[0], dir: args[1] });
            });
            return sorts;
        }
        return null;
    }
    static GetServiceUrl(relativeUrl) {
        if ($.support.cors) {
            return ServiceUrl + ((ServiceUrl[ServiceUrl.length - 1] === '/' && relativeUrl[0] === '/') ? relativeUrl.slice(1) : relativeUrl);
        }
        else {
            return "/api/get?Url=" + encodeURIComponent(relativeUrl);
        }
    }
    static PostServiceUrl(relativeUrl) {
        if ($.support.cors) {
            return ServiceUrl + relativeUrl;
        }
        else {
            return "/api/post?Url=" + encodeURIComponent(relativeUrl);
        }
    }
    static PutServiceUrl(relativeUrl) {
        if ($.support.cors) {
            return ServiceUrl + relativeUrl;
        }
        else {
            return "/api/put?Url=" + encodeURIComponent(relativeUrl);
        }
    }
    static DeleteServiceUrl(relativeUrl) {
        if ($.support.cors) {
            return ServiceUrl + relativeUrl;
        }
        else {
            return "/api/delete?Url=" + encodeURIComponent(relativeUrl);
        }
    }
    static decodeBase64(s) {
        var e = {}, i, b = 0, c, x, l = 0, a, r = '', w = String.fromCharCode, L = s.length;
        var A = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        for (i = 0; i < 64; i++) {
            e[A.charAt(i)] = i;
        }
        for (x = 0; x < L; x++) {
            c = e[s.charAt(x)];
            b = (b << 6) + c;
            l += 6;
            while (l >= 8) {
                ((a = (b >>> (l -= 8)) & 0xff) || (x < (L - 2))) && (r += w(a));
            }
        }
        return r;
    }
    static chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
    static encodebase64(data) {
        if (window.btoa != null)
            return window.btoa(data);
        var str = String(data);
        var output = "";
        for (
        // initialize result and counter
        var block, charCode, idx = 0, map = Helpers.chars, output = ''; 
        // if the next str index does not exist:
        //   change the mapping table to "="
        //   check if d has no fractional digits
        str.charAt(idx | 0) || (map = '=', idx % 1); 
        // "8 - idx % 1 * 8" generates the sequence 2, 4, 6, 8
        output += map.charAt(63 & block >> 8 - idx % 1 * 8)) {
            charCode = str.charCodeAt(idx += 3 / 4);
            if (charCode > 0xFF) {
                throw "'btoa' failed: The string to be encoded contains characters outside of the Latin1 range.";
            }
            block = block << 8 | charCode;
        }
        return output;
    }
    static ShowDialog(title, url, actions = null, width, height, parameters = null) {
        var deferred = $.Deferred();
        var loaded = false;
        var kendoWindow = $('<div data-url="' + url + '" style="overflow: hidden;"/>').kendoWindow({
            title: title,
            resizable: false,
            draggable: false,
            modal: true,
            visible: false,
            actions: actions || [],
            width: width || "auto",
            height: height || "75px",
            iframe: true,
            content: url,
            pinned: false,
            refresh: function () {
                if (loaded)
                    return;
                loaded = true;
                kendoWindow.data("kendoWindow").center().open();
            },
            deactivate: function () {
                this.destroy();
            },
            parameters: parameters,
            close: function (e) {
                if (e.userTriggered) {
                    //userTriggered will be true if the close action has been triggered by the user clicking the close button or escape key -> cancel.
                    deferred.reject();
                }
                else {
                    //close was triggered programatically by clicking an action button in the window
                    deferred.resolve(kendoWindow.data("kendoWindow").options.returnResults);
                }
            }
        });
        return deferred;
    }
    static ShowToast(message) {
        var d = jQuery.Deferred();
        var notificationWidget = $("#NotificationsKendo").kendoNotification({ button: true, autoHideAfter: 60000, allowHideAfter: 10000, hideOnClick: false }).data("kendoNotification");
        notificationWidget.show(message, 'info');
        return d;
    }
    static GetDialogWindow() {
        return this.GetDialogWindowElement().data("kendoWindow");
    }
    static GetDialogWindowElement() {
        var dialog = window.parent.$("div[data-url^='" + window.location.pathname + "']");
        return dialog;
    }
    static ShowAlert(title, html, width, actions) {
        var d = jQuery.Deferred();
        var kendoWindow = $("<div style=\"max-height: 600px;\"/>").kendoWindow({
            title: title,
            resizable: false,
            modal: true,
            actions: actions || [],
            width: width || 800,
            iframe: true
        });
        var lcHtml = '';
        //Strip out the <html> and <body> tags
        if ($.isPlainObject(html)) {
            if (html.errors && html.errors.length > 0) {
                lcHtml = (html.errors[0].Description || '').toLowerCase();
            }
            else {
                lcHtml = (html || '').toLowerCase();
            }
        }
        else {
            lcHtml = (html || '').toLowerCase();
        }
        var bodyStart = lcHtml.indexOf("<body");
        if (bodyStart > -1) {
            var bodyEnd = lcHtml.indexOf(">", bodyStart);
            if (bodyEnd > -1) {
                html = html.substring(bodyEnd + 1);
                lcHtml = html.toLowerCase();
                var endBody = lcHtml.indexOf("</body>");
                if (endBody > -1)
                    html = html.substring(0, endBody);
            }
        }
        html = "<div style=\"max-height:450px;overflow:auto;\">" + html + "</div>";
        kendoWindow.data("kendoWindow").content(html + '\r\n<div style="text-align: center; margin-top:25px;"><button class="btn btn-default" style="width:120px;" id="btnOK">OK</button></div>');
        kendoWindow.data("kendoWindow").center().open();
        kendoWindow.parent().find(".k-window-action").css("visibility", "hidden");
        kendoWindow.find("#btnOK").click((e) => {
            kendoWindow.data("kendoWindow").close();
            d.resolve();
        }).end();
        return d;
    }
    static ShowErrorAlert(title, error, width) {
        this.ShowAlert(title, this.ProcessAjaxError(error), width, ["Close"]);
    }
    static ProcessAjaxError(error) {
        if (error.responseText != null && error.responseText.toLowerCase().indexOf("<body") > -1)
            return error.responseText;
        if (typeof error == "string") //It's just a string of text, return it.
            return error;
        if (error.message) {
            return error.message;
        }
        var errorString = "Status: " + error.status + "<br/>Server Message: " + error.statusText;
        //Should parse the error text etc. from the array
        if (error.responseText != null && error.responseText != "") {
            try {
                var result = JSON.parse(error.responseText);
                if (result.results != null) {
                    errorString = "";
                    for (var j = 0; j < result.results.length; j++) {
                        var r = result.results[j];
                        r.errors.forEach((item) => {
                            errorString += (errorString == "" ? "" : "<br/>") + item.Description.replace(/(?:\r\n|\r|\n)/g, '<br />');
                        });
                    }
                }
                else if (result.Message != null) {
                    errorString = result.Message.replace(/(?:\r\n|\r|\n)/g, '<br />') + "<br/><br/>" + result.MessageDetail.replace(/(?:\r\n|\r|\n)/g, '<br />');
                }
                else if (result.errors != null) {
                    errorString = "";
                    result.errors.forEach((error) => {
                        errorString += error.Description + Constants.LineBreak;
                    });
                    if (errorString.length > 0)
                        errorString = errorString.substr(0, errorString.length - Constants.LineBreak.length);
                }
                else {
                    throw "No Errors";
                }
            }
            catch (err) {
                errorString += "<br/>Error: <br/>" + error.responseText.replace(/(?:\r\n|\r|\n)/g, '<br />');
            }
        }
        return errorString;
    }
    //Confirms an operation
    static ShowConfirm(title, html) {
        var d = jQuery.Deferred();
        var kendoWindow = $("<div/>").kendoWindow({
            title: title,
            resizable: false,
            modal: true,
            actions: [],
            iframe: true
        });
        var window = kendoWindow.data("kendoWindow");
        window.content(html + '<br/><div style="text-align: center;"><button class="btn btn-default" id="confirmYes">Yes</button>&nbsp;&nbsp;<button class="btn btn-default" id="confirmNo">No</button></div>');
        window.center().open();
        kendoWindow.find("#confirmYes, #confirmNo")
            .click((e) => {
            window.close();
            if ((e.srcElement || e.target).id == "confirmYes") {
                d.resolve();
            }
            else {
                d.reject();
            }
        }).end();
        return d;
    }
    static ShowPrompt(title, promptMessage, width, singleLine) {
        var d = jQuery.Deferred();
        var kendoWindow = $("<div/>").kendoWindow({
            title: title,
            resizable: false,
            modal: true,
            actions: [],
            iframe: true,
            width: width || 600,
        });
        var window = kendoWindow.data("kendoWindow");
        var content = '<form><p>' + promptMessage + '</p>';
        if (singleLine) {
            content += '<input type="text" id="confirmInputValue" class="form-control" style="width:95%" />';
        }
        else {
            content += '<textarea id="confirmInputValue" class="form-control" rows="3" style="width:95%;" ></textarea>';
        }
        content += '</form><br/><div style="text-align: center;"><button class="btn btn-default pull-right" style="width:100px" id="confirmNo">Cancel</button><button class="btn btn-default pull-right" style="width:100px;margin-right:15px;" id="confirmYes">Continue</button></div>';
        window.content(content);
        window.center().open();
        kendoWindow.find("#confirmYes, #confirmNo")
            .click((e) => {
            window.close();
            if ((e.srcElement || e.target).id == "confirmYes") {
                var message = $(kendoWindow.find('#confirmInputValue')).val();
                d.resolve(message);
            }
            else {
                d.reject();
            }
        }).end();
        return d;
    }
    static ShowExecuting() {
        kendo.ui.progress($("#Content"), true);
    }
    static HideExecuting() {
        kendo.ui.progress($("#Content"), false);
    }
    static kilobyte = 1024;
    static megabyte = 1024 * 1024;
    static gigabyte = 1024 * 1024 * 1024;
    static formatFileSize(length) {
        if (length > this.gigabyte)
            return (length / this.gigabyte).toFixed(2) + ' Gb';
        if (length > this.megabyte)
            return (length / this.megabyte).toFixed(2) + ' Mb';
        if (length > this.kilobyte)
            return (length / this.kilobyte).toFixed(2) + ' Kb';
        return length + ' bytes';
    }
    /*Checks the specified url has a value, and appends a / to it if missing before initiating the redirect.*/
    static RedirectTo(url) {
        if (url == null || url == undefined || url == '')
            return;
        var startsWith = (value) => {
            return url.slice(0, value.length).toLowerCase() == value.toLowerCase();
        };
        if (startsWith('http') || startsWith('https') || startsWith('/')) {
            window.top.location.href = url;
        }
        else {
            window.top.location.href = '/' + url;
        }
    }
    static StartsWith(content, value) {
        content = content || '';
        value = value || '';
        if (content.length == 0 || value.length == 0)
            return false;
        return content.slice(0, value.length).toLowerCase() == value.toLowerCase();
    }
    /* Replaces the current page history item with new details */
    static HistoryReplaceState(pageTitle, uri) {
        if (Helpers.StartsWith(uri, '/') == false)
            uri = '/' + uri;
        window.history.replaceState(null, pageTitle, uri);
    }
    /** Helper that replaces filter parameter values with a custom object that allows for specific formatting of the odata filter string for the specified field. */
    static UpdateKendoGridFilterOptions(options, customFilterStrings) {
        var updateFilterValue = (filters) => {
            for (var j = 0; j < filters.length; j++) {
                for (var k = 0; k < customFilterStrings.length; k++) {
                    var fs = customFilterStrings[k];
                    if (fs.field === filters[j].field && filters[j].value != null) {
                        filters[j].value = new ODataCustomFilterValueFormatter(filters[j].value, fs.format);
                    }
                }
                if (filters[j].filters != null && filters[j].filters.length > 0) {
                    updateFilterValue(filters[j].filters);
                }
            }
        };
        if (options.filter != null && options.filter.filters != null) {
            updateFilterValue(options.filter.filters);
        }
        return options;
    }
    static DetectInternetExplorer() {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf('MSIE ');
        if (msie > 0) {
            return true;
        }
        var trident = ua.indexOf('Trident/');
        if (trident > 0) {
            return true;
        }
        return false;
    }
}
/** Custom class that overrides the default toString implementation to return the value specified using the a specific string format. */
export class ODataCustomFilterValueFormatter extends Object {
    value;
    format;
    constructor(value, format) {
        super();
        this.value = value;
        this.format = format || '';
    }
    toString() {
        if (this.format.length == 0)
            return this.value;
        return kendo.format(this.format, this.value);
    }
}
export function ShowTerms(params) {
    var param;
    if (params == null) {
        param = "<p>This Distributed Query Tool is for use by authorized HMO Research Network investigators and staff, and affiliated authorized partners. This resource is intended for preparatory to research or preliminary (feasibility) data queries. All results are aggregated across participating health plans. No attempt should be made to identify data from individual health plans.</p><p> Data from the Distributed Query Tool cannot be published without prior approval.External recipients of data from this tool must agree to these terms and conditions and all data shared must be marked CONFIDENTIAL and sent securely.</p ><p>The privilege to use this tool may be revoked at any time.</p >";
    }
    else {
        param = params;
    }
    Helpers.ShowDialog("Terms And Conditions", "/termsandconditions", ["close"], 800, 600, param).done(() => { });
}
export function ShowInfo(params) {
    var param;
    if (params == null) {
        param = "<p>Funded under Contract No. 290-05-0033 from the Agency for Healthcare Research and Quality, US Department of Health and Human Services as part of the Developing Evidence to Inform Decisions about Effectiveness (DEcIDE) program, awarded to the DEcIDE centers at the HMO Research Network Center for Education and Research on Therapeutics (HMORN CERT) and the University of Pennsylvania. PI: Richard Platt.</p>";
    }
    else {
        param = params;
    }
    Helpers.ShowDialog("Information", "/info", ["close"], 800, 600, param).done(() => { });
}
//Gets or sets session data
export function Session(key, data) {
    if (data) {
        sessionStorage.setItem(key, JSON.stringify(data));
    }
    else {
        return JSON.parse(sessionStorage.getItem(key));
    }
}
export function SessionRemoveKey(key) {
    sessionStorage.removeItem(key);
}
//Gets or sets local data
export function Local(key, data) {
    if (data) {
        localStorage.setItem(key, JSON.stringify(data));
    }
    else {
        return JSON.parse(localStorage.getItem(key));
    }
}
//Gets a query parameter and is not case sensitive.
export function GetQueryParam(param) {
    let params = new URLSearchParams(document.location.search);
    return params.get(param);
}
/* A class representing a key/value pair */
export class KeyValuePair {
    constructor(_key, _value) {
        this.key = _key;
        this.value = _value;
    }
    /*The key property of type k*/
    key;
    /*The value property of type v*/
    value;
}
export class UserInfo {
    ID = null;
    AuthToken = null;
    AuthInfo = null;
    Employers = [];
    EmployerID = null;
    PasswordExpiration = null;
    SessionExpireMinutes = null;
    SessionTimeout = null;
    constructor() {
        let self = this;
        let getCookie = (name) => {
            let result = new RegExp('(?:^|;\\s*)' + ('' + name).replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&') + '=([^;]*)').exec(document.cookie);
            if (result == null || result.length < 2)
                return null;
            return result[1];
        };
        let authorizationToken = getCookie("Authorization");
        if (!authorizationToken)
            return;
        this.AuthInfo = JSON.parse(decodeURIComponent(authorizationToken));
        if (!this.AuthInfo)
            return;
        this.ID = this.AuthInfo.ID;
        if (this.AuthInfo.UserName) {
            this.AuthToken = this.AuthInfo.Authorization;
            this.Employers = this.AuthInfo.Employers || [];
            //See if there is an employerID. If not and there is only one employer, set it.
            if (!this.AuthInfo.EmployerID && this.Employers.length > 0)
                this.SetEmployer(this.Employers[0].ID);
            if (this.AuthInfo.EmployerID) {
                this.EmployerID = this.AuthInfo.EmployerID;
                this.AuthToken += ":" + this.EmployerID;
            }
            this.AuthToken = Helpers.encodebase64(this.AuthToken);
            this.PasswordExpiration = this.AuthInfo.PasswordExpiration;
        }
        else {
            this.AuthToken = null;
        }
        let invalidURLs = ["/dialogs/sessionexpiring", "/login", "/account/login"];
        if ($.inArray(window.location.pathname.toLowerCase(), invalidURLs) == -1) {
            self.SessionExpireMinutes = self.AuthInfo.SessionExpireMinutes;
            //localStorage.setItem("SessionExpire", addMinutes(new Date(), self.SessionExpireMinutes).toJSON());
        }
        if (this.AuthToken) {
            $.ajaxSetup({
                headers: { "Authorization": "PopMedNet " + this.AuthToken },
                converters: {
                    "text json": function (response) {
                        //required to handle when the dataType is set to json for the request, but the endpoint returns null in success scenarios.
                        return (response == null || response == "") ? null : JSON.parse(response);
                    }
                }
            });
        }
        //if (self.SessionExpireMinutes && self.SessionExpireMinutes > 0 && $.inArray(window.location.pathname.toLowerCase(), invalidURLs) == -1) {
        //    self.SessionTimeout = setTimeout(function () { self.SessionExiring() }, (self.SessionExpireMinutes - 5) * 60 * 1000);
        //    setInterval(() => {
        //        self.CheckAuth();
        //    }, 15000);
        //}
    }
    SessionExiring() {
        //let self = this;
        //setTimeout(() => { }, 1000)
        /////Due to regression in IE if we want to get an updated local storage item that may have been updated cross tab, we need to set a random Item first which will force a refresh of the localStorage.
        //if (Helpers.DetectInternetExplorer()) {
        //    localStorage.setItem("ieFix", null);
        //}
        //var expireDateTime = localStorage.getItem("SessionExpire");
        ////if ((parseJSON(expireDateTime).getTime() - new Date().getTime()) < (5 * 60 * 1000)) {
        ////    this.SessionExpringDialog();
        ////}
        ////else {
        ////    clearTimeout(self.SessionTimeout);
        ////    self.SessionTimeout = setTimeout(function () { self.SessionExiring() }, (((parseJSON(expireDateTime).getTime() - new Date().getTime()) - 5) * 60 * 1000));
        ////}
    }
    SessionExpringDialog() {
        //let self = this;
        //Helpers.ShowDialog("Session Expiring", "/Dialogs/SessionExpiring", ["close"], 800, 600).done((response: boolean) => {
        //    if (response == true) {
        //        $.ajax({
        //            type: "GET",
        //            url: "/Home/RefreshSession",
        //            dataType: "json",
        //        }).done((result) => {
        //            User = new UserInfo();
        //        }).fail((error) => {
        //            window.location.href = "/login?Error=" + encodeURIComponent("You have been logged out due to inactivity. Please relogin.") + "&returnURL=" + encodeURIComponent(window.location.href);
        //        });
        //    }
        //    else {
        //        ///Due to regression in IE if we want to get an updated local storage item that may have been updated cross tab, we need to set a random Item first which will force a refresh of the localStorage.
        //        if (Helpers.DetectInternetExplorer()) {
        //            localStorage.setItem("ieFix", null);
        //        }
        //        //let time = parseJSON(localStorage.getItem("SessionExpire")).getTime() - new Date().getTime();
        //        //let minutes = Math.floor((time / 1000 / 60));
        //        //if (isBefore(parseJSON(localStorage.getItem("SessionExpire")), new Date()) || minutes <= 0) {
        //        //    window.location.href = "/account/login?Error=" + encodeURIComponent("You have been logged out due to inactivity. Please relogin.") + "&returnURL=" + encodeURIComponent(window.location.href);
        //        //}
        //    }
        //});
    }
    CheckAuth() {
        //$.ajax({
        //    type: "GET",
        //    url: "/Home/CheckAuth",
        //    dataType: "json",
        //}).done((result) => {
        //    if (!result.IsAuthenticated) {
        //        window.location.href = "/login?Error=" + encodeURIComponent("You have been logged out due to inactivity. Please relogin.") + "&returnURL=" + encodeURIComponent(window.location.href);
        //    }
        //}).fail((error) => {
        //    //if (new Date() > parseJSON(localStorage.getItem("SessionExpire"))) {
        //    //    if (Helpers.DetectInternetExplorer()) {
        //    //        localStorage.removeItem("ieFix");
        //    //    }
        //    //    localStorage.removeItem("SessionExpire");
        //    //    window.location.href = "/account/login?Error=" + encodeURIComponent("You have been logged out due to inactivity. Please relogin.") + "&returnURL=" + encodeURIComponent(window.location.href);
        //    //}
        //});
    }
    SetEmployer(employerID) {
        this.AuthInfo.EmployerID = employerID;
        this.Update();
    }
    Update() {
        $.cookie("Authorization", JSON.stringify(this.AuthInfo), { path: "/" });
        _user = new UserInfo();
    }
    RefreshSession() {
        //$.ajax({
        //    type: "GET",
        //    url: "/api/noop",
        //    dataType: "json",
        //}).done((result) => {
        //    User = new UserInfo();
        //}).fail((error) => {
        //    window.location.href = "/login?Error=" + encodeURIComponent("You have been logged out due to inactivity. Please relogin.");
        //});
    }
    Logout() {
        //$.ajax({
        //    type: "GET",
        //    url: "/api/logout",
        //    dataType: "json",
        //}).done((result) => {
        //    this.AuthInfo.ID = null;
        //    this.AuthInfo.Authorization = null;
        //    this.AuthInfo.UserName = null;
        //    this.AuthToken = null;
        //    this.ID = null;
        //    this.EmployerID = null;
        //    window.location.href = "/login";
        //}).fail((e) => {
        //    Helpers.ShowAlert("Logout Failed", "<p>We're sorry but we could not properly log you out. Please close your browser window as an alternative.</p>");
        //});
    }
}
var _user = new UserInfo();
export const User = _user;
//# sourceMappingURL=global.js.map