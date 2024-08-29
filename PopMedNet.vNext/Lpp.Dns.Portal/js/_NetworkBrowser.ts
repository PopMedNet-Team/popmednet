/// <reference path="_rootlayout.ts" />

declare module kendo.data {
    interface Node {
        treenodeurl?: string;
    }
}

module NetworkBrowser {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public valNetworkBrowser: KnockoutObservable<boolean>;
        public dsNetworkDraftRequest: kendo.data.DataSource;
        public dsNetworkRecentRequest: kendo.data.DataSource;
        public dsNetworkSubmittedRequest: kendo.data.DataSource;
        public dsNetworkCompletedRequest: kendo.data.DataSource;
        public colNetworkDraftRequest: KnockoutObservable<boolean>;
        public colNetworkRecentRequest: KnockoutObservable<boolean>;
        public colNetworkSubmittedRequest: KnockoutObservable<boolean>;
        public colNetworkCompletedRequest: KnockoutObservable<boolean>;
        public dsNetworkOrganizations: kendo.data.DataSource;
        public dsNetworkGroups: kendo.data.DataSource;
        public SelectedGroup: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public SelectedOrganization: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public SelectedRequestsDrafts: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public SelectedRequestsRecent: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public SelectedRequestsSubmittd: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public SelectedRequestsCompleted: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        constructor(bindingControl: JQuery) {
            super(bindingControl);
            this.valNetworkBrowser = ko.observable(false);

            this.colNetworkDraftRequest = ko.observable(false);
            this.colNetworkRecentRequest = ko.observable(false);
            this.colNetworkSubmittedRequest = ko.observable(false);
            this.colNetworkCompletedRequest = ko.observable(false);
            this.dsNetworkDraftRequest = new kendo.data.HierarchicalDataSource({
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
            this.SelectedRequestsDrafts = (e: kendo.ui.TreeViewSelectEvent) => {
                var data = $('#RequestDrafttreeview').data('kendoTreeView').dataItem(e.node);

                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;

                return true;
            }
            this.dsNetworkSubmittedRequest = new kendo.data.HierarchicalDataSource({
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
            this.SelectedRequestsSubmittd = (e: kendo.ui.TreeViewSelectEvent) => {
                var data = $('#RequestSubmittedtreeview').data('kendoTreeView').dataItem(e.node);

                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;

                return true;
            }
            this.dsNetworkRecentRequest = new kendo.data.HierarchicalDataSource({
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
            this.SelectedRequestsRecent = (e: kendo.ui.TreeViewSelectEvent) => {
                var data = $('#RequestRecenttreeview').data('kendoTreeView').dataItem(e.node);

                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;

                return true;
            }
            this.dsNetworkCompletedRequest = new kendo.data.HierarchicalDataSource({
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
            this.SelectedRequestsCompleted = (e: kendo.ui.TreeViewSelectEvent) => {
                var data = $('#RequestCompletedtreeview').data('kendoTreeView').dataItem(e.node);

                console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;

                return true;
            }

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



            this.dsNetworkGroups = new kendo.data.HierarchicalDataSource({
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
            this.SelectedGroup = (e: kendo.ui.TreeViewSelectEvent) => {
                var data = $('#Grouptreeview').data('kendoTreeView').dataItem(e.node);

                //console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;

                return true;
            }

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
            this.dsNetworkOrganizations = new kendo.data.HierarchicalDataSource({
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
            this.SelectedOrganization = (e: kendo.ui.TreeViewSelectEvent) => {
                var data = $('#Organizationtreeview').data('kendoTreeView').dataItem(e.node);

                //console.log(data.id);
                var url = (data.treenodeurl + data.id);
                location.href = url;

                return true;
            }

        }

        public onClick(value) {
            this.valNetworkBrowser(!this.valNetworkBrowser())
        }
    }



    function init() {
        $(() => {
            var bindingControl = $("#efNetworkBrowser");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 