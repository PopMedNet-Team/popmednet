define(['jQuery', './utilities', 'css!./tree.css'], function ($) {

    // this             -   tree's root element. 
    //                      Should contain several node elements, where each node element has:
    //                          * class .Node     -   marks the node element itself
    //                          * attr @id        -   ID of the node, used for loading children
    //                          * child .Expand   -   the expand/collapse icon
    //                          * child .Text     -   the name of the node, its icon, etc.
    //                                                Everything that's displayed to the right of the "expand" sign.
    //                          * child .Children -   gets shown/hidden on expand/collapse
    //                                                when a .Children element is not present, this means that the children
    //                                                haven't been loaded and will need to be loaded when the user clicks "expand"
    //
    //                      Upon expand, the node element will receive an "Expanded" class.
    //                      Upon collapse, the class will be removed.
    //
    // fnGetLoadChildrenUrl  -   function of the form NodeId -> Url
    //                           Should return an URL for loading the .Children element for the given node
    //
    // returns an object with methods allowing to control the tree
    $.fn.treeView = function jQuery$treeView(fnGetLoadChildrenUrl) {
        var $tree = this;
        var key = "{4D216FF6-A692-4E45-83F3-FC80906CBBA5}";
        var tree = $tree.data(key);
        if (tree) return tree;

        function expandCollapse() {
            var n = tree.node($(this).closest(".Node"));
            n.expanded(!n.expanded());
        }

        function loadChildren(id, fnDone) {
            var $placeholder = $('<div class="Children">');
            if (!fnGetLoadChildrenUrl || !id) return $placeholder;

            var $loading = $('<div class="TreeLoading LoadingSign">').html("&nbsp;").appendTo($placeholder);
            $.ajax({
                type: "GET",
                url: fnGetLoadChildrenUrl(id),
                success: function (result) {
                    $loading.replaceWith(result);
                    handleExpand($placeholder);
                    !fnDone || fnDone();
                    $(tree).trigger( "hiveloaded", tree.node($placeholder.closest(".Node")) );
                },
                error: function (res) {
                    try { res = $.parseJSON(res.responseText) || {}; } catch (_) { res = {}; }
                    $placeholder.floatErrorMessage("", res.message || "Error loading content", { foregroundClass: "TreeLoading ErrorMessage" });
                    $placeholder.remove();
                }
            });

            return $placeholder;
        }

        function handleExpand(root) { root.find(".Expand").click(expandCollapse); }
        handleExpand($tree);

        tree = {
            topLevelNodes: function () {
                return $.map($tree.children(".Node"), function (n) { return tree.node($(n)); });
            },

            node: function(jQueryObjectOrSelector) {
                var $node = (typeof jQueryObjectOrSelector == "string") ? $(jQueryObjectOrSelector, $tree) : jQueryObjectOrSelector;
                if (!$node.size()) return { exists: false };

                if ( $node.data("Expanded") == undefined ) $node.data("Expanded", $node.hasClass("Expanded") );

                return {
                    ui: $node,
                    id: function() { return $node.attr("id") || ""; },
                    exists: true,
                    ensureChildren: function (cbOnLoaded) {
                        if ($node.find(".Children").size() == 0) {
                            $node.append(loadChildren($node.attr("id"), cbOnLoaded));
                        } else {
                            cbOnLoaded();
                        }
                    },
                    children: function() {
                        return $.map( $node.children(".Children").children(".Node"), function(n) { return tree.node($(n)); } );
                    },
                    addChild: function (id, text) {
                        var newNode = $('<div class="Node"> <div class="Expand">&nbsp;</div> <div class="Text"/> <div class="Children"/> </div>');
                        newNode.attr("id", id).find(".Text").append(text);
                        $node.children(".Children").append(newNode);
                        handleExpand(newNode);
                        return tree.node(newNode);
                    },
                    reloadChildren: function (fnOnDone) {
                        $node.children(".Children").remove();
                        $node.append(loadChildren($node.attr("id"), fnOnDone));
                        $node.addClass("Expanded");
                    },
                    selected: function (bSelected) {
                        var selected = $node.hasClass("Selected");
                        if (bSelected == undefined) return selected;
                        if (selected == !!bSelected) return;

                        $(".Selected.Node", $tree).removeClass("Selected");
                        $node.addClass("Selected");
                    },
                    expanded: function (bExpanded) {
                        var expanded = $node.data("Expanded");
                        if (bExpanded == undefined) return expanded;
                        if (expanded == !!bExpanded) return;

                        var children = $node.find(".Children").first();
                        var id = $node.attr("id");
                        expanded = !!bExpanded;
                        $node.data("Expanded", expanded);
                        
                        if (expanded) {
                            if (children.size() == 0) {
                                $node.append(loadChildren(id));
                                $node.addClass("Expanded");
                            } else {
                                children.slideDown(100, function () { $node.addClass("Expanded"); });
                            }
                        } else {
                            children.slideUp(100, function () { $node.removeClass("Expanded"); });
                        }
                    }
                };
            }
        };

        $tree.data(key, tree);
        $tree.trigger("treeViewInitialized");
        return tree;
    };

});