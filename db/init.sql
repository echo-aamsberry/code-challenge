CREATE DATABASE [Arbitr]
GO

USE [Arbitr]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE SCHEMA [Clutch]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clutch].[LoadMapping]
(
	[LoadId] INTEGER NOT NULL,
	[LoadKey] UNIQUEIDENTIFIER NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GETDATE()
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Arbitr_Clutch_LoadId] ON [Clutch].[LoadMapping] ([LoadId] ASC),
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Arbitr_Clutch_LoadKey] ON [Clutch].[LoadMapping] ([LoadKey] ASC)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clutch].[TruckMapping]
(
	[TruckId] INTEGER NOT NULL,
	[TruckKey] UNIQUEIDENTIFIER NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT GETDATE()
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Arbitr_Clutch_TruckId] ON [Clutch].[TruckMapping] ([TruckId] ASC),
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Arbitr_Clutch_TruckKey] ON [Clutch].[TruckMapping] ([TruckKey] ASC)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[InsertTruckMapping]
	@TruckId INTEGER,
	@TruckKey UNIQUEIDENTIFIER
AS
	INSERT INTO [Clutch].[TruckMapping] ([TruckId], [TruckKey])
	VALUES (@TruckId, @TruckKey)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[RemoveTruckMapping]
	@TruckId INTEGER,
	@TruckKey UNIQUEIDENTIFIER
AS
	DELETE FROM
		[Clutch].[TruckMapping]
	WHERE
		[TruckId] = @TruckId AND [TruckKey] = @TruckKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[InsertLoadMapping]
	@LoadId INTEGER,
	@LoadKey UNIQUEIDENTIFIER
AS
	INSERT INTO [Clutch].[LoadMapping] ([LoadId], [LoadKey])
	VALUES (@LoadId, @LoadKey)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[RemoveLoadMapping]
	@LoadId INTEGER,
	@LoadKey UNIQUEIDENTIFIER
AS
	DELETE FROM
		[Clutch].[LoadMapping]
	WHERE
		[LoadId] = @LoadId AND [LoadKey] = @LoadKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[GetTruckKeyById]
	@TruckId INTEGER
AS
	SELECT
		[TruckKey]
	FROM
		[Clutch].[TruckMapping]
	WHERE
		[TruckId] = @TruckId
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[GetTruckIdByKey]
	@TruckKey UNIQUEIDENTIFIER
AS
	SELECT
		[TruckId]
	FROM
		[Clutch].[TruckMapping]
	WHERE
		[TruckKey] = @TruckKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[GetLoadKeyById]
	@LoadId INTEGER
AS
	SELECT
		[LoadKey]
	FROM
		[Clutch].[LoadMapping]
	WHERE
		[LoadId] = @LoadId
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clutch].[GetLoadIdByKey]
	@LoadKey UNIQUEIDENTIFIER
AS
	SELECT
		[LoadId]
	FROM
		[Clutch].[LoadMapping]
	WHERE
		[LoadKey] = @LoadKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Location]
(
	[LocationId] INTEGER IDENTITY(1,1) NOT NULL,
	[City] VARCHAR(100) NOT NULL,
	[State] VARCHAR(50) NOT NULL,
	[Zip] VARCHAR(25) NOT NULL,
	[Country] VARCHAR(100) NOT NULL,
	[Latitude] DECIMAL(9,4) NOT NULL,
    [Longitude] DECIMAL(9,4) NOT NULL,

	CONSTRAINT [PK_Arbitr_Location_LocationId] PRIMARY KEY CLUSTERED ([LocationId] ASC)
)
GO
CREATE NONCLUSTERED INDEX [IX_Arbitr_Location_LocationId] ON [dbo].[Location] ([LocationId])
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Truck]
(
    [TruckKey] UNIQUEIDENTIFIER NOT NULL,
	[OriginLocationId] INTEGER NOT NULL,
    [AvailableDate] DATETIMEOFFSET NOT NULL,
	
	CONSTRAINT [PK_Arbitr_Truck_TruckKey] PRIMARY KEY CLUSTERED ([TruckKey] ASC),
	CONSTRAINT [FK_Arbitr_Truck_Location_OriginLocationId] FOREIGN KEY ([OriginLocationId]) REFERENCES [dbo].[Location] ([LocationId])
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Arbitr_Truck_TruckKey] ON [dbo].[Truck] ([TruckKey] ASC)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[TruckDestination]
(
    [TruckKey] UNIQUEIDENTIFIER NOT NULL,
	[DestinationLocationId] INTEGER NOT NULL,
	
	CONSTRAINT [FK_Arbitr_TruckDestination_Location_DestinationLocationId] FOREIGN KEY ([DestinationLocationId]) REFERENCES [dbo].[Location] ([LocationId])
)
CREATE NONCLUSTERED INDEX [UIX_Arbitr_TruckDestination_TruckKey] ON [dbo].[TruckDestination] ([TruckKey] ASC)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UFIX_Arbitr_TruckDestination_TruckKey_DestinationLocationId] ON [dbo].[TruckDestination]
(
	[TruckKey] ASC,
	[DestinationLocationId] ASC
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Load]
(
	[LoadKey] UNIQUEIDENTIFIER NOT NULL,
	[OriginLocationId] INTEGER NOT NULL,
	[DestinationLocationId] INTEGER NOT NULL,
	[PickupDate] DATETIMEOFFSET NOT NULL,
	
	CONSTRAINT [PK_Arbitr_Load_LoadKey] PRIMARY KEY CLUSTERED ([LoadKey] ASC),
	CONSTRAINT [FK_Arbitr_Load_Location_OriginLocationId] FOREIGN KEY ([OriginLocationId]) REFERENCES [dbo].[Location] ([LocationId]),
	CONSTRAINT [FK_Arbitr_Load_Location_DestinationLocationId] FOREIGN KEY ([DestinationLocationId]) REFERENCES [dbo].[Location] ([LocationId])
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Arbitr_Load_LoadKey] ON [dbo].[Load] ([LoadKey] ASC)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Match]
(
	[TruckKey] UNIQUEIDENTIFIER NOT NULL,
	[LoadKey] UNIQUEIDENTIFIER NOT NULL,
	[Matchiness] TINYINT NOT NULL

	CONSTRAINT [FK_Arbitr_Match_Truck_TruckKey] FOREIGN KEY ([TruckKey]) REFERENCES [dbo].[Truck] ([TruckKey]),
	CONSTRAINT [FK_Arbitr_Match_Load_LoadKey] FOREIGN KEY ([LoadKey]) REFERENCES [dbo].[Load] ([LoadKey]),
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UFIX_Arbitr_Match_TruckKey_LoadKey] ON [dbo].[Match]
(
	[TruckKey] ASC,
	[LoadKey] ASC
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TYPE [dbo].[TruckDestinationType] AS TABLE
(
	[DestinationLocationId] INTEGER NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TYPE [dbo].[MatchType] AS TABLE
(
	[TruckKey] UNIQUEIDENTIFIER NOT NULL,
	[LoadKey] UNIQUEIDENTIFIER NOT NULL,
	[Matchiness] TINYINT NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SelectAllLocations]
AS
	SELECT
		[LocationId]
	   ,[City]
	   ,[State]
	   ,[Zip]
	   ,[Country]
	   ,[Latitude]
	   ,[Longitude]
	FROM
		[dbo].[Location]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SelectLocationByZip]
	@Zip VARCHAR(25)
AS
	SELECT
		[LocationId]
	   ,[City]
	   ,[State]
	   ,[Country]
	   ,[Latitude]
	   ,[Longitude]
	FROM
		[dbo].[Location]
	WHERE
		[Zip] = @Zip
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SelectAllLoads]
AS
	SELECT
		[LoadKey]
	   ,[OriginLocationId]
	   ,[DestinationLocationId]
	   ,[PickupDate]
	FROM
		[dbo].[Load]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CheckLoadExists]
	@LoadKey UNIQUEIDENTIFIER
AS
	IF EXISTS(SELECT 1 FROM [dbo].[Load] WHERE [LoadKey] = @LoadKey)
		SELECT 1
	ELSE
		SELECT 0
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[InsertLoad]
	@LoadKey UNIQUEIDENTIFIER
   ,@OriginLocationId INTEGER
   ,@DestinationLocationId INTEGER
   ,@PickupDate DATETIMEOFFSET
AS
	INSERT INTO [dbo].[Load]
	(
		[LoadKey]
	   ,[OriginLocationId]
	   ,[DestinationLocationId]
	   ,[PickupDate]
	)
	VALUES
	(
		@LoadKey
	   ,@OriginLocationId
	   ,@DestinationLocationId
	   ,@PickupDate
	)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UpdateLoad]
	@LoadKey UNIQUEIDENTIFIER
   ,@OriginLocationId INTEGER
   ,@DestinationLocationId INTEGER
   ,@PickupDate DATETIMEOFFSET
AS
	UPDATE
		[dbo].[Load]
	SET
		[OriginLocationId] = @OriginLocationId
	   ,[DestinationLocationId] = @DestinationLocationId
	   ,[PickupDate] = @PickupDate
	WHERE
		[LoadKey] = @LoadKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RemoveLoad]
	@LoadKey UNIQUEIDENTIFIER
AS
	DELETE FROM
		[dbo].[Load]
	WHERE
		[LoadKey] = @LoadKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[InsertTruck]
	@TruckKey UNIQUEIDENTIFIER
   ,@OriginLocationId INTEGER
   ,@PossibleDestinationLocationIds [TruckDestinationType] READONLY
   ,@AvailableDate DATETIMEOFFSET
AS
	INSERT INTO [dbo].[Truck]
	(
		[TruckKey]
	   ,[OriginLocationId]
	   ,[AvailableDate]
	)
	VALUES
	(
		@TruckKey
	   ,@OriginLocationId
	   ,@AvailableDate
	)

	INSERT INTO [dbo].[TruckDestination]
	SELECT @TruckKey, DestinationLocationId FROM @PossibleDestinationLocationIds
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UpdateTruck]
	@TruckKey UNIQUEIDENTIFIER
   ,@OriginLocationId INTEGER
   ,@PossibleDestinationLocationIds [TruckDestinationType] READONLY
   ,@AvailableDate DATETIMEOFFSET
AS
	UPDATE
		[dbo].[Truck]
	SET
		[OriginLocationId] = @OriginLocationId
	   ,[AvailableDate] = @AvailableDate
	WHERE
		[TruckKey] = @TruckKey

	MERGE [dbo].[TruckDestination] AS TARGET
	USING @PossibleDestinationLocationIds AS SOURCE
	ON [TruckKey] = @TruckKey AND TARGET.[DestinationLocationId] = SOURCE.[DestinationLocationId]
	WHEN NOT MATCHED BY TARGET
		THEN INSERT ([TruckKey], [DestinationLocationId]) VALUES (@TruckKey, SOURCE.[DestinationLocationId])
	WHEN NOT MATCHED BY SOURCE AND TARGET.[TruckKey] = @TruckKey
		THEN DELETE;
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RemoveTruck]
	@TruckKey UNIQUEIDENTIFIER
AS
	DELETE FROM
		[dbo].[TruckDestination]
	WHERE
		[TruckKey] = @TruckKey

	DELETE FROM
		[dbo].[Truck]
	WHERE
		[TruckKey] = @TruckKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SelectAllTrucks]
AS
	SELECT
		[TruckKey]
	   ,[OriginLocationId]
	   ,[AvailableDate]
	FROM
		[dbo].[Truck]

	SELECT
		[TruckKey]
	   ,[DestinationLocationId]
	FROM
		[dbo].[TruckDestination]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CheckTruckExists]
	@TruckKey UNIQUEIDENTIFIER
AS
	IF EXISTS(SELECT 1 FROM [dbo].[Truck] WHERE [TruckKey] = @TruckKey)
		SELECT 1
	ELSE
		SELECT 0
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SaveAllMatches]
	@Matches [MatchType] READONLY
AS
	MERGE [dbo].[Match] AS TARGET
	USING @Matches AS SOURCE
	ON TARGET.[LoadKey] = SOURCE.[LoadKey] AND TARGET.[TruckKey] = SOURCE.[TruckKey]
	WHEN MATCHED
		THEN UPDATE SET TARGET.[Matchiness] = SOURCE.[Matchiness]
	WHEN NOT MATCHED BY TARGET
		THEN INSERT ([TruckKey], [LoadKey], [Matchiness]) VALUES (SOURCE.[TruckKey], SOURCE.[LoadKey], SOURCE.[Matchiness])
	WHEN NOT MATCHED BY SOURCE
		THEN DELETE;
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SelectAllMatches]
AS
	SELECT
		[TruckKey]
	   ,[LoadKey]
	   ,[Matchiness]
	FROM
		[dbo].[Match]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RemoveMatchesForLoad]
	@LoadKey UNIQUEIDENTIFIER
AS
	DELETE FROM
		[dbo].[Match]
	WHERE
		[LoadKey] = @LoadKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RemoveMatchesForTruck]
	@TruckKey UNIQUEIDENTIFIER
AS
	DELETE FROM
		[dbo].[Match]
	WHERE
		[TruckKey] = @TruckKey
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('Chicago', 'IL', '60654', 'US', 41.8787, 87.6298)
INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('Las Vegas', 'NV', '89109', 'US', 36.1699, 115.1398)
INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('Los Angeles', 'CA', '90007', 'US', 34.0522, 118.2437)
INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('New York', 'NY', '10001', 'US', 40.7128, 74.0060)
INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('Dallas', 'TX', '76011', 'US', 32.7767, 96.7970)
INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('San Antonio', 'TX', '78205', 'US', 29.4241, 98.4936)
INSERT INTO [dbo].[Location] ([City], [State], [Zip], [Country], [Latitude], [Longitude]) VALUES ('Miami', 'FL', '33140', 'US', 25.7617, 80.1918)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------