/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2008 R2 (10.50.1600)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2008 R2
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [Fraud-Detection]
GO
CREATE TABLE [dbo].[CardInfo](
	[CardNumber] [varchar](100) NOT NULL,
	[CardType] [varchar](50) NOT NULL,
	[CardStartDate] [varchar](50) NOT NULL,
	[CardExpiryDate] [varchar](50) NOT NULL,
	[CardLimit] [varchar](50) NOT NULL,
	[CustomerNo] [int] NOT NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Customers](
	[CustomerNo] [int] NOT NULL,
	[CustomerName] [varchar](50) NOT NULL,
	[EmailID] [varchar](50) NOT NULL,
	[CompanyName] [varchar](50) NULL,
	[ContactNo] [varchar](50) NULL,
	[City] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[PostalAddress] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [EmailID]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [CompanyName]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [ContactNo]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [Country]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [PostalAddress]
GO

ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [Fax]
GO

CREATE TABLE [dbo].[Transactions](
	[IPAdress] [varchar](50) NULL,
	[TransactionAmount] [varchar](50) NULL,
	[Result] [varchar](50) NULL,
	[CustomerNo] [int] NULL,
	[CardNumber] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[Email] [varchar](50) NULL
) ON [PRIMARY]
GO
