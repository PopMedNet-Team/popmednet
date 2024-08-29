import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';

@Component({
    template: '#datamart-adapter'
})
export default class DataMartAdapter extends Vue {
    @Prop()
    adapterid!: any;
}