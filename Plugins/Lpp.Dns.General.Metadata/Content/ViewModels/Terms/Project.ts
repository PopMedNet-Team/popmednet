/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Project.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class ProjectTerm extends RequestCriteriaViewModels.Term {
        public Project: KnockoutObservable<string>;

        constructor(projectData?: RequestCriteriaModels.IProjectTermData) {
            super(RequestCriteriaModels.TermTypes.ProjectTerm);

            this.Project = ko.observable(projectData ? projectData.Project : '{00000000-0000-0000-0000-000000000000}');

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IProjectTermData {
            var superdata = super.toData();

            var projectData: RequestCriteriaModels.IProjectTermData = {
                TermType: superdata.TermType,
                Project: this.Project()
            };

            //console.log('Project: ' + JSON.stringify(projectData));

            return projectData;
        }
    }
}