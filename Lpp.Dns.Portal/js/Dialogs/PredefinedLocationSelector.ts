/// <reference path="../../Scripts/page/Page.ts" />

module Dialog.PredefinedLocationSelector {

    export interface IDefinedLocation {
        StateAbbrev: string;
        Location: string;
        PostalCodes: string[]
    }

    export class ViewModel extends Global.DialogViewModel {
        queryTimer: number = -1;
        public Query: KnockoutObservable<string>;
        public Changed: KnockoutObservable<boolean>;
        public dsStates: kendo.data.DataSource;
        public SelectedState: KnockoutObservable<string>;
        public dsResults: kendo.data.DataSource;
        public dsSelected: kendo.data.DataSource;

        public Save: (data: any, evt: any) => void;
        public Cancel: (data: any, evt: any) => void;
        public AddCode: (arg: kendo.ui.GridChangeEvent) => void;
        public RemoveCode: (arg: kendo.ui.GridChangeEvent) => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            var self = this;

            this.Changed = ko.observable(false);            
            this.SelectedState = ko.observable('');

            this.dsSelected = new kendo.data.DataSource({
                data: [],
                sort: [
                    { field: 'StateAbbrev', dir: 'asc' },
                    { field: 'Location', dir: 'asc' }
                ]
            });

            this.dsStates = new kendo.data.DataSource({
                data: []
            });

            this.dsResults = new kendo.data.DataSource({
                data: []
            });
            
            if (this.Parameters != null) {
                self.dsSelected.data(this.Parameters);
            }

            $.getJSON('/api/demographics/GetGeographicLocationStates').done((results) => {
                if (!results)
                    return;

                results.unshift({ 'State': '', 'StateAbbrev': '' });
                self.dsStates.data(results);

                self.SelectedState.subscribe((value) => {
                    var grid: kendo.ui.Grid = $("#gResults").data("kendoGrid");

                    self.dsResults.data([]);

                    if ((value || '').length == 0) {
                        grid.refresh();
                        return;
                    }

                    self.Query('');

                    $.getJSON('/api/demographics/GetGeographicLocationsByState?stateAbbrev=' + value).done((results: IDefinedLocation[]) => {
                        if (results) {
                            self.dsResults.data(results);
                            grid.refresh();
                        }
                    });

                });

            });            


            this.Query = ko.observable("");
            this.Query.subscribe((value: any) => {
                if (!value) {
                    self.dsResults.data([]);
                    return;
                } else if (value.length < 2) {
                    return;
                }

                self.SelectedState('');

                if (self.queryTimer > -1)
                    clearTimeout(self.queryTimer);                

                //Set a timer that gets cancelled so that we can time it out.
                self.queryTimer = setTimeout(() => {
                    var grid: kendo.ui.Grid = $("#gResults").data("kendoGrid");
                    var lookup = self.Query();
                    if (!lookup) {
                        self.dsResults.data([]);
                        grid.refresh();
                        return;
                    }

                    $.getJSON('/api/demographics/QueryGeographicLocations?lookup=' + lookup).done((results: IDefinedLocation[]) => {
                        if (results) {
                            self.dsResults.data(results);
                            grid.refresh();
                        }
                    });
                    

                }, 250);
            });


            this.Save = (data, evt) => {
                self.Close(self.dsSelected.data());
            };

            this.Cancel = (data, evt) => {
                self.Close();
            };

            this.AddCode = (arg) => {
                $.each(arg.sender.select(),(count: number, item: JQuery) => {
                    var dataItem: any = arg.sender.dataItem(item);
                    self.dsSelected.data().push(dataItem);
                    $.each(self.dsResults.data(),(count: number, data: any) => {
                        if (data == null)
                            return;

                        if (data.StateAbbrev == dataItem.StateAbbrev && data.Location == dataItem.Location) {
                            self.dsResults.data().splice(count, 1);
                            return;
                        }
                    });

                });

                self.Changed(true); 
            };

            this.RemoveCode = (arg) => {
                $.each(arg.sender.select(),(count: number, item: JQuery) => {
                    var dataItem: any = arg.sender.dataItem(item);
                    var grid: kendo.ui.Grid = $("#gResults").data("kendoGrid");
                    grid.dataSource.add(dataItem);
                    
                    $.each(self.dsSelected.data(),(count: number, data: any) => {
                        if (data == null)
                            return;

                        if (data.StateAbbrev == dataItem.StateAbbrev && data.Location == dataItem.Location) {
                            self.dsSelected.data().splice(count, 1);
                            return;
                        }
                    });
                });
                
                self.Changed(true);
            };

        }
    }


    function init() {
        $(() => {
            var bindingControl = $('body');
            var vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }
    init();
}