var Controls;
(function (Controls) {
    var ExpandoMenu;
    (function (ExpandoMenu) {
        $(function () {
            var expandos = $("section.ExpandoFrame");
            $.each(expandos, function (count, item) {
                var header = $(item).children("h2").first();
                var buttons = header.prepend('<nav id="ExpandoButtons"></nav>');
            });
            //Get all of the ones that also have the Maximize css on it
            var minimizable = expandos.filter(".Minimizable");
            //Add the button manually in code and enable it's hook up for what it does.
            $.each(minimizable, function (count, item) {
                var header = $(item).children("h2").first();
                if (header.length == 0)
                    return;
                header.prepend('<img id="btnMinimize" src="' + ResourceUrl + '/images/minimize16.png" title="Minimize the current frame"/>');
            });
            minimizable.find("img#btnMinimize").click(function (e) {
                var expando = $(e.target).parents("section.ExpandoFrame").first();
                var state = expando.data("state") || "normal";
                switch (state) {
                    case "maximized":
                    case "normal":
                        expando.children("article").slideUp("slow");
                        expando.children("nav").slideUp("slow");
                        state = "minimized";
                        break;
                    default:
                        expando.children("article").slideDown("slow");
                        expando.children("nav").slideDown("slow");
                        state = "normal";
                        break;
                }
                expando.data("state", state);
            });
            var maximizable = expandos.filter(".Maximizable");
            $.each(maximizable, function (count, item) {
                var header = $(item).children("h2").first();
                if (header.length == 0)
                    return;
                var buttons = header.find("nav#ExpandoButtons").append('<img id="btnMaximize" src="' + ResourceUrl + '/images/maximize16.png" title="Maximize the current frame and minimize all others"/>');
            });
            maximizable.find("img#btnMaximize").click(function (e) {
                var expando = $(e.target).parents("section.ExpandoFrame").first();
                var expSiblings = expando.siblings("section.ExpandoFrame");
                var state = expando.data("state") || "normal";
                switch (state) {
                    case "maximized": //Return the others to normal state
                        expSiblings.children("article").slideDown("slow");
                        expSiblings.children("nav").slideDown("slow");
                        expSiblings.data("state", "normal");
                        state = "normal";
                        break;
                    default:
                        expSiblings.children("article").slideUp("slow");
                        expSiblings.children("nav").slideUp("slow");
                        expSiblings.data("state", "minimized");
                        expando.children("article").slideDown("slow");
                        expando.children("nav").slideDown("slow");
                        state = "maximized";
                        break;
                }
                expando.data("state", state);
            });
            //Get all of the ones that have help css on it and add the help button
            var help = expandos.filter(".Help");
            $.each(help, function (count, item) {
                var header = $(item).children("h2").first();
                if (header.length == 0)
                    return;
                header.find("nav#ExpandoButtons").prepend('<img id="btnHelp" src="' + ResourceUrl + '/images/help16.png" title="Help and Information about the current Area"/>');
            });
            help.find("img#btnHelp").click(function (e) {
                var expando = $(e.target).parents("section.ExpandoFrame").first();
                var helpText = expando.find("aside.Help").html();
                Global.Helpers.ShowAlert("Help", helpText);
            });
            //Handle other buttons that you want to add to the header here.
        });
    })(ExpandoMenu = Controls.ExpandoMenu || (Controls.ExpandoMenu = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=ExpandoMenu.js.map