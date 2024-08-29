import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import { AutoProcesses } from '../interfaces';

@Component({
    template: '#datamart-autoprocessor-settings'
})
export default class DataMartAutoProcessorSetting extends Vue {
    @Prop({})
    autoProcess!: AutoProcesses;

    get myAutoProcess() {
        return this.autoProcess
    }

    set myAutoProcess(value) {
        this.$emit('autoprocess-changed', value)
    }
}