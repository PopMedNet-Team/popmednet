import { User } from "../page/global.js";
export default class PMNGridDataSource extends kendo.data.DataSource {
    constructor(serviceUrl, sort, requestEnd, c) {
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
                        request.setRequestHeader('Authorization', "PopMedNet " + User.AuthToken);
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
    static ResizeGridFromResults(e, fullSize, minSize) {
        if (e.response != null && e.response.Results != null && e.response.Results.length && (e.response.Count != null && e.response.Count * 36 > fullSize)) {
            return fullSize;
        }
        else {
            return minSize;
        }
    }
}
//# sourceMappingURL=PMNGridDataSource.js.map