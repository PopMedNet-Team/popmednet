import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { DataMartDTO } from '../interfaces';

@Component({
    template: '#datamart-detail'
})
export default class DataMartDetail extends Vue {
    @Prop()
    DataMart!: DataMartDTO;
}