
CREATE TABLE dbo.users
(
	[Id] int NOT NULL,
	[Password] nvarchar(64) NOT NULL,
    CONSTRAINT PK_users_Id PRIMARY KEY ([Id])
)
GO



CREATE TABLE dbo.usage
(
	[Id] int NOT NULL,
	[StartTime] bigint NOT NULL,
	[DurationMinutes] int NOT NULL,
	[Type] int NOT NULL,
	[Floor] int NOT NULL,
    CONSTRAINT PK_usgae_Id_StartTime PRIMARY KEY ([Id], [StartTime])
)
GO
