/// <reference path="../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
module DataChecker {
    export interface IResultsModelData {
        RawData: any;
    }

    export class ResponseMetricsItem {
        public title: string;
        public value: number;

        constructor(title, value) {
            this.title = title;
            this.value = value;
        }
    }

    export interface IDnsRequestType {
        Id: string;
        Name: string;
        Description: string;
        ShortDescription: string;
        IsMetadataRequest: boolean;
    }

    export interface IRaceItemData {
        DP: string;
        RACE: number;
        Total: number;
    }

    export interface IEthnicityItemData {
        DP: string;
        HISPANIC: string;
        Total: number;
    }

    export interface IDiagnosesItemData {
        DP: string;
        DX: string;
        Dx_Codetype: string;
        n: number;
    }

    export interface IProcedureItemData {
        DP: string;
        PX: string;
        Px_Codetype: string;
        n: number;
    }

    export interface INDCItemData {
        DP: string;
        NDC: string;
    }

    ///Defines a compound key used in grouping types that have a code and codetype.
    export interface ICodeTypeKey {
        Code: string;
        CodeType: string;
    }

    export class ChartSource {
        public data: Array<Array<any>>;
        public title: string;
        public showTooltips: boolean;
        private _rotateXAxis: boolean;
        public xaxis_label: string = null;
        public yaxis_label: string = null;
        //default to 2 decimal for the point label format string
        public pointLabelFormatString: string = '%.2f';
        public showPiePointLabels: boolean = false;
        public isPercentage: boolean = false;
        public multiSeriesData: IChartSeriesData = null;

        constructor();
        constructor(data: Array<Array<any>>);
        constructor(data: Array<Array<any>>, title: string);
        constructor(data?: Array<Array<any>>, title?: string) {
            this.data = data || null;
            this.title = title || null;
            this._rotateXAxis = false;
            this.showTooltips = false;
        }

        public hasTitle(): boolean {
            return this.title != null && this.title.length >= 0;
        }

        public rotateXAxisLabels(): boolean {
            return this._rotateXAxis;
        }

        public setXAxisLabelRotation(rotate: boolean): void {
            this._rotateXAxis = rotate;
        }

        public setPointLabelDecimals(value: number) {
            if (value == 0) {
                this.pointLabelFormatString = '';
            } else {
                this.pointLabelFormatString = '%.' + value + 'f';
            }
        }
    }

    export interface IChartSeriesData {
        series_labels: IChartSeriesLabel[];
        ticks: string[];
    }

    export interface IChartSeriesLabel {
        label: string;
    } 
}

declare module DataChecker.Charting {
    function plotBarChart(element: JQuery, source: DataChecker.ChartSource): any;

    function plotPieChart(element: JQuery, source: DataChecker.ChartSource): any;

    function replot(element: any);
}