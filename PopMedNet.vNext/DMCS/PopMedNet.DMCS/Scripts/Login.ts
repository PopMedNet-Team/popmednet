import Vue from 'vue';
import axios from 'axios';
import DMCSError from './components/DMCSError';
import Loader from './components/Loader';
import ForgotPasswordDialog from './components/ForgotPasswordDialog';
import { Button } from '@progress/kendo-vue-buttons';

let a = new Vue({
    el: '#vue_login',
    components: {
        'errors': DMCSError,
        'pmn-loader': Loader,
        'forgot-password-dialog': ForgotPasswordDialog,
        'kbutton': Button
    },
    data: {
        username: '',
        password: '',
        errors: [],
        LoadSpinner: false,
        loaderTitle: 'Authenticating...',
        loaderBody: 'Logging into DMCS...'
    } as loginDTO,
    methods: {
        Login(evt) {
            if (evt) {
                evt.preventDefault();
            }

            const self = this;
            self.LoadSpinner = true;
            self.errors = [];
            if (self.username?.trim() === '') {
                self.errors.push("You must provide an username.");
            }

            if (self.password?.trim() === '') {
                self.errors.push("You must provide a password.");
            }

            if (self.errors.length > 0) {
                self.LoadSpinner = false;
                return;
            }


            axios.post('/api/Authentication', { username: self.username, password: self.password } as loginDTO).then(() => {
                var params = new URLSearchParams(window.location.search);
                if (params.get('ReturnUrl') && params.get('ReturnUrl') != '') {
                    window.location.href = params.get('ReturnUrl') || '';
                } else {
                    window.location.href = '/';
                }
            }).catch((err) => {
                self.LoadSpinner = false;
                if (err.response.data.error.length > 0) {
                    for (let i = 0; i < err.response.data.error.length; i++) {
                        self.errors.push(err.response.data.error[i]);
                    }
                }

                
            });
        },
        onPasswordKeyup(evt: KeyboardEvent) {
            if (evt.keyCode == 13 && (this.username || '').length > 0) {
                this.Login(evt);
            }
        },
        onShowLoaderForForgotPassword(value: boolean) {
            if (value) {
                this.loaderTitle = "Forgot Password Request";
                this.loaderBody = "Sending forgot password request to PopMedNet API...";
            } else {
                this.loaderTitle = "Authenticating...";
                this.loaderBody = "Logging into DMCS...";
            }
            this.LoadSpinner = value;
        }
    }
});

interface loginDTO {
    username: string | null,
    password: string | null,
    errors: string[],
    LoadSpinner: boolean,
    loaderTitle: string,
    loaderBody: string;
}



