/// <reference path="..\..\..\node_modules\@types\kendo-ui\index.d.ts" />
import * as Global from "../page/global.js";
import { UserSettingHelper } from "../../js/_RootLayout.js";
ko.bindingHandlers.pmnGrid = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, context) {
        const binding = valueAccessor();
        const options = binding;
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
        if (options.columnMenu === undefined || options.columnMenu === null || options.columnMenu.columns === null || options.columnMenu === true) {
            options.columnMenu = { columns: true };
        }
        if (options.filterable === undefined || options.filterable === null || options.filterable.operators === null || options.filterable === true) {
            options.filterable = Global.Helpers.GetColumnFilterOperatorDefaults();
        }
        const setupTempates = function (templates, options, element, context) {
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
                    ko.memoization.unmemoizeDomNodeAndDescendants(element);
                    if (existingHandler) {
                        existingHandler.apply(this, arguments);
                    }
                };
            }
        };
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
        const binding = valueAccessor();
        const kendoOptions = widget.getOptions();
        //Need to bind these events after the grid has bound to the view.
        if (kendoOptions.columnMenu !== undefined || kendoOptions.columnMenu === null || kendoOptions.columnMenu.columns !== null) {
            widget.bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
        }
        if (binding.columnDefaults !== undefined && binding.columnDefaults != null) {
            const onColumnVisibleChange = function (e) {
                let grd = e.sender;
                let visibleColumns = ko.utils.arrayFilter(grd.columns, function (item) { return item.hidden == undefined || item.hidden == false; });
                if (visibleColumns.length < grd.columns.length) {
                    let colWidth = grd.table.width() / visibleColumns.length;
                    visibleColumns.forEach(function (col) {
                        grd.resizeColumn(col, colWidth);
                    });
                }
                else {
                    Global.Helpers.SetGridDefaultColumnDefaultSizes(widget, binding.columnDefaults);
                }
            };
            widget.bind("columnShow", onColumnVisibleChange);
            widget.bind("columnHide", onColumnVisibleChange);
        }
        if (binding.savedSetting !== undefined) {
            widget.bind("dataBound", function (e) {
                UserSettingHelper.SetSetting(binding.savedSetting.Key + Global.User.ID, Global.Helpers.GetGridSettings(e.sender));
            });
        }
    }
};
//# sourceMappingURL=PmnGrid.js.map