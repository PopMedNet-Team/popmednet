/// <reference path="../../Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />
/// <reference path="../../../Lpp.Pmn.Resources/Scripts/typings/rx/rx.d.ts" />
/// <reference path="acl.d.ts" />

module Acl {
    interface PrivilegeRow { tr: JQuery; allow: JQuery; deny: JQuery; inherited: JQuery; }

    var res: ($: JQueryStatic) => (viewId: string) => IPrivilegesEditor = 
    $ =>
    {
        return viewId =>
        {
            viewId = "#" + viewId;
            var inheritedFromKey = "{D2C0C70A-59EF-429C-9EC2-097A06BD0B2F}";
            var _view: JQuery;
            var _map: { [key: string]: PrivilegeRow; } = {};

            function bindOne(privilegeId: string, allow: boolean, enableCheckboxes: boolean,
                extra: (row: PrivilegeRow) => void ) {
                var m = _map[privilegeId];
                if ( !m ) return;
                m.allow.prop("checked", allow == true).prop("disabled", !enableCheckboxes);
                m.deny.prop("checked", allow == false).prop("disabled", !enableCheckboxes);
                extra(m);
            }

            let gate: Function = (act: Function, bind: Function) => {
                //don't know if this is what the original did, cannot find the original function in any referenced library.
                return bind;
            };

            let releaseGate: Function = (bind: Function) => {
                //don't know what the original did, cannot find the original function in any referenced library.
            };

            var bind = function (own: PrivilegeSet, inherited: InheritedPrivilegeSet) {
                _view.find("input[type=checkbox]").prop("checked", false).prop("disabled", false);
                _view.find("a.Inherited").hide();
                $.each(inherited || {}, (privilegeId: string, p: InheritedPrivilege) =>
                    bindOne(privilegeId, p.allow, false, row =>
                        row.inherited.show().data( inheritedFromKey, p.inheritedFrom )
                    )
                );
                $.each(own || {}, (privilegeId: string, allow: boolean) =>
                    bindOne(privilegeId, allow, true, row => row.inherited.hide() )
                );
            };
            bind = gate(() => !!_view, bind);

            $(() => {
                _view = $(viewId);
                _view.find("span[data-id]").each(function () {
                    var $this = $(this);
                    var tr = $this.closest("tr");
                    _map[$this.data("id")] = { tr: tr, allow: tr.find("input.Allow"), deny: tr.find( "input.Deny" ), inherited: tr.find("a.Inherited") };
                });

                _view.find("a.Inherited").click(function () {
                    var inheritanceResetBox = $(".Acl-InheritedResetPopup:first");
                    var inheritanceResetLink = $(".Override", inheritanceResetBox);
                    if (inheritanceResetBox.is(":visible")) return;

                    var inhFrom = $(this).data(inheritedFromKey);
                    inheritanceResetBox.find(".InheritedFromName").text(inhFrom);
                    inheritanceResetBox.find(".InheritedFromBox").toggle(!!inhFrom);

                    var oldParent = inheritanceResetBox.parent();
                    inheritanceResetBox
                        .appendTo("body")
                        .css({ left: "", top: "", position: "absolute" })
                        .position({ my: "left top", at: "left bottom", of: this })
                        .fadeIn(100);

                    var row = $(this).parents("tr").eq(0);
                    var close = function (e) {
                        if (e.target == inheritanceResetLink[0]) {
                            row.find("input[type=checkbox]").prop("disabled", false).prop("checked", false);
                            row.find("a.Inherited").empty();
                        }
                        inheritanceResetBox.fadeOut(100, () => inheritanceResetBox.appendTo( oldParent ));
                        $(document).unbind("click", close);
                        return false;
                    }
                    setTimeout(function () { $(document).click(close); }, 1);

                    return false;
                });

                _view.find("input.Allow, input.Deny").bind("click change", 
                    function() {
                        if ($(this).prop("checked")) {
                            $(this).closest("tr").find("input.Allow, input.Deny").prop("checked", false);
                            $(this).prop("checked", true);
                        }
                        $(editor).trigger("change");
                    } );

                releaseGate(bind);
            });

            var editor: IPrivilegesEditor = {
                setPrivileges: bind,

                getPrivileges: () => {
                    var res = {};
                    $.each( _map, (id: string, row: PrivilegeRow) =>
                    {
                        if (row.inherited.is(":visible")) return;
                        res[id] = 
                            row.allow.prop("checked") ? true :
                            row.deny.prop("checked") ? false :
                            null;
                    });

                    return res;
                },

                onChange: (f: any) => $(editor).bind("change", f)
            };

            return editor;
        };
    };

    define(['jQuery', 'lpp.mvc.controls/utilities'], res);
}