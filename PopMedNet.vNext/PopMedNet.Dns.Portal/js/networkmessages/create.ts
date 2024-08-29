import * as Global from "../../scripts/page/global.js";
import { NetworkMessageViewModel } from "../Lpp.Dns.ViewModels.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';

export default class ViewModel extends Global.PageViewModel {
    public Message: NetworkMessageViewModel;
    public Target: KnockoutObservable<string>;
    public Targets: KnockoutObservable<any[]>;
    public dsTargets: kendo.data.DataSource;

    constructor(bindingControl: JQuery) {
        super(bindingControl);

        this.Message = new NetworkMessageViewModel();
        this.Target = ko.observable("0");
        this.Targets = ko.observable([]);

        this.dsTargets = new kendo.data.DataSource({
            serverFiltering: true,
            transport: {
                read: {
                    url: Global.Helpers.GetServiceUrl("/security/ListSecurityEntities"),
                    method: "GET",
                    dataType: "json",
                    beforeSend: function (request) {
                        request.setRequestHeader('Authorization', "PopMedNet " + Global.User.AuthToken)
                    }
                },
                parameterMap: function (data) {
                    //need to modify the query string, inlinecount specified with the dollar sign is not accepted in the webapi, replace without dollar sign.
                    //the map is an actual object representing the query string
                    let map = kendo.data.transports.odata.parameterMap(data, "read");
                    map.inlinecount = map["$inlinecount"];
                    delete map.$inlinecount;
                    return map;
                }
            }
        });
    }

    public btnSendMessage_Click() {
        if (!this.Validate())
            return;

        let data = this.Message.toData();

        data.Targets = this.Targets();

        WebApi.NetworkMessages.Insert([data]).done(() => {
            window.location.href = "/";
        });
    }
}

$(() => {
    let bindingControl = $("#Content");
    let vm = new ViewModel(bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});