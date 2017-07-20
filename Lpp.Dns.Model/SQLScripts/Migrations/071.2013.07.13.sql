if exists( select * from sys.indexes where object_id = object_id('Subscriptions') and name = 'IX_Subscriptions_LastRunTime_NextDueTime' )
	EXECUTE('drop index Subscriptions.IX_Subscriptions_LastRunTime_NextDueTime')
GO

create index IX_Subscriptions_LastRunTime_NextDueTime on Subscriptions( NextDueTime, LastRunTime )
GO