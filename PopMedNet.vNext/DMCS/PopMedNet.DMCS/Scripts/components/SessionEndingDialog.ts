import Vue from 'vue';
import { Dialog, DialogActionsBar } from '@progress/kendo-vue-dialogs';
import { Component, Prop } from 'vue-property-decorator';
import { Button } from "@progress/kendo-vue-buttons";

@Component({
    template: '#session-ending-dialog-tmpl',
    components: {
        "k-dialog": Dialog,
        "dialog-actions-bar": DialogActionsBar,
        "kbutton": Button
    }
})
export default class SessionEndingDialog extends Vue {
    @Prop()
    showDialog!: boolean;
    @Prop()
    sessionEndTime!: Date;

    get ShowDialog() {
        return this.showDialog;
    }
    set ShowDialog(value: boolean) {
        this.showDialog = value;
    }

    get SessionEnd() {
        return kendo.toString(this.sessionEndTime, "g");
    }
}