import * as signalR from "@microsoft/signalr";
import { DataResult, GroupDescriptor, process, SortDescriptor, State, CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { DatePicker } from '@progress/kendo-vue-dateinputs';
//import '@progress/kendo-theme-default/dist/all.css';
//import '@progress/kendo-ui';
import { Dialog, DialogActionsBar } from '@progress/kendo-vue-dialogs';
import { Grid, GridColumnProps } from '@progress/kendo-vue-grid';
import { Button } from '@progress/kendo-vue-buttons';
import Axios from 'axios';
import moment from 'moment';
import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { LogMessageDTO, LogMessageListViewModel } from '../interfaces';

enum LogTypes {
    All = 0,
    Specific = 1,
}

@Component({
    template: '#logging-grid',
    components: {
        'kendo-grid': Grid,
        'kendo-dialog': Dialog,
        'kendo-action': DialogActionsBar,
        'kendo-date': DatePicker,
        'kbutton': Button
    }
})
export default class LogViewer extends Vue {
    @Prop({ default: null })
    responseid!: any;

    logItems: LogMessageListViewModel[] = [];

    columns: GridColumnProps[] = [
        { field: 'dateTime', title: 'Date Time', filterable: false, format: '{0:MMM d, yyyy} {0:t}' },
        { field: 'level', title: 'Level', sortable: true },
        { field: 'message', title: 'Message', sortable: false, cell: this.renderMessage }
    ];

    columnMenu: boolean = false;

    sort: SortDescriptor[] = [
        { field: 'dateTime', dir: 'desc' }
    ];

    sortable: any = {
        allowUnsort: false,
        mode: 'single'
    };

    group: GroupDescriptor[] = [];

    groupable: boolean = false;

    filter: CompositeFilterDescriptor | undefined = { logic: "and", filters:[] };

    filterable: boolean = false;
    filterTrigger: number = 0;

    reorderable: boolean = false;

    resizable: boolean = true;

    LogType: LogTypes = LogTypes.All;

    OpenExportDialog: boolean = false;

    ExportStartDate: Date = moment().add(-15, 'days').toDate();

    ExportEndDate: Date = moment().toDate();

    connection!: signalR.HubConnection | null;    

    /**
     * @description
     * Returns the filtered dataset the grid is bound to.
     */
    get result(): DataResult {
        const self = this;
        this.filterTrigger;
        const state: State = {
            filter: self.filter,
            sort: self.sort
        }
        return process(self.logItems, state);
    }

    renderMessage(h, tdElement, props, clickHandler) {
        let msg = props.dataItem.message.replace(/(\r\n|\r|\n)/gm, '<br\/>');        
        return h('td', { domProps: { innerHTML: msg } });
    }

    onAddResponseLog(msg: LogMessageDTO) {
        this.logItems.push(new LogMessageListViewModel(msg));
    }

    sortChangeHandler(e) {
        this.sort = e.sort;
    }

    filterChangeHandler(e) {
        this.filter = e.filter;
        this.filterTrigger++;
    }

    resetExport() {
        const self = this;
        self.LogType = LogTypes.All;
        self.ExportStartDate = moment().add(-15, 'days').toDate();
        self.ExportEndDate = moment().toDate();
    }

    onCloseExportDialog() {
        const self = this;
        this.OpenExportDialog = false;
        self.resetExport();
    }

    onOpenExportDialog() {
        this.OpenExportDialog = true;
    }

    onSendExport() {
        const self = this;
        this.OpenExportDialog = false;

        let href = "";

        if (self.responseid !== null) {
            if (self.LogType === LogTypes.All) {
                href = "/api/logs/export?exportAll=true&responseID=" + self.responseid + "";
            }
            else {
                href = "/api/logs/export?exportAll=false&responseID=" + self.responseid + "&startDate=" + moment(self.ExportStartDate).toISOString() + "&endDate=" + moment(self.ExportEndDate).toISOString();
            }
        }
        else {
            if (self.LogType === LogTypes.All) {
                href = "/api/logs/export?exportAll=true";
            }
            else {
                href = "/api/logs/export?exportAll=false&startDate=" + moment(self.ExportStartDate).toISOString() + "&endDate=" + moment(self.ExportEndDate).toISOString();
            }
        }


        window.location.href = href;
        self.resetExport();
    }

    onStartDatePickerChange(event) {
        this.ExportStartDate = event.value;
    }

    onEndDatePickerChange(event) {
        this.ExportEndDate = event.value;
    }

    created() {
        const self = this;
        self.connection = new signalR.HubConnectionBuilder()
            .withUrl("/LogHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();
        self.connection.start().then(() => {
            if (self.responseid !== undefined && self.responseid !== null) {
                Axios.get<LogMessageDTO[]>('/api/logs/response?responseID=' + self.responseid).then((logVal) => {
                    self.logItems = logVal.data.map(l => new LogMessageListViewModel(l));
                    self.connection!.invoke("ConnectedToResponse", self.responseid).catch(function (innerErr) {
                        return console.error(innerErr.toString());
                    });
                });
            }
            else {
                Axios.get<LogMessageDTO[]>('/api/logs/global?take=500').then((logVal) => {
                    self.logItems = logVal.data.map(l => new LogMessageListViewModel(l));
                    self.connection!.invoke("ConnectedToGlobal").catch(function (innerErr) {
                        return console.error(innerErr.toString());
                    });
                });
            }
        }).catch(function (err) {
            return console.error(err.toSting());
        });
    }

    mounted() {
        const self = this;
        if (self.responseid !== undefined && self.responseid !== null) {
            self.connection!.on("addedResponseLog", self.onAddResponseLog);
        }
        else {
            self.connection!.on("addedGlobalLog", self.onAddResponseLog);
        }
    }
    
}