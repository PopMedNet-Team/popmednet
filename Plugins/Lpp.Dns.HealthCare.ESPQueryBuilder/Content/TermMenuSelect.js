define(['ko', 'lpp.mvc.healthcare.espquerybuilder/TermViewModel'], function (ko, TermViewModel) {
    return function (serviceUrl) {
        return function (event, ui) {
            event.preventDefault();

            var term = $(ui.item[0]).find("span.TermMenuItem > span").attr("class");
            var url = window.location.protocol + '//' + window.location.host + '/' + serviceUrl + '/' + term;
            var defaults = {
                url: url,
                type: 'GET',
                success: function (data, status) {
                    // Add html data for term widget under .InputParameters
                    if (data != null) {
                        $(".TermSubmenu").hide();
                        // append the data to the InputParameters placeholder
                        var appended = $(data).appendTo($(ui.item[0]).parents(".CriteriaGroup").find(".InputParameters"));                        

                        // hook the remove term button
                        $(appended).find(".ui-button-remove").attr("data-bind", "click: removeTerm");

                        // update the report parameters with this new term and dirty the form
                        updatePrimarySelectors();
                        $("form").formChanged(true);
                    
                        ko.applyBindings(TermViewModel(), $(appended)[0]);
                    }
                    return false;
                },
                error: function (jqXHR, status, error) {
                    alert("Ajax request to \"" + url + "\" failed with a status of \"" + status + "\". The HTTP Error Message is \"" + error + "\". ");
                    return false;
                }
            };

            $.ajax(defaults);

        }
    }
});