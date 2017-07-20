define(['jQuery'], function($) {
    var AlreadyDone = "{F9BBAC57-F5C3-47EA-99E4-4C45FFD8CD2E}";

    function fireRequest(url, onDone, waitPrompt, authFailMessage) {
        var prompt = $("<div>" + waitPrompt + "...</div>").dialog({ title: "Please wait", width: 600, modal: true });
        var err = function (msg) {
            prompt
                .text(msg).addClass("ErrorMessage")
                .dialog("option", "buttons", { Close: function () { prompt.dialog("destroy"); } })
                .dialog("option", "title", "Error");
        };

        $.ajax({
            type: "POST",
            url: url,
            success: function (res) {
                if (res == "OK") { prompt.dialog("destroy"); onDone(); }
                else if (res == "Auth") err(authFailMessage);
                else err(res);
            },
            error: function (e) { err(e); }
        });
    }

    var me = {
        setupSharedFolders: function(folder) {
            var $this = $(folder);
            var folderId = $this.data("folderid");
            if (!folderId || $this.data(AlreadyDone)) return;
            $this.data(AlreadyDone, true);

            $this.droppable({
                accept: ".RequestsGrid td.Name > a",
                activeClass: "DropActive",
                hoverClass: "DropHover",
                tolerance: "pointer",
                scope: "Request",
                drop: function (_, ui) {
                    var requestId = ui.draggable.closest("tr").attr("id");
                    if (!requestId || !confirm("This request will be shared with all users who have access to this shared folder.\r\nContinue?"))
                        return;

                    fireRequest(me.fnGetShareUrl(folderId, requestId), function () { }, "Sharing the request", "You do not have permission to share this request in this folder.");
                }
            });
        },

        initializeSharedFolders: function (fnGetShareUrl /* function( folderId, requestId ) */) {
            me.fnGetShareUrl = fnGetShareUrl;
            $(".SharedFolder").each(function () { me.setupSharedFolders(this); } );
        },

        unshareRequest: function (unshareUrl, onDone) {
            fireRequest(unshareUrl, onDone, "Removing the request", "You do not have permission to remove this request from this folder.");
        }
    };

    return me;
});