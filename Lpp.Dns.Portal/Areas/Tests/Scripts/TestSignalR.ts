/// <reference path="../../../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../../../node_modules/@types/signalr/index.d.ts" />


var CrudFunction: (data) => void;

$(() => {
    Dns.WebApi.RequestsHub.NotifyCrud((data) => {
        console.log(data);
        
    });
});