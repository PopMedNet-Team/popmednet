/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
var DataChecker;
(function (DataChecker) {
    var ResponseMetricsItem = (function () {
        function ResponseMetricsItem(title, value) {
            this.title = title;
            this.value = value;
        }
        return ResponseMetricsItem;
    }());
    DataChecker.ResponseMetricsItem = ResponseMetricsItem;
    var ChartSource = (function () {
        function ChartSource(data, title) {
            this.xaxis_label = null;
            this.yaxis_label = null;
            //default to 2 decimal for the point label format string
            this.pointLabelFormatString = '%.2f';
            this.showPiePointLabels = false;
            this.isPercentage = false;
            this.multiSeriesData = null;
            this.data = data || null;
            this.title = title || null;
            this._rotateXAxis = false;
            this.showTooltips = false;
        }
        ChartSource.prototype.hasTitle = function () {
            return this.title != null && this.title.length >= 0;
        };
        ChartSource.prototype.rotateXAxisLabels = function () {
            return this._rotateXAxis;
        };
        ChartSource.prototype.setXAxisLabelRotation = function (rotate) {
            this._rotateXAxis = rotate;
        };
        ChartSource.prototype.setPointLabelDecimals = function (value) {
            if (value == 0) {
                this.pointLabelFormatString = '';
            }
            else {
                this.pointLabelFormatString = '%.' + value + 'f';
            }
        };
        return ChartSource;
    }());
    DataChecker.ChartSource = ChartSource;
})(DataChecker || (DataChecker = {}));
//# sourceMappingURL=Common.js.map