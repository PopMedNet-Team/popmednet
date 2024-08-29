//import Vue from 'vue';
import { Vue, Component, Prop } from 'vue-property-decorator';

@Component({
    template: '<div class="alert alert-danger" role="alert" v-bind:class="{\'d-none\': errors.length === 0}">'
                + 'An Error has occured.  Below were the errors:'
                +'<ul>'
                    +'<li v-for="error in errors">{{error}}</li>'
                +'</ul>'
               +'</div>'
})
export default class DMCSError extends Vue {
    @Prop({ default: true })
    errors!: string[];

    CloseDialog() {
        this.$emit('closedialog')
    }

    get filteredErrors() {
        const self = this;
        return self.errors
    }
}