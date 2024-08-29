export enum AutoProcesses {
    None = 0,
    Notify = 1,
    Run = 2,
    RunAndUpload = 3
}

export enum Priorities {
    Low = 0,
    Medium = 1,
    High = 2,
    Urgent = 3
}

export enum RouteStatus {
    Draft = 1,
    Submitted = 2,
    Completed = 3,
    AwaitingRequestApproval = 4,
    RequestRejected = 5,
    Canceled = 6,
    Resubmitted = 7,
    PendingUpload = 8,
    AwaitingResponseApproval = 10,
    Hold = 11,
    ResponseRejectedBeforeUpload = 12,
    ResponseRejectedAfterUpload = 13,
    ExaminedByInvestigator = 14,
    ResultsModified = 16,
    Failed = 99
}

export enum LogLevel {
    Verbose = 0,
    Debug = 1,
    Information = 2,
    Warning = 3,
    Error = 4,
    Fatal = 5
}

export interface DataMartDTO {
    id: any;
    name: string;
    acronym: string;
    description: string;
    adapterID: any;
    adapter: string;
    cacheDays: number;
    encryptCache: boolean;
    enableExplictCacheRemoval: boolean;
    autoProcess: AutoProcesses;
}

export interface RoutePermissionsComponent {
    readonly id: any;
    readonly requestDataMartID: any;
    readonly seeRequest: boolean;
    readonly uploadResults: boolean;
    readonly holdRequest: boolean;
    readonly rejectRequest: boolean;
    readonly modifyResults: boolean;
    readonly viewAttachments: boolean;
    readonly modifyAttachments: boolean;
}

export interface RoutingDTO {
    id: any;
    dataMartName: string;
    dueDate: Date; 
    msRequestID: string;
    requestName: string;
    project: string;
    priority: Priorities;
    requestDate: Date;
    requestType: string;
    status: RouteStatus;
    submittedBy: string;
    requestIdentifier: number;
    dataModel: string;
    respondedBy: string;
    respondedDate: Date | null;
}

export class RoutingListViewModel {
    readonly dto: RoutingDTO;

    constructor(dto: RoutingDTO) {
        this.dto = dto;
    }

    get id(): any {
        return this.dto.id;
    }

    get dataMartName(): string {
        return this.dto.dataMartName;
    }

    get dueDateLocal(): Date | null {
        if (this.dto.dueDate) {
            return kendo.parseDate(this.dto.dueDate.toString() + 'Z', 'yyyy-MM-ddTHH:mm:sszzz');
        }
        return null;
    }

    get requestDateLocal(): Date {
        let dd: Date = <Date> kendo.parseDate(this.dto.requestDate.toString() + 'Z', 'yyyy-MM-ddTHH:mm:sszzz');
        return new Date(dd.getFullYear(), dd.getMonth(), dd.getDate());
    }

    get msRequestID(): string {
        return this.dto.msRequestID;
    }

    get requestName(): string {
        return this.dto.requestName;
    }

    get project(): string {
        return this.dto.project;
    }

    get priority(): string {
        return Helpers.GetPrioritiesText(this.dto.priority);
    }

    get requestType(): string {
        return this.dto.requestType;
    }

    get status(): string {
        return Helpers.GetRouteStatusText(this.dto.status);
    }

    get submittedBy(): string {
        return this.dto.submittedBy;
    }

    get requestIdentifier(): number {
        return this.dto.requestIdentifier;
    }

    get dataModel(): string {
        return this.dto.dataModel;
    }

    get respondedBy(): string {
        return this.dto.respondedBy;
    }

    get respondedDateLocal(): Date | null {
        if (this.dto.respondedDate) {
            return kendo.parseDate(this.dto.respondedDate.toString() + 'Z', 'yyyy-MM-ddTHH:mm:sszzz');;
        }

        return null;
    }
}

export interface RequestMetaDataDTO extends RoutingDTO {
    requestID: any;
    responseID: any;
    responseMessage: string;
    purposeOfUse: string;
    levelOfReportAggregation: string;
    requestorCenter: string;
    additionalInstructions: string;
    description: string;

    taskOrder: string;
    activity: string;
    activityProject: string;

    sourceTaskOrder: string;
    sourceActivity: string;
    sourceActivityProject: string;

    permissions: RoutePermissionsComponent;
}

export interface DocumentDTO {
    id: any;
    name: string;
    length: number;
    documentType: DocumentType;
    documentState: DocumentStates;
}

export enum DocumentType {
    Input = 0,
    Output = 1,
    AttachmentInput = 2,
    AttachmentOutput = 3
}

export enum DocumentStates {
    Local = 0,
    Remote = 1
}

export interface LogMessageDTO {
    dateTime: Date;
    level: LogLevel;
    message: string;
    source: string;
}

export class LogMessageListViewModel {
    readonly dto: LogMessageDTO;

    constructor(dto: LogMessageDTO) {
        this.dto = dto;
    }

    get dateTime(): Date {
        return <Date> kendo.parseDate(this.dto.dateTime.toString());
    }

    get level(): string {
        return Helpers.GetLevelText(this.dto.level);
    }

    get message(): string {
        return this.dto.message;
    }

    get source(): string {
        return this.dto.source;
    }
}

export enum ChangeType {
    NoChange = 0,
    Added,
    Updated,
    Deleted
}

export interface ChangeNotification<T> {
    changeType: ChangeType;
    entity: T;
}

export enum NotificationEventIdentifiers {
    RequestList_RequestListUpdated = "requestListUpdated",
    RequestDataMart_Metadata = "requestMetadata",
    Response_DocumentAdded = "responseDocAdded",
    Response_DocumentRemoved = "responseDocRemoved",
    Response_CacheCleared = "responseCacheCleared",
    User_Logout = "onLogout",
    User_SessionRefresh = "onSessionRefresh"
}

export enum NotificationGroups {
    RequestList = "RequestList"
}

export class Helpers {
    private static kilobyte: number = 1024;
    private static megabyte: number = 1024 * 1024;
    private static gigabyte: number = 1024 * 1024 * 1024;

    public static formatFileSize(length: number): string {
        if (length > Helpers.gigabyte)
            return (length / Helpers.gigabyte).toFixed(2) + ' Gb';

        if (length > Helpers.megabyte)
            return (length / Helpers.megabyte).toFixed(2) + ' Mb';

        if (length > Helpers.kilobyte)
            return (length / Helpers.kilobyte).toFixed(2) + ' Kb';

        return length + ' bytes';
    }

    public static newGuid(): any {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    public static GetRouteStatusText(status: RouteStatus): string {
        switch (status) {
            case RouteStatus.Draft: {
                return "Draft";
            }
            case RouteStatus.AwaitingRequestApproval: {
                return "Awaiting Request Approval";
            }
            case RouteStatus.AwaitingResponseApproval: {
                return "Awaiting Response Approval";
            }
            case RouteStatus.Canceled: {
                return "Cancelled";
            }
            case RouteStatus.Completed: {
                return "Completed";
            }
            case RouteStatus.ExaminedByInvestigator: {
                return "Examined By Investigator";
            }
            case RouteStatus.Failed: {
                return "Failed";
            }
            case RouteStatus.Hold: {
                return "Hold";
            }
            case RouteStatus.PendingUpload: {
                return "Pending Upload";
            }
            case RouteStatus.RequestRejected: {
                return "Request Rejected";
            }
            case RouteStatus.ResponseRejectedAfterUpload: {
                return "Response Rejected After Upload";
            }
            case RouteStatus.ResponseRejectedBeforeUpload: {
                return "Response Rejected Before Upload";
            }
            case RouteStatus.Resubmitted: {
                return "Re-submitted";
            }
            case RouteStatus.ResultsModified: {
                return "Results Modified";
            }
            case RouteStatus.Submitted: {
                return "Submitted";
            }
            default: {
                return "Unknown";
            }
        }
    }

    public static GetPrioritiesText(priority: Priorities): string {
        switch (priority) {
            case Priorities.Low: {
                return "Low";
            }
            case Priorities.Medium: {
                return "Medium";
            }
            case Priorities.High: {
                return "High";
            }
            case Priorities.Urgent: {
                return "Urgent";
            }
            default: {
                return "Unknown Priority";
            }
        }
    }

    public static GetLevelText(level: LogLevel): string {
        switch (level) {
            case LogLevel.Debug: {
                return "Debug";
            }
            case LogLevel.Verbose: {
                return "Verbose";
            }
            case LogLevel.Information: {
                return "Information";
            }
            case LogLevel.Warning: {
                return "Warning";
            }
            case LogLevel.Error: {
                return "Error";
            }
            case LogLevel.Fatal: {
                return "Fatal";
            }
            default: {
                return "Unknown Log Level";
            }
        }
    }

    public static GetDocumentStateText(documentState: DocumentStates): string {
        if (documentState == DocumentStates.Local)
            return "Local";

        return "Remote";
    }

    /** Removes all instances of containers labeled with the loading-panel css class from the dom.
     * If the parentElement is not specified or null, the root document element is used. */
    public static removeLoadingPanel(parentElement?: Document | HTMLElement | null) {
        if (!parentElement) {
            parentElement = document;
        }
        
        let elements = parentElement.getElementsByClassName("loading-panel");
        for (let i = 0; i < elements.length; i++) {
            let elm = elements.item(i);
            if (elm) {
                elm.remove();
            }
        }
    }
}



