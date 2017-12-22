define("requireCss", ["jQuery"], function ($) {
    return function (file) {
        if ( !file || !file.length ) return;

        file = file.toLowerCase();
        if ( $( "head > link" ).filter( function() { return $(this).attr( "href" ) == file; } ).size() ) return;
        $("head").append('<link rel="stylesheet" type="text/css" href="' + file + '" />');
    };
});