import Vue from 'vue';
import DMCSError from './components/DMCSError';
import DataMartDetail from './components/DataMartDetail';
import DataMartAdapter from './components/DataMartAdapter';
import DataMartCacheSetting from './components/DataMartCacheSetting';
import DataMartAutoProcessorSetting from './components/DataMartAutoProcessorSetting';
import Axios from 'axios';
import { DataMartDTO, AutoProcesses, Helpers } from './interfaces';
import { Button } from '@progress/kendo-vue-buttons';

let rootElement = document.getElementById('vue_DataMart');
if (rootElement) {
    let dmID: string | null = rootElement.attributes['data-itemid'].value;

    Promise.all([
        Axios.get<DataMartDTO>('/api/datamarts/' + dmID),
    ]).then(values => {

        let dm = values[0].data;

        new Vue({
            el: '#vue_DataMart',
            components: {
                'errors': DMCSError,
                'datamart-details': DataMartDetail,
                'datamart-adapter': DataMartAdapter,
                'datamart-cachesetting': DataMartCacheSetting,
                'datamart-autoprocessorsetting': DataMartAutoProcessorSetting,
                'kbutton': Button
            },
            data: {
                DataMart: dm,
                Errors: [],
            } as DataMartVue,
            methods: {
                UpdateAutoProcess(val: AutoProcesses) {
                    this.DataMart.autoProcess = val;
                },
                UpdateCacheDays(val: number) {
                    this.DataMart.cacheDays = val;
                },
                UpdateEncrypt(val: boolean) {
                    this.DataMart.encryptCache = val;
                },
                UpdateExplicit(val: boolean) {
                    this.DataMart.enableExplictCacheRemoval = val;
                },
                Save() {
                    Axios.post('/api/datamarts', this.DataMart).then(() => {
                        this.Cancel();
                    }).catch(err => {
                        this.Errors = [];
                        this.Errors.push(err);
                    });
                },
                Cancel() {
                    window.location.href = "/Configuration";
                }                
            },
            mounted() {
                this.$nextTick(function () {
                    Helpers.removeLoadingPanel();
                });
            }
        });
    });
}

interface DataMartVue {
    DataMart: DataMartDTO;
    Errors: string[];
}