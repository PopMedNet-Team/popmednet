import Vue from 'vue';
import { Dialog, DialogActionsBar } from '@progress/kendo-vue-dialogs';
import { Button } from "@progress/kendo-vue-buttons";
import { Component } from 'vue-property-decorator';
import axios from 'axios';

@Component({
    template: '#forgotPasswordDialogTmpl',
    components: {
        "k-dialog": Dialog,
        'dialog-actions-bar': DialogActionsBar,
        "kbutton": Button
    }
})
export default class ForgotPasswordDialog extends Vue {
    showDialog: boolean = false;
    email: string = '';
    username: string = '';

    onToggleDialog() {
        this.showDialog = !this.showDialog;
    }

    onSendResetEmail() {
        this.$emit("load-spinner", true);
        try {
            var payload = { username: this.username, email: this.email };
            this.ShowDialog = false;

            axios.post('/api/authentication/forgot-password', payload);            

        } finally {
            this.$emit('load-spinner', false);
        }
    }

    get DisableSendButton() {
        return (this.email + this.username).trim().length == 0;
    }

    get ShowDialog() {
        return this.showDialog;
    }
    set ShowDialog(value: boolean) {
        this.showDialog = value;
        if (!value) {
            this.email = '';
            this.username = '';
        }
    }
}