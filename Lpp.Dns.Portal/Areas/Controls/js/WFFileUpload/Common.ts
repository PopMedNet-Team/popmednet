/// <reference path="../../../../js/requests/details.ts" />
module Controls.WFFileUpload {

    export interface Credentials {
        Address: string;
        Port: number;
        Login: string;
        Password: string;
    }

    export enum ItemTypes { Folder = 0, File = 1 }

    export class sFtpFileResults {
        public Results: KnockoutObservableArray<sFtpResult>;
        constructor() {
            this.Results = ko.observableArray<sFtpResult>();
        }

        public RemoveFile(data, event) {
        }
    }

    export class sFtpResult {
        public Path: KnockoutObservable<string>;
        public Status: KnockoutObservable<string>;

        constructor(path: string, status: string) {
            this.Path = ko.observable(path);
            this.Status = ko.observable(status);
        }
    }

    export class sFtpItem {
        public Name: KnockoutObservable<string>;
        public Path: KnockoutObservable<string>;
        public Type: KnockoutObservable<ItemTypes>;
        public Length: KnockoutObservable<number>;
        public LengthFormatted: KnockoutComputed<string>;
        public Loaded: KnockoutObservable<boolean>;

        public Selected: KnockoutObservable<boolean>;

        public Items: KnockoutObservableArray<sFtpItem>;
        public Folders: KnockoutComputed<sFtpItem[]>;
        public Files: KnockoutComputed<sFtpItem[]>;

        constructor(name: string, path: string, type: ItemTypes, length: number) {
            this.Name = ko.observable(name);
            this.Path = ko.observable(path);
            this.Type = ko.observable(type);
            this.Length = ko.observable(length);
            this.Selected = ko.observable(false);
            this.Loaded = ko.observable(false);
            this.Items = ko.observableArray<sFtpItem>();

            this.Files = ko.computed(() => {
                if (this.Items == null || this.Items().length == 0)
                    return [];

                var arr = ko.utils.arrayFilter(this.Items(), (item: sFtpItem) => {
                    return item.Type() == ItemTypes.File;
                });
                return arr;
            });

            this.Folders = ko.computed(() => {

                if (this.Items == null || this.Items().length == 0)
                    return [];

                var arr: Array<sFtpItem> = [];
                this.Items().forEach((item) => {
                    if (item.Type() == ItemTypes.Folder)
                        arr.push(item);
                });
                return arr;
            });

            this.LengthFormatted = ko.computed(() => {
                if (this.Length() == null)
                    return '';

                return Global.Helpers.formatFileSize(this.Length());
            });

        }
    }

}