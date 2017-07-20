/// <reference path="../../Lpp.Pmn.Resources/Scripts/Page.ts" />


module Layout {
    //Set defaults for qtip
    $.fn.qtip.defaults.show.event = 'click';
    $.fn.qtip.defaults.hide.event = 'click';
    $.fn.qtip.defaults.show.solo = true;


    $(() => {
        //Set all of the items with title tags to have qtip enabled
        var title = <any> $('[title!=""]');

        title.qtip({
            show: {
                event: 'click'
            },
            hide: {
                event: 'click',
                fixed: true,
                distance: 30
            }
        });


    });
}