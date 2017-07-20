if  exists( select * from sys.tables where object_id = object_id( 'FileDistributionDocuments' ) )
begin
	alter table FileDistributionDocuments alter column Name varchar(500) not null
	alter table FileDistributionDocuments alter column MimeType varchar(500) not null
end