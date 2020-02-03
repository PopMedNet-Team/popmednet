if not exists( select * from sys.columns where name = 'PropertiesXml' and object_id = object_id( 'DataMartInstalledModels' ) )
	alter table DataMartInstalledModels add PropertiesXml nvarchar(max)
GO
