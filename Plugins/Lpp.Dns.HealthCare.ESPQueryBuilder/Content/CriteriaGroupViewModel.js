define(['ko'], function (ko) {
    return function (bindRemove) {
        if (bindRemove)
        {
            this.removeCriteriaGroup = function (data, event) {
                $(event.currentTarget).parents(".CriteriaGroup").remove();
                return false;
            }
        }
    }
});