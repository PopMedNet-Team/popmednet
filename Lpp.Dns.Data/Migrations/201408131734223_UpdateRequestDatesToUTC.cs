namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestDatesToUTC : DbMigration
    {
        public override void Up()
        {
            /** TimeZone information table **/
            Sql(@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TimeZoneInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TimeZoneInfo]");
            Sql(@"CREATE TABLE [dbo].[TimeZoneInfo] (
	[TimeZoneID] [int] IDENTITY (1, 1) NOT NULL ,
	[Display] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Bias] [smallint] NOT NULL ,
	[StdBias] [smallint] NOT NULL ,
	[DltBias] [smallint] NOT NULL ,
	[StdMonth] [smallint] NOT NULL ,
	[StdDayOfWeek] [smallint] NOT NULL ,
	[StdWeek] [smallint] NOT NULL ,
	[StdHour] [smallint] NOT NULL ,
	[DltMonth] [smallint] NOT NULL ,
	[DltDayOfWeek] [smallint] NOT NULL ,
	[DltWeek] [smallint] NOT NULL ,
	[DltHour] [smallint] NOT NULL 
) ON [PRIMARY]");
            Sql(@"ALTER TABLE [dbo].[TimeZoneInfo] WITH NOCHECK ADD 
	CONSTRAINT [PK_TimeZoneInfo] PRIMARY KEY  CLUSTERED 
	(
		[TimeZoneID]
	)  ON [PRIMARY] ");
            Sql(@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDaylightStandardDateTime]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetDaylightStandardDateTime]");
            Sql(@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLocalDateTime]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetLocalDateTime]");
            Sql(@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUtcDateTime]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetUtcDateTime]");
            Sql(@"CREATE  FUNCTION GetDaylightStandardDateTime
(
    @Year int,        -- a valid year value
    @Month int,        -- 1..12
    @DayOfWeek smallint,    -- 1..7
    @Week smallint,      -- 1..5, 1 - first week, 2 - second, etc.,  5 - the last week
    @Hour smallint -- hour value when daylight or standard time begins.
)
RETURNS datetime
AS
BEGIN
    DECLARE @FirstOfMonth datetime
    DECLARE @DoW smallint
    DECLARE @Ret datetime

    -- find day of the week of the first day of a given month:
    SET @FirstOfMonth = CAST( @Year AS NVARCHAR ) + '/' +  CAST( @Month AS NVARCHAR ) + '/01'

    -- 5th week means the last week of the month, so go one month forth, then one week back
    IF @Week = 5
    BEGIN
        SET @FirstOfMonth = DATEADD( Month, 1, @FirstOfMonth )
    END

    SET @DoW = DATEPART( weekday, @FirstOfMonth )

    -- find first given day of the week of the given month:
    IF @DoW > @DayOfWeek
        SET @Ret = DATEADD( Day, 7 + @DayOfWeek - @DoW , @FirstOfMonth )
    ELSE
        SET @Ret = DATEADD( Day, @DayOfWeek - @DoW , @FirstOfMonth )

    -- advance to the given week ( 5th week means the last one of the month )
    IF @Week < 5
    BEGIN
        SET @Ret = DATEADD( Week, @Week - 1, @Ret )
    END
    ELSE
    BEGIN
        -- the last week of the previous month; go one week backward
        SET @Ret = DATEADD( Week, -1, @Ret )
    END


   SET @Ret = DATEADD( Hour, @Hour, @Ret )

    RETURN @Ret
END");
            Sql(@"CREATE FUNCTION dbo.GetLocalDateTime
(
@UTCDate DATETIME,
@TimeZoneID SMALLINT
)

--RETURNS NVARCHAR(500)
RETURNS DATETIME
AS
BEGIN

--DECLARE @TimeZoneID SMALLINT
DECLARE @LocalDateTime DATETIME
DECLARE @DltBiasFactor SMALLINT

DECLARE @Display NVARCHAR(50)
DECLARE @Bias INT
DECLARE @DltBias INT
DECLARE @StdMonth SMALLINT
DECLARE @StdDow SMALLINT
DECLARE @StdWeek SMALLINT
DECLARE @StdHour SMALLINT
DECLARE @DltMonth SMALLINT
DECLARE @DltDow SMALLINT
DECLARE @DltWeek SMALLINT
DECLARE @DltHour SMALLINT

DECLARE @DaylightDate DATETIME
DECLARE @StandardDate DATETIME

SET @DltBiasFactor = 0

SELECT 
@Display = Display,
@Bias = (-1 * Bias), 
@DltBias = (-1 * DltBias) ,
@StdMonth  = StdMonth,
@StdDow = StdDayOfWeek + 1,
@StdWeek = StdWeek,
@StdHour = StdHour,
@DltMonth = DltMonth,
@DltDow = DltDayOfWeek + 1,
@DltWeek = DltWeek,
@DltHour = DltHour
FROM 
TimeZoneInfo 
WHERE 
TimeZoneID = @TimeZoneID


IF @StdMonth = 0
BEGIN
	SET @LocalDateTime = DateAdd( minute, @Bias , @UTCDate)
END
ELSE
BEGIN
	SET @StandardDate =  dbo.GetDaylightStandardDateTime( DATEPART( year, @UTCDate ), @StdMonth, @StdDow, @StdWeek, @StdHour )
	SET @DaylightDate = dbo. GetDaylightStandardDateTime( DATEPART( year, @UTCDate ), @DltMonth, @DltDow, @DltWeek, @DltHour )

	
	IF (  @StandardDate > @DaylightDate )
	BEGIN
		IF ( DATEADD( minute, @Bias, @UTCDate )  BETWEEN @DaylightDate AND @StandardDate   )
		BEGIN
			SET @DltBiasFactor = 1
		END
	END
	ELSE
	BEGIN
		IF ( DATEADD( minute, @Bias, @UTCDate )  BETWEEN @StandardDate AND @DaylightDate )
		BEGIN
			SET @DltBiasFactor = 0
		END
	END

	SET @LocalDateTime = DATEADD( minute, @Bias + ( @DltBiasFactor * @DltBias ) , @UTCDate )

END

	--RETURN  'Time Zone ID:' + CAST( @TimeZoneID  AS CHAR(2) ) + ' - '  + @Display + ' - <UTC DT:' + CAST ( @UTCDate AS CHAR(20) ) + '> - <Local DT:' + CAST(  @LocalDateTime AS CHAR(20) ) + '>'
	RETURN @LocalDateTime

END ");
            Sql(@"CREATE FUNCTION dbo.GetUtcDateTime
(
@LocalDate DATETIME,
@TimeZoneID SMALLINT
)

--RETURNS NVARCHAR(500)
RETURNS DATETIME
AS
BEGIN

--DECLARE @TimeZoneID SMALLINT
DECLARE @UtcTime DATETIME
DECLARE @DltBiasFactor SMALLINT

DECLARE @Display NVARCHAR(50)
DECLARE @Bias INT
DECLARE @DltBias INT
DECLARE @StdMonth SMALLINT
DECLARE @StdDow SMALLINT
DECLARE @StdWeek SMALLINT
DECLARE @StdHour SMALLINT
DECLARE @DltMonth SMALLINT
DECLARE @DltDow SMALLINT
DECLARE @DltWeek SMALLINT
DECLARE @DltHour SMALLINT

DECLARE @DaylightDate DATETIME
DECLARE @StandardDate DATETIME

SET @DltBiasFactor = 0

SELECT 
@Display = Display,
@Bias = (-1 * Bias), 
@DltBias = (-1 * DltBias) ,
@StdMonth  = StdMonth,
@StdDow = StdDayOfWeek + 1,
@StdWeek = StdWeek,
@StdHour = StdHour,
@DltMonth = DltMonth,
@DltDow = DltDayOfWeek + 1,
@DltWeek = DltWeek,
@DltHour = DltHour
FROM 
TimeZoneInfo 
WHERE 
TimeZoneID = @TimeZoneID


IF @StdMonth = 0
BEGIN
	SET @UtcTime = DateAdd( minute, -@Bias , @LocalDate)
END
ELSE
BEGIN
	SET @StandardDate =  dbo.GetDaylightStandardDateTime( DATEPART( year, @LocalDate ), @StdMonth, @StdDow, @StdWeek, @StdHour )
	SET @DaylightDate = dbo. GetDaylightStandardDateTime( DATEPART( year, @LocalDate ), @DltMonth, @DltDow, @DltWeek, @DltHour )

	
	IF (  @StandardDate > @DaylightDate )
	BEGIN
		IF ( DATEADD( minute, @Bias, @LocalDate )  BETWEEN @DaylightDate AND @StandardDate   )
		BEGIN
			SET @DltBiasFactor = -1
		END
	END
	ELSE
	BEGIN
		IF ( DATEADD( minute, @Bias, @LocalDate )  BETWEEN @StandardDate AND @DaylightDate )
		BEGIN
			SET @DltBiasFactor = 0
		END
	END

	SET @UtcTime = DATEADD( minute, -@Bias + ( @DltBiasFactor * @DltBias ) , @LocalDate )

END

	--RETURN  'Time Zone ID:' + CAST( @TimeZoneID  AS CHAR(2) ) + ' - '  + @Display + ' - <UTC DT:' + CAST ( @UtcTime AS CHAR(20) ) + '> - <Local DT:' + CAST( @LocalDate  AS CHAR(20) ) + '>'
	RETURN @UtcTime

END");
            Sql(@"SET NOCOUNT ON

ALTER TABLE [TimeZoneInfo] NOCHECK CONSTRAINT ALL
SET IDENTITY_INSERT [TimeZoneInfo] ON
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (1,'(GMT-12:00) International Date Line West',720,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (2,'(GMT-11:00) Midway Island, Samoa',660,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (3,'(GMT-10:00) Hawaii',600,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (4,'(GMT-09:00) Alaska',540,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (5,'(GMT-08:00) Pacific Time (US & Canada); Tijuana',480,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (6,'(GMT-07:00) Chihuahua, La Paz, Mazatlan',420,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (7,'(GMT-07:00) Mountain Time (US & Canada)',420,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (8,'(GMT-07:00) Arizona',420,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (9,'(GMT-06:00) Guadalajara, Mexico City, Monterrey',360,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (10,'(GMT-06:00) Saskatchewan',360,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (11,'(GMT-06:00) Central America',360,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (12,'(GMT-06:00) Central Time (US & Canada)',360,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (13,'(GMT-05:00) Eastern Time (US & Canada)',300,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (14,'(GMT-05:00) Bogota, Lima, Quito',300,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (15,'(GMT-05:00) Indiana (East)',300,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (16,'(GMT-04:00) Caracas, La Paz',240,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (17,'(GMT-04:00) Santiago',240,0,-60,3,6,2,0,10,6,2,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (18,'(GMT-04:00) Atlantic Time (Canada)',240,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (19,'(GMT-03:30) Newfoundland',210,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (20,'(GMT-03:00) Buenos Aires, Georgetown',180,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (21,'(GMT-03:00) Brasilia',180,0,-60,2,0,2,2,10,0,3,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (22,'(GMT-03:00) Greenland',180,0,-60,10,0,5,2,4,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (23,'(GMT-02:00) Mid-Atlantic',120,0,-60,9,0,5,2,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (24,'(GMT-01:00) Azores',60,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (25,'(GMT-01:00) Cape Verde Is.',60,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (26,'(GMT) Casablanca, Monrovia',0,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (27,'(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London',0,0,-60,10,0,5,2,3,0,5,1)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (28,'(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague',-60,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (29,'(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb',-60,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (30,'(GMT+01:00) Brussels, Copenhagen, Madrid, Paris',-60,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (31,'(GMT+01:00) West Central Africa',-60,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (32,'(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna',-60,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (33,'(GMT+02:00) Harare, Pretoria',-120,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (34,'(GMT+02:00) Jerusalem',-120,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (35,'(GMT+02:00) Athens, Beirut, Istanbul, Minsk',-120,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (36,'(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius',-120,0,-60,10,0,5,4,3,0,5,3)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (37,'(GMT+02:00) Bucharest',-120,0,-60,10,0,5,1,3,0,5,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (38,'(GMT+02:00) Cairo',-120,0,-60,9,3,5,2,5,5,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (39,'(GMT+03:00) Nairobi',-180,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (40,'(GMT+03:00) Kuwait, Riyadh',-180,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (41,'(GMT+03:00) Baghdad',-180,0,-60,10,0,1,4,4,0,1,3)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (42,'(GMT+03:00) Moscow, St. Petersburg, Volgograd',-180,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (43,'(GMT+03:30) Tehran',-210,0,-60,9,2,4,2,3,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (44,'(GMT+04:00) Abu Dhabi, Muscat',-240,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (45,'(GMT+04:00) Baku, Tbilisi, Yerevan',-240,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (46,'(GMT+04:30) Kabul',-270,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (47,'(GMT+05:00) Ekaterinburg',-300,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (48,'(GMT+05:00) Islamabad, Karachi, Tashkent',-300,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (49,'(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi',-330,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (50,'(GMT+05:45) Kathmandu',-345,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (51,'(GMT+06:00) Almaty, Novosibirsk',-360,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (52,'(GMT+06:00) Sri Jayawardenepura',-360,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (53,'(GMT+06:00) Astana, Dhaka',-360,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (54,'(GMT+06:30) Rangoon',-390,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (55,'(GMT+07:00) Krasnoyarsk',-420,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (56,'(GMT+07:00) Bangkok, Hanoi, Jakarta',-420,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (57,'(GMT+08:00) Kuala Lumpur, Singapore',-480,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (58,'(GMT+08:00) Taipei',-480,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (59,'(GMT+08:00) Irkutsk, Ulaan Bataar',-480,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (60,'(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi',-480,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (61,'(GMT+08:00) Perth',-480,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (62,'(GMT+09:00) Yakutsk',-540,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (63,'(GMT+09:00) Seoul',-540,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (64,'(GMT+09:00) Osaka, Sapporo, Tokyo',-540,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (65,'(GMT+09:30) Darwin',-570,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (66,'(GMT+09:30) Adelaide',-570,0,-60,3,0,5,3,10,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (67,'(GMT+10:00) Canberra, Melbourne, Sydney',-600,0,-60,3,0,5,3,10,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (68,'(GMT+10:00) Brisbane',-600,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (69,'(GMT+10:00) Guam, Port Moresby',-600,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (70,'(GMT+10:00) Hobart',-600,0,-60,3,0,5,3,10,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (71,'(GMT+10:00) Vladivostok',-600,0,-60,10,0,5,3,3,0,5,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (72,'(GMT+11:00) Magadan, Solomon Is., New Caledonia',-660,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (73,'(GMT+12:00) Fiji, Kamchatka, Marshall Is.',-720,0,-60,0,0,0,0,0,0,0,0)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (74,'(GMT+12:00) Auckland, Wellington',-720,0,-60,3,0,3,2,10,0,1,2)
INSERT INTO [TimeZoneInfo] ([TimeZoneID],[Display],[Bias],[StdBias],[DltBias],[StdMonth],[StdDayOfWeek],[StdWeek],[StdHour],[DltMonth],[DltDayOfWeek],[DltWeek],[DltHour]) VALUES (75,'(GMT+13:00) Nuku''alofa',-780,0,-60,0,0,0,0,0,0,0,0)
SET IDENTITY_INSERT [TimeZoneInfo] OFF
ALTER TABLE [TimeZoneInfo] CHECK CONSTRAINT ALL");

            /*** Update the Requests Table ***/
            Sql(@"UPDATE Requests SET CreatedOn = ISNULL(SubmittedOn, GETDATE()) WHERE CreatedOn IS NULL");
            Sql(@"DECLARE @dfName nvarchar(100)
                -- default for CreatedOn column
                SELECT @dfName = df.[name] FROM sys.columns c JOIN sys.default_constraints df ON df.parent_object_id = c.object_id AND df.parent_column_id = c.column_id WHERE c.object_id = OBJECT_ID('Requests') AND c.[name] = 'CreatedOn'
                IF @dfName IS NOT NULL
                BEGIN
	                EXEC ('ALTER TABLE Requests DROP CONSTRAINT ' + @dfName)
                END
                EXEC ('ALTER TABLE [dbo].[Requests] ADD  CONSTRAINT ' + @dfName + '  DEFAULT (getutcdate()) FOR [CreatedOn]')

                -- default for UpdatedOn column
                SELECT @dfName = df.[name] FROM sys.columns c JOIN sys.default_constraints df ON df.parent_object_id = c.object_id AND df.parent_column_id = c.column_id WHERE c.object_id = OBJECT_ID('Requests') AND c.[name] = 'UpdatedOn'
                IF @dfName IS NOT NULL
                BEGIN
	                EXEC ('ALTER TABLE Requests DROP CONSTRAINT ' + @dfName)
                END
                EXEC ('ALTER TABLE [dbo].[Requests] ADD  CONSTRAINT ' + @dfName + '  DEFAULT (getutcdate()) FOR [UpdatedOn]')");
            Sql(@"UPDATE Requests SET CreatedOn = dbo.GetUtcDateTime(CreatedOn, 13), DueDate = dbo.GetUtcDateTime(DueDate, 13), SubmittedOn = dbo.GetUtcDateTime(SubmittedOn, 13), UpdatedOn = dbo.GetUtcDateTime(UpdatedOn, 13), ApprovedForDraftOn = dbo.GetUtcDateTime(ApprovedForDraftOn, 13), RejectedOn = dbo.GetUtcDateTime(RejectedOn, 13), CancelledOn = dbo.GetUtcDateTime(CancelledOn, 13)");

            /*** Update the RequestDataMarts Table ***/
            Sql(@"UPDATE RequestDataMarts SET RequestTime = dbo.GetUtcDateTime(RequestTime, 13), ResponseTime = dbo.GetUtcDateTime(ResponseTime, 13)");

            /*** Update the RequestDataMartResponses Table ***/
            //because the columns are datetime2 some massaging is required to be able to convert to datetime to use in the utc function
            Sql(@"UPDATE RequestDataMartResponses SET ResponseTime = NULL WHERE ResponseTime = '0001-01-01 00:00:00.0000000'");
            Sql(@"UPDATE RequestDataMartResponses SET SubmittedOn = '2/2/1753' WHERE SubmittedOn = '0001-01-01 00:00:00.0000000'");
            Sql(@"UPDATE RequestDataMartResponses SET ResponseTime = dbo.GetUtcDateTime(CAST(ResponseTime as datetime), 13), SubmittedOn = dbo.GetUtcDateTime(CAST(SubmittedOn as datetime), 13)");
            Sql(@"UPDATE RequestDataMartResponses SET SubmittedOn = '0001-01-01 00:00:00.0000000' WHERE SubmittedOn = '2/2/1753'");

            /*** RequestDataMarts InsertUpdateDelete trigger ***/
            Sql(@"ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                    ON  [dbo].[RequestDataMarts]
                    AFTER INSERT, UPDATE, DELETE
                AS 
                BEGIN
	                IF ((SELECT COUNT(*) FROM inserted) > 0)
					BEGIN
		                UPDATE Requests SET UpdatedOn = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
						UPDATE Requests SET Status = 
							CASE WHEN NOT CancelledOn IS NULL THEN 9999 
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed = Total AND RequestID = Requests.ID) THEN 10000
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed > 0 AND RequestID = Requests.ID) THEN 9000
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted > 0 AND RequestID = Requests.ID) THEN 500
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedRequest > 0 AND RequestID = Requests.ID) THEN 400
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
							WHEN Requests.Private = 1 THEN 200
							ELSE 250
							END
							WHERE Requests.ID IN (SELECT RequestID FROM inserted)
					END
                END");
        }
        
        public override void Down()
        {
            Sql(@"DECLARE @dfName nvarchar(100)
                -- default for CreatedOn column
                SELECT @dfName = df.[name] FROM sys.columns c JOIN sys.default_constraints df ON df.parent_object_id = c.object_id AND df.parent_column_id = c.column_id WHERE c.object_id = OBJECT_ID('Requests') AND c.[name] = 'CreatedOn'
                IF @dfName IS NOT NULL
                BEGIN
	                EXEC ('ALTER TABLE Requests DROP CONSTRAINT ' + @dfName)
                END
                EXEC ('ALTER TABLE [dbo].[Requests] ADD  CONSTRAINT ' + @dfName + '  DEFAULT (getdate()) FOR [CreatedOn]')

                -- default for UpdatedOn column
                SELECT @dfName = df.[name] FROM sys.columns c JOIN sys.default_constraints df ON df.parent_object_id = c.object_id AND df.parent_column_id = c.column_id WHERE c.object_id = OBJECT_ID('Requests') AND c.[name] = 'UpdatedOn'
                IF @dfName IS NOT NULL
                BEGIN
	                EXEC ('ALTER TABLE Requests DROP CONSTRAINT ' + @dfName)
                END
                EXEC ('ALTER TABLE [dbo].[Requests] ADD  CONSTRAINT ' + @dfName + '  DEFAULT (getdate()) FOR [UpdatedOn]')");
            Sql(@"UPDATE Requests SET CreatedOn = dbo.GetLocalDateTime(CreatedOn, 13), DueDate = dbo.GetLocalDateTime(DueDate, 13), SubmittedOn = dbo.GetLocalDateTime(SubmittedOn, 13), UpdatedOn = dbo.GetLocalDateTime(UpdatedOn, 13), ApprovedForDraftOn = dbo.GetLocalDateTime(ApprovedForDraftOn, 13), RejectedOn = dbo.GetLocalDateTime(RejectedOn, 13), CancelledOn = dbo.GetLocalDateTime(CancelledOn, 13)");
            Sql(@"UPDATE RequestDataMarts SET RequestTime = dbo.GetLocalDateTime(RequestTime, 13), ResponseTime = dbo.GetLocalDateTime(ResponseTime, 13)");

            Sql(@"UPDATE RequestDataMartResponses SET ResponseTime = NULL WHERE ResponseTime = '0001-01-01 00:00:00.0000000'");
            Sql(@"UPDATE RequestDataMartResponses SET SubmittedOn = '2/2/1753' WHERE SubmittedOn = '0001-01-01 00:00:00.0000000'");
            Sql(@"UPDATE RequestDataMartResponses SET ResponseTime = dbo.GetLocalDateTime(CAST(ResponseTime as datetime), 13), SubmittedOn = dbo.GetLocalDateTime(CAST(SubmittedOn as datetime), 13)");
            Sql(@"UPDATE RequestDataMartResponses SET SubmittedOn = '0001-01-01 00:00:00.0000000' WHERE SubmittedOn = '2/2/1753'");
            
            Sql(@"ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                        ON  [dbo].[RequestDataMarts]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
	                    BEGIN
		                    UPDATE Requests SET UpdatedOn = GETDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
		                    UPDATE Requests SET Status = 
			                    CASE WHEN NOT CancelledOn IS NULL THEN 9999 
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed = Total AND RequestID = Requests.ID) THEN 10000
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed > 0 AND RequestID = Requests.ID) THEN 9000
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted > 0 AND RequestID = Requests.ID) THEN 500
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedRequest > 0 AND RequestID = Requests.ID) THEN 400
			                    WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
			                    WHEN Requests.Private = 1 THEN 200
			                    ELSE 250
			                    END
			                    WHERE Requests.ID IN (SELECT RequestID FROM inserted)
	                    END
                    END");
        }
    }
}
