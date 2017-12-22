/// <reference path="../../../../lpp.pmn.resources/scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../../../lpp.pmn.resources/scripts/typings/signalr/signalr.d.ts" />

var CrudFunction: (data) => void;

$(() => {
    Dns.WebApi.RequestsHub.NotifyCrud((data) => {
        console.log(data);
        
    });
});