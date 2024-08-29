import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { DataMartDTO } from '../interfaces';
import { Button } from '@progress/kendo-vue-buttons';

@Component({
    template: '#datamart-cache-settings',
    components: {
        'kbutton': Button
    }
})
export default class DataMartCacheSetting extends Vue {
    @Prop()
    DataMart!: DataMartDTO;

    ClearCache() {
        alert("This action is not hooked up yet!");
    }

    get cacheDays() {
        return this.DataMart.cacheDays;
    }

    set cacheDays(val: number) {
        this.$emit('update-cachedays', val);
    }

    get encryptCache() {
        return this.DataMart.encryptCache;
    }

    set encryptCache(val: boolean) {
        this.$emit('update-encrypt', val);
    }

    get explicitRemoval() {
        return this.DataMart.enableExplictCacheRemoval;
    }

    set explicitRemoval(val: boolean) {
        this.$emit('update-explicit', val);
    }
}