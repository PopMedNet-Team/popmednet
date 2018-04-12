﻿module Plugins.Requests.QueryBuilder.DataMartRouting {
    export var vm: ViewModel;

    export class Routings {
        //binding fields here
        public Priority: KnockoutObservable<Dns.Enums.Priorities>;
        public DueDate: KnockoutObservable<Date>;
        public Name: string;
        public Organization: string;
        public OrganizationID: any;
        public DataMartID: any;
        public RequestID: any;


        private _existingRequestDataMart: Dns.Interfaces.IRequestDataMartDTO;

        public toRequestDataMartDTO: () => Dns.Interfaces.IRequestDataMartDTO;

        constructor(dataMart: Dns.Interfaces.IDataMartListDTO, existingRequestDataMart: Dns.Interfaces.IRequestDataMartDTO) {
            this.Priority = ko.observable(existingRequestDataMart != null ? existingRequestDataMart.Priority : dataMart.Priority);
            this.DueDate = ko.observable(existingRequestDataMart != null ? existingRequestDataMart.DueDate : dataMart.DueDate);

            this.Name = dataMart.Name;
            this.Organization = dataMart.Organization;
            this.OrganizationID = dataMart.OrganizationID;
            this.DataMartID = dataMart.ID;
            this._existingRequestDataMart = existingRequestDataMart;

            var self = this;
            self.toRequestDataMartDTO = () => {

                var route: Dns.Interfaces.IRequestDataMartDTO = null;
                if (self._existingRequestDataMart != null) {
                    //do a deep copy clone of the existing routing information;
                    route = <Dns.Interfaces.IRequestDataMartDTO>jQuery.extend(true, {}, self._existingRequestDataMart);
                } else {
                    route = new Dns.ViewModels.RequestDataMartViewModel().toData();
                    route.DataMartID = self.DataMartID;
                    route.RequestID = self.RequestID;
                    route.DataMart = self.Name;
                }

                route.Priority = self.Priority();
                route.DueDate = self.DueDate();

                return route;
            };
        }
    }

    export class ViewModel extends Global.PageViewModel {
        DataMarts: KnockoutObservableArray<Plugins.Requests.QueryBuilder.DataMartRouting.Routings>;
        LoadDataMarts: (ProjectID: any, strQuery: string) => void;
        public DataMartsBulkEdit: () => void;
        public SelectedRoutings: () => Dns.Interfaces.IRequestDataMartDTO[];
        public SelectedDataMartIDs: KnockoutObservableArray<any>;
        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];
        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;
        public ExistingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[];
        public DefaultPriority: KnockoutObservable<Dns.Enums.Priorities>;
        public DefaultDueDate: KnockoutObservable<Date>;
        public DataMartAdditionalInstructions: KnockoutObservable<string>;
        private RoutesSelectAll: KnockoutComputed<boolean>;

        constructor(
            bindingControl: JQuery,
            fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
            existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
            defaultDueDate: Date,
            defaultPriority: Dns.Enums.Priorities,
            additionalInstructions: string
        ){
            super(bindingControl);
            var self = this;
            this.DataMarts = ko.observableArray([]);
            this.ExistingRequestDataMarts = existingRequestDataMarts || [];
            this.SelectedDataMartIDs = ko.observableArray(ko.utils.arrayMap(self.ExistingRequestDataMarts, (exdm) => exdm.DataMartID));

            this.DefaultPriority = ko.observable(defaultPriority);
            this.DefaultDueDate = ko.observable(defaultDueDate);
            this.DataMartAdditionalInstructions = ko.observable(additionalInstructions || '');



            this.DataMarts = ko.observableArray<Plugins.Requests.QueryBuilder.DataMartRouting.Routings>();
            this.LoadDataMarts = (projectID: any, strQuery: string) => {
                Dns.WebApi.Requests.GetCompatibleDataMarts({
                    TermIDs: null,
                    ProjectID: projectID,
                    Request: strQuery,
                    RequestID: Global.GetQueryParam("ID")
                }).done((dataMarts) => {
                    let routes = [];
                    for (let di = 0; di < dataMarts.length; di++) {
                        let dm = dataMarts[di];
                        dm.Priority = self.DefaultPriority();
                        dm.DueDate = self.DefaultDueDate();

                        let existingRoute = ko.utils.arrayFirst(self.ExistingRequestDataMarts, (r) => r.DataMartID == dm.ID);

                        routes.push(new Plugins.Requests.QueryBuilder.DataMartRouting.Routings(dm, existingRoute));
                    }

                    self.DataMarts(routes);

                });
            }

            this.SelectedRoutings = () => {
                let dms: Dns.Interfaces.IRequestDataMartDTO[];
                dms = ko.utils.arrayMap(ko.utils.arrayFilter(self.DataMarts(), (route) => {
                    return self.SelectedDataMartIDs.indexOf(route.DataMartID) > -1;
                }), (route) => route.toRequestDataMartDTO());

                return dms;
            };

            this.DataMartsBulkEdit = () => {
                let defaultDueDate = self.DefaultDueDate() != null ? new Date(self.DefaultDueDate().getTime()) : null;
                Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: self.DefaultPriority(), defaultDueDate: defaultDueDate })
                    .done((result: any) => {
                        if (result != null) {

                            let priority: Dns.Enums.Priorities = null;
                            if (result.UpdatePriority) {
                                priority = result.PriorityValue;
                            }

                            if (result.UpdatePriority || result.UpdateDueDate) {
                                ko.utils.arrayFilter(self.DataMarts(), (route) => {
                                    return self.SelectedDataMartIDs.indexOf(route.DataMartID) > -1;
                                }).forEach((route) => {
                                    if (priority != null)
                                        route.Priority(priority);
                                    if (result.UpdateDueDate)
                                        route.DueDate(new Date(result.stringDate));
                                });
                            }

                        }
                    });

            };

            self.FieldOptions = fieldOptions || [];

            self.IsFieldRequired = (id: string) => {
                let options = ko.utils.arrayFirst(self.FieldOptions || [], (item) => { return item.FieldIdentifier == id; });
                return options != null && options.Permission != null && options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                let options = ko.utils.arrayFirst(self.FieldOptions || [], (item) => { return item.FieldIdentifier == id; });
                return options == null || (options.Permission != null && options.Permission != Dns.Enums.FieldOptionPermissions.Hidden);
            };

            self.RoutesSelectAll = ko.pureComputed<boolean>({
                read: () => {
                    return self.DataMarts().length > 0 && self.SelectedDataMartIDs().length === self.DataMarts().length;
                },
                write: (value) => {
                    if (value) {
                        let allID = ko.utils.arrayMap(self.DataMarts(), (i) => { return i.DataMartID; });
                        self.SelectedDataMartIDs(allID);
                    } else {
                        self.SelectedDataMartIDs([]);
                    }
                }
            });  
        }

        public UpdateRoutings(updates) {
            let newPriority = updates != null ? updates.newPriority : null;
            let newDueDate = updates != null ? updates.newDueDate : null;

            this.DefaultDueDate(newDueDate);
            this.DefaultPriority(newPriority);

            this.DataMarts().forEach((dm) => {
                if (newPriority != null) {
                    dm.Priority(newPriority);
                }
                if (newDueDate != null) {
                    dm.DueDate(newDueDate);
                }
            });
        }

    }

    export function init(
        bindingControl: JQuery,
        fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
        existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
        defaultDueDate: Date,
        defaultPriority: Dns.Enums.Priorities,
        additionalInstructions: string
    ) {
        //let bindingControl = $('#DataMartsControl');
        vm = new Plugins.Requests.QueryBuilder.DataMartRouting.ViewModel(bindingControl, fieldOptions, existingRequestDataMarts, defaultDueDate, defaultPriority, additionalInstructions);
        ko.applyBindings(vm, $('#DataMartsControl')[0]);
    }
}