import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import { DocumentDTO, RequestMetaDataDTO, Helpers } from '../interfaces';
import moment from 'moment';

@Component({
    template: '#request-metadata',
    filters: {
        formatLength: (length: number) => {
            return Helpers.formatFileSize(length);
        }
    }
})
export default class RequestMetadata extends Vue {
    @Prop({ default: []})
    attachments!: DocumentDTO[]

    @Prop({ required: true })
    RequestMetadata!: RequestMetaDataDTO;

    get PriorityText(): string {
        return Helpers.GetPrioritiesText(this.RequestMetadata.priority);
    };
    get StatusText(): string {
        return Helpers.GetRouteStatusText(this.RequestMetadata.status);
    };
    get SubmittedOnText(): string {
        return moment.utc(this.RequestMetadata.requestDate).local().format('LL');
    };
    get DueDateText(): string {
        if (this.RequestMetadata.dueDate == null)
            return "";
        return moment.utc(this.RequestMetadata.dueDate).local().format('LL');
    };
}