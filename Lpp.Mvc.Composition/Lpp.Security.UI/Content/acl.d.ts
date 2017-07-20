/// <reference path="../../Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />

/// <reference path="../../Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />

declare module Acl {
	export interface InheritedPrivilege { allow: boolean; inheritedFrom: string; }
	export interface InheritedPrivilegeSet {
		[privilegeId: string]: InheritedPrivilege;
	}
	export interface PrivilegeSet {
		// Here, 'privilegeId' is actually a combined ID consisting of a security target ID and a privilege ID joined through a colon.
		// And "security target ID" consists of IDs of all objects comprising that target, joined through 'x'
		[privilegeId: string]: boolean;
	}
	export interface SubjectAcl {
		own: PrivilegeSet;
		inherited: InheritedPrivilegeSet;
	}
	export interface AclData {
		[subjectId: string]: SubjectAcl;
	}

	export interface IPrivilegesEditor {
		setPrivileges( own: PrivilegeSet, inherited: InheritedPrivilegeSet ): void;
		getPrivileges(): PrivilegeSet;
		onChange( f: Function ): void;
	}

	export interface ISubjectSelector {
		selectSubject( onSelected: ( selectedSubjects: { Id: string; Name: string; }[] ) => void );
	}
}