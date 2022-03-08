var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Tasks;
(function (Tasks) {
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(task, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Task = new Dns.ViewModels.TaskViewModel(task);
                _this.WatchTitle(_this.Task.Subject, "Task: ");
                _this.Task.PercentComplete.subscribe(function (value) {
                    if (value == 100) {
                        _this.Task.Status(Dns.Enums.TaskStatuses.Complete);
                    }
                    else if (value > 0) {
                        _this.Task.Status(Dns.Enums.TaskStatuses.InProgress);
                    }
                    else {
                        _this.Task.Status(Dns.Enums.TaskStatuses.NotStarted);
                    }
                });
                _this.Task.Status.subscribe(function (value) {
                    if (value == Dns.Enums.TaskStatuses.Complete) {
                        _this.Task.PercentComplete(100);
                    }
                    else if (value == Dns.Enums.TaskStatuses.InProgress) {
                        if (_this.Task.PercentComplete() == 100) {
                            _this.Task.PercentComplete(50);
                        }
                        else if (_this.Task.PercentComplete() == 0) {
                            _this.Task.PercentComplete(20);
                        }
                    }
                    else if (value == Dns.Enums.TaskStatuses.NotStarted) {
                        _this.Task.PercentComplete(0);
                    }
                });
                return _this;
            }
            ViewModel.prototype.Save = function () {
                var task = vm.Task.toData();
                Dns.WebApi.Tasks.InsertOrUpdate([task]).done(function (tasks) {
                    vm.Task.ID(tasks[0].ID);
                    vm.Task.Timestamp(tasks[0].Timestamp);
                    Global.Helpers.ShowAlert("Success", "<p>Save completed successfully!</p>");
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            $.when(id == null ? null : Dns.WebApi.Tasks.Get([id])).done(function (tasks) {
                var task;
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
                }
                else {
                    task = tasks[0];
                }
                var bindingControl = $("#Content");
                vm = new ViewModel(task, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Details = Tasks.Details || (Tasks.Details = {}));
})(Tasks || (Tasks = {}));
