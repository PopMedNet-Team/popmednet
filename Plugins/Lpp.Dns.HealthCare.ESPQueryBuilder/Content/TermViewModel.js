define(['ko'], function (ko) {
    return function () {
        this.removeTerm = function (data, event) {
            $(event.currentTarget).parents(".Term").remove();
  
            // update the report parameters with this removed term and dirty the form
            updatePrimarySelectors();
            $("form").formChanged(true);

            return false;
        }
    }
});