/// <reference path="../../../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../../../node_modules/@types/signalr/index.d.ts" />
var CrudFunction;
$(function () {
    Dns.WebApi.RequestsHub.NotifyCrud(function (data) {
        console.log(data);
    });
});
