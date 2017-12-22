var config = config || {
    reportcodes: {},
    termSelectionUrl: ''
};

var reportsModel = reportsModel || {};

// Bind ICD9, Disease, Period, and Age Report Selector visibility to whether these Terms were added by the user.
// These counts are performed each time a term is added or deleted or a criteria group is deleted (ViewModel.js and TermViewModel.js).
// This is shared between this file, TermMenuSelect, and TermViewModel.
function updatePrimarySelectors() {
    var hasICD9Selectors = $("#cg1.CriteriaGroup .InputParameters .ICD9CodeSelectorTerm, #cg1.CriteriaGroup .OrInputParameters .ICD9CodeSelectorTerm, #cg1.CriteriaGroup .AddInputParameters .ICD9CodeSelectorTerm").length > 0;
    var hasDiseaseSelectors = $("#cg1.CriteriaGroup .InputParameters .DiseaseSelectorTerm, #cg1.CriteriaGroup .OrInputParameters .DiseaseSelectorTerm, #cg1.CriteriaGroup .AddInputParameters .DiseaseSelectorTerm").length > 0;
    var hasVisitsSelectors = $("#cg1.CriteriaGroup .InputParameters .VisitsTerm, #cg1.CriteriaGroup .OrInputParameters .VisitsTerm, #cg1.CriteriaGroup .AddInputParameters .VisitsTerm").length > 0;

    // console.log(hasICD9Selectors + " " + hasDiseaseSelectors + " " + hasVisitsSelectors);
    var showAgeSelector = (hasICD9Selectors || hasDiseaseSelectors || hasVisitsSelectors) ? "table-row" : "none";
    var showCenterSelector = (hasICD9Selectors || hasDiseaseSelectors) ? "table-row" : "none";
    var showDiseaseSelector = (!hasICD9Selectors && hasDiseaseSelectors) ? "table-row" : "none";
    var showICD9Selector = (hasICD9Selectors) ? "table-row" : "none";
    var showPeriodSelector = (hasICD9Selectors || hasDiseaseSelectors) ? "table-row" : "none";
    var showRaceSelector = (hasICD9Selectors || hasDiseaseSelectors || hasVisitsSelectors) ? "table-row" : "none";
    var showEthnicitySelector = (hasICD9Selectors || hasDiseaseSelectors || hasVisitsSelectors) ? "table-row" : "none";
    var showSexSelector = (hasICD9Selectors || hasDiseaseSelectors || hasVisitsSelectors) ? "table-row" : "none";
    var showZipSelector = (hasICD9Selectors || hasDiseaseSelectors || hasVisitsSelectors) ? "table-row" : "none";

    $(".ReportParameters .Selections table tr#" + config.reportcodes.age).css("display", showAgeSelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.center).css("display", showCenterSelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.disease).css("display", showDiseaseSelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.icd9).css("display", showICD9Selector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.period).css("display", showPeriodSelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.race).css("display", showRaceSelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.ethnicity).css("display", showEthnicitySelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.sex).css("display", showSexSelector);
    $(".ReportParameters .Selections table tr#" + config.reportcodes.zip).css("display", showZipSelector);

    // for each checkbox in a hidden table row, if it is checked, run the click handler to uncheck the box AND reset the hidden report value
    $(".ReportParameters .Selections table tr :hidden input[type=checkbox]").each(function () {
        if ($(this).attr("checked"))
            $(this).click();
    });
};

function addCriteriaGroup() {
    var index = parseInt($(".CriteriaGroup").last().attr("id").substring(2)) + 1;
    var id = "cg" + index;
    var clone = $("#cg0.CriteriaGroup").clone(true, true).attr("id", id).removeAttr("style");

    $(clone).find(".ui-section-title").text("Criteria Group " + index);
    $(clone).find(".CriteriaGroupName").attr("id", "CriteriaGroupName_" + index).attr("value", "Criteria Group " + index);
    $(clone).find("#Terms").attr("id", "Terms_" + index);
    $(clone).find("#ExcludeCriteriaGroup").attr("id", "ExcludeCriteriaGroup_" + index);
    $(clone).find("#SaveCriteriaGroup").attr("id", "SaveCriteriaGroup_" + index);
    $(clone).find(".DivCriteriaGroupAnd").show();

    var title = $(clone).find('img[data-original-title!=""]');
    for (var j = 0; j < title.length; j++) {
        $(title[j]).attr("title", $(title[j]).attr("data-original-title"));
        $(title[j]).removeAttr("data-original-title");
    }
    title.tooltip({
        html: true,
        placement: 'auto right',
        trigger: 'click',
        width: 350
    });

    title.click(function (event) {
        event.preventDefault();
        event.stopPropagation();
    });


    //The position of the dropdown does not seem to get set correctly when cloning, simplest solution is to reinitialize the kendo DatePicker.
    var ctrl = $(clone).find("[id^=StartPeriod_]");
    var startPicker = $(ctrl).data("kendoDatePicker");
    if (startPicker) {
        startPicker.destroy();
    }
    $(ctrl).kendoDatePicker({
        max: new Date(2299, 11, 31),
        min: new Date(1900, 0, 1)
    });

    ctrl = $(clone).find("[id^=EndPeriod_]");
    var endPicker = $(ctrl).data("kendoDatePicker");
    if (endPicker) {
        endPicker.destroy();
    }
    $(ctrl).kendoDatePicker({
        max: new Date(2299, 11, 31),
        min: new Date(1900, 0, 1)
    });

    $(clone).find('.criteriagroup-heading .ui-button-remove').click(removeCriteriaGroup);
    $(clone).find('.CriteriaGroupFooter .ObservationPeriodTerm .ui-button-remove').click(removeTerm);

    clone.appendTo('#CriteriaGroupsContainer');

    return false;
};

function removeCriteriaGroup(event) {
    // to support IE8, need to look at window.event instead
    var src = event.currentTarget ? event.currentTarget : window.event.srcElement;

    $(src).parents(".CriteriaGroup").remove();
    $("form").formChanged(true);
    return false;
};

function termMenuSelect(evt) {
    evt.preventDefault();

    var anchor = this;
    var term = $(anchor).attr('data-term');
    
    if (!term || term == 'Dummy') {
        return false;
    }
    
    var url = window.location.protocol + '//' + window.location.host + '/' + config.termSelectionUrl + '/' + term;
    var defaults = {
        url: url,
        type: 'GET',
        cache: false,
        success: function (data, status) {
            // Add html data for term widget under .InputParameters
            if (data != null) {
                $(".TermSubmenu").hide();

                //switch to determine which section to add the term to.
                var appended;

                if (url.endsWith('/term/ICD9CodeSelector') || url.endsWith('/term/DiseaseSelector')) {
                    // append the data to the InputParameters placeholder
                    appended = $(data).appendTo($(anchor).parents(".CriteriaGroup").find(".OrInputParameters"));
                    $(anchor).parents(".CriteriaGroup").find(".OrInputParameters").show();
                } else {
                    // append the data to the InputParameters placeholder
                    appended = $(data).appendTo($(anchor).parents(".CriteriaGroup").find(".AddInputParameters"));
                    $(anchor).parents(".CriteriaGroup").find(".AddInputParameters").show();
                }

                // hook the remove term button
                $(appended).find(".ui-button-remove").attr("onclick", "removeTerm(event)");

    //            //TODO: hook up to new tooltip library
    //            //$(appended).find('[title!=""]').qtip();

                // update the report parameters with this new term and dirty the form
                updatePrimarySelectors();
                $("form").formChanged(true);
            }
            return false;
        },
        error: function (jqXHR, status, error) {
            alert("Ajax request to \"" + url + "\" failed with a status of \"" + status + "\". The HTTP Error Message is \"" + error + "\". ");
            return false;
        }
    };

    $.ajax(defaults);
};

function removeTerm(event) {
    // to support IE8, need to look at window.event instead
    var src = event.currentTarget ? event.currentTarget : window.event.srcElement;

    $(src).parents(".Term").remove();

    // update the report parameters with this removed term and dirty the form
    updatePrimarySelectors();
    $("form").formChanged(true);

    return false;
};

function initCompose() {
    // now that the JS is loaded, we still need to make sure the whole doc is stable, so run this on doc ready!!
    $(document).ready(function () {

        $('li.TermMenuItem a').click(termMenuSelect);

        $('.reportSettingDropdown').kendoDropDownList();

        $('#btnAddGroup').click(addCriteriaGroup);

        $(".criteriagroup-heading .ui-button-remove").attr("onclick", "removeCriteriaGroup(event)");

        // Move the recreated terms into their respective criteria groups.  In some cases, this could be empty.
        $(".CriteriaGroupOuter .CriteriaTerms").each(function () {
            var self = $(this);

            $(self).find(".ui-button-remove").attr("onclick", "removeTerm(event)");            

            //Put it into the right group here.
            var ors = self.children(".ICD9CodeSelectorTerm, .DiseaseSelectorTerm, .VisitsTerm");

            if (ors.length > 0) {
                ors.appendTo(self.siblings(".CriteriaGroup").find(".OrInputParameters"));
                self.siblings(".CriteriaGroup").find(".OrInputParameters").show();
            }

            var ands = self.children(":not(.ICD9CodeSelectorTerm), :not(.DiseaseSelectorTerm), :not(.VisitsTerm)");
            if (ands.length > 0) {
                ands.appendTo(self.siblings(".CriteriaGroup").find(".AddInputParameters"));
                self.siblings(".CriteriaGroup").find(".AddInputParameters").show();
            }

            $('input[data-datepicker="datepicker"]').kendoDatePicker({
                max: new Date(2299, 11, 31),
                min: new Date(1900, 0, 1)
            });
        });

        var reportsViewModel = (function () {
            function reportsViewModel() {
                this.Report = ko.observableArray(reportsModel.Report);
                this.ReportSelections = reportsModel.ReportSelections;
                this.AgeStratification = ko.observable(reportsModel.AgeStratification == null ? 1 : reportsModel.AgeStratification);
                this.PeriodStratification = ko.observable(reportsModel.PeriodStratification == null ? 1 : reportsModel.PeriodStratification);
                this.ICD9Stratification = ko.observable(reportsModel.ICD9Stratification == null ? 3 : reportsModel.ICD9Stratification);

            }
            reportsViewModel.prototype.GetReportSettingProperty = function (itemName) {
                if (itemName == null)
                    return;

                if (itemName === 'AgeStratification') {
                    return this.AgeStratification;
                }
                if (itemName === 'PeriodStratification') {
                    return this.PeriodStratification;
                }
                if (itemName === 'ICD9Stratification') {
                    return this.ICD9Stratification;
                }

                return null;
            };
            return reportsViewModel;
        })();
        
        var reportsBindingControl = $('#reports-container');
        ko.applyBindings(new reportsViewModel(), reportsBindingControl[0]);

        // This HAS to happen after we plant the terms into each criteria group, so they can be counted
        updatePrimarySelectors();

        //------------- HACK ALERT --------------------
        // Since the timing of a page is currently unpredictable, it is not known whether the layout page
        // has been able to run "applySectionsAndHints()" to transform the button bindings into an actual button;
        // so, delay this action just a tiny bit!
        window.setTimeout(function () {
            // patch the primary criteria group...
            // kill the remove button in the header only (no terms!), this is a required group
            $('#cg1 .criteriagroup-heading .ui-button-remove').remove();
            $('#cg1 .CriteriaGroupFooter .ObservationPeriodTerm .ui-button-remove').remove();

            $("#cg1 .helptip").attr("title", "The Primary Criteria Group establishes the broadest cohort of interest (e.g. diabetes).  All subsequent Criteria Groups are selected or excluded from the Primary Criteria Group.  Stratification variables can only be applied to the Primary Criteria Group (primary cohort).<br /><br />To further define the cohort, press the [+] in the upper right of the Criteria Groups box.");
        }, 750);

        // replace the exclude checkbox with the moved report parameters table, made visible
        // currently, there is something causing the page to load multiple times... so, if this was
        // done on one line, we'd clear the (already moved) report params and have nothing to
        // push back in; saving it first, then emptying/replacing, prevents this side-effect!
        var repParam = $(".ReportParameters").css("display", "block").detach();
        $("#ExcludeCriteriaGroup_1").empty().append($(repParam));

        // --------------- submit handler  --------------
        // as part of the closure, define myDoc inside the closure (onReady), and outside of the
        // submitHandler function that is called by the framework file UI.cshtml so this form
        // has a reference that keeps it in context when the handler is invoked
        var myDoc = $(this);

        var form = $("#Content form").submit(function () {
            var groups = new Array();
            $(myDoc).find(".CriteriaGroup").each(function () {
                var self = $(this);
                if (self.is(":visible")) {
                    var terms = new Array();
                    self.find(".Term").each(function () {
                        var self = $(this);

                        var values = self.find("input,select,textarea:not(':hidden')").serializeArray();
                        terms.push(values);
                    });

                    // since these ui elements may not be there, we need to code defensively
                    var bExclude = false;
                    var exCG = self.find("input.ExcludeCriteriaGroup");
                    if (exCG.length > 0)
                        bExclude = $(exCG)[0].checked;
                    var bSave = true;
                    var svCG = self.find("input.SaveCriteriaGroup");
                    if (svCG.length > 0)
                        bSave = $(svCG)[0].checked;

                    var groupId = self.find('input.CriteriaGroupId').attr('value');
                    var group = {
                        CriteriaGroupId: groupId,
                        CriteriaGroupName: self.find('input.CriteriaGroupName').val(),
                        ExcludeCriteriaGroup: bExclude,
                        SaveCriteriaGroup: bSave,
                        Terms: terms
                    };

                    //console.log("criteriaGroup pushed on submit = " + JSON.stringify(group));
                    groups.push(group);
                }
            });

            // push the criteria groups (as JSON) into the hidden input field to send back
            //console.log("criteriaGroups pushed on submit == " + JSON.stringify(groups));
            var json = JSON.stringify(groups);
            $(myDoc).find("#CriteriaGroupsJSON").attr("value", json);

            return true;
        });
    });
}
