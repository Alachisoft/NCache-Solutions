/*    ==Scripting Parameters==
    Source Server Version : SQL Server 2017 (14.0.1000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server
    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

CREATEDATABASE [NCacheDb]
USE [NCacheDb]
GO
CREATETABLE [dbo].[CacheItem](
        [Key] [varchar](450)NOTNULL,
        [UserObject] [varchar](max)NULL,
        [ExpirationType] [int] NOTNULL,
        [ItemType] [int] NOTNULL,
        [TimeToLive] [time](7)NOTNULL,
        [EvictionHint] [int] NOTNULL,
        [Group] [nvarchar](45)NULL,
        [ItemPriority] [tinyint] NOTNULL,
        [InsertionTime] [datetime2](7)NOTNULL,
CONSTRAINT [PK_StoreItem] PRIMARYKEYCLUSTERED
([Key] ASC)WITH (PAD_INDEX=OFF,STATISTICS_NORECOMPUTE=OFF, IGNORE_DUP_KEY=OFF, ALLOW_ROW_LOCKS=ON,ALLOW_PAGE_LOCKS=ON) ON [PRIMARY]) 
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ((0))FOR [ExpirationType]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ((0))FOR [ItemType]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ((0))FOR [OperationType]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ((0))FOR [EvictionPercentage]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ('00:00:00')FOR [TimeToLive]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ((0))FOR [EvictionHint]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT (CONVERT([tinyint],(0)))FOR [ItemPriority]
GO
ALTER TABLE [dbo].[CacheItem] ADD  DEFAULT ('0001-01-01T00:00:00.0000000')FOR [InsertionTime]
GO