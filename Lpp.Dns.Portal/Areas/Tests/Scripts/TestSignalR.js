/// <reference path="../../../../lpp.pmn.resources/scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../../../lpp.pmn.resources/scripts/typings/signalr/signalr.d.ts" />
var CrudFunction;
$(function () {
    Dns.WebApi.RequestsHub.NotifyCrud(function (data) {
        console.log(data);
    });
});
//# sourceMappingURL=TestSignalR.js.map