namespace Plugins.Requests.QueryBuilder {

    export class TermsObserver {

        public DistinctTerms: KnockoutComputed<any[]>;

        private _allTerms: KnockoutObservableArray<Global.KeyValuePair<any,KnockoutObservableArray<any>>>;

        constructor() {
            this._allTerms = ko.observableArray<Global.KeyValuePair<any, KnockoutObservableArray<any>>>([]);

            this.DistinctTerms = ko.computed<any[]>({
                read: () => {
                    let distinctTerms: any[] = [];

                    this._allTerms().forEach((terms) => {
                        terms.value().forEach((termID) => {
                            if (distinctTerms.indexOf(termID) < 0) {
                                distinctTerms.push(termID);
                            }
                        });
                    });

                    return distinctTerms;
                },
                owner: this,
                deferEvaluation: true
            }).extend({ rateLimit: { timeout:1500, method:"notifyWhenChangesStop" }});
        }

        public RegisterTermCollection(queryID: any, terms: KnockoutObservableArray<any>): void {
            this._allTerms.push(new Global.KeyValuePair(queryID, terms));
        }

        public RemoveTermCollection(queryID: any) {
            this._allTerms.remove((item) => item.key == queryID);
        }

    }

}