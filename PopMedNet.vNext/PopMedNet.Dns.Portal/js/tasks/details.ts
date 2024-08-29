import * as Global from "../../scripts/page/global.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import * as Interfaces from "../Dns.Interfaces.js";
import * as Enums from "../Dns.Enums.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';

export class ViewModel extends Global.PageViewModel {
    public Task: ViewModels.TaskViewModel;
    public PrioritiesTranslation = Enums.PrioritiesTranslation;
    public TaskStatusesTranslation = Enums.TaskStatusesTranslation;

    public readonly EditorTools: string[] = ["bold",
        "italic",
        "underline",
        "undo",
        "redo",
        "strikethrough",
        "justifyLeft",
        "justifyCenter",
        "justifyRight",
        "justifyFull",
        "insertUnorderedList",
        "insertOrderedList",
        "insertUpperRomanList",
        "insertLowerRomanList",
        "indent",
        "outdent",
        "createLink",
        "unlink",        
        "subscript",
        "superscript",
        "tableWizard",
        "createTable",
        "addRowAbove",
        "addRowBelow",
        "addColumnLeft",
        "addColumnRight",
        "deleteRow",
        "deleteColumn",
        "mergeCellsHorizontally",
        "mergeCellsVertically",
        "splitCellHorizontally",
        "splitCellVertically",
        "tableAlignLeft",
        "tableAlignCenter",
        "tableAlignRight",
        "formatting",
        "cleanFormatting",
        "copyFormat",
        "applyFormat",
        "fontName",
        "fontSize",
        "foreColor",
        "backColor"];

    constructor(
        task: Dns.Interfaces.ITaskDTO,
        bindingControl: JQuery) {
        super(bindingControl);

        this.Task = new ViewModels.TaskViewModel(task);
        this.WatchTitle(this.Task.Subject, "Task: ");
        this.Task.PercentComplete.subscribe((value) => {
            if (value == 100) {
                this.Task.Status(Enums.TaskStatuses.Complete);
            } else if (value > 0) {
                this.Task.Status(Enums.TaskStatuses.InProgress);
            } else {
                this.Task.Status(Enums.TaskStatuses.NotStarted);
            }
        });

        this.Task.Status.subscribe((value) => {
            if (value == Enums.TaskStatuses.Complete) {
                this.Task.PercentComplete(100);
            } else if (value == Enums.TaskStatuses.InProgress) {
                if (this.Task.PercentComplete() == 100) {
                    this.Task.PercentComplete(50);
                } else if (this.Task.PercentComplete() == 0) {
                    this.Task.PercentComplete(20);
                }
            } else if (value == Enums.TaskStatuses.NotStarted) {
                this.Task.PercentComplete(0);
            }
        });
    }

    public Save() {
        let task = this.Task.toData();

        WebApi.Tasks.InsertOrUpdate([task]).done((tasks) => {
            this.Task.ID(tasks[0].ID);
            this.Task.Timestamp(tasks[0].Timestamp);

            Global.Helpers.ShowAlert("Success", "<p>Save completed successfully!</p>");
        });
    }

    public Cancel() {
        window.location.href = "/";
    }
}

let id = Global.GetQueryParam("ID");

$.when<any>(
    id == null ? null : WebApi.Tasks.Get(id)
).done((task: Interfaces.ITaskDTO) => {
    if (task == null || task == undefined) {
        task = {
            Body: "",
            CreatedOn: new Date(),
            DueDate: null,
            EndOn: null,
            EstimatedCompletedOn: null,
            ID: null,
            Location: null,
            PercentComplete: 0,
            Priority: Enums.Priorities.Medium,
            StartOn: new Date(),
            Status: Enums.TaskStatuses.NotStarted,
            Subject: "New Task",
            Type: Enums.TaskTypes.Task,
            Timestamp: null,
            DirectToRequest: false,
            WorkflowActivityID: null
        };
    }

    let bindingControl = $("#Content");
    let vm = new ViewModel(task, bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});