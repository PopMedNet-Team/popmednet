/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />

module Dialog.CodeSelector {
    var vm: CodeSelectorViewModel;

    export class CodeSelectorViewModel extends Global.DialogViewModel {
        queryTimer: number = -1;
        public Query: KnockoutObservable<string>;
        public CategoriesValue: KnockoutObservable<number>;
        public dsResults: kendo.data.DataSource;
        public dsCategories: kendo.data.DataSource;
        public dsSelected: kendo.data.DataSource;
        public Changed: KnockoutObservable<boolean>;
        public ShowCategoryDropdown: KnockoutObservable<boolean>;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            var self = this;
            this.dsSelected = new kendo.data.DataSource({
                data: [],
                sort: {
                    field: 'ItemCode', 
                    dir: 'asc'
                }
            });

            this.Changed = ko.observable(false);

            this.dsCategories = new kendo.data.DataSource({
                data: [] 
            });
            var existingCodes = [];
            if (this.Parameters.Codes.length != 0) {
                
                for(var v in this.Parameters.Codes)
                {
                    var code = this.Parameters.Codes[v] || '';
                    if (code.length == 0)
                        continue;
                    if (typeof code === 'object') {
                        code.Name.replace('&#44;', ',');
                        code.Code.replace('&#44;', ',');
                        //existingCodes.push(code.replace('&#44;', ','));
                        existingCodes.push(code);
                    }
                    else {
                        existingCodes.push(code.replace('&#44;', ','));
                        existingCodes.push(code);
                    }
                    
                }               
            }

            if (existingCodes.length > 0) {
                if (typeof this.Parameters.Codes[0] === 'object') {
                    var strCodes: Dns.Interfaces.ILookupListValueDTO[] = this.Parameters.Codes.map((item) => {
                        var codeMap: Dns.Interfaces.ILookupListValueDTO = {
                            ListId: this.Parameters.ListID,
                            CategoryId: null,
                            ItemName: item.Name,
                            ItemCode: item.Code,
                            ItemCodeWithNoPeriod: null,
                            ExpireDate: item.ExpireDate,
                            ID: null,
                        };
                        return codeMap

                    });
                    self.dsSelected.data(strCodes)
                }
                else {
                    Dns.WebApi.LookupListValue.GetCodeDetailsByCode({
                        ListID: this.Parameters.ListID,
                        Codes: existingCodes
                    }).done((results: Dns.Interfaces.ILookupListValueDTO[]) => {
                        self.dsSelected.data(results);
                    });
                }
            }

            Dns.WebApi.LookupListCategory.GetList(self.Parameters.ListID).done((categories: Dns.Interfaces.ILookupListCategoryDTO[]) => {
                if (categories == null || categories.length == 0)
                    return;
                categories.unshift({ ListId: -1, CategoryId: -1, CategoryName: "" });
                categories[0].CategoryName = "";
                this.dsCategories.data(categories);
            });

            this.dsResults = new kendo.data.DataSource({
                data: [],
                sort: {
                    field: 'ItemCode',
                    dir: 'asc'
                }
            });

            this.ShowCategoryDropdown = ko.observable(this.Parameters.ShowCategoryDropdown == null ? true : this.Parameters.ShowCategoryDropdown);

            this.Query = ko.observable("");
            this.Query.subscribe((value: any) => {
                if (!value) {
                    vm.dsResults.data([]);
                    return;
                } else if (value.length < 2) {
                    return;
                }

                this.CategoriesValue(-1);

                if (this.queryTimer > -1)
                    clearTimeout(this.queryTimer);                

                //Set a timer that gets cancelled so that we can time it out.
                this.queryTimer = setTimeout(() => {
                    var grid: kendo.ui.Grid = $("#gResults").data("kendoGrid");
                    var lookup = this.Query();                                             
                    if (!lookup) {                            
                        vm.dsResults.data([]);
                        grid.refresh();
                        return;
                    }                                   

                    Dns.WebApi.LookupListValue.LookupList(self.Parameters.ListID, lookup).done((data: Dns.Interfaces.ILookupListValueDTO[]) => {
                        data.forEach((item) => {
                            item["Selected"] = false;
                        });

                        this.dsResults.data(data);
                        grid.refresh();
                    });
                }, 250);
            });
            
            this.CategoriesValue = ko.observable(null);
            this.CategoriesValue.subscribe((categoryValue) => {                
                var grid: kendo.ui.Grid = $("#gResults").data("kendoGrid");
                vm.dsResults.data([]);
                if (!categoryValue) {                    
                    grid.refresh();
                    return;
                }
                
                Dns.WebApi.LookupListValue.GetList("CategoryId eq " + categoryValue + " and ListId eq Lpp.Dns.DTO.Enums.Lists'" + self.Parameters.ListID + "'").done((data: Dns.Interfaces.ILookupListValueDTO[]) => {
                    data.forEach((item) => {
                        item["Selected"] = false;
                    });

                    this.dsResults.data(data);
                    grid.refresh();
                });;
            });
            
        } 

        public Save(data, event) {
            
            //var codes = this.dsSelected.data().map((item: any) => {
            //    return item.ItemCode.trim();
            //});

            //this.Close(codes);

            var codes = this.dsSelected.data().map((item: any) => {
                //var val: Dns.ViewModels.CodeSelectorValueViewModel;
                //val = new Dns.ViewModels.CodeSelectorValueViewModel();
                //val.Code(item.ItemCode.trim());
                //val.Name(item.ItemName.trim());
                //val.ExpireDate = item.ExpireDate;
                var val = <Dns.Interfaces.ICodeSelectorValueDTO>{ Code: item.ItemCode.trim(), Name: item.ItemName.trim(), ExpireDate: null };
                return val;
            }); 

            this.Close(codes);
        }

        public Cancel(data, event) {
            this.Close();
        }
        
        public AddCode(arg: kendo.ui.GridChangeEvent) {
            $.each(arg.sender.select(), (count: number, item: JQuery) => {
                var dataItem: any = arg.sender.dataItem(item);                
                vm.dsSelected.data().push(dataItem);
                $.each(vm.dsResults.data(), (count: number, data: any) => {
                    if (data == null)
                        return;

                    if (data.ID == dataItem.ID) {
                        vm.dsResults.data().splice(count, 1);
                        return;
                    }
                });
                
            });

            vm.Changed(true);           
        }

        public RemoveCode(arg: kendo.ui.GridChangeEvent) {
            $.each(arg.sender.select(), (count: number, item: JQuery) => {
                var dataItem: any = arg.sender.dataItem(item);
                vm.dsResults.data().push(dataItem);
                $.each(vm.dsSelected.data(), (count: number, data: any) => {
                    if (data == null)
                        return;

                    if (data.ID == dataItem.ID) {
                        vm.dsSelected.data().splice(count, 1);
                        return;
                    }
                });
            });

            vm.Changed(true);
        }
    }
    
    function init() {
        //In this case we do all of the data stuff in the view model because it has the parameters.
        $(() => {
            var bindingControl = $("body");
            vm = new CodeSelectorViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}