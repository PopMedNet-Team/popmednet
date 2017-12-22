/// <reference path="../../Lpp.Mvc.Controls.Interfaces/jquery.d.ts" />
/// <reference path="../../Lpp.Mvc.Controls.Interfaces/jqueryui.d.ts" />
/// <reference path="../../Lpp.Mvc.Controls.Interfaces/require.d.ts" />
/// <reference path="../../Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />
/// <reference path="acl.d.ts" />

module Acl {
	define( ['jQuery', 'lpp.mvc.controls/utilities'], function ( $: JQueryStatic ) {

		return function ( acl: AclData, viewId: string, subjectSelector: ISubjectSelector,
			noRecordsMessage: string, privilegeEditors: IPrivilegesEditor[] ) {
			$( function () {
				viewId = "#" + viewId;
				var root = $( viewId );
				var subjectsGrid = $( ".Grid.Subjects", root );
				var privsBox = $( ".PrivilegesBox", root );
				function subjectsRows(): JQuery { return $( ".Grid.Subjects > tbody > tr", root ); }

				function updateHiddenField() {
					var newValue =
						$.map( acl, ( pp: SubjectAcl, subjectId: string ) =>
							$.map( pp.own, ( allow: bool, privilegeId: string ) =>
								( allow === null || allow === undefined ) ? undefined :
								( subjectId + ':' + privilegeId + ':' + ( allow ? "allow" : "deny" ) )
								)
							).join();
					$( "input.ValueField", root ).val( newValue ).trigger( "change" );
				}

				$.each( privilegeEditors, function () {
					var e: IPrivilegesEditor = this;
					e.onChange( () => {
						var subj = currentSubject();
						if( !subj ) return;

						$.extend( subj.own, e.getPrivileges() );

						$.each( privilegeEditors, function () {
							var e1: IPrivilegesEditor = this;
							if( e != e1 ) e1.setPrivileges( subj.own, subj.inherited );
						});
						updateHiddenField();
					});
				});

				var _currentSubject;
				function currentSubject( subj?: SubjectAcl ) {
					if( subj === undefined ) return _currentSubject;

					_currentSubject = subj;
					if( subj ) {
						privsBox.show();
						$.each( privilegeEditors, function () {
							var e: IPrivilegesEditor = this;
							e.setPrivileges( subj.own, subj.inherited );
						});
					} else {
						privsBox.hide();
					}
				}

				function subjIdFromRow( row: JQuery ) {
					return row.find( "a.Remove" ).data( "id" );
				}

				root.delegate( ".Subject", "click", function () {
					var row = $( this ).closest( "tr" );
					var subjId = subjIdFromRow( row );
					if( !subjId ) return;

					currentSubject( acl[subjId] );
					subjectsRows().removeClass( "Selected" );
					row.addClass( "Selected" );
				});

				$( ".Subjects .Add", root ).click( function () {
					subjectSelector.selectSubject( function ( subjs ) {
						var rows = subjectsRows();
						rows.filter( function () { return !subjIdFromRow( $( this ) ); }).remove();

						var lastAdded;
						$.each( subjs, function () {
							var existing = rows.filter( "[data-id=" + this.Id + "]" );
							if( existing.length ) {
								lastAdded = existing.first();
								return;
							}
							acl[this.Id] = { own: {}, inherited: {} };
							subjectsGrid.children( "tbody" ).append(
								lastAdded =
								$( "<tr>" )
									.append( '<td><a class="Remove" href="#" data-id="' + this.Id + '">[remove]</a></td>' )
									.append( $( '<td class="Subject">' ).html( this.Name ).attr( "sort-value", this.Name ) )
								);
						});

						subjectsRows().alternateClasses( "", "Alt" );
						if( lastAdded ) lastAdded.find( ".Subject" ).click();
					});
					return false;
				});

				root.delegate( ".Subjects .Remove", "click", function () {
					var row = $( this ).closest( "tr" );
					var subjId = subjIdFromRow( row );
					row.remove();

					delete acl[subjId];
					updateHiddenField();

					var rows = subjectsRows();
					if( rows.length == 0 ) {
						subjectsGrid.children( "tbody" ).append( $( "<tr><td colspan=2>" ).html( noRecordsMessage || "No access control records found" ) );
					} else {
						rows.alternateClasses( "", "Alt" );
					}

					var firstSubject = $( "tbody > tr > td.Subject", root ).first();
					if( firstSubject.length ) firstSubject.click();
					else currentSubject( null );
					return false;
				});

				privsBox.hide();
				$( "tbody > tr > td.Subject", root ).first().click();
			});
		};

	});
}