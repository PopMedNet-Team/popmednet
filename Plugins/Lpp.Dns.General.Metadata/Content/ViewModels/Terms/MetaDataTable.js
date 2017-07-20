/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/metadatatable.ts" />
///// <reference path="../Terms.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
    var MetaDataTableTerm = (function (_super) {
        __extends(MetaDataTableTerm, _super);
        function MetaDataTableTerm(tableData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.MetaDataTableTerm) || this;
            var dummy = [];
            _this.Tables = ko.observableArray(tableData ? tableData.Tables : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        MetaDataTableTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var pdxData = {
                TermType: superdata.TermType,
                Tables: this.Tables()
            };
            return pdxData;
        };
        return MetaDataTableTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.MetaDataTableTerm = MetaDataTableTerm;
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
