import Vue from 'vue';
import { Helpers } from './interfaces';
import LogViewer from './components/LogViewer';

new Vue({
    el: '#logs-container',
    components: {
        'log-viewer': LogViewer
    },
    mounted() {
        this.$nextTick(function () {
            Helpers.removeLoadingPanel();
        });
    }
    
});