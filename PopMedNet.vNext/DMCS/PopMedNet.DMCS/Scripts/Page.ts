import Vue from 'vue';
import '@progress/kendo-ui';
import '@progress/kendo-theme-bootstrap/dist/all.css';
import { Dialog, DialogActionsBar } from '@progress/kendo-vue-dialogs';
import { Button } from "@progress/kendo-vue-buttons";
import * as signalR from "@microsoft/signalr";
import { NotificationEventIdentifiers } from './interfaces';
import SessionEndingDialog from './components/SessionEndingDialog';
import axios from 'axios';

Vue.component('session-ending-dialog', SessionEndingDialog);

new Vue({
    el: '#header',
    components: {
        'k-dialog': Dialog,
        'dialog-actions-bar': DialogActionsBar,
        'kbutton': Button
    },
    data: function () {
        return {
            aboutDialogVisible: false
        };
    },
    methods: {
        toggleAboutDialog() {
            this.aboutDialogVisible = !this.aboutDialogVisible;
            return false;
        }
    }
});

interface SessionMonitorVue {
    connection: signalR.HubConnection | null,
    sessionStart: Date,
    sessionTimeoutTimerID: number,
    sessionEndingDialogVisible: boolean
}

var authUserElement = document.getElementById('session-container');

if (authUserElement) {
    new Vue({
        el: authUserElement,
        data: {
            connection: null,
            sessionStart: new Date(),
            sessionTimeoutTimerID: -1,
            sessionEndingDialogVisible: false
        } as SessionMonitorVue,
        methods: {
            onLogout: function () {
                window.location.reload();
            },
            onRefreshSessionDate: function () {
                this.sessionStart = new Date();
                //console.info('Session start date updated to:' + this.sessionStart);
                if (this.sessionEndingDialogVisible) {
                    this.sessionEndingDialogVisible = false;
                }
            },
            onSessionIntervalElapsed: function () {
                let showWarningTime = this.computeSessionWarningTime();
                let sessionEndTime = this.computeSessionEndTime();

                //console.info('Current time:' + new Date() + '; show warning time:' + showWarningTime + '; session end time:' + sessionEndTime);

                let currentTime = new Date();

                if (currentTime.getTime() > showWarningTime.getTime() && !this.sessionEndingDialogVisible) {
                    //show the warning dialog
                    this.sessionEndingDialogVisible = true;
                }

                if (currentTime > sessionEndTime) {
                    window.location.reload();
                }                

            },
            onRefreshSession: function () {
                this.sessionEndingDialogVisible = false;
                axios.get('/touch');
            },
            computeSessionWarningTime: function () {
                return new Date(this.sessionStart.getTime() + this.sessionDuration - this.sessionWarning);
            },
            computeSessionEndTime: function () {
                return new Date(this.sessionStart.getTime() + this.sessionDuration);
            }
        },
        computed: {
            sessionDuration: function () {
                return kendo.parseInt(document.body.getAttribute('data-session-duration') || '20') * 60 * 1000;
            },
            sessionWarning: function () {
                return kendo.parseInt(document.body.getAttribute('data-session-waring') || '5') * 60 * 1000;
            }
        },
        created() {
            const self = this;
            self.connection = new signalR.HubConnectionBuilder()
                .withUrl("/SessionHub")
                .configureLogging(signalR.LogLevel.Information)
                .build();

            self.connection.start();

            //interval is 20 seconds
            self.sessionTimeoutTimerID = setInterval(<TimerHandler>self.onSessionIntervalElapsed.bind(self), 20 * 1000);
        },
        mounted() {
            const self = this;
            self.connection!.on(NotificationEventIdentifiers.User_Logout, self.onLogout);
            self.connection!.on(NotificationEventIdentifiers.User_SessionRefresh, self.onRefreshSessionDate);
        }
    });
}