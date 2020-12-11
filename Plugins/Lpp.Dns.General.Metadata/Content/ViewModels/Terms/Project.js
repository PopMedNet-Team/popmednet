/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Project.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var ProjectTerm = /** @class */ (function (_super) {
        __extends(ProjectTerm, _super);
        function ProjectTerm(projectData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.ProjectTerm) || this;
            _this.Project = ko.observable(projectData ? projectData.Project : '{00000000-0000-0000-0000-000000000000}');
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        ProjectTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var projectData = {
                TermType: superdata.TermType,
                Project: this.Project()
            };
            //console.log('Project: ' + JSON.stringify(projectData));
            return projectData;
        };
        return ProjectTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.ProjectTerm = ProjectTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
