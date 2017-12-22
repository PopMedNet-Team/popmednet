delete from AclEntries where
exists( 
	select * from AclEntries e where 
		e.TargetId = AclEntries.TargetId and
		e.SubjectId = AclEntries.SubjectId and
		e.PrivilegeId = AclEntries.PrivilegeId and
		e.[Order] = AclEntries.[Order] and
		e.Allow = AclEntries.Allow and
		e.Id < AclEntries.Id
)