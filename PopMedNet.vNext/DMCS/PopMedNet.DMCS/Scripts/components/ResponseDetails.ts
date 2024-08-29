import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';
import Axios from 'axios';
import { Helpers, RequestMetaDataDTO, DocumentDTO, RouteStatus, DocumentStates } from '../interfaces';
import { Button } from '@progress/kendo-vue-buttons';

interface FileMetadata {
    UploadID: any;
    RequestDataMartID: any;
    RequestResponseID: any;
    TotalChunks: number;
    CurrentChunk: number;
    FileName: string;
    FileSize: number;
}

interface RemovableDocument extends DocumentDTO {
    selected: boolean;
}

@Component({
    template: '#response-details',
    components: {
        'kbutton': Button
    }
})
export default class ResponseDetails extends Vue {
    @Prop()
    adapterid!: any;

    @Prop({ required:true, default: true })
    isFileBasedRequest!: boolean;

    enableButtons: boolean = false;

    @Prop({ required: true })
    RequestMetadata!: RequestMetaDataDTO;

    @Prop({ required: true })
    ResponseDocuments!: RemovableDocument[];

    showClearCache: boolean = false;
    showUploadResults: boolean = false;
    uploadButtonText: string = 'Upload Results';
    uploadingCount: number = 0;

    get showUpdateResults(): boolean {
        const self = this;
        if ((self.RequestMetadata.status === RouteStatus.AwaitingResponseApproval || self.RequestMetadata.status === RouteStatus.Completed) && self.RequestMetadata.permissions.modifyResults == false) {
            return false;
        }
        else if (self.ResponseDocuments.filter((doc) => doc.documentState == DocumentStates.Local).length > 0) {
            return true;
        }
        else {
            return false;
        }
    }

    get clearCacheEnabled(): boolean {
        return this.ResponseDocuments.length > 0 &&
            this.ResponseDocuments.findIndex((doc) => doc.documentState == DocumentStates.Local) >= 0;
    }

    get canAddResponseFiles(): boolean {
        let routeStatus = this.RequestMetadata.status;

        //matching the logic of the DMC for filebased requests
            
        let enabled = this.RequestMetadata.permissions.uploadResults &&
                (routeStatus != RouteStatus.RequestRejected &&
                routeStatus != RouteStatus.Failed &&
                routeStatus != RouteStatus.Canceled &&
                routeStatus != RouteStatus.AwaitingResponseApproval &&
                routeStatus != RouteStatus.Completed);

        if ((routeStatus == RouteStatus.Completed ||
            routeStatus == RouteStatus.ResultsModified ||
            routeStatus == RouteStatus.AwaitingResponseApproval)) {

            enabled = this.RequestMetadata.permissions.modifyResults;
            this.uploadButtonText = 'Re-Upload Results';
        }           

        return enabled;
    }

    canRemoveAddedFile(document: DocumentDTO): boolean {
        if (document.documentState == DocumentStates.Remote)
            return false;

        return this.canAddResponseFiles;
    }

    uploadChunck(chunkSize: number, metaData: FileMetadata, file: any) {
        const self = this;

        const offset = metaData.CurrentChunk * chunkSize;
        const fileData = file.slice(offset, offset + chunkSize);

        var form = new FormData();
        form.append('UploadID', metaData.UploadID);
        form.append('FileSize', metaData.FileSize.toString());
        form.append('RequestDataMartID', metaData.RequestDataMartID);
        form.append('RequestResponseID', metaData.RequestResponseID);
        form.append('TotalChunks', metaData.TotalChunks.toString());
        form.append('CurrentChunk', (metaData.CurrentChunk + 1).toString());
        form.append('FileName', metaData.FileName);
        form.append('file', fileData);

        Axios.post('/api/route/upload-part', form).then(() => {
            if ((metaData.CurrentChunk + 1) < metaData.TotalChunks) {
                metaData.CurrentChunk++;
                self.uploadChunck(chunkSize, metaData, file);
            }
            else {
                //self.onFileRemove(file);                
            }
        }).finally(() => {
            self.uploadingCount--;
            if (self.uploadingCount <= 0) {
                let uploadEditor = self.$refs.fileInput as HTMLInputElement;
                uploadEditor.value = '';
                self.uploadingCount = 0;
            }
        });

    }

    onFilesAdded(e: any) {
        const self = this;
        let filesRef = this.$refs.fileInput as HTMLInputElement;
        if (filesRef.files != null) {

            self.uploadingCount = filesRef.files.length;

            for (var i = 0; i < filesRef.files.length; i++) {
                const file = filesRef.files[i] as File;
                const fileSize = file.size;
                const uid = Helpers.newGuid();

                const chunkSize = 25000000;

                const totalChunks = Math.ceil(file.size / chunkSize);

                var metaData = { FileName: file.name, TotalChunks: totalChunks, RequestDataMartID: self.RequestMetadata.id, RequestResponseID: self.RequestMetadata.responseID, UploadID: uid, CurrentChunk: 0, FileSize: fileSize } as FileMetadata;

                self.uploadChunck(chunkSize, metaData, file);
            }            
        }
    }

    onCacheFileRemove(id: any) {
        const self = this;

        Axios.get('/api/cache/remove-document?documentID=' + id + '&responseID=' + self.RequestMetadata.responseID).then(() => {
            let documentIndex = self.ResponseDocuments.findIndex((doc) => doc.id == id);
            if (documentIndex >= 0) {
                self.ResponseDocuments.splice(documentIndex, 1);
            }
        });
    }

    onClearCache() {
        this.$parent.$emit('response-clearCache');
    }

    onUploadResults() {
        this.$parent.$emit('response-upload');
    }

    mounted() {
        const self = this;
        this.enableButtons = self.ResponseDocuments.length > 0;
    }
}