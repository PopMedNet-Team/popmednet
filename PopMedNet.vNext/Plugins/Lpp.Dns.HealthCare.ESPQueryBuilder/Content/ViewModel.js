define(['ko', 'lpp.mvc.healthcare.espquerybuilder/CriteriaGroupViewModel', 'lpp.mvc.healthcare.espquerybuilder/TermViewModel', 'lpp.mvc.healthcare.espquerybuilder/TermMenuSelect'], function (ko, CriteriaGroupViewModel, TermMenuModel, TermMenuSelect) {
    return function (serviceUrl) {
        this.addCriteriaGroup = function () {
            var index = parseInt($(".CriteriaGroup").last().attr("id").substring(2)) + 1;
            var id = "cg" + index;
            var clone = $("#cg0.CriteriaGroup").clone(true).attr("id", id).attr("settingsKey", "CriteriaGroup." + index).removeAttr("style");
            $(clone).find(".ui-section-title").text("Criteria Group " + index);
            $(clone).find(".CriteriaGroupName").attr("id", "CriteriaGroupName_" + index).attr("value", "Criteria Group " + index);
            $(clone).find("#Terms").attr("id", "Terms_" + index);
            $(clone).find("#ExcludeCriteriaGroup").attr("id", "ExcludeCriteriaGroup_" + index);
            $(clone).find("#SaveCriteriaGroup").attr("id", "SaveCriteriaGroup_" + index);
            clone.appendTo('#CriteriaGroups > .ui-section-outside > .ui-section');

            var menu = $("#Terms_" + index).menu(
                {
                    select: TermMenuSelect(serviceUrl)
                });

            $(menu).mouseover(function () {
                menu.menu("expand");
            });

            $(menu).mouseleave(function () {
                $(".TermSubmenu").hide(300);
            });

            ko.applyBindings(CriteriaGroupViewModel(false), $("div#CriteriaGroups #" + id)[0]);

            return false;
        }

        this.removeCriteriaGroup = function (data, event) {
            $(event.currentTarget).parents(".CriteriaGroup").remove();

            return false;
        }
    }
});