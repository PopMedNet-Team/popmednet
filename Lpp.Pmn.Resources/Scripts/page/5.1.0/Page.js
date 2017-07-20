/// <reference path="../../typings/bootstrap.dns.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
//V5.1.0
var Global;
(function (Global) {
    var WorkflowActivityViewModel = (function () {
        function WorkflowActivityViewModel(bindingControl, screenPermissions) {
            if (screenPermissions === void 0) { screenPermissions = null; }
            var self = this;
            this._BindingControl = bindingControl;
            this.ScreenPermissions = screenPermissions;
            this.HasPermission = function (permissionID) {
                if (self.ScreenPermissions == null)
                    throw 'Screen Permissions were not passed';
                return ko.utils.arrayFirst(self.ScreenPermissions, function (item) {
                    return item.toLowerCase() == permissionID.toLowerCase();
                }) != null;
            };
        }
        WorkflowActivityViewModel.prototype.Complete = function (data, event) {
            var ctl = $(event.target);
            var ResultID = data.GetResultID($(event.target));
            this.PostComplete(ResultID);
        };
        WorkflowActivityViewModel.prototype.GetResultID = function (ctl) {
            var ResultID = ctl.attr("data-ResultID");
            return ResultID;
        };
        WorkflowActivityViewModel.prototype.PostComplete = function (resultID) {
            throw "Not implemented";
        };
        return WorkflowActivityViewModel;
    }());
    Global.WorkflowActivityViewModel = WorkflowActivityViewModel;
    var PageViewModel = (function () {
        function PageViewModel(bindingControl, screenPermissions) {
            if (screenPermissions === void 0) { screenPermissions = null; }
            var _this = this;
            this._BindingControl = bindingControl;
            var self = this;
            this.ScreenPermissions = screenPermissions == null ? null : screenPermissions.map(function (sp) {
                return sp.toLowerCase();
            });
            this.ValidationMessage = ko.observable("");
            this.Validator = bindingControl.kendoValidator({
                validate: function () {
                    var errors = "";
                    _this.Validator.errors().forEach(function (item) {
                        errors += "<li>" + item + "</li>\r\n";
                    });
                    if (errors.length > 0)
                        errors = "<ul style=\"margin-left: 50px;\">\r\n" + errors + "</ul>";
                    _this.ValidationMessage(errors);
                }
            }).data("kendoValidator");
            this.title = ko.observable("");
            this.Title = ko.computed({
                read: function () {
                    return _this.title();
                },
                write: function (value) {
                    _this.title(value);
                    document.title = value;
                }
            });
            this.HasPermission = function (permissionID) {
                if (self.ScreenPermissions == null)
                    throw 'Screen Permissions were not passed';
                return ko.utils.arrayFirst(self.ScreenPermissions, function (item) {
                    return item.toLowerCase() == permissionID.toLowerCase();
                }) != null;
            };
        }
        PageViewModel.prototype.Validate = function () {
            return this.Validator.validate();
        };
        PageViewModel.prototype.WatchTitle = function (observable, prefix) {
            observable.subscribe(function (value) {
                document.title = prefix + (!value ? "New" : value);
            });
            observable.notifySubscribers(observable());
        };
        return PageViewModel;
    }());
    Global.PageViewModel = PageViewModel;
    var DialogViewModel = (function (_super) {
        __extends(DialogViewModel, _super);
        function DialogViewModel(bindingControl) {
            var _this = _super.call(this, bindingControl) || this;
            _this.lastDocumentHeight = -1;
            _this.Parameters = _this.Window().options.parameters;
            return _this;
        }
        DialogViewModel.prototype.Window = function () {
            return Helpers.GetDialogWindow();
        };
        DialogViewModel.prototype.WindowElement = function () {
            return Helpers.GetDialogWindowElement();
        };
        DialogViewModel.prototype.Close = function (results) {
            if (results === void 0) { results = null; }
            this.Window().options.returnResults = results;
            this.Window().close();
        };
        DialogViewModel.prototype.Center = function () {
            this.Window().center();
        };
        return DialogViewModel;
    }(PageViewModel));
    Global.DialogViewModel = DialogViewModel;
    var UserInfo = (function () {
        function UserInfo() {
            this.ID = null;
            this.AuthToken = null;
            this.AuthInfo = null;
            this.Employers = [];
            this.EmployerID = null;
            this.PasswordExpiration = null;
            this.SessionExpireMinutes = null;
            var sAuthorization = Global.Cookies.get("Authorization");
            if (!sAuthorization)
                return;
            this.AuthInfo = JSON.parse(sAuthorization);
            if (!this.AuthInfo)
                return;
            //if (!this.AuthInfo) {//Not logged in yet
            //    //Check that the current URL isn't the login page
            //    if (window.location.href.toLowerCase().indexOf("/account/login") >= 0 || window.location.protocol.toLowerCase() == "http")
            //        return;  //On the login page or other insecure page before login, get out.
            //    window.location.href = "/account/login?ReturnUrl=" + encodeURIComponent(window.location.href);
            //    return;
            //}
            this.ID = this.AuthInfo.ID;
            if (this.AuthInfo.UserName) {
                this.AuthToken = this.AuthInfo.UserName + ":" + this.AuthInfo.Password;
                this.Employers = this.AuthInfo.Employers || [];
                //See if there is an employerID. If not and there is only one employer, set it.
                if (!this.AuthInfo.EmployerID && this.Employers.length > 0)
                    this.SetEmployer(this.Employers[0].ID);
                if (this.AuthInfo.EmployerID) {
                    this.EmployerID = this.AuthInfo.EmployerID;
                    this.AuthToken += ":" + this.EmployerID;
                }
                this.AuthToken = Global.Helpers.encodebase64(this.AuthToken);
                this.PasswordExpiration = this.AuthInfo.PasswordExpiration;
            }
            else {
                this.AuthToken = null;
            }
            this.SessionExpireMinutes = this.AuthInfo.SessionExpireMinutes;
            if (this.AuthToken) {
                $.ajaxSetup({
                    headers: { "Authorization": "Basic " + this.AuthToken }
                });
            }
            if (this.SessionExpireMinutes && this.SessionExpireMinutes > 0) {
                setTimeout(function () {
                    window.location.href = "/account/login?ReturnUrl=" + encodeURIComponent(window.location.href);
                }, this.SessionExpireMinutes * 60 * 1000);
            }
        }
        UserInfo.prototype.SetEmployer = function (employerID) {
            this.AuthInfo.EmployerID = employerID;
            this.Update();
        };
        UserInfo.prototype.Update = function () {
            $.cookie("Authorization", JSON.stringify(this.AuthInfo), { path: "/" });
            User = new UserInfo();
        };
        UserInfo.prototype.RefreshSession = function () {
            $.ajax({
                type: "GET",
                url: "/api/noop",
                dataType: "json",
            }).done(function (result) {
                User = new UserInfo();
            }).fail(function (error) {
                window.location.href = "/account/login?Error=" + encodeURIComponent("You have been logged out due to inactivity. Please relogin.");
            });
        };
        UserInfo.prototype.SetLogin = function (login, password, userID, passwordExpiration, employerID) {
            this.AuthInfo.UserName = login;
            this.AuthInfo.Password = password;
            this.AuthInfo.ID = userID;
            this.AuthInfo.EmployerID = employerID;
            this.AuthInfo.PasswordExpiration = passwordExpiration;
            this.Update();
        };
        UserInfo.prototype.Logout = function () {
            var _this = this;
            $.ajax({
                type: "GET",
                url: "/api/logout",
                dataType: "json",
            }).done(function (result) {
                _this.AuthInfo.ID = null;
                _this.AuthInfo.Password = null;
                _this.AuthInfo.UserName = null;
                _this.AuthToken = null;
                _this.ID = null;
                _this.EmployerID = null;
                window.location.href = "/account/login";
            }).fail(function (e) {
                Helpers.ShowAlert("Logout Failed", "<p>We're sorry but we could not properly log you out. Please close your browser window as an alternative.</p>");
            });
        };
        return UserInfo;
    }());
    Global.UserInfo = UserInfo;
    //this class is a temporary fix until jquery.cookie can handle % correctly.
    var Cookies = (function () {
        function Cookies() {
        }
        Cookies.get = function (name) {
            var result = new RegExp('(?:^|;\\s*)' + ('' + name).replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&') + '=([^;]*)').exec(document.cookie);
            if (result == null || result.length < 2)
                return null;
            return result[1];
        };
        return Cookies;
    }());
    Global.Cookies = Cookies;
    var Helpers = (function () {
        function Helpers() {
        }
        Helpers.ToPercent = function (count, total) {
            return total <= 0 ? 0 : parseFloat((count / total * 100).toFixed(2));
        };
        //Create a copy of an object when type is not known.
        Helpers.CopyObject = function (object) {
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
        };
        Helpers.ConvertTermObject = function (object) {
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
        };
        //Tests the passed password for strength, with 0 being blank, and 5 being very strong.
        Helpers.TestPasswordStrength = function (password) {
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
        };
        Helpers.GetEnumString = function (enumArray, value) {
            var item = ko.utils.arrayFirst(enumArray, function (item) {
                return item.value == value;
            });
            if (!item)
                return "";
            return item.text;
        };
        Helpers.GetEnumValue = function (enumArray, text, defaultValue) {
            var item = ko.utils.arrayFirst(enumArray, function (item) {
                return item.text == text;
            });
            if (!item)
                return defaultValue;
            return item.value;
        };
        Helpers.AddNotSelected = function (arr, displayProperty, valueProperty) {
            if (displayProperty === void 0) { displayProperty = null; }
            if (valueProperty === void 0) { valueProperty = null; }
            displayProperty = displayProperty || "text";
            valueProperty = valueProperty || "value";
            if (arr.length == 0 || arr[0][valueProperty] != null) {
                var add = {};
                add[displayProperty] = '<Not Selected>';
                add[valueProperty] = null;
                arr.unshift(add);
            }
            return arr;
        };
        Helpers.GetDataSourceSettings = function (ds) {
            var state = {
                page: ds.page(),
                pageSize: ds.pageSize(),
                sort: ds.sort(),
                group: ds.group(),
                filter: ds.filter()
            };
            return JSON.stringify(state);
        };
        Helpers.GetGridSettings = function (grid) {
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
        };
        Helpers.SetDataSourceFromSettings = function (ds, settings) {
            var s = JSON.parse(settings);
            if (s == null) {
                ds.query();
            }
            else {
                delete s["columns"];
                ds.query(s);
            }
        };
        Helpers.SetGridFromSettings = function (grid, settings) {
            grid.options["Updating"] = true;
            try {
                var s = JSON.parse(settings);
                this.SetDataSourceFromSettings(grid.dataSource, settings);
                if (s == null)
                    return;
                var filteredMembers = [];
                var filter;
                var x = function () {
                    //reset filtered members
                    filteredMembers = [];
                    //dataSource has been updated already so use the grid's dataSource for the filters instead of the settings variable s
                    var gridFitler = grid.dataSource.filter();
                    if (gridFitler != null) {
                        filter = gridFitler.filters;
                        filter.forEach(function (f) {
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
                s.columns.forEach(function (col) {
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
        };
        Helpers.SetDataSourceFromSettingsWithDates = function (ds, settings, colDates) {
            var s = JSON.parse(settings);
            if (s == null) {
                ds.query();
            }
            else {
                delete s["columns"];
                if (s.filter != null && s.filter.filters != null) {
                    var originalFilters = s.filter;
                    var filter = { logic: s.filter.logic, filters: [] };
                    originalFilters.filters.forEach(function (item) {
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
        };
        Helpers.SetGridFromSettingsWithDates = function (grid, settings, colDates) {
            grid.options["Updating"] = true;
            try {
                var s = JSON.parse(settings);
                this.SetDataSourceFromSettingsWithDates(grid.dataSource, settings, colDates);
                if (s == null)
                    return;
                var filteredMembers = [];
                var filter;
                var x = function () {
                    //reset filtered members
                    filteredMembers = [];
                    //dataSource has been updated already so use the grid's dataSource for the filters instead of the settings variable s
                    var gridFitler = grid.dataSource.filter();
                    if (gridFitler != null) {
                        filter = gridFitler.filters;
                        filter.forEach(function (f) {
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
                s.columns.forEach(function (col) {
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
        };
        Helpers.WatchGridForChanges = function (grid, func) {
            grid.bind("columnResize", function () {
                if (grid.options["Updating"])
                    return;
                func();
            });
            grid.bind("columnReorder", function () {
                if (grid.options["Updating"])
                    return;
                func();
            });
            grid.bind("columnHide", function () {
                if (grid.options["Updating"])
                    return;
                func();
            });
            grid.bind("columnShow", function () {
                if (grid.options["Updating"])
                    return;
                func();
            });
            grid.bind("columnLock", function () {
                if (grid.options["Updating"])
                    return;
                func();
            });
            grid.bind("columnUnlock", function () {
                if (grid.options["Updating"])
                    return;
                func();
            });
            grid.dataSource.bind("change", function (e) {
                if (grid.options["Updating"])
                    return;
                func();
            });
        };
        Helpers.PersistDataSourceSettingsToUrl = function (ds, setCurrentUrl) {
            var params = "";
            //Build up the url based on the sort etc
            if (ds.sort()) {
                ds.sort().forEach(function (item) {
                    if ((item.field == "SubmittedOn" && item.dir == "desc"))
                        return;
                    params += "&sort=" + encodeURIComponent(item.field) + "|" + encodeURIComponent(item.dir);
                });
            }
            if (ds.filter()) {
                ds.filter().filters.forEach(function (item) {
                    params += "&filter=" + encodeURIComponent(item.field) + "|" + encodeURIComponent(item.operator) + "|" + encodeURIComponent(item.value);
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
        };
        Helpers.GetFiltersFromUrl = function () {
            var filter = Global.GetQueryParam("filter");
            if (filter) {
                if (!$.isArray(filter))
                    filter = $.makeArray(filter);
                var filters = [];
                filter.forEach(function (item) {
                    var args = item.split("|");
                    filters.push({ field: args[0], operator: args[1], value: args[2] });
                });
                return filters;
            }
            return null;
        };
        Helpers.GetSortsFromUrl = function () {
            var sort = Global.GetQueryParam("sort");
            if (sort) {
                var sorts = [];
                if (!$.isArray(sort))
                    sort = $.makeArray(sort);
                sort.forEach(function (item) {
                    var args = item.split("|");
                    sorts.push({ field: args[0], dir: args[1] });
                });
                return sorts;
            }
            return null;
        };
        Helpers.GetServiceUrl = function (relativeUrl) {
            if ($.support.cors) {
                return ServiceUrl + relativeUrl;
            }
            else {
                return "/api/get?Url=" + encodeURIComponent(relativeUrl);
            }
        };
        Helpers.PostServiceUrl = function (relativeUrl) {
            if ($.support.cors) {
                return ServiceUrl + relativeUrl;
            }
            else {
                return "/api/post?Url=" + encodeURIComponent(relativeUrl);
            }
        };
        Helpers.PutServiceUrl = function (relativeUrl) {
            if ($.support.cors) {
                return ServiceUrl + relativeUrl;
            }
            else {
                return "/api/put?Url=" + encodeURIComponent(relativeUrl);
            }
        };
        Helpers.DeleteServiceUrl = function (relativeUrl) {
            if ($.support.cors) {
                return ServiceUrl + relativeUrl;
            }
            else {
                return "/api/delete?Url=" + encodeURIComponent(relativeUrl);
            }
        };
        Helpers.decodeBase64 = function (s) {
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
        };
        Helpers.encodebase64 = function (data) {
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
        };
        Helpers.ShowDialog = function (title, url, actions, width, height, parameters) {
            if (actions === void 0) { actions = null; }
            if (parameters === void 0) { parameters = null; }
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
                    var win = kendoWindow.data("kendoWindow");
                    //win.setOptions({
                    //    //Center to the view port
                    //    position: {
                    //        left: (window.innerWidth - kendoWindow.width()) / 2,
                    //        top: (window.innerHeight - kendoWindow.height()) / 2 - window.pageYOffset
                    //    }
                    //});
                    kendoWindow.data("kendoWindow").center().open();
                },
                deactivate: function () {
                    this.destroy();
                },
                parameters: parameters,
                //open: function () {
                //    // Kendo window's center does not work correctly. This allows it to somewhat center it vertically.
                //    //kendoWindow.parent().css("top", "50%").css("padding-top", "0px");
                // 
                //},
                close: function () {
                    deferred.resolve(kendoWindow.data("kendoWindow").options.returnResults);
                }
            });
            //kendoWindow.data("kendoWindow").center().open();
            return deferred;
        };
        Helpers.ShowToast = function (message) {
            var d = jQuery.Deferred();
            var notificationWidget = $("#NotificationsKendo").kendoNotification().data("kendoNotification");
            notificationWidget.show(message, 'info');
            return d;
        };
        Helpers.GetDialogWindow = function () {
            return this.GetDialogWindowElement().data("kendoWindow");
        };
        Helpers.GetDialogWindowElement = function () {
            var dialog = window.parent.$("div[data-url^='" + window.location.pathname + "']");
            return dialog;
        };
        Helpers.ShowAlert = function (title, html, width, actions) {
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
            kendoWindow.find("#btnOK").click(function (e) {
                kendoWindow.data("kendoWindow").close();
                d.resolve();
            }).end();
            return d;
        };
        Helpers.ShowErrorAlert = function (title, error, width) {
            this.ShowAlert(title, this.ProcessAjaxError(error), width, ["Close"]);
        };
        Helpers.ProcessAjaxError = function (error) {
            if (error.responseText != null && error.responseText.toLowerCase().indexOf("<body") > -1)
                return error.responseText;
            if (typeof error == "string")
                return error;
            var errorString = "Status: " + error.status + "<br/>Server Message: " + error.statusText;
            //Should parse the error text etc. from the array
            if (error.responseText != null && error.responseText != "") {
                try {
                    var result = JSON.parse(error.responseText);
                    if (result.results != null) {
                        errorString = "";
                        for (var j = 0; j < result.results.length; j++) {
                            var r = result.results[j];
                            r.errors.forEach(function (item) {
                                errorString += (errorString == "" ? "" : "<br/>") + item.Description.replace(/(?:\r\n|\r|\n)/g, '<br />');
                            });
                        }
                    }
                    else if (result.Message != null) {
                        errorString = result.Message.replace(/(?:\r\n|\r|\n)/g, '<br />') + "<br/><br/>" + result.MessageDetail.replace(/(?:\r\n|\r|\n)/g, '<br />');
                    }
                    else if (result.errors != null) {
                        errorString = "";
                        result.errors.forEach(function (error) {
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
        };
        //Confirms an operation
        Helpers.ShowConfirm = function (title, html) {
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
                .click(function (e) {
                window.close();
                if ((e.srcElement || e.target).id == "confirmYes") {
                    d.resolve();
                }
                else {
                    d.reject();
                }
            }).end();
            return d;
        };
        Helpers.ShowPrompt = function (title, promptMessage, width, singleLine) {
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
                .click(function (e) {
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
        };
        Helpers.ShowExecuting = function () {
            kendo.ui.progress($("#Content"), true);
        };
        Helpers.HideExecuting = function () {
            kendo.ui.progress($("#Content"), false);
        };
        Helpers.formatFileSize = function (length) {
            if (length > this.gigabyte)
                return (length / this.gigabyte).toFixed(2) + ' Gb';
            if (length > this.megabyte)
                return (length / this.megabyte).toFixed(2) + ' Mb';
            if (length > this.kilobyte)
                return (length / this.kilobyte).toFixed(2) + ' Kb';
            return length + ' bytes';
        };
        /*Checks the specified url has a value, and appends a / to it if missing before initiating the redirect.*/
        Helpers.RedirectTo = function (url) {
            if (url == null || url == undefined || url == '')
                return;
            var startsWith = function (value) {
                return url.slice(0, value.length).toLowerCase() == value.toLowerCase();
            };
            if (startsWith('http') || startsWith('https') || startsWith('/')) {
                window.location.href = url;
            }
            else {
                window.location.href = '/' + url;
            }
        };
        Helpers.StartsWith = function (content, value) {
            content = content || '';
            value = value || '';
            if (content.length == 0 || value.length == 0)
                return false;
            return content.slice(0, value.length).toLowerCase() == value.toLowerCase();
        };
        /* Replaces the current page history item with new details */
        Helpers.HistoryReplaceState = function (pageTitle, uri) {
            if (Helpers.StartsWith(uri, '/') == false)
                uri = '/' + uri;
            window.history.replaceState(null, pageTitle, uri);
        };
        /** Helper that replaces filter parameter values with a custom object that allows for specific formatting of the odata filter string for the specified field. */
        Helpers.UpdateKendoGridFilterOptions = function (options, customFilterStrings) {
            var updateFilterValue = function (filters) {
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
        };
        return Helpers;
    }());
    Helpers.chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
    Helpers.kilobyte = 1024;
    Helpers.megabyte = 1024 * 1024;
    Helpers.gigabyte = 1024 * 1024 * 1024;
    Global.Helpers = Helpers;
    /** Custom class that overrides the default toString implementation to return the value specified using the a specific string format. */
    var ODataCustomFilterValueFormatter = (function (_super) {
        __extends(ODataCustomFilterValueFormatter, _super);
        function ODataCustomFilterValueFormatter(value, format) {
            var _this = _super.call(this) || this;
            _this.value = value;
            _this.format = format || '';
            return _this;
        }
        ODataCustomFilterValueFormatter.prototype.toString = function () {
            if (this.format.length == 0)
                return this.value;
            return kendo.format(this.format, this.value);
        };
        return ODataCustomFilterValueFormatter;
    }(Object));
    Global.ODataCustomFilterValueFormatter = ODataCustomFilterValueFormatter;
    function ShowTerms(params) {
        var param;
        if (params == null) {
            param = "<p>This Distributed Query Tool is for use by authorized HMO Research Network investigators and staff, and affiliated authorized partners. This resource is intended for preparatory to research or preliminary (feasibility) data queries. All results are aggregated across participating health plans. No attempt should be made to identify data from individual health plans.</p><p> Data from the Distributed Query Tool cannot be published without prior approval.External recipients of data from this tool must agree to these terms and conditions and all data shared must be marked CONFIDENTIAL and sent securely.</p ><p>The privilege to use this tool may be revoked at any time.</p >";
        }
        else {
            param = params;
        }
        Helpers.ShowDialog("Terms And Conditions", "/home/termsandconditions", ["close"], 800, 600, param).done(function () { });
    }
    Global.ShowTerms = ShowTerms;
    function ShowInfo(params) {
        var param;
        if (params == null) {
            param = "<p>Funded under Contract No. 290-05-0033 from the Agency for Healthcare Research and Quality, US Department of Health and Human Services as part of the Developing Evidence to Inform Decisions about Effectiveness (DEcIDE) program, awarded to the DEcIDE centers at the HMO Research Network Center for Education and Research on Therapeutics (HMORN CERT) and the University of Pennsylvania. PI: Richard Platt.</p>";
        }
        else {
            param = params;
        }
        Helpers.ShowDialog("Information", "/home/info", ["close"], 800, 600, param).done(function () { });
    }
    Global.ShowInfo = ShowInfo;
    //Gets or sets session data
    function Session(key, data) {
        if (data) {
            sessionStorage.setItem(key, JSON.stringify(data));
        }
        else {
            return JSON.parse(sessionStorage.getItem(key));
        }
    }
    Global.Session = Session;
    function SessionRemoveKey(key) {
        sessionStorage.removeItem(key);
    }
    Global.SessionRemoveKey = SessionRemoveKey;
    //Gets or sets local data
    function Local(key, data) {
        if (data) {
            localStorage.setItem(key, JSON.stringify(data));
        }
        else {
            return JSON.parse(localStorage.getItem(key));
        }
    }
    Global.Local = Local;
    //Gets a query parameter and is not case sensitive.
    function GetQueryParam(param) {
        return $.url(window.location.href.toLowerCase()).param(param.toLowerCase());
    }
    Global.GetQueryParam = GetQueryParam;
})(Global || (Global = {}));
var User = new Global.UserInfo();
var Constants;
(function (Constants) {
    Constants.GuidEmpty = "00000000-0000-0000-0000-000000000000";
    Constants.DateFormat = "MM/d/yyyy";
    Constants.DateFormatter = "{0:MM/d/yyyy}";
    Constants.DateTimeFormat = "MM/d/yyyy h:mm tt";
    Constants.DateTimeFormatter = "{0:MM/d/yyyy h:mm tt}";
    Constants.LineBreak = "<br/><br/>";
    function FormatDate(date) {
        if (!date)
            return "";
        if (!(date instanceof Date))
            date = new Date(date);
        return kendo.toString(date, Constants.DateFormat);
    }
    Constants.FormatDate = FormatDate;
    function FormatDateTime(date) {
        if (!date)
            return "";
        if (!(date instanceof Date))
            date = new Date(date);
        return kendo.toString(date, Constants.DateTimeFormat);
    }
    Constants.FormatDateTime = FormatDateTime;
    var Guid = (function () {
        function Guid() {
        }
        Guid.newGuid = function () {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        };
        return Guid;
    }());
    Constants.Guid = Guid;
})(Constants || (Constants = {}));
if ((typeof Dns != 'undefined') && Dns.WebApi != null)
    Dns.WebApi.Helpers.RegisterFailMethod(function (error) {
        Global.Helpers.ShowErrorAlert("Error", error, 800);
    });
$(function () {
    if (!("autofocus" in document.createElement("input"))) {
        $("[autofocus]").focus();
    }
});
$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        if (!settings || !settings.type)
            return;
        switch (settings.type.toLowerCase()) {
            case "post":
            case "put":
            case "delete":
            case "patch":
                Global.Helpers.ShowExecuting();
                break;
        }
    },
    complete: function (xhr) {
        Global.Helpers.HideExecuting();
    }
});
//# sourceMappingURL=Page.js.map