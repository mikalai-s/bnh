/*
Run this script on:

localhost\sqlserver.bnh-test    -  This database will be modified

to synchronize it with:

localhost\sqlserver.bnh

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 9.0.0 from Red Gate Software Ltd at 14/02/2012 11:49:41 PM

*/
		
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

-- Drop constraints from [bl].[Community]
ALTER TABLE [bl].[Community] DROP CONSTRAINT [FK_Community_Zone]

-- Drop constraints from [dbo].[aspnet_UsersInRoles]
ALTER TABLE [dbo].[aspnet_UsersInRoles] DROP CONSTRAINT [FK__aspnet_Us__RoleI__4AB81AF0]
ALTER TABLE [dbo].[aspnet_UsersInRoles] DROP CONSTRAINT [FK__aspnet_Us__UserI__49C3F6B7]

-- Drop constraints from [dbo].[aspnet_Membership]
ALTER TABLE [dbo].[aspnet_Membership] DROP CONSTRAINT [FK__aspnet_Me__Appli__21B6055D]
ALTER TABLE [dbo].[aspnet_Membership] DROP CONSTRAINT [FK__aspnet_Me__UserI__22AA2996]

-- Drop constraints from [bl].[Zone]
ALTER TABLE [bl].[Zone] DROP CONSTRAINT [FK_Zone_City]

-- Drop constraints from [dbo].[aspnet_Users]
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [FK__aspnet_Us__Appli__0DAF0CB0]

-- Drop constraint FK__aspnet_Pe__UserI__693CA210 from [dbo].[aspnet_PersonalizationPerUser]
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] DROP CONSTRAINT [FK__aspnet_Pe__UserI__693CA210]

-- Drop constraint FK__aspnet_Pr__UserI__38996AB5 from [dbo].[aspnet_Profile]
ALTER TABLE [dbo].[aspnet_Profile] DROP CONSTRAINT [FK__aspnet_Pr__UserI__38996AB5]

-- Drop constraints from [dbo].[aspnet_Roles]
ALTER TABLE [dbo].[aspnet_Roles] DROP CONSTRAINT [FK__aspnet_Ro__Appli__440B1D61]

-- Drop constraint FK__aspnet_Pa__Appli__5AEE82B9 from [dbo].[aspnet_Paths]
ALTER TABLE [dbo].[aspnet_Paths] DROP CONSTRAINT [FK__aspnet_Pa__Appli__5AEE82B9]

-- Add 1 row to [dbo].[aspnet_Applications]
INSERT INTO [dbo].[aspnet_Applications] ([ApplicationId], [ApplicationName], [LoweredApplicationName], [Description]) VALUES ('1267113d-29b5-4f83-8dd5-5d7e6cbdd0c0', N'/', N'/', NULL)

-- Add 1 row to [bl].[City]
INSERT INTO [bl].[City] ([Id], [Name]) VALUES ('a99dee26-ce8b-4635-af06-cf0871f6d299', N'Calgary')

-- Add 2 rows to [dbo].[aspnet_Roles]
INSERT INTO [dbo].[aspnet_Roles] ([RoleId], [ApplicationId], [RoleName], [LoweredRoleName], [Description]) VALUES ('7cdd88b4-7eff-4e51-998d-36157df38292', '1267113d-29b5-4f83-8dd5-5d7e6cbdd0c0', N'admin', N'admin', NULL)
INSERT INTO [dbo].[aspnet_Roles] ([RoleId], [ApplicationId], [RoleName], [LoweredRoleName], [Description]) VALUES ('5e056376-0a43-4b27-b9a5-6b6eb718a0ea', '1267113d-29b5-4f83-8dd5-5d7e6cbdd0c0', N'content_manager', N'content_manager', NULL)

-- Add 1 row to [dbo].[aspnet_Users]
INSERT INTO [dbo].[aspnet_Users] ([UserId], [ApplicationId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES ('c64049a9-59d3-485d-95ec-b770d26ea989', '1267113d-29b5-4f83-8dd5-5d7e6cbdd0c0', N'admin', N'admin', NULL, 0, '2012-02-15 06:45:37.163')

-- Add 4 rows to [bl].[Zone]
INSERT INTO [bl].[Zone] ([Id], [Name], [CityId]) VALUES ('7d5402cb-fb2f-4bc0-ae81-0829e7dc88b4', N'NE', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
INSERT INTO [bl].[Zone] ([Id], [Name], [CityId]) VALUES ('cf1ba708-849f-4d8a-b1ce-13723dfb3cd0', N'NW', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
INSERT INTO [bl].[Zone] ([Id], [Name], [CityId]) VALUES ('12361602-6505-41c4-90f3-6ac637ab3310', N'SW', 'a99dee26-ce8b-4635-af06-cf0871f6d299')
INSERT INTO [bl].[Zone] ([Id], [Name], [CityId]) VALUES ('8178ab50-dc7d-451e-bfaa-e8892e5e3155', N'SE', 'a99dee26-ce8b-4635-af06-cf0871f6d299')

-- Add 1 row to [dbo].[aspnet_Membership]
INSERT INTO [dbo].[aspnet_Membership] ([UserId], [ApplicationId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES ('c64049a9-59d3-485d-95ec-b770d26ea989', '1267113d-29b5-4f83-8dd5-5d7e6cbdd0c0', N'aZRcIczQa71LWqeOa9cpkNG1mmg=', 1, N'V0iH4UhF/0Af6osEKL2ABw==', NULL, N'a@b.c', N'a@b.c', NULL, NULL, 1, 0, '2012-02-15 06:10:27.000', '2012-02-15 06:45:37.163', '2012-02-15 06:10:27.000', '1754-01-01 00:00:00.000', 0, '1754-01-01 00:00:00.000', 0, '1754-01-01 00:00:00.000', NULL)

-- Add 2 rows to [dbo].[aspnet_UsersInRoles]
INSERT INTO [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES ('c64049a9-59d3-485d-95ec-b770d26ea989', '7cdd88b4-7eff-4e51-998d-36157df38292')
INSERT INTO [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES ('c64049a9-59d3-485d-95ec-b770d26ea989', '5e056376-0a43-4b27-b9a5-6b6eb718a0ea')

-- Add 5 rows to [bl].[Community]
INSERT INTO [bl].[Community] ([Id], [Name], [ZoneId]) VALUES ('5ef35410-468a-43dd-bb4a-571468b082f4', N'Evergreen', '12361602-6505-41c4-90f3-6ac637ab3310')
INSERT INTO [bl].[Community] ([Id], [Name], [ZoneId]) VALUES ('615f8fd1-c4d3-4c18-bd2e-8af4ac3743f2', N'Silverado', '12361602-6505-41c4-90f3-6ac637ab3310')
INSERT INTO [bl].[Community] ([Id], [Name], [ZoneId]) VALUES ('7fc1d51b-c7a1-4da8-b2a4-8b422995da63', N'Midnapore', '8178ab50-dc7d-451e-bfaa-e8892e5e3155')
INSERT INTO [bl].[Community] ([Id], [Name], [ZoneId]) VALUES ('2c73c4a9-b71c-4d2f-8cc6-a84762f9b4c1', N'Montgomery', 'cf1ba708-849f-4d8a-b1ce-13723dfb3cd0')
INSERT INTO [bl].[Community] ([Id], [Name], [ZoneId]) VALUES ('e11a03a2-5283-4903-b340-a9637610619d', N'Rundle', '7d5402cb-fb2f-4bc0-ae81-0829e7dc88b4')

-- Add constraints to [bl].[Community]
ALTER TABLE [bl].[Community] ADD CONSTRAINT [FK_Community_Zone] FOREIGN KEY ([ZoneId]) REFERENCES [bl].[Zone] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE

-- Add constraints to [dbo].[aspnet_UsersInRoles]
ALTER TABLE [dbo].[aspnet_UsersInRoles] ADD CONSTRAINT [FK__aspnet_Us__RoleI__4AB81AF0] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[aspnet_Roles] ([RoleId])
ALTER TABLE [dbo].[aspnet_UsersInRoles] ADD CONSTRAINT [FK__aspnet_Us__UserI__49C3F6B7] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])

-- Add constraints to [dbo].[aspnet_Membership]
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [FK__aspnet_Me__Appli__21B6055D] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [FK__aspnet_Me__UserI__22AA2996] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])

-- Add constraints to [bl].[Zone]
ALTER TABLE [bl].[Zone] ADD CONSTRAINT [FK_Zone_City] FOREIGN KEY ([CityId]) REFERENCES [bl].[City] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE

-- Add constraints to [dbo].[aspnet_Users]
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [FK__aspnet_Us__Appli__0DAF0CB0] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])

-- Add constraint FK__aspnet_Pe__UserI__693CA210 to [dbo].[aspnet_PersonalizationPerUser]
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] WITH NOCHECK ADD CONSTRAINT [FK__aspnet_Pe__UserI__693CA210] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])

-- Add constraint FK__aspnet_Pr__UserI__38996AB5 to [dbo].[aspnet_Profile]
ALTER TABLE [dbo].[aspnet_Profile] WITH NOCHECK ADD CONSTRAINT [FK__aspnet_Pr__UserI__38996AB5] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])

-- Add constraints to [dbo].[aspnet_Roles]
ALTER TABLE [dbo].[aspnet_Roles] ADD CONSTRAINT [FK__aspnet_Ro__Appli__440B1D61] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])

-- Add constraint FK__aspnet_Pa__Appli__5AEE82B9 to [dbo].[aspnet_Paths]
ALTER TABLE [dbo].[aspnet_Paths] WITH NOCHECK ADD CONSTRAINT [FK__aspnet_Pa__Appli__5AEE82B9] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
COMMIT TRANSACTION
GO
