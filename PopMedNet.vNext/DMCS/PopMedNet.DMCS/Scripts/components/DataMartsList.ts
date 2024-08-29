import Vue from 'vue';
import DMCSError from './DMCSError';
import { DataMartDTO, Helpers } from '../interfaces';
import Axios from 'axios';
import { Grid, GridSortChangeEvent, GridColumnProps } from '@progress/kendo-vue-grid';
import { SortDescriptor, orderBy } from '@progress/kendo-data-query';

Vue.component('errors', DMCSError);
Vue.component('kendo-grid', Grid);

new Vue({
    el: '#crdDataMarts',
    data: function (): DataMartVue {
        return {
            DataMarts: [],
            Errors: [],
            columns: [
                { field: 'name', title: 'Name', cell: 'nameTemplate', className:'datamart-link-cell' },
                { field: 'acronym', title: 'Acronym' }
            ],
            sort: [{ field: 'name', dir: 'asc' }],
            sortable: {
                allowUnsort: false,
                mode: 'single'
            }
        }
    },
    computed: {
        result(): any {
            const self = this;
            return orderBy(self.DataMarts, self.sort);
        }
    },
    methods: {
        GetDatamarts: function () {
            const self = this;
            Axios.get<DataMartDTO[]>('/api/datamarts').then(val => {
                self.DataMarts = val.data;
            }).catch(err => {
                self.Errors = [];
                self.Errors.push(err);
            });
        },
        Refresh: function () {
            const self = this;
            self.DataMarts = [];
            Axios.get('/api/datamarts/trigger-sync').then(() => {
                self.GetDatamarts();
            }).catch(err => {
                self.Errors = [];
                self.Errors.push(err);
            });
        },
        onSortChange: function (e: GridSortChangeEvent) {
            this.sort = e.sort;
        }
    },
    mounted() {
        this.GetDatamarts();
        this.$nextTick(function () {
            Helpers.removeLoadingPanel();
        });
    }
});

interface DataMartVue {
    DataMarts: DataMartDTO[];
    Errors: string[];
    columns: GridColumnProps[];
    sort: SortDescriptor[],
    sortable: any;
}