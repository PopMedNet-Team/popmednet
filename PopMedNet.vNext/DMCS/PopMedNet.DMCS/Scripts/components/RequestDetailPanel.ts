import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import { DocumentDTO, Helpers, RequestMetaDataDTO, RouteStatus } from '../interfaces';
import { Button } from "@progress/kendo-vue-buttons";

@Component({
    template: '#request-details',
    filters: {
        formatLength: (length: number) => {
            return Helpers.formatFileSize(length);
        }
    },
    components: {
        'kbutton': Button
    }
})
export default class RequestDetailPanel extends Vue {
    @Prop()
    adapterid!: any;

    @Prop({ required: true, default: [] })
    RequestDocuments!: DocumentDTO;

    @Prop({ required: true })
    RequestMetadata!: RequestMetaDataDTO;

    private readonly _isFileBasedRequest = true;

    get enableHold(): boolean {
        let routeStatus = this.RequestMetadata.status;
        return this.RequestMetadata.permissions.holdRequest &&
            (
                routeStatus == RouteStatus.Submitted ||
                routeStatus == RouteStatus.Resubmitted ||
            routeStatus == RouteStatus.PendingUpload ||
            routeStatus == RouteStatus.Hold
            ) == false;
    }

    get enableReject(): boolean {
        let routeStatus = this.RequestMetadata.status;
        return this.RequestMetadata.permissions.rejectRequest &&
            (
            routeStatus == RouteStatus.Submitted ||
            routeStatus == RouteStatus.Resubmitted ||
            routeStatus == RouteStatus.PendingUpload
            ) == false;
    }

    get HoldButtonTitle(): string {
        const self = this;
        return self.RequestMetadata.status === RouteStatus.Hold ? "Remove Hold" : "Hold";
    }

    onHold() {
        this.$parent.$emit('request-hold');
    }

    onReject() {
        this.$parent.$emit('request-reject');
    }
}