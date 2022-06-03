/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../lpp.dns.general.metadata/content/models/requestcriteriamodels.ts" />
/// <reference path="../../../../lpp.dns.portal/scripts/page/page.ts" />
/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
/// <reference path="../../../lpp.dns.general.metadata/content/viewmodels/requestcriteriaviewmodels.ts" />

module DataChecker.Create {
    var vm: ViewModel;

    export interface IDataCheckerRequestData extends RequestCriteriaModels.IRequestCriteriaData {
    }

    export class ViewModel extends Dns.PageViewModel {
        public RequestCriteria: RequestCriteriaViewModels.RequestCriteria;

        constructor(dataCheckerRequestData: IDataCheckerRequestData, hiddenDataControl: JQuery) {
            super(hiddenDataControl);

            this.RequestCriteria = new RequestCriteriaViewModels.RequestCriteria(dataCheckerRequestData);
        }

        //This is the event handler for the button click to add
        public AddCriteriaGroup(data, event): boolean {
            return true;
        }

        public save(): boolean {
            

            //ko validation here
            ////if ( !this.isValid() )
            ////    return false;

            var superdata: RequestCriteriaModels.IRequestCriteriaData = this.RequestCriteria.toData();
            $.each(superdata.Criterias, (index, criteria) => {
                criteria.Terms = criteria.RequestTerms;
            });
            var dataCheckerRequestData: IDataCheckerRequestData = {
                Criterias: superdata.Criterias
            };

            return this.store(dataCheckerRequestData);
        }
    }

    export function init(dataCheckerRequestData: IDataCheckerRequestData, bindingControl: JQuery, hiddenDataControl: JQuery): void {
        // initialize dynamic lookup lists...???
        vm = new DataChecker.Create.ViewModel(dataCheckerRequestData, hiddenDataControl);
        ko.applyBindings(vm, bindingControl[0]);
        bindingControl.fadeIn(100);
        Dns.EnableValidation();
    }
}