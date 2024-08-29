import * as signalR from "@microsoft/signalr";
import { CompositeFilterDescriptor, process, SortDescriptor, State, DataResult } from '@progress/kendo-data-query';
import '@progress/kendo-theme-default/dist/all.css';
import '@progress/kendo-ui';
import { Grid, GridRowClickEvent, filterGroupByField, GridColumnProps } from '@progress/kendo-vue-grid';
import axios from 'axios';
import Vue from 'vue';
import { ChangeNotification, ChangeType, Helpers, NotificationEventIdentifiers, RoutingDTO, RoutingListViewModel } from './interfaces';
import ColumnMenu from './components/ColumnMenu';

Vue.component('kendo-grid', Grid);
Vue.component('custom-menu', ColumnMenu);

interface DataModel {
    res: RoutingListViewModel[];
    columns: GridColumnProps[];
    scrollable: boolean;
    columnMenu: boolean;
    sort: SortDescriptor[];
    sortable: any;
    filter: CompositeFilterDescriptor | undefined;
    filterable: boolean;
    reorderable: boolean;
    resizable: boolean;
    connection: signalR.HubConnection | null;
}

Promise.all([
    axios.get<RoutingDTO[]>('/api/routes'),
]).then((responses) => {
    const response = responses[0];
    let routes = <RoutingListViewModel[]>response.data.map(d => new RoutingListViewModel(d));

    let vue = new Vue({
        el: '#vue_requests',
        data: function (): DataModel {
            return {
                res: routes,                
                columns: [
                    { field: 'project', title: 'Project', columnMenu: 'customMenuTemplate' },
                    { field: 'dataMartName', title: 'DataMart', columnMenu: 'customMenuTemplate' },
                    { field: 'msRequestID', title: 'Request ID', columnMenu: 'customMenuTemplate' },
                    { field: 'status', title: 'Status', columnMenu: 'customMenuTemplate' },
                    { field: 'priority', title: 'Priority', columnMenu: 'customMenuTemplate' },
                    { field: 'dueDateLocal', title: 'Due Date', format: '{0:MMM d, yyyy}', filter: 'date', columnMenu: 'customMenuTemplate' },
                    { field: 'requestName', title: 'Name', columnMenu: 'customMenuTemplate' },
                    { field: 'requestType', title: 'Request Type', hidden: true, columnMenu: 'customMenuTemplate' },
                    { field: 'dataModel', title: 'Data Model', hidden: true, columnMenu: 'customMenuTemplate' },
                    { field: 'requestIdentifier', title: 'System Number', format: '{0:############}', columnMenu: 'customMenuTemplate' },
                    { field: 'requestDateLocal', title: 'Submitted On', format: '{0:MMM d, yyyy}', filter: 'date', columnMenu: 'customMenuTemplate' },
                    { field: 'submittedBy', title: 'Requested By', hidden: true, columnMenu: 'customMenuTemplate' },
                    { field: 'respondedBy', title: 'Responder', columnMenu: 'customMenuTemplate' },
                    { field: 'respondedDateLocal', title: 'Response Time', filter: 'date', format: '{0:MMM d, yyyy} {0:t}', columnMenu: 'customMenuTemplate' }
                ],
                columnMenu: true,
                scrollable: true,
                sort: [
                    { field: 'requestIdentifier', dir: 'desc' }
                ],
                sortable: {
                    allowUnsort: false,
                    mode: 'single'
                },
                filter: undefined,
                filterable: false,
                reorderable: true,
                resizable: true,
                connection: null,
            };
        },
        computed: {
            result(): DataResult {
                const self = this;
                const state: State = {
                    filter: self.filter,
                    sort: self.sort,
                    group: []
                }
                return process(self.res, state)
            }
        },
        methods: {
            onSortChange: function (newSort: SortDescriptor[], e) {
                this.sort = newSort;
            },
            onFilterChange: function (newFilter: CompositeFilterDescriptor, e) {
                let changedColumn = this.columns.find(function (column) {
                    return column.field === e.field;
                });

                if (changedColumn) {
                    let isActiveColumn = filterGroupByField(e.field, newFilter);
                    changedColumn.headerClassName = isActiveColumn ? "active" : "";
                }

                this.filter = newFilter;
            },
            onRowClick: function (e: GridRowClickEvent) {
                let evt = e.event as PointerEvent;

                evt.cancelBubble = true;
                evt.stopPropagation();

                if (evt.ctrlKey) {
                    //open in a new tab
                    window.open("/route/" + e.dataItem.id, "_blank");
                } else {
                    window.location.href = "/route/" + e.dataItem.id;
                }
            },
            onRequestChanged: function (e: ChangeNotification<RoutingDTO>) {
                const self = this;
                if (e.changeType == ChangeType.Added) {
                    self.res.push(new RoutingListViewModel(e.entity));
                }
                if (e.changeType == ChangeType.Updated || e.changeType == ChangeType.Deleted) {
                    self.res = self.res.filter((val) => { return val.id !== e.entity.id });
                }
                if (e.changeType == ChangeType.Updated) {
                    self.res.push(new RoutingListViewModel(e.entity));
                }
            },
            onColumnsSubmit(columnsState) {
                this.columns = columnsState;
            }
        },
        created() {
            const self = this;
            self.connection = new signalR.HubConnectionBuilder()
                .withUrl("/RequestHub")
                .configureLogging(signalR.LogLevel.Information)
                .build();
            self.connection.start().then(() => {
                self.connection!.invoke("ConnectedToRequestList").catch(function (innerErr) {
                    return console.error(innerErr.toString());
                });
            }).catch(function (err) {
                return console.error(err.toSting());
            });
        },
        mounted() {
            const self = this;
            self.connection!.on(NotificationEventIdentifiers.RequestList_RequestListUpdated, self.onRequestChanged);

            self.$nextTick(function () {
                Helpers.removeLoadingPanel();
            });
        }
    });

}).catch((error) => {
    debugger;
});