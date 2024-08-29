import * as signalR from "../microsoft/signalr/index.js";

const OnLogoutIdentifier: string = "onLogout";
const OnSessionRefreshIdentifier: string = "onSessionRefresh";
const SessionDialogContainerID: string = "sessionWarningDialog";
const SessionDialogContentID: string = "sessionWarningDialogContent";

export default class SessionManager {
    

    sessionStart: Date;
    connection: signalR.HubConnection = null;
    sessionEndingDialogVisible: boolean = false;
    sessionTimeoutTimerID: number = -1;
    sessionDialog: kendo.ui.Dialog;
    contentTemplate = kendo.template($(`#${SessionDialogContentID}`).html());

    constructor() {
        this.sessionStart = new Date();

        this.sessionDialog = $(`#${SessionDialogContainerID}`).kendoDialog({
            width: "500px",
            title: "Session Ending Soon!",
            closable: false,
            modal: true,
            visible: false,
            actions: [
                { text: "Refresh", primary: true, action: this.onRefreshSession.bind(this) }
            ]
        }).data("kendoDialog");
        
    }

    public initialize() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/SessionHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.start();

        //interval is 20 seconds
        this.sessionTimeoutTimerID = setInterval(<TimerHandler>this.onSessionIntervalElapsed.bind(this), 20 * 1000);

        this.connection!.on(OnLogoutIdentifier, this.onLogout);
        this.connection!.on(OnSessionRefreshIdentifier, this.onRefreshSessionDate.bind(this));

        
    }

    public onLogout() {
        window.location.reload();
    }

    public onRefreshSessionDate() {
        this.sessionStart = new Date();
        console.info('Session start date updated to:' + this.sessionStart);
        if (this.sessionEndingDialogVisible) {
            this.sessionEndingDialogVisible = false;
        }

        this.sessionDialog.close();
    }

    public onRefreshSession(evt) {
        this.sessionEndingDialogVisible = false;
        $.get("/touch");

        return true;
    }

    onSessionIntervalElapsed() {
        let showWarningTime = this.computeSessionWarningTime();
        let sessionEndTime = this.computeSessionEndTime();

        //console.info('Current time:' + new Date() + '; show warning time:' + showWarningTime + '; session end time:' + sessionEndTime);

        let currentTime = new Date();

        if (currentTime.getTime() > showWarningTime.getTime() && !this.sessionEndingDialogVisible) {
            //show the warning dialog
            this.sessionEndingDialogVisible = true;

            this.sessionDialog.content(this.contentTemplate({ sessionEnd: sessionEndTime }));

            this.sessionDialog.open();
        }

        if (currentTime > sessionEndTime) {
            window.location.reload();
        }
    }

    computeSessionWarningTime() {
        return new Date(this.sessionStart.getTime() + this.sessionDuration - this.sessionWarning);
    }

    computeSessionEndTime() {
        return new Date(this.sessionStart.getTime() + this.sessionDuration);
    }

    get sessionDuration() {
        return kendo.parseInt(document.getElementById(`${SessionDialogContainerID}`).getAttribute('data-session-duration') || '20') * 60 * 1000;
    }
    get sessionWarning() {
        return kendo.parseInt(document.getElementById(`${SessionDialogContainerID}`).getAttribute('data-session-waring') || '5') * 60 * 1000;
    }
}

let vm = new SessionManager();
vm.initialize();

