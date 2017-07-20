/// <reference path="../_rootlayout.ts" />
module Tasks.Details {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public Task: Dns.ViewModels.TaskViewModel;

        constructor(
            task: Dns.Interfaces.ITaskDTO,
            bindingControl: JQuery) {
            super(bindingControl);

            this.Task = new Dns.ViewModels.TaskViewModel(task);
            this.WatchTitle(this.Task.Subject, "Task: ");  
            this.Task.PercentComplete.subscribe((value) => {
                if (value == 100) {
                    this.Task.Status(Dns.Enums.TaskStatuses.Complete);
                } else if (value > 0) {
                    this.Task.Status(Dns.Enums.TaskStatuses.InProgress);
                } else {
                    this.Task.Status(Dns.Enums.TaskStatuses.NotStarted);
                }
            });

            this.Task.Status.subscribe((value) => {
                if (value == Dns.Enums.TaskStatuses.Complete) {
                    this.Task.PercentComplete(100);
                } else if (value == Dns.Enums.TaskStatuses.InProgress) {
                    if (this.Task.PercentComplete() == 100) {
                        this.Task.PercentComplete(50);
                    } else if (this.Task.PercentComplete() == 0) {
                        this.Task.PercentComplete(20);
                    }
                } else if (value == Dns.Enums.TaskStatuses.NotStarted) {
                    this.Task.PercentComplete(0);
                }
            });          
        }

        public Save() {
            var task = vm.Task.toData();

            Dns.WebApi.Tasks.InsertOrUpdate([task]).done((tasks) => {
                vm.Task.ID(tasks[0].ID);
                vm.Task.Timestamp(tasks[0].Timestamp);

                Global.Helpers.ShowAlert("Success", "<p>Save completed successfully!</p>");
            });
        }

        public Cancel() {
            window.history.back();
        }
    }

    function init() {
        var id = $.url().param("ID");

        $.when<any>(
            id == null ? null : Dns.WebApi.Tasks.Get([id])
        ).done((tasks: Dns.Interfaces.ITaskDTO[]) => {
            var task: Dns.Interfaces.ITaskDTO;
            if (tasks == null || tasks.length == 0) {
                task = {
                    Body: "",
                    CreatedOn: new Date(),
                    DueDate: null,
                    EndOn: null,
                    EstimatedCompletedOn: null,
                    ID: null,
                    Location: null,
                    PercentComplete: 0,
                    Priority: Dns.Enums.Priorities.Medium,
                    StartOn: new Date(),
                    Status: Dns.Enums.TaskStatuses.NotStarted,
                    Subject: "New Task",
                    Type: Dns.Enums.TaskTypes.Task,
                    Timestamp: null,
                    DirectToRequest: false,
                    WorkflowActivityID: null
                };
            } else {
                task = tasks[0];
            }

            var bindingControl = $("#Content");
            vm = new ViewModel(task, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}