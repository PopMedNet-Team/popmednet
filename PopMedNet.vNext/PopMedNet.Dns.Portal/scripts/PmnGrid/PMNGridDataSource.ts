import { User } from "../page/global.js";
export default class PMNGridDataSource<T> extends kendo.data.DataSource {

    constructor(serviceUrl: string, sort: any | null, requestEnd: (e: kendo.data.DataSourceRequestEndEvent) => void|null, c: new () => T) {
        super({
            type: "odata-v4",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 50,
            transport: {
                read: {
                    url: serviceUrl,
                    type: "GET",
                    beforeSend: function (request) {
                        request.setRequestHeader('Authorization', "PopMedNet " + User.AuthToken)
                    }
                }
            },
            schema: {
                model: kendo.data.Model.define(c),
                total: "Count",
                data: "Results"
            },
            sort: sort,
            requestEnd: requestEnd
        });      
    }

    public static ResizeGridFromResults(e: kendo.data.DataSourceRequestEndEvent, fullSize: number, minSize: number): number {
        if (e.response != null && e.response.Results != null && e.response.Results.length && (e.response.Count != null && e.response.Count * 36 > fullSize)) {
            return fullSize;
        } else {
            return minSize;
        }
    }
}
