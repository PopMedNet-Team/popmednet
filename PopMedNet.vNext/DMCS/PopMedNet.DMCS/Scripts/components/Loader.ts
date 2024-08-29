import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { Dialog } from '@progress/kendo-vue-dialogs';

Vue.component('kendo-dialog', Dialog);

@Component({
    template: '#loading-dialog'
})
export default class Loader extends Vue {
    @Prop({ default: false })
    closable!: boolean;

    @Prop({ default: false })
    isActive!: boolean;

    @Prop({ default: "Loading Content" })
    textBody!: string;

    @Prop({ default: "Loading" })
    textHeader!: string;
}