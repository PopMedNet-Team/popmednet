/// <reference path="../../lpp.mvc.composition/lpp.mvc.controls.interfaces/utilities.d.ts" />
$(function utilities() {
    $.fn.alternateClasses = function jQuery$alternateClasses(arrayClasses) {
        if ((arrayClasses instanceof Array) == false)
            arrayClasses = arguments;
        arrayClasses = $.makeArray(arrayClasses);
        this.removeClass(arrayClasses.join(" "));
        var arrayCopy = arrayClasses.slice(0);
        this.each(function () {
            $(this).addClass(arrayCopy.shift());
            if (!arrayCopy.length)
                arrayCopy = arrayClasses.slice(0);
        });
        return this;
    };
    function targetCenterTopPosition(target) {
        var bodyTop = $("body").scrollTop();
        var bodyHeight = $(window).height();
        var targetTop = target.position().top;
        var targetHeight = target.height();
        var ofs = (bodyTop > targetTop ? bodyTop - targetTop : 0) +
            Math.min(bodyHeight, target.height()) / 4;
        return { my: "center top", at: "center top", of: target, offset: "0 " + ofs };
    }
    $.fn.dataDisplay = function jQuery$dataDisplay(strDisplay) {
        return strDisplay === undefined ?
            this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}") :
            this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}", strDisplay);
    };
    function gate(condition, func) {
        var lastArgs = null;
        var lastThis = null;
        var result = function () {
            if (condition())
                return func.apply(this, arguments);
            lastArgs = arguments;
            lastThis = this;
        };
        result.release = function () {
            if (lastArgs && condition())
                func.apply(lastThis, lastArgs);
        };
        return result;
    }
    function releaseGate(gate) {
        if (gate.release)
            gate.release();
    }
});
