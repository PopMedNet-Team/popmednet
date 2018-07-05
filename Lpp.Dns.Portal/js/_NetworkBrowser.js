/// <reference path="_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var NetworkBrowser;
(function (NetworkBrowser) {
    var vm;
    var ViewModel = /** @class */ (function (_super) {
        __extends(ViewModel, _super);
        function ViewModel(bindingControl) {
            var _this = _super.call(this, bindingControl) || this;
            _this.valNetworkBrowser = ko.observable(false);
            _this.colNetworkDraftRequest = ko.observable(false);
            _this.colNetworkRecentRequest = ko.observable(false);
            _this.colNetworkSubmittedRequest = ko.observable(false);
            _this.colNetworkCompletedRequest = ko.observable(false);
            _this.dsNetworkDraftRequest = new kendo.data.HierarchicalDataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 10,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/requests/list?$filter=Status eq Lpp.Dns.DTO.Enums.RequestStatuses'Draft'"),
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: function () {
                            return false;
                        }
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/Request/details?ID=";
                        }
                        return response;
                    }
                }
            });
            _this.SelectedRequestsDrafts = function (e) {
                var data = $('#RequestDrafttreeview').data('kendoTreeView').dataItem(e.node);
                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;
                return true;
            };
            _this.dsNetworkSubmittedRequest = new kendo.data.HierarchicalDataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 10,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/requests/list?$filter=Status eq Lpp.Dns.DTO.Enums.RequestStatuses'Resubmitted'"),
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: function () {
                            return false;
                        }
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/Request/details?ID=";
                        }
                        return response;
                    }
                }
            });
            _this.SelectedRequestsSubmittd = function (e) {
                var data = $('#RequestSubmittedtreeview').data('kendoTreeView').dataItem(e.node);
                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;
                return true;
            };
            _this.dsNetworkRecentRequest = new kendo.data.HierarchicalDataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 10,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/requests/list"),
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: function () {
                            return false;
                        }
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/Request/details?ID=";
                        }
                        return response;
                    }
                },
                sort: { field: "SubmittedOn", dir: "desc" }
            });
            _this.SelectedRequestsRecent = function (e) {
                var data = $('#RequestRecenttreeview').data('kendoTreeView').dataItem(e.node);
                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;
                return true;
            };
            _this.dsNetworkCompletedRequest = new kendo.data.HierarchicalDataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 10,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/requests/list?$filter=Status eq Lpp.Dns.DTO.Enums.RequestStatuses'Complete'"),
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: function () {
                            return false;
                        }
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/Request/details?ID=";
                        }
                        return response;
                    }
                },
                sort: { field: "SubmittedOn", dir: "desc" }
            });
            _this.SelectedRequestsCompleted = function (e) {
                var data = $('#RequestCompletedtreeview').data('kendoTreeView').dataItem(e.node);
                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;
                return true;
            };
            var dsNetworkProjectsSecurity = {
                type: "webapi",
                loadOnDemand: true,
                select: function (e) {
                    window.location.href = "/SecurityGroups/detail/" + e.id;
                },
                transport: {
                    read: {
                        url: function (options) {
                            return kendo.format(Global.Helpers.GetServiceUrl("/SecurityGroups/list?$filter=OwnerID eq " + options.ID));
                        }
                    }
                },
                schema: {
                    model: {
                        hasChildren: function () {
                            return false;
                        }
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/SecurityGroups/details?ID=";
                        }
                        return response;
                    }
                }
            };
            var dsNetworkGroupProjects = {
                type: "webapi",
                loadOnDemand: true,
                transport: {
                    read: {
                        url: function (options) {
                            return kendo.format(Global.Helpers.GetServiceUrl("/projects/list?$filter=GroupID eq Guid" + "'" + options.ID + "'"));
                        }
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: true,
                        children: dsNetworkProjectsSecurity
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/projects/details?ID=";
                        }
                        return response;
                    }
                },
            };
            _this.dsNetworkGroups = new kendo.data.HierarchicalDataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/groups/list"),
                    },
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: true,
                        children: dsNetworkGroupProjects
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/groups/details?ID=";
                        }
                        return response;
                    }
                },
            });
            _this.SelectedGroup = function (e) {
                var data = $('#Grouptreeview').data('kendoTreeView').dataItem(e.node);
                //console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;
                return true;
            };
            var dsNetworkOrganizationsUsers = {
                type: "webapi",
                loadOnDemand: true,
                select: function (e) {
                    window.location.href = "/users/detail/" + e.id;
                },
                transport: {
                    read: {
                        url: function (options) {
                            return kendo.format(Global.Helpers.GetServiceUrl("/users/list?$filter=OrganizationID eq Guid" + "'" + options.ID + "'"));
                        }
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: false,
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/users/details?ID=";
                        }
                        return response;
                    }
                },
            };
            _this.dsNetworkOrganizations = new kendo.data.HierarchicalDataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/organizations/list"),
                    }
                },
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: true,
                        children: dsNetworkOrganizationsUsers
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.results.length; i++) {
                            response.results[i].treenodeurl = "/organizations/details?ID=";
                        }
                        return response;
                    }
                },
            });
            _this.SelectedOrganization = function (e) {
                var data = $('#Organizationtreeview').data('kendoTreeView').dataItem(e.node);
                //console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;
                return true;
            };
            return _this;
        }
        ViewModel.prototype.onClick = function (value) {
            this.valNetworkBrowser(!this.valNetworkBrowser());
        };
        return ViewModel;
    }(Global.PageViewModel));
    NetworkBrowser.ViewModel = ViewModel;
    function init() {
        $(function () {
            var bindingControl = $("#efNetworkBrowser");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }
    init();
})(NetworkBrowser || (NetworkBrowser = {}));
//# sourceMappingURL=_NetworkBrowser.js.map