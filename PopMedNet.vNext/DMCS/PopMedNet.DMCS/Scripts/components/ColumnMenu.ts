import Vue from 'vue';
import {
    GridColumnMenuFilter,
    GridColumnMenuSort,
    GridColumnMenuItemGroup,
    GridColumnMenuItemContent,
    GridColumnMenuItem,
    GridColumnProps
} from '@progress/kendo-vue-grid';
import { Component, Prop } from 'vue-property-decorator';
import { Button } from '@progress/kendo-vue-buttons';

@Component({
    template: '#column-menu-template',
    components: {
        GridColumnMenuFilter,
        GridColumnMenuSort,
        GridColumnMenuItemGroup,
        GridColumnMenuItemContent,
        GridColumnMenuItem,
        'kbutton': Button
    },
    computed: {
        oneVisibleColumn() {
            return this.$data.currentColumns.filter(c => !c.hidden).length === 1;
        }
    },
    created: function () {
        this.$data.currentColumns = this.$props.columns;
    }
})
export default class ColumnMenu extends Vue {
    @Prop()
    column!: object;

    @Prop()
    sortable!: boolean | object;

    @Prop()
    sort!: { type: Array<any> };

    @Prop()
    filter!: object;

    @Prop()
    filterable!: boolean;

    @Prop()
    columns!: GridColumnProps[]

    currentColumns: GridColumnProps[] = [];
    columnsExpanded: boolean = false;
    filterExpanded: boolean = false;

    handleFocus(e) {
        this.$emit("contentfocus", e);
    }

    onToggleColumn(id) {
        this.currentColumns = this.currentColumns.map((column, idx) => {
            return idx === id ? { ...column, hidden: !column.hidden } : column;
        });
    }

    onReset(event) {
        event.preventDefault();

        const hiddenFields: string[] = ["requestType", "dataModel",  "submittedBy"];

        const allColumns = this.$props.columns.map(col => {
            let c = col as GridColumnProps;
            return {
                ...col,
                hidden: hiddenFields.indexOf(c.field||'', 0) >= 0
            };
        });

        this.currentColumns = allColumns;
        this.onSubmit(null);
    }

    onSubmit(event) {
        if (event) {
            event.preventDefault();
        }
        this.$emit('columnssubmit', this.currentColumns);
        this.$emit('closemenu');
    }

    onMenuItemClick() {
        const value = !this.columnsExpanded;
        this.columnsExpanded = value;
        this.filterExpanded = value ? false : this.filterExpanded;
    }

    onFilterExpandChange(value) {
        this.filterExpanded = value;
        this.columnsExpanded = value ? false : this.columnsExpanded;
    }

    expandChange() {
        this.$emit('expandchange');
    }

    closeMenu() {
        this.$emit('closemenu');
    }

    filterChange(newDescriptor, e) {
        this.$emit('filterchange', newDescriptor, e);
    }

    sortChange(newDescriptor, e) {
        this.$emit('sortchange', newDescriptor, e);
    }

}