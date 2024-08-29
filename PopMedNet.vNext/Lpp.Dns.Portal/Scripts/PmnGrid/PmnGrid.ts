/// <reference path="..\..\..\node_modules\@types\kendo-ui\index.d.ts" />
/// <reference path="../page/page.ts" />

interface ISavedSetting {
    Key: string;
    Setting: string;
    dateColumns: string[];
}

interface IPmnGridSettings extends kendo.ui.GridOptions {
    savedSetting: ISavedSetting;
    useKOTemplates: boolean;
    columnDefaults: any;
}

ko.bindingHandlers.pmnGrid = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, context) {
        const binding: IPmnGridSettings = valueAccessor();
        const options: kendo.ui.GridOptions = binding;

        if (options.autoBind === undefined || options.autoBind === null) {
            options.autoBind = true;
        }

        if (options.height === undefined || options.height === null || options.height === '') {
            options.height = '100%';
        }

        if (options.sortable === undefined || options.sortable === null) {
            options.sortable = true;
        }

        if (options.resizable === undefined || options.resizable === null) {
            options.resizable = true;
        }

        if (options.reorderable === undefined || options.reorderable === null) {
            options.reorderable = true;
        }

        if (options.columnMenu === undefined || options.columnMenu === null || (options.columnMenu as kendo.ui.GridColumnMenu).columns === null || (options.columnMenu as boolean) === true) {
            options.columnMenu = { columns: true };
        }

        if (options.filterable === undefined || options.filterable === null || (options.filterable as kendo.ui.GridFilterable).operators === null || (options.filterable as boolean) === true) {
            options.filterable = Global.Helpers.GetColumnFilterOperatorDefaults();
        }

        const setupTempates = function (templates: string[], options, element, context) {
            let i, j, option, existingHandler;

            const templateRenderer = function (id, context) {
                return function (data) {
                    return ko.renderTemplate(id, context.createChildContext((data._raw && data._raw()) || data));
                };
            };


            if (templates && options && options.useKOTemplates) {
                //create a function to render each configured template
                for (i = 0, j = templates.length; i < j; i++) {
                    option = templates[i];
                    if (options[option]) {
                        options[option] = templateRenderer(options[option], context);
                    }
                }

            //initialize bindings in dataBound event
            existingHandler = options.dataBound;
            options.dataBound = function () {
                (ko.memoization as any).unmemoizeDomNodeAndDescendants(element);
                if (existingHandler) {
                    existingHandler.apply(this, arguments);
                }
             };
            }
        }

        setupTempates(["rowTemplate", "altRowTemplate"], binding, element, context);

        const widget = $(element).kendoGrid(options);

        if (binding.savedSetting !== undefined) {
            if (binding.savedSetting !== null && binding.savedSetting.Setting !== null && binding.savedSetting.dateColumns != undefined && binding.savedSetting.dateColumns.length > 0)
                Global.Helpers.SetDataSourceFromSettingsWithDates(widget.data("kendoGrid").dataSource, binding.savedSetting !== null ? binding.savedSetting.Setting : null, binding.savedSetting.dateColumns);
            else if (binding.savedSetting !== null && binding.savedSetting.Setting !== null)
                Global.Helpers.SetDataSourceFromSettings(widget.data("kendoGrid").dataSource, binding.savedSetting !== null ? binding.savedSetting.Setting : null);
            else 
                widget.data("kendoGrid").dataSource.read();
        }

        if (binding.columnDefaults !== undefined && binding.columnDefaults != null) {    
            Global.Helpers.SetGridDefaultColumnDefaultSizes(widget.data("kendoGrid"), binding.columnDefaults);
        }       

        if (options && binding.useKOTemplates) {
            return { controlsDescendantBindings: true };
        }
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, context) {
        const widget = $(element).data("kendoGrid");
        const binding: IPmnGridSettings = valueAccessor();
        const kendoOptions = widget.getOptions();

        //Need to bind these events after the grid has bound to the view.
        if (kendoOptions.columnMenu !== undefined || kendoOptions.columnMenu === null || (kendoOptions.columnMenu as kendo.ui.GridColumnMenu).columns !== null) {
            widget.bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
        }

        if (binding.columnDefaults !== undefined && binding.columnDefaults != null) {
            const onColumnVisibleChange = function (e) {
                let grd = e.sender as kendo.ui.Grid;
                let visibleColumns = ko.utils.arrayFilter(grd.columns, function (item: kendo.ui.GridColumn) { return item.hidden == undefined || item.hidden == false; });
                if (visibleColumns.length < grd.columns.length) {

                    let colWidth = grd.table.width() / visibleColumns.length;
                    visibleColumns.forEach(function (col: kendo.ui.GridColumn) {
                        grd.resizeColumn(col, colWidth);
                    });

                } else {
                    Global.Helpers.SetGridDefaultColumnDefaultSizes(widget, binding.columnDefaults);
                }
            };
            widget.bind("columnShow", onColumnVisibleChange);
            widget.bind("columnHide", onColumnVisibleChange);
        }

        if (binding.savedSetting !== undefined) {
            widget.bind("dataBound", function (e: any) {
                Users.SetSetting(binding.savedSetting.Key + User.ID, Global.Helpers.GetGridSettings(e.sender));
            });
        }
        
    }
};