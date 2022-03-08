var CrudFunction;
$(function () {
    Dns.WebApi.RequestsHub.NotifyCrud(function (data) {
        console.log(data);
    });
});
